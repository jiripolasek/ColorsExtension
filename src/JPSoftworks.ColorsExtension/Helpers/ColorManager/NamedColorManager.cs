// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using FuzzySharp;

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal class NamedColorManager
{
    private const double NameWeight = 0.7;
    private const double PaletteWeight = 0.3;

    private readonly List<IColorSet> _colorSets;

    private readonly List<SearchEntry> _searchEntries = [];
    private Dictionary<RgbColor, List<NamedColorResult>>? _reverseCache;

    public NamedColorManager()
    {
        this._colorSets = [];
        this.InitializeDefaultColorSets();
        this.InitializeSearchMap();
    }

    private void InitializeSearchMap()
    {
        foreach (var colorSet in this._colorSets)
        {
            foreach (var (key, color) in colorSet.Colors)
            {
                this._searchEntries.Add(new SearchEntry(key, NormalizeKey(key), colorSet, color));
            }
        }
    }

    private static string NormalizeKey(string key)
    {
        return key.Replace('_', ' ').Replace('-', ' ').Trim().ToLowerInvariant();
    }

    private void InitializeDefaultColorSets()
    {
        this.AddColorSet(new WebColorSet());
        this.AddColorSet(new PopularColorSet());
        this.AddColorSet(new MaterialColorSet());
        this.AddColorSet(new PantoneColorSet());
        this.AddColorSet(new FluentColorSet());
    }

    private void AddColorSet(IColorSet colorSet)
    {
        if (this._colorSets.Any(cs => cs.Id.Equals(colorSet.Id, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"Color set with ID '{colorSet.Id}' already exists.");
        }

        this._colorSets.Add(colorSet);
        this._reverseCache = null;
    }

    public ICollection<NamedColorResult> GetColorByName(
        string query,
        int limit = 30,
        int cutoff = 50,
        string? palette = null)
    {
        if (string.IsNullOrWhiteSpace(query))
            return [];

        query = query.Trim().ToLowerInvariant();

        var isExact = query.Contains('"');

        if (isExact)
        {
            query = query.Replace("\"", "");
            query = NormalizeKey(query);
            return this.ExactSearch(query, palette);
        }

        var result = this.FuzzySearch(NormalizeKey(query), limit, cutoff, palette);

        return result;
    }

    private List<NamedColorResult> ExactSearch(string query, string? palette)
    {
        return this._searchEntries.Where(t => palette == null || t.ColorSet.Id == palette)
            .Where(t => t.SearchKey == query)
            .Select(static exactMatch => NamedColorResult.Ok(exactMatch.ColorSet.Name, exactMatch.Color, exactMatch.ColorSet))
            .ToList();
    }

    private List<NamedColorResult> FuzzySearch(string query, int limit, int cutoff, string? palette)
    {
        IEnumerable<SearchEntry> searchPool = this._searchEntries;

        string? boostedPalette = null;
        if (palette == null)
        {
            this.TryGetPalette(query, out boostedPalette, out query);
        }

        if (palette != null)
        {
            searchPool = this._searchEntries.Where(t => t.ColorSet.Id == palette);
        }

        var result =
            searchPool.Select(e =>
                {
                    int nameScore = ScoreName(query, e.SearchKey);
                    int paletteScore = ScorePalette(query, boostedPalette, e);
                    int combined = (int)Math.Round(nameScore * NameWeight + paletteScore * PaletteWeight);
                    return (Entry: e, Score: combined, Name: nameScore, Palette: paletteScore);
                })
                .Where(searchResult => searchResult.Score >= cutoff)
                .OrderByDescending(static x => x.Score)
                .ThenByDescending(static x => x.Name)
                .Take(limit)
                .Select(static searchResult => NamedColorResult.Ok(
                    // $"{searchResult.Entry.ColorName} (score: name {searchResult.Name}, pal {searchResult.Palette}, com {searchResult.Score})",
                    searchResult.Entry.ColorName,
                    searchResult.Entry.Color,
                    searchResult.Entry.ColorSet))
                .ToList();
        return result;
    }

    private static int ScorePalette(string query, string? boostedPalette, SearchEntry searchEntry)
    {
        var score = boostedPalette == searchEntry.ColorSet.Id ? 80 : Fuzz.PartialRatio(query, searchEntry.ColorSet.Id);
        return score >= 80 ? score : score / 2;
    }

    private void TryGetPalette(string query, out string? paletteId, out string cleanedQuery)
    {
        var words = query
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var tempPaletteId = this._colorSets.Select(static cs => cs.Id).FirstOrDefault(id => words.Contains(id));
        paletteId = tempPaletteId;

        if (string.IsNullOrEmpty(paletteId))
        {
            cleanedQuery = query;
            return;
        }

        var cleanedWords = words.Where(word => word != tempPaletteId);
        cleanedQuery = string.Join(' ', cleanedWords);
    }

    private static int ScoreName(string query, string colorName)
    {
        if (colorName == query)
            return 110;

        // shades of the color usually ends with the color name (e.g. dark red, forest green)
        if (colorName.EndsWith(query, StringComparison.Ordinal)) // both sides already lowered
            return 95;

        // names starting with the color name are usually mixed with another (blueviolet, yellowgreen, greenyellow)
        if (colorName.StartsWith(query, StringComparison.Ordinal)) // both sides already lowered
            return 90;

        if (colorName.Contains(query))
            return 80;

        return Fuzz.TokenSetRatio(query, colorName);
    }

    public IEnumerable<NamedColorResult> GetNameByRgb(int r, int g, int b)
    {
        var results = new List<NamedColorResult>();

        // Use cache if available
        if (this._reverseCache != null && this._reverseCache.TryGetValue(new RgbColor(r, g, b), out var cached))
        {
            return cached;
        }

        // Search all color sets in order
        foreach (var colorSet in this._colorSets)
        {
            foreach (var kvp in colorSet.Colors)
            {
                if (kvp.Value.R == r && kvp.Value.G == g && kvp.Value.B == b)
                {
                    var result = NamedColorResult.Ok(kvp.Key, (r, g, b), colorSet);
                    results.Add(result);
                }
            }
        }

        // Cache the results
        if (results.Count > 0)
        {
            this._reverseCache ??= new();
            this._reverseCache[new RgbColor(r, g, b)] = results;
        }

        return results;
    }

    public NamedColorResult GetClosestNamedColor(int r, int g, int b)
    {
        NamedColorResult? closest = null;
        var minDistance = double.MaxValue;

        foreach (var colorSet in this._colorSets)
        {
            foreach (var kvp in colorSet.Colors)
            {
                var distance = Math.Sqrt(
                    Math.Pow(r - kvp.Value.R, 2) +
                    Math.Pow(g - kvp.Value.G, 2) +
                    Math.Pow(b - kvp.Value.B, 2)
                );

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = NamedColorResult.Ok(kvp.Key, kvp.Value, colorSet);
                }
            }
        }

        return closest ?? NamedColorResult.Fail();
    }

    record SearchEntry(string ColorName, string SearchKey, IColorSet ColorSet, RgbColor Color);
}