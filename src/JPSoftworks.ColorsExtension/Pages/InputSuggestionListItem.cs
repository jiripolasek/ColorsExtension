// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Commands;
using JPSoftworks.ColorsExtension.Helpers.QueryParser;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class InputSuggestionListItem : ListItem
{
    public InputSuggestionListItem(Suggestion suggestion, IDynamicListPage target)
    {
        ArgumentNullException.ThrowIfNull(suggestion);
        ArgumentNullException.ThrowIfNull(target);

        this.Title = suggestion.CompletionText;
        this.Subtitle = suggestion.Description ?? string.Empty;

        // TextToSuggest is the reason why the final search text is precalculated
        // and not applied as update action after the command is executed.
        this.TextToSuggest = ApplySuggestion(suggestion, target.SearchText ?? string.Empty);
        this.Command = new UpdateSearchTextCommand(this.TextToSuggest, target) { Name = "Apply" };
    }

    /// <summary>
    /// Applies the suggestion to the current search text, handling text replacement and formatting.
    /// </summary>
    /// <param name="suggestion">The suggestion to apply</param>
    /// <param name="searchText">The current search text</param>
    /// <returns>The updated search text with the suggestion applied</returns>
    private static string ApplySuggestion(Suggestion suggestion, string searchText)
    {
        var completionTextWithSuffix = GetCompletionTextWithSuffix(suggestion, searchText);

        return suggestion.ReplaceStart >= 0
            ? ReplaceTextAtPosition(searchText, suggestion, completionTextWithSuffix)
            : AppendToSearchText(searchText, completionTextWithSuffix);
    }

    /// <summary>
    /// Gets the completion text with appropriate suffix (e.g., space for values).
    /// </summary>
    private static string GetCompletionTextWithSuffix(Suggestion suggestion, string searchText)
    {
        return suggestion.Type != SuggestionType.Value || ShouldNotAddSpace(suggestion, searchText)
            ? suggestion.CompletionText
            : suggestion.CompletionText + " ";
    }

    /// <summary>
    /// Determines if a space should not be added after a value suggestion.
    /// </summary>
    private static bool ShouldNotAddSpace(Suggestion suggestion, string searchText)
    {
        // Don't add space if completion text already ends with one
        if (suggestion.CompletionText.EndsWith(' '))
        {
            return true;
        }

        // Don't add space if there's already whitespace after the replacement position
        var positionAfterReplacement = suggestion.ReplaceStart + suggestion.ReplaceLength;
        return positionAfterReplacement < searchText.Length &&
               char.IsWhiteSpace(searchText[positionAfterReplacement]);
    }

    /// <summary>
    /// Replaces text at a specific position in the search text.
    /// </summary>
    private static string ReplaceTextAtPosition(string searchText, Suggestion suggestion, string replacementText)
    {
        var replaceLength = CalculateActualReplaceLength(suggestion, searchText);
        var safeReplaceLength = Math.Min(replaceLength, searchText.Length - suggestion.ReplaceStart);

        if (suggestion.ReplaceStart >= searchText.Length)
        {
            return searchText + replacementText;
        }

        return searchText.Remove(suggestion.ReplaceStart, safeReplaceLength)
            .Insert(suggestion.ReplaceStart, replacementText);
    }

    /// <summary>
    /// Appends completion text to the trimmed search text.
    /// </summary>
    private static string AppendToSearchText(string searchText, string completionText)
    {
        return searchText.TrimEnd() + completionText;
    }

    /// <summary>
    /// Calculates the actual length of text to replace based on suggestion type.
    /// </summary>
    private static int CalculateActualReplaceLength(Suggestion suggestion, string searchText)
    {
        return suggestion.Type == SuggestionType.Switch
            ? Math.Min(suggestion.ReplaceLength + 1, searchText.Length - suggestion.ReplaceStart)
            : suggestion.ReplaceLength;
    }
}