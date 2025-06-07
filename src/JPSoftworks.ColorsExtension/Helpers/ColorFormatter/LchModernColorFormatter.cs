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
        var l = Math.Round(lch.L, 1);
        var c = Math.Round(lch.C, 1);
        var h = Math.Round(lch.H, 1);

        return
            $"lch({l.ToString(CultureInfo.InvariantCulture)}% {c.ToString(CultureInfo.InvariantCulture)} {h.ToString(CultureInfo.InvariantCulture)})";
    }
}