using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class RgbModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.RgbModern;

    public string Format(Unicolour color)
    {
        var rgb = color.Rgb;
        var r = (int)Math.Round(rgb.R * 255);
        var g = (int)Math.Round(rgb.G * 255);
        var b = (int)Math.Round(rgb.B * 255);

        return $"rgb({r} {g} {b})";
    }
}