// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class RgbColorParser : IColorParser
{
    // Matches: rgb(r, g, b) or rgba(r, g, b, a)
    // Examples: rgb(255, 0, 0), rgba(255, 0, 0, 0.5)
    private static readonly Regex RgbPattern = new(
        @"^rgba?\s*\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)(?:\s*,\s*([0-9.]+))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = RgbPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid RGB format: {input}");
        }

        try
        {
            var r = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var g = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            var b = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Rgb255, r, g, b), ParsedColorFormat.Rgb);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing RGB color: {ex.Message}");
        }
    }
}