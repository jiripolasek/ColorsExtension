// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class HexColorParser : IColorParser
{
    // Matches: #RGB, #RGBA, #RRGGBB, #RRGGBBAA, RGB, RGBA, RRGGBB, RRGGBBAA
    // Examples: #F00, #F00A, #FF0000, #FF0000AA, F00, F00A, FF0000, FF0000AA
    private static readonly Regex HexPattern = new(@"^#?([0-9A-Fa-f]{3,4}|[0-9A-Fa-f]{6,8})$", RegexOptions.Compiled);

    public ColorParseResult TryParse(string input)
    {
        var match = HexPattern.Match(input);
        if (!match.Success)
        {
            return ColorParseResult.Fail($"Invalid hex format: {input}");
        }

        var hex = match.Groups[1].Value;
        var hasHash = input.StartsWith("#", StringComparison.OrdinalIgnoreCase);

        try
        {
            if (hex.Length == 3 || hex.Length == 4)
            {
                // Expand short hex (RGB/RGBA)
                var r = int.Parse(string.Concat(hex.AsSpan(0, 1), hex.AsSpan(0, 1)), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture) / 255.0;
                var g = int.Parse(string.Concat(hex.AsSpan(1, 1), hex.AsSpan(1, 1)), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture) / 255.0;
                var b = int.Parse(string.Concat(hex.AsSpan(2, 1), hex.AsSpan(2, 1)), NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture) / 255.0;

                // Ignore alpha for now as requested
                var format = hasHash ? ParsedColorFormat.HexShort : ParsedColorFormat.HexWithoutHash;
                return ColorParseResult.Ok(new Unicolour(ColourSpace.Rgb, r, g, b), format);
            }
            else
            {
                // Long hex (RRGGBB/RRGGBBAA)
                var r = int.Parse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
                var g = int.Parse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;
                var b = int.Parse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture) / 255.0;

                // Ignore alpha for now as requested
                var format = hasHash ? ParsedColorFormat.HexLong : ParsedColorFormat.HexWithoutHash;
                return ColorParseResult.Ok(new Unicolour(ColourSpace.Rgb, r, g, b), format);
            }
        }
        catch (Exception ex)
        {
            return ColorParseResult.Fail($"Error parsing hex color: {ex.Message}");
        }
    }
}