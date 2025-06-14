// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Text.RegularExpressions;

namespace JPSoftworks.ColorsExtension.Helpers;

internal static class TextUtilities
{
    /// <summary>
    /// Finds the index in the "after" string where the caret should be placed (as if the user had typed the text).
    /// </summary>
    /// <param name="before">The original text before the change.</param>
    /// <param name="after">The modified text after the change.</param>
    /// <returns> The index in the "after" string where the caret should be placed, or -1 if there was no change.</returns>
    internal static int FindLastCommonStringIndex(string before, string after)
    {
        ArgumentNullException.ThrowIfNull(before);
        ArgumentNullException.ThrowIfNull(after);

        // If no change at all, signal with -1
        if (before.Equals(after, StringComparison.Ordinal))
            return -1;

        // 1) Find common prefix length
        var prefix = 0;
        var minLen = Math.Min(before.Length, after.Length);
        while (prefix < minLen && before[prefix] == after[prefix])
            prefix++;

        // 2) Find common suffix length (but don't overlap the prefix)
        var suffix = 0;
        while (suffix < before.Length - prefix
               && suffix < after.Length - prefix
               && before[before.Length - suffix - 1] == after[after.Length - suffix - 1])
        {
            suffix++;
        }

        // 3) Caret goes to the end of the changed segment in "after"
        //    which is at index (after.Length - suffix).
        return after.Length - suffix;
    }

    /// <summary>
    /// Removes all switches with the specified name from the query string
    /// </summary>
    /// <param name="query">The query string to process</param>
    /// <param name="switchName">The name of the switch to remove (without the '/' prefix)</param>
    /// <returns>The query string with all specified switches removed</returns>
    public static string RemoveSwitches(string query, string switchName)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(switchName);

        if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(switchName))
        {
            return query;
        }

        // 1. /switchName followed by space or end of string
        // 2. /switchName:value (unquoted value without spaces) followed by space or end of string
        // 3. /switchName:"value with spaces" (quoted value that can contain spaces)
        var pattern = $"""\s*/({Regex.Escape(switchName)})(?::(?:"[^"]*"|[^\s"]*)?)?(?=\s|$)""";
        var result = Regex.Replace(query, pattern, "", RegexOptions.IgnoreCase);
        return result.Trim();
    }
}