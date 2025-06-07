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

    public CombinedParseResult Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            var emptyExact = ColorParseResult.Fail("Input cannot be empty");
            var emptyNamed = NamedColorResolveResult.Empty(input);
            return new CombinedParseResult(input, emptyExact, emptyNamed);
        }

        var queryParserResult = ColorQueryParser.Instance.Parse(input);

        // Try exact parsing first
        var exactResult = this._exactParser.Parse(queryParserResult.Query);
        if (exactResult.Success)
        {
            return new CombinedParseResult(queryParserResult.Query, exactResult, null);
        }

        // Try named color resolution
        var results = this._namedColorManager.GetColorByName(queryParserResult.Query.Trim(), palette: queryParserResult.Options.Palette);
        var namedResult = results.Count > 0
            ? new NamedColorResolveResult(queryParserResult.Query, [.. results])
            : NamedColorResolveResult.Empty(queryParserResult.Query);
        return new CombinedParseResult(queryParserResult.Query, exactResult, namedResult);
    }
}