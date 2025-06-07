// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class HwbModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.HwbModern;

    public string Format(Unicolour color)
    {
        var hwb = color.Hwb;
        var h = Math.Round(hwb.H, 0);
        var w = Math.Round(hwb.W * 100, 1);
        var b = Math.Round(hwb.B * 100, 1);

        return $"hwb({h} {w.ToString(CultureInfo.InvariantCulture)}% {b.ToString(CultureInfo.InvariantCulture)}%)";
    }
}