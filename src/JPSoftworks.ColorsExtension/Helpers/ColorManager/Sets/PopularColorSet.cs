// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal sealed class PopularColorSet : IColorSet
{
    private readonly Dictionary<string, RgbColor> _colors
        = new(StringComparer.OrdinalIgnoreCase)
        {
            // Modern and trendy color names
            { "mint", (189, 252, 201) },
            { "sky", (135, 206, 250) },
            { "rose", (255, 182, 193) },
            { "sage", (158, 169, 147) },
            { "blush", (255, 240, 241) },
            { "taupe", (72, 60, 50) },
            { "charcoal", (54, 69, 79) },
            { "seafoam", (93, 188, 210) },
            { "canary", (255, 239, 0) },
            { "wine", (114, 47, 55) },
            { "rust", (183, 65, 14) },
            { "cream", (255, 253, 208) },
            { "moss", (173, 223, 173) },
            { "dusk", (79, 90, 117) },
            { "dawn", (252, 247, 218) },
            { "storm", (130, 143, 155) },
            { "glacier", (134, 184, 204) },
            { "ash", (197, 200, 198) },
            { "sand", (237, 212, 160) },
            { "clay", (191, 118, 87) },
            { "copper", (184, 115, 51) },
            { "smoke", (115, 130, 118) },
            { "fog", (230, 232, 236) },
            { "mist", (217, 234, 243) },
            { "cloud", (209, 222, 251) },
            { "frost", (225, 244, 255) },
            { "ice", (199, 252, 243) },
            { "pearl", (234, 225, 215) },
            { "bone", (221, 204, 174) },
            { "parchment", (249, 240, 180) },
            { "vanilla", (255, 246, 196) },
            { "custard", (255, 246, 143) },
            { "butter", (255, 239, 153) },
            { "honey", (255, 203, 5) },
            { "amber", (255, 194, 0) },
            { "caramel", (150, 103, 62) },
            { "coffee", (111, 78, 55) },
            { "espresso", (59, 38, 33) },
            { "mocha", (72, 49, 36) },
            { "cocoa", (101, 67, 33) },
            { "mahogany", (192, 64, 0) },
            { "cedar", (81, 38, 23) },
            { "walnut", (58, 44, 37) },
            { "oak", (121, 94, 68) },
            { "ebony", (49, 46, 46) }
        };

    public string Name => "popular colors";

    public string Id => "popular";

    public IReadOnlyDictionary<string, RgbColor> Colors => this._colors;
}