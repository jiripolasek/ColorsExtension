using System;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class HexLongColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.HexLong;

    public string Format(Unicolour color)
    {
        var rgb = color.Rgb;
        var r = (int)Math.Round(rgb.R * 255);
        var g = (int)Math.Round(rgb.G * 255);
        var b = (int)Math.Round(rgb.B * 255);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
}