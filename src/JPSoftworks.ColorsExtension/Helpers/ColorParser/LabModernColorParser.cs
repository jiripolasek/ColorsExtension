// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class LabModernColorParser : IColorParser
{
    // Matches: lab(l% a b) or lab(l% a b / alpha)
    // Examples: lab(50% 40 30), lab(50% 40 30 / 0.5)
    private static readonly Regex LabPattern = new(
        @"^lab\s*\(\s*(\d+(?:\.\d+)?%?)\s+(-?\d+(?:\.\d+)?)\s+(-?\d+(?:\.\d+)?)(?:\s*\/\s*(\d+(?:\.\d+)?%?))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = LabPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid LAB format: {input}");
        }

        try
        {
            var l = ParseLightness(match.Groups[1].Value);
            var a = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            var b = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Lab, l, a, b), ParsedColorFormat.LabModern);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing LAB color: {ex.Message}");
        }
    }

    private static double ParseLightness(string value)
    {
        if (value.EndsWith("%"))
        {
            // For L* in LAB, 0% = 0, 100% = 100
            return double.Parse(value.TrimEnd('%'), CultureInfo.InvariantCulture);
        }

        return double.Parse(value, CultureInfo.InvariantCulture);
    }
}