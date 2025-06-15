// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class LabModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.LabModern;

    public string Format(Unicolour color)
    {
        var lab = color.Lab;
        var l = Math.Round(lab.L, 2);
        var a = Math.Round(lab.A, 2);
        var b = Math.Round(lab.B, 2);

        return string.Format(CultureInfo.InvariantCulture, "lab({0}% {1} {2})", l, a, b);
    }
}