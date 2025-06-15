// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class AnyColorParser
{
    private readonly List<IColorParser> _parsers =
    [
        new HexColorParser(), // Handles both #RGB and RGB (without hash)
        new RgbColorParser(), // Standard CSS rgb() function
        new RgbPlainColorParser(), // Plain "r, g, b" and "r g b" formats
        new HslColorParser(),
        new HsvColorParser(),

        // CSS Level 4 formats
        new RgbModernColorParser(),
        new HslModernColorParser(),
        new HwbModernColorParser(),
        new LabModernColorParser(),
        new LchModernColorParser(),

        new CmykModernColorParser(),
    ];

    public ColorParseResult Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ColorParseResult.Fail("Input cannot be empty");
        }

        var errors = new List<string>();

        foreach (var parser in this._parsers)
        {
            var result = parser.TryParse(input);
            if (result.Success)
            {
                return result;
            }

            errors.Add(result.Error!);
        }

        return ColorParseResult.Fail($"Unable to parse color '{input}'. Errors: {string.Join("; ", errors)}");
    }
}