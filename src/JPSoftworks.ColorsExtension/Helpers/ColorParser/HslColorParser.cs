using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class HslColorParser : IColorParser
{
    // Matches: hsl(h, s%, l%) or hsla(h, s%, l%, a)
    // Examples: hsl(0, 100%, 50%), hsla(0, 100%, 50%, 0.5)
    private static readonly Regex HslPattern = new Regex(
        @"^hsla?\s*\(\s*(\d+(\.\d+)?)\s*,\s*(\d+(\.\d+)?)%\s*,\s*(\d+(\.\d+)?)%(?:\s*,\s*([0-9.]+))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = HslPattern.Match(input);
        if (!match.Success)
            return ColorParseResult.Fail($"Invalid HSL format: {input}");

        try
        {
            var h = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var s = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture) / 100.0;
            var l = double.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture) / 100.0;

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Hsl, h, s, l), ParsedColorFormat.Hsl);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing HSL color: {ex.Message}");
        }
    }
}