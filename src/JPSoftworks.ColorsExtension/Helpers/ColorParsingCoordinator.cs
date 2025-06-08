// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using JPSoftworks.ColorsExtension.Helpers.QueryParser;

namespace JPSoftworks.ColorsExtension.Helpers;

/// <summary>
/// Top level parsing of input representing color.
/// </summary>
internal class ColorParsingCoordinator
{
    private readonly NamedColorManager _namedColorManager;
    private readonly AnyColorParser _exactParser;

    public ColorParsingCoordinator(NamedColorManager namedColorManager)
    {
        this._namedColorManager = namedColorManager;
        ArgumentNullException.ThrowIfNull(namedColorManager);

        this._exactParser = new AnyColorParser();
    }

    public CombinedParseResult Parse(string input, string? palette)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            var emptyExact = ColorParseResult.Fail("Input cannot be empty");
            var emptyNamed = NamedColorResolveResult.Empty(input);
            return new CombinedParseResult(input, emptyExact, emptyNamed);
        }

        // Try exact parsing first
        var exactResult = this._exactParser.Parse(input);
        if (exactResult.Success)
        {
            return new CombinedParseResult(input, exactResult, null);
        }

        // Try named color resolution
        var results = this._namedColorManager.GetColorByName(input, palette: palette);
        var namedResult = results.Count > 0
            ? new NamedColorResolveResult(input, [.. results])
            : NamedColorResolveResult.Empty(input);
        return new CombinedParseResult(input, exactResult, namedResult);
    }
}