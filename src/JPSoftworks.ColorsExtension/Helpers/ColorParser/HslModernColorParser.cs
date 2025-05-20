using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class HslModernColorParser : IColorParser
{
    // Matches: hsl(h s% l%) or hsl(h s% l% / a)
    // Examples: hsl(0 100% 50%), hsl(0 100% 50% / 0.5)
    private static readonly Regex HslModernPattern = new(
        @"^hsla?\s*\(\s*(\d+(?:\.\d+)?)\s+(\d+(?:\.\d+)?)%\s+(\d+(?:\.\d+)?)%(?:\s*\/\s*(\d+(?:\.\d+)?%?))?\s*\)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ColorParseResult TryParse(string input)
    {
        var match = HslModernPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid modern HSL format: {input}");
        }

        try
        {
            var h = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            var s = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture) / 100.0;
            var l = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture) / 100.0;

            // Ignore alpha for now as requested
            return ColorParseResult.Ok(new Unicolour(ColourSpace.Hsl, h, s, l), ParsedColorFormat.HslModern);
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing modern HSL color: {ex.Message}");
        }
    }
}