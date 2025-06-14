// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

/// <summary>
/// Parser designed for command palette input with support for incremental parsing
/// </summary>
/// <typeparam name="TOptions">The type of options object to populate</typeparam>
public partial class CommandPaletteParser<TOptions> where TOptions : class, new()
{
    private const StringComparison PrefixComparison = StringComparison.OrdinalIgnoreCase;
    private readonly List<SwitchDefinition<TOptions>> _switches = [];
    private readonly string _switchPrefix;
    private readonly Regex _switchRegex;
    private readonly char _valueSeparator;
    private readonly Regex _whitespaceRegex = WhitespaceRegex();

    public CommandPaletteParser(string switchPrefix = "/", char valueSeparator = ':')
    {
        this._switchPrefix = Regex.Escape(switchPrefix);
        this._valueSeparator = valueSeparator;
        var pattern
            = $"""
               {this._switchPrefix}(?<name>[^\s{this._switchPrefix}{Regex.Escape(valueSeparator.ToString())}]*?)
               (?:{Regex.Escape(valueSeparator.ToString())}(?<value>[^\s{this._switchPrefix}]*))?
               (?=\s|$|{this._switchPrefix})
               """;
        this._switchRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
    }

    /// <summary>
    /// Adds a switch definition to the parser
    /// </summary>
    public CommandPaletteParser<TOptions> AddSwitch(
        string name,
        bool hasArgument,
        Action<TOptions, string?> handler,
        Func<string?, (bool isValid, string? errorMessage)>? validator = null,
        Dictionary<string, string>? aliases = null,
        string? description = null,
        SwitchValueDefinition[]? valueSuggestions = null)
    {
        this._switches.Add(new SwitchDefinition<TOptions>(name, hasArgument, handler, validator, aliases, description,
            valueSuggestions));
        return this;
    }

    /// <summary>
    /// Adds a boolean flag switch
    /// </summary>
    public CommandPaletteParser<TOptions> AddFlag(string name, Action<TOptions> handler, string? description = null)
    {
        return this.AddSwitch(name, false, (opts, _) => handler(opts), null, null, description);
    }

    /// <summary>
    /// Adds a switch with a value
    /// </summary>
    public CommandPaletteParser<TOptions> AddValueSwitch(
        string name,
        Action<TOptions, string> handler,
        Func<string, (bool isValid, string? errorMessage)>? validator = null,
        Dictionary<string, string>? aliases = null,
        string? description = null,
        SwitchValueDefinition[]? valueSuggestions = null)
    {
        return this.AddSwitch(name, true, (opts, val) => handler(opts, val!),
            validator != null ? (val) => val == null ? (false, "Value required") : validator(val) : null,
            aliases, description, valueSuggestions);
    }

    /// <summary>
    /// Adds a switch with a value
    /// </summary>
    public CommandPaletteParser<TOptions> AddEnumValueSwitch(
        string name,
        Action<TOptions, string> handler,
        Dictionary<string, string>? aliases = null,
        string? description = null,
        SwitchValueDefinition[]? valueSuggestions = null)
    {
        return this.AddSwitch(name: name,
                              hasArgument: true,
                              handler: (opts, val) => handler(opts, val!),
                              validator: val => ValidateExactSuggestion(val, valueSuggestions),
                              aliases: aliases,
                              description: description,
                              valueSuggestions: valueSuggestions);
    }

    /// <summary>
    /// Parse input with support for incremental parsing
    /// </summary>
    public ParseResult<TOptions> Parse(string input, int cursorPosition = -1)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new ParseResult<TOptions>(string.Empty, new TOptions(), [], new ParseContext(cursorPosition));
        }

        var options = new TOptions();
        var errors = new List<ParseError>();
        var warnings = new List<ParseWarning>();
        var suggestions = new List<Suggestion>();
        var switchMatchers = this._switchRegex.Matches(input);

        // Determine if we're at the end and might be typing a new switch
        var isAtEnd = cursorPosition < 0 || cursorPosition >= input.Length;
        var endsWithPrefix = input.EndsWith(this._switchPrefix, StringComparison.OrdinalIgnoreCase);

        // Check for incomplete switch at the end
        var incompleteSwitch = this.DetectIncompleteSwitch(input, cursorPosition);
        if (incompleteSwitch != null)
        {
            suggestions.AddRange(this.GenerateSwitchSuggestions(incompleteSwitch));
        }

        // Suggest switches if at end with space or switch prefix
        // if (isAtEnd && (endsWithSpace || endsWithPrefix))
        else if (isAtEnd && endsWithPrefix)
        {
            suggestions.AddRange(this.GenerateAvailableSwitchSuggestions());
        }

        var queryBuilder = new StringBuilder(input.Length);
        var lastIndex = 0;

        foreach (Match match in switchMatchers)
        {
            // and anything between switches to the query
            if (match.Index > lastIndex)
            {
                queryBuilder.Append(input.AsSpan(lastIndex, match.Index - lastIndex));
            }

            lastIndex = match.Index + match.Length;

            var parseContext = this.ProcessSwitchMatch(match, options, errors, warnings, cursorPosition);
            if (suggestions.Count == 0)
            {
                if (parseContext.CurrentSuggestions.Count != 0)
                {
                    suggestions.AddRange(parseContext.CurrentSuggestions);
                }
            }
        }

        if (lastIndex < input.Length)
        {
            queryBuilder.Append(input.AsSpan(lastIndex));
        }

        var query = this._whitespaceRegex.Replace(queryBuilder.ToString(), " ").Trim();

        var context = new ParseContext(cursorPosition)
        {
            Suggestions = suggestions,
            Warnings = warnings,
            IsIncomplete = incompleteSwitch != null || endsWithPrefix
        };

        return new ParseResult<TOptions>(query, options, errors, context);
    }

    private IncompleteSwitchInfo? DetectIncompleteSwitch(string input, int cursorPosition)
    {
        // Look for incomplete switches at the end or at cursor position
        var checkPosition = cursorPosition < 0 ? input.Length : cursorPosition;

        // Find the last switch prefix before the cursor
        var lastPrefixIndex = input.LastIndexOf(this._switchPrefix, Math.Max(0, checkPosition - 1), PrefixComparison);

        if (lastPrefixIndex >= 0)
        {
            var afterPrefix = input[(lastPrefixIndex + this._switchPrefix.Length)..];
            var spaceIndex = afterPrefix.IndexOf(' ');
            var nextPrefixIndex = afterPrefix.IndexOf(this._switchPrefix, PrefixComparison);

            var endIndex = Math.Min(
                spaceIndex >= 0 ? spaceIndex : int.MaxValue,
                nextPrefixIndex >= 0 ? nextPrefixIndex : int.MaxValue
            );

            // We're at the end of input
            var partial = afterPrefix.TrimEnd();
            var separatorIndex = partial.IndexOf(this._valueSeparator);

            if (separatorIndex == -1)
            {
                partial = new string(partial.TakeWhile(static t => !char.IsWhiteSpace(t)).ToArray());
                return new IncompleteSwitchInfo(partial, false, null, lastPrefixIndex);
            }

            // We have a separator, check if we need value suggestions
            var switchName = partial[..separatorIndex];
            var partialValue = endIndex == int.MaxValue
                ? partial[(separatorIndex + 1)..]
                : partial.Substring(separatorIndex + 1, endIndex - separatorIndex - 1);
            return new IncompleteSwitchInfo(switchName, true, partialValue, lastPrefixIndex);

        }

        return null;
    }

    private List<Suggestion> GenerateSwitchSuggestions(IncompleteSwitchInfo incomplete)
    {
        var suggestions = new List<Suggestion>();

        if (!incomplete.HasSeparator)
        {
            // Suggest switch names
            foreach (var sw in this._switches)
            {
                if (string.IsNullOrEmpty(incomplete.PartialName) ||
                    sw.Name.StartsWith(incomplete.PartialName, StringComparison.OrdinalIgnoreCase))
                {
                    var completion = this._switchPrefix + sw.Name;
                    if (sw.HasArgument)
                    {
                        completion += this._valueSeparator;
                    }

                    suggestions.Add(new Suggestion(
                        SuggestionType.Switch,
                        completion,
                        sw.Description ?? $"Switch: {sw.Name}",
                        incomplete.Position,
                        incomplete.PartialName?.Length ?? 0
                    ));
                }
            }
        }
        else
        {
            // Suggest values for the switch
            var switchDef = this._switches.FirstOrDefault(s =>
                string.Equals(s.Name, incomplete.PartialName, StringComparison.OrdinalIgnoreCase));

            if (switchDef != null)
            {
                // Add alias suggestions
                if (switchDef.Aliases != null)
                {
                    foreach (var alias in switchDef.Aliases)
                    {
                        if (string.IsNullOrEmpty(incomplete.PartialValue) ||
                            alias.Key.StartsWith(incomplete.PartialValue, StringComparison.OrdinalIgnoreCase))
                        {
                            suggestions.Add(new Suggestion(
                                SuggestionType.Value,
                                alias.Key,
                                $"{alias.Key} → {alias.Value}",
                                incomplete.Position + incomplete.PartialName.Length + 2,
                                incomplete.PartialValue?.Length ?? 0
                            ));
                        }
                    }
                }

                // Add custom value suggestions
                if (switchDef.ValueSuggestions != null)
                {
                    var matchesExactly = switchDef.HasValue(incomplete.PartialValue);

                    // If partial value matches exactly some value, then skip suggestions entirely. Practical reason is
                    // that in that case the query is complete.
                    if (!matchesExactly)
                    {
                        foreach (var suggestion in switchDef.ValueSuggestions)
                        {
                            if (string.IsNullOrEmpty(incomplete.PartialValue) || suggestion.StartsWith(incomplete.PartialValue))
                            {
                                suggestions.Add(new Suggestion(
                                    SuggestionType.Value,
                                    suggestion.Value,
                                    suggestion.Description,
                                    incomplete.Position + incomplete.PartialName.Length + 2,
                                    incomplete.PartialValue?.Length ?? 0
                                ));
                            }
                        }
                    }
                }
            }
        }

        return suggestions;
    }

    private List<Suggestion> GenerateAvailableSwitchSuggestions()
    {
        return [.. this._switches.Select(sw => new Suggestion(
            Type: SuggestionType.Switch,
            CompletionText: this._switchPrefix + sw.Name + (sw.HasArgument ? this._valueSeparator : ""),
            Description: sw.Description,
            ReplaceStart: 0,
            ReplaceLength: 0
        ))];
    }

    private SwitchParseContext ProcessSwitchMatch(
        Match match,
        TOptions options,
        List<ParseError> errors,
        List<ParseWarning> warnings,
        int cursorPosition)
    {
        var context = new SwitchParseContext();
        var switchName = match.Groups["name"].Value;
        var switchValue = match.Groups["value"].Value;

        // Check if cursor is within this match
        var isActive = cursorPosition >= match.Index && cursorPosition <= match.Index + match.Length;

        if (string.IsNullOrEmpty(switchName))
        {
            if (!isActive) // Only report error if not actively typing
            {
                var errorMessage = string.IsNullOrEmpty(switchValue)
                    ? $"Missing switch name after '{this._switchPrefix}'"
                    : $"Missing switch name in '{this._switchPrefix}{(string.IsNullOrEmpty(switchValue) ? "" : this._valueSeparator + switchValue)}'";

                errors.Add(new ParseError(ParseErrorType.MissingSwitchName, errorMessage, match.Index, match.Length));
            }

            return context;
        }

        if (!IsValidSwitchName(switchName))
        {
            var error = new ParseError(
                ParseErrorType.InvalidSwitchName,
                $"Invalid switch name '{this._switchPrefix}{switchName}' - switch names should contain only letters, numbers, and hyphens",
                match.Index + this._switchPrefix.Length,
                match.Length);

            if (isActive)
            {
                warnings.Add(new ParseWarning(error.Message, error.Position, error.Length));
            }
            else
            {
                errors.Add(error);
            }

            return context;
        }

        var switchDef = this.FindSwitchDefinition(switchName, out var suggestion);

        if (switchDef == null)
        {
            // For unknown switches, provide suggestions but don't error if actively typing

            var suggestions = isActive
                ? this.GenerateSwitchSuggestions(new IncompleteSwitchInfo(switchName, false, null, match.Index))
                :  [];

            if (isActive && suggestions.Count > 0)
            {
                context.CurrentSuggestions.AddRange(suggestions);
            }
            else
            {
                var errorMessage = suggestion != null
                    ? $"Unknown switch '{this._switchPrefix}{switchName}' - did you mean '{this._switchPrefix}{suggestion}'?"
                    : $"Unknown switch '{this._switchPrefix}{switchName}'. Available switches: {string.Join(", ", this._switches.Select(static s => s.Name))}";

                errors.Add(new ParseError(ParseErrorType.UnknownSwitch, errorMessage, match.Index, match.Length));
            }

            return context;
        }

        // Handle aliases
        if (switchDef.Aliases != null && switchDef.Aliases.TryGetValue(switchValue, out var value))
        {
            switchValue = value;
        }

        // Check for missing required value
        if (switchDef.HasArgument && string.IsNullOrEmpty(switchValue))
        {
            var suggestions = isActive
                ? this.GenerateSwitchSuggestions(new IncompleteSwitchInfo(switchName, true, "", match.Index))
                : [];

            if (isActive && suggestions.Count > 0)
            {
                // Provide value suggestions instead of error
                context.CurrentSuggestions.AddRange(suggestions);
            }
            else
            {
                var validValues = switchDef.Aliases is { Count: > 0 }
                    ? $"Expected values: {string.Join(", ", switchDef.Aliases.Keys.Take(5))}..."
                    : "Expected: value required";

                errors.Add(new ParseError(
                    ParseErrorType.MissingArgument,
                    $"Switch '{this._switchPrefix}{switchName}' requires a value. {validValues}",
                    match.Index,
                    match.Length));
            }

            return context;
        }

        if (!switchDef.HasArgument && !string.IsNullOrEmpty(switchValue))
        {
            errors.Add(new ParseError(
                ParseErrorType.UnexpectedArgument,
                $"Switch '{this._switchPrefix}{switchName}' does not accept a value",
                match.Index,
                match.Length));
            return context;
        }

        // Validate the value if validator is provided
        if (switchDef.Validator != null && !string.IsNullOrEmpty(switchValue))
        {
            var (isValid, errorMessage) = switchDef.Validator(switchValue);
            if (!isValid)
            {
                if (isActive)
                {
                    warnings.Add(new ParseWarning(errorMessage ?? "Invalid value", match.Index, match.Length));
                }
                else
                {
                    errors.Add(new ParseError(ParseErrorType.InvalidValue, errorMessage ?? "Invalid value", match.Index,
                        match.Length));
                }

                return context;
            }
        }

        // Execute the handler
        try
        {
            switchDef.Handler(options, switchValue);
        }
        catch (Exception ex)
        {
            errors.Add(new ParseError(ParseErrorType.InvalidValue, $"Error processing switch: {ex.Message}", match.Index, match.Length));
        }

        return context;
    }

    private SwitchDefinition<TOptions>? FindSwitchDefinition(string switchName, out string? suggestion)
    {
        suggestion = null;

        var exactMatch = this._switches.FirstOrDefault(s =>
            string.Equals(s.Name, switchName, StringComparison.OrdinalIgnoreCase));

        if (exactMatch != null)
        {
            return exactMatch;
        }

        suggestion = this.FindClosestSwitch(switchName);
        return null;
    }

    private string? FindClosestSwitch(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var bestMatch = string.Empty;
        var bestScore = int.MaxValue;

        foreach (var switchDef in this._switches)
        {
            var score = CalculateLevenshteinDistance(input.ToLowerInvariant(), switchDef.Name.ToLowerInvariant());

            if (score < bestScore && score <= Math.Max(1, Math.Min(input.Length, switchDef.Name.Length) / 2))
            {
                bestScore = score;
                bestMatch = switchDef.Name;
            }
        }

        return string.IsNullOrEmpty(bestMatch) ? null : bestMatch;
    }

    private static int CalculateLevenshteinDistance(string? source, string? target)
    {
        if (string.IsNullOrEmpty(source))
        {
            return target?.Length ?? 0;
        }

        if (string.IsNullOrEmpty(target))
        {
            return source.Length;
        }

        var matrix = new int[source.Length + 1, target.Length + 1];

        for (var i = 0; i <= source.Length; i++)
        {
            matrix[i, 0] = i;
        }

        for (var j = 0; j <= target.Length; j++)
        {
            matrix[0, j] = j;
        }

        for (var i = 1; i <= source.Length; i++)
        {
            for (var j = 1; j <= target.Length; j++)
            {
                var cost = source[i - 1] == target[j - 1] ? 0 : 1;
                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }

        return matrix[source.Length, target.Length];
    }

    private static bool IsValidSwitchName(string name)
    {
        return !string.IsNullOrEmpty(name) &&
               name.All(static c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
    }

    /// <summary>
    /// Gets help text for all registered switches
    /// </summary>
    public string GetHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available switches:");

        foreach (var sw in this._switches.OrderBy(static s => s.Name))
        {
            sb.Append(CultureInfo.CurrentCulture, $"  {this._switchPrefix}{sw.Name}");

            if (sw.HasArgument)
            {
                sb.Append(CultureInfo.CurrentCulture, $"{this._valueSeparator}<value>");
            }

            if (!string.IsNullOrEmpty(sw.Description))
            {
                sb.Append(CultureInfo.CurrentCulture, $" - {sw.Description}");
            }

            if (sw.Aliases?.Count > 0)
            {
                sb.Append(CultureInfo.CurrentCulture, $" (aliases: {string.Join(", ", sw.Aliases.Keys.Take(5))})");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    [GeneratedRegex(@"\s+", RegexOptions.Compiled)]
    private static partial Regex WhitespaceRegex();

    private static (bool isValid, string? errorMessage) ValidateExactSuggestion(string? val, SwitchValueDefinition[]? valueSuggestions)
    {
        var isValid = valueSuggestions?.Any(t => t.Value == val) ?? false;
        return (isValid, isValid ? null : "Invalid value. Expected one of: " + string.Join(", ", valueSuggestions?.Select(static t => t.Value) ?? []));
    }
}