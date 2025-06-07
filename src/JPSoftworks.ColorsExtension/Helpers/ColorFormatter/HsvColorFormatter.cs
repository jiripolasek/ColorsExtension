// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class HsvColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.Hsv;

    public string Format(Unicolour color)
    {
        var hsb = color.Hsb;
        var h = Math.Round(hsb.H, 0);
        var s = Math.Round(hsb.S * 100, 1);
        var v = Math.Round(hsb.B * 100, 1); // Note: B (brightness) is V (value)

        return $"hsv({h}, {s.ToString(CultureInfo.InvariantCulture)}%, {v.ToString(CultureInfo.InvariantCulture)}%)";
    }
}