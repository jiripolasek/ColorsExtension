using System;
using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class HslModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.HslModern;

    public string Format(Unicolour color)
    {
        var hsl = color.Hsl;
        var h = Math.Round(hsl.H, 0);
        var s = Math.Round(hsl.S * 100, 1);
        var l = Math.Round(hsl.L * 100, 1);

        return $"hsl({h} {s.ToString(CultureInfo.InvariantCulture)}% {l.ToString(CultureInfo.InvariantCulture)}%)";
    }
}