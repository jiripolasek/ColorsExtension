// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class HsvColorParser : IColorParser
{
    // Matches: hsv(h, s%, v%) or hsva(h, s%, v%, a)
    // Examples: hsv(0, 100%, 100%), hsva(0, 100%, 100%, 0.5)
    private static readonly Regex HsvPattern = new(
        @"^hsva?\s*\(\s*(\d+(\.\d+)?)\s*,\s*(\d+(\.\d+)?)%\s*,\s*(\d+(\.\d+)?)%(?:\s*,\s*([0-9.]+))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = HsvPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid HSV format: {input}");
        }

        try
        {
            var h = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var s = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture) / 100.0;
            var v = double.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture) / 100.0;

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Hsb, h, s, v), ParsedColorFormat.Hsv);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing HSV color: {ex.Message}");
        }
    }
}