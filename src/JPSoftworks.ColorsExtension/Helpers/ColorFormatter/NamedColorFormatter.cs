// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

internal class NamedColorFormatter : IColorFormatter
{
    private readonly NamedColorManager _colorManager;
    private readonly bool _includeSetName;

    public NamedColorFormatter(NamedColorManager? colorManager = null, bool includeSetName = true)
    {
        this._colorManager = colorManager ?? new NamedColorManager();
        this._includeSetName = includeSetName;
    }

    public ParsedColorFormat TargetFormat => ParsedColorFormat.NamedColor;

    public string Format(Unicolour color)
    {
        var rgb = color.Rgb.Byte255;

        // Try exact match first
        var exactResult = this._colorManager.GetNameByRgb(rgb.R, rgb.G, rgb.B).ToList();
        if (exactResult.Count > 0)
        {
            return this._includeSetName ? exactResult.First().GetQualifiedName() : exactResult.First().ColorName!;
        }

        // Fall back to closest match
        var closestResult = this._colorManager.GetClosestNamedColor(rgb.R, rgb.G, rgb.B);
        if (closestResult.Success)
        {
            return this._includeSetName ? closestResult.GetQualifiedName() : closestResult.ColorName!;
        }

        // Fallback to hex if no named color found
        return $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
    }
}