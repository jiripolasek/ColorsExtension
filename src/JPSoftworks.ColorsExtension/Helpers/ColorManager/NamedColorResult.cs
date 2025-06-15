// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal sealed class NamedColorResult
{
    public bool Success { get; }
    public string? ColorName { get; }
    public IColorSet? ColorSetObject { get; }
    public RgbColor? Rgb { get; }

    private NamedColorResult(bool success, string? colorName, RgbColor? rgb, IColorSet? colorSetObject)
    {
        this.Success = success;
        this.ColorName = colorName;
        this.Rgb = rgb;
        this.ColorSetObject = colorSetObject;
    }

    public static NamedColorResult Ok(string colorName, RgbColor rgb, IColorSet colorSetObject)
    {
        return new NamedColorResult(true, colorName, rgb, colorSetObject);
    }

    public static NamedColorResult Fail()
    {
        return new NamedColorResult(false, null, null, null);
    }

    public string GetQueryName()
    {
        if (!this.Success)
        {
            return "";
        }

        return $"""
                "{this.ColorName}" /palette:{this.ColorSetObject?.Id}
                """;
    }

    public string GetQualifiedName()
    {
        if (!this.Success)
        {
            return "";
        }

        return $"{this.ColorName} ({this.ColorSetObject?.Name})";
    }
}