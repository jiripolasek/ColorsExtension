using System;
using System.Collections.Generic;
using System.Linq;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class AnyColorParser
{
    private readonly List<IColorParser> _parsers;

    public AnyColorParser()
    {
        this._parsers =
        [
            new HexColorParser(), // Handles both #RGB and RGB (without hash)
            new RgbColorParser(), // Standard CSS rgb() function
            new RgbPlainColorParser(), // Plain "r, g, b" and "r g b" formats
            new HslColorParser(),
            new HsvColorParser(),
            new NamedColorParser(),

            // CSS Level 4 formats
            new RgbModernColorParser(),
            new HslModernColorParser(),
            new HwbModernColorParser(),
            new LabModernColorParser(),
            new LchModernColorParser()
        ];
    }

    // Constructor that allows custom parsers for extensibility
    public AnyColorParser(IEnumerable<IColorParser> customParsers)
    {
        this._parsers = customParsers.ToList();
    }

    public ColorParseResult Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return ColorParseResult.Fail("Input cannot be empty");

        var errors = new List<string>();

        foreach (var parser in this._parsers)
        {
            var result = parser.TryParse(input);
            if (result.Success)
                return result;

            errors.Add(result.Error!);
        }

        return ColorParseResult.Fail($"Unable to parse color '{input}'. Errors: {string.Join("; ", errors)}");
    }

    // Convenience method that throws on failure
    public Unicolour ParseOrThrow(string input)
    {
        var result = this.Parse(input);
        if (!result.Success)
            throw new FormatException(result.Error);

        return result.Color!;
    }

    // Extension method for adding custom parsers
    public void AddParser(IColorParser parser)
    {
        this._parsers.Insert(0, parser); // Insert at beginning for highest priority
    }
}