// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public sealed record SwitchDefinition<TOptions>(
    string Name,
    bool HasArgument,
    Action<TOptions, string?> Handler,
    Func<string?, (bool isValid, string? errorMessage)>? Validator,
    Dictionary<string, string>? Aliases,
    string? Description,
    IEnumerable<SwitchValueDefinition>? ValueSuggestions)
{
    public bool HasValueStartingWith(string text)
    {
        return this.ValueSuggestions?.Any(t => t.StartsWith(text)) == true;
    }

    public bool HasValue(string? text)
    {
        return this.ValueSuggestions?.Any(t => t.Value.Equals(text, StringComparison.OrdinalIgnoreCase)) == true;
    }
}

public sealed record SwitchValueDefinition(string Value, string Description)
{
    public bool StartsWith(string text)
    {
        return this.Value.StartsWith(text, StringComparison.OrdinalIgnoreCase);
    }
}