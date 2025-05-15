using System.Collections.Generic;

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public interface IColorSet
{
    string Name { get; }
    IReadOnlyDictionary<string, (int r, int g, int b)> Colors { get; }
}