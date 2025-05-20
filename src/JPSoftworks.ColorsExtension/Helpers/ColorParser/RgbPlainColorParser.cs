using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class RgbPlainColorParser : IColorParser
{
    // Matches: "r, g, b", "r,g,b,a", "r g b", "r g b / a"  
    // Examples: "255, 0, 0", "255,0,0,0.5", "255 0 0", "255 0 0 / 0.5"
    private static readonly Regex[] Patterns =
    [
        // Comma-separated with spaces
        new(@"^(\d+(?:\.\d+)?%?)\s*,\s*(\d+(?:\.\d+)?%?)\s*,\s*(\d+(?:\.\d+)?%?)(?:\s*,\s*(\d+(?:\.\d+)?%?))?\s*$",
            RegexOptions.Compiled),

        // Space-separated with optional slash for alpha
        new(@"^(\d+(?:\.\d+)?%?)\s+(\d+(?:\.\d+)?%?)\s+(\d+(?:\.\d+)?%?)(?:\s*\/\s*(\d+(?:\.\d+)?%?))?\s*$",
            RegexOptions.Compiled)
    ];

    public ColorParseResult TryParse(string input)
    {
        // Skip if it looks like a function call
        if (input.Contains("(", StringComparison.InvariantCultureIgnoreCase))
        {
            return ColorParseResult.Fail($"Not a plain RGB format: {input}");
        }

        foreach (var pattern in Patterns)
        {
            var match = pattern.Match(input);
            if (match.Success)
            {
                try
                {
                    var r = ParseValueOrPercentage(match.Groups[1].Value, 255);
                    var g = ParseValueOrPercentage(match.Groups[2].Value, 255);
                    var b = ParseValueOrPercentage(match.Groups[3].Value, 255);

                    // Alpha is parsed but ignored for now as requested
                    return ColorParseResult.Ok(new Unicolour(ColourSpace.Rgb, r, g, b), ParsedColorFormat.RgbPlain);
                }
                catch (Exception ex)
                {
                    return ColorParseResult.Fail($"Error parsing plain RGB color: {ex.Message}");
                }
            }
        }

        return ColorParseResult.Fail($"Invalid plain RGB format: {input}");
    }

    private static double ParseValueOrPercentage(string value, double maxValue)
    {
        if (value.EndsWith("%", StringComparison.InvariantCultureIgnoreCase))
        {
            return double.Parse(value.TrimEnd('%'), CultureInfo.InvariantCulture) / 100.0;
        }

        return double.Parse(value, CultureInfo.InvariantCulture) / maxValue;
    }
}