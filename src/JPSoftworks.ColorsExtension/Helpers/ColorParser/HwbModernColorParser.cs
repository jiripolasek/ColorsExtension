using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class HwbModernColorParser : IColorParser
{
    // Matches: hwb(h w% b%) or hwb(h w% b% / a)
    // Examples: hwb(0 0% 0%), hwb(0 0% 0% / 0.5)
    private static readonly Regex HwbPattern = new Regex(
        @"^hwb\s*\(\s*(\d+(?:\.\d+)?)\s+(\d+(?:\.\d+)?)%\s+(\d+(?:\.\d+)?)%(?:\s*\/\s*(\d+(?:\.\d+)?%?))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = HwbPattern.Match(input);
        if (!match.Success)
            return ColorParseResult.Fail($"Invalid HWB format: {input}");

        try
        {
            var h = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var w = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture) / 100.0;
            var b = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture) / 100.0;

            // Use ColourSpace.Hwb directly - Unicolour supports it
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Hwb, h, w, b), ParsedColorFormat.HwbModern);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing HWB color: {ex.Message}");
        }
    }
}