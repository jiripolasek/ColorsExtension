// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class CmykModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.CmykModern;

    public string Format(Unicolour color)
    {
        var cmyk = color.Icc;
        var c = Math.Round(cmyk.Values[0] * 100, 0);
        var m = Math.Round(cmyk.Values[1] * 100, 0);
        var y = Math.Round(cmyk.Values[2] * 100, 0);
        var k = Math.Round(cmyk.Values[3] * 100, 0);
        return string.Format(CultureInfo.InvariantCulture, "device-cmyk({0}% {1}% {2}% {3}%)", c, m, y, k);
    }
}