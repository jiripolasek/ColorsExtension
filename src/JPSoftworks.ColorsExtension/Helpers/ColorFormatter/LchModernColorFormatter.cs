// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class LchModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.LchModern;

    public string Format(Unicolour color)
    {
        var lch = color.Lchab;
        var l = Math.Round(lch.L, 2);
        var c = Math.Round(lch.C, 2);
        var h = Math.Round(lch.H, 2);

        return string.Format(CultureInfo.InvariantCulture, "lch({0}% {1} {2})", l, c, h);
    }
}