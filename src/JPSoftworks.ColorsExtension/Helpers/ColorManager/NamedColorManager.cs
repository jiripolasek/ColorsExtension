using System;
using System.Collections.Generic;

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public class NamedColorManager
{
    // Ordered list of color sets - first one is searched first
    private readonly List<IColorSet> _colorSets;

    // Reverse lookup cache for performance
    private Dictionary<(int r, int g, int b), NamedColorResult>? _reverseCache;

    public NamedColorManager()
    {
        this._colorSets = [];
        this.InitializeDefaultColorSets();
    }

    private void InitializeDefaultColorSets()
    {
        // Add default color sets in priority order
        this.AddColorSet(new WebColorSet());
        this.AddColorSet(new PopularColorSet());
        this.AddColorSet(new MaterialColorSet());
        this.AddColorSet(new PantoneColorSet());
        this.AddColorSet(new FluentColorSet());
    }

    // Add a color set (can be one of the concrete classes or a custom set)
    public void AddColorSet(IColorSet colorSet)
    {
        this._colorSets.Add(colorSet);
        this._reverseCache = null; // Invalidate cache
    }

    // Add a new custom color set from dictionary
    public void AddColorSet(string name, Dictionary<string, (int r, int g, int b)> colors)
    {
        this.AddColorSet(new CustomColorSet(name, colors));
    }

    // Remove a color set by name
    public void RemoveColorSet(string setName)
    {
        this._colorSets.RemoveAll(cs => cs.Name.Equals(setName, StringComparison.OrdinalIgnoreCase));
        this._reverseCache = null; // Invalidate cache
    }

    // Look up a color by name (returns first match in order)
    public NamedColorResult GetColorByName(string name)
    {
        foreach (var colorSet in this._colorSets)
        {
            if (colorSet.Colors.TryGetValue(name, out var rgb))
            {
                return NamedColorResult.Ok(name, colorSet.Name, rgb);
            }
        }

        return NamedColorResult.Fail();
    }

    // Look up a name by RGB values (returns first match in order)
    public NamedColorResult GetNameByRgb(int r, int g, int b)
    {
        // Use cache if available
        if (this._reverseCache != null && this._reverseCache.TryGetValue((r, g, b), out var cached))
        {
            return cached;
        }

        // Search all color sets in order
        foreach (var colorSet in this._colorSets)
        {
            foreach (var kvp in colorSet.Colors)
            {
                if (kvp.Value.r == r && kvp.Value.g == g && kvp.Value.b == b)
                {
                    var result = NamedColorResult.Ok(kvp.Key, colorSet.Name, (r, g, b));

                    // Cache the result
                    if (this._reverseCache == null)
                        this._reverseCache = new Dictionary<(int, int, int), NamedColorResult>();
                    this._reverseCache[(r, g, b)] = result;

                    return result;
                }
            }
        }

        return NamedColorResult.Fail();
    }

    // Find the closest named color by RGB distance
    public NamedColorResult GetClosestNamedColor(int r, int g, int b)
    {
        NamedColorResult? closest = null;
        double minDistance = double.MaxValue;

        foreach (var colorSet in this._colorSets)
        {
            foreach (var kvp in colorSet.Colors)
            {
                var distance = Math.Sqrt(
                    Math.Pow(r - kvp.Value.r, 2) +
                    Math.Pow(g - kvp.Value.g, 2) +
                    Math.Pow(b - kvp.Value.b, 2)
                );

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = NamedColorResult.Ok(kvp.Key, colorSet.Name, kvp.Value);
                }
            }
        }

        return closest ?? NamedColorResult.Fail();
    }

    // Get all color sets
    public IReadOnlyList<IColorSet> GetAllColorSets()
    {
        return this._colorSets.AsReadOnly();
    }

    // Get all color names in order of priority
    public IEnumerable<(string name, string setName, (int r, int g, int b) rgb)> GetAllColors()
    {
        foreach (var colorSet in this._colorSets)
        {
            foreach (var kvp in colorSet.Colors)
            {
                yield return (kvp.Key, colorSet.Name, kvp.Value);
            }
        }
    }
}