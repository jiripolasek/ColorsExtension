using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class RgbModernColorParser : IColorParser
{
    // Matches: rgb(r g b) or rgb(r g b / a)
    // Examples: rgb(255 0 0), rgb(255 0 0 / 0.5), rgb(100% 0% 0%)
    private static readonly Regex RgbModernPattern = new Regex(
        @"^rgba?\s*\(\s*(\d+(?:\.\d+)?%?)\s+(\d+(?:\.\d+)?%?)\s+(\d+(?:\.\d+)?%?)(?:\s*\/\s*(\d+(?:\.\d+)?%?))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = RgbModernPattern.Match(input);
        if (!match.Success)
            return ColorParseResult.Fail($"Invalid modern RGB format: {input}");

        try
        {
            var r = ParseValueOrPercentage(match.Groups[1].Value, 255);
            var g = ParseValueOrPercentage(match.Groups[2].Value, 255);
            var b = ParseValueOrPercentage(match.Groups[3].Value, 255);

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Rgb, r, g, b), ParsedColorFormat.RgbModern);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing modern RGB color: {ex.Message}");
        }
    }

    private static double ParseValueOrPercentage(string value, double maxValue)
    {
        if (value.EndsWith("%"))
        {
            return double.Parse(value.TrimEnd('%'), CultureInfo.InvariantCulture) / 100.0;
        }
        else
        {
            return double.Parse(value, CultureInfo.InvariantCulture) / maxValue;
        }
    }
}