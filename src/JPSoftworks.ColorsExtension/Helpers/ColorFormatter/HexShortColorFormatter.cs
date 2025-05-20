using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

// Interface for individual format converters

// Formatter for hex colors (short format)
public class HexShortColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.HexShort;

    public string Format(Unicolour color)
    {
        var rgb = color.Rgb;
        var r = (int)Math.Round(rgb.R * 255);
        var g = (int)Math.Round(rgb.G * 255);
        var b = (int)Math.Round(rgb.B * 255);

        // Check if it can be represented as short hex (only if each component can be represented as single hex digit)
        if (r % 17 == 0 && g % 17 == 0 && b % 17 == 0)
        {
            return $"#{r / 17:X}{g / 17:X}{b / 17:X}";
        }

        // Fallback to long format if can't be represented as short
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}