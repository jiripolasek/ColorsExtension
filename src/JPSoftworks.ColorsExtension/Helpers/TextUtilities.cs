// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

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
}