// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class RgbPlainColorFormatter : IColorFormatter
{
    private readonly bool _useSpaces;

    public RgbPlainColorFormatter(bool useSpaces = false)
    {
        this._useSpaces = useSpaces;
    }

    public ParsedColorFormat TargetFormat => ParsedColorFormat.RgbPlain;

    public string Format(Unicolour color)
    {
        var rgb = color.Rgb;
        var r = (int)Math.Round(rgb.R * 255);
        var g = (int)Math.Round(rgb.G * 255);
        var b = (int)Math.Round(rgb.B * 255);

        return this._useSpaces ? $"{r} {g} {b}" : $"{r}, {g}, {b}";
    }
}