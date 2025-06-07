// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class LchModernColorParser : IColorParser
{
    // Matches: lch(l% c h) or lch(l% c h / alpha)  
    // Examples: lch(50% 40 30), lch(50% 40 30 / 0.5)
    private static readonly Regex LchPattern = new(
        @"^lch\s*\(\s*(\d+(?:\.\d+)?%?)\s+(\d+(?:\.\d+)?)\s+(\d+(?:\.\d+)?)(?:\s*\/\s*(\d+(?:\.\d+)?%?))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = LchPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid LCH format: {input}");
        }

        try
        {
            var l = ParseLightness(match.Groups[1].Value);
            var c = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
            var h = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Lchab, l, c, h), ParsedColorFormat.LchModern);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing LCH color: {ex.Message}");
        }
    }

    private static double ParseLightness(string value)
    {
        if (value.EndsWith("%"))
        {
            // For L* in LCH, 0% = 0, 100% = 100
            return double.Parse(value.TrimEnd('%'), CultureInfo.InvariantCulture);
        }

        return double.Parse(value, CultureInfo.InvariantCulture);
    }
}