// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal sealed class CustomColorSet : IColorSet
{
    public string Id { get; }
    public string Name { get; }
    public IReadOnlyDictionary<string, RgbColor> Colors { get; }

    public CustomColorSet(string id, string name, Dictionary<string, RgbColor> colors)
    {
        this.Id = id;
        this.Name = name;
        this.Colors = colors;
    }
}