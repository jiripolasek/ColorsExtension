using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

// Enum to specify which color format was parsed

// Result of a parsing attempt
public class ColorParseResult
{
    public bool Success { get; }
    public Unicolour? Color { get; }
    public ParsedColorFormat? Format { get; }
    public string? Error { get; }

    private ColorParseResult(bool success, Unicolour? color, ParsedColorFormat? format, string? error)
    {
        this.Success = success;
        this.Color = color;
        this.Format = format;
        this.Error = error;
    }

    public static ColorParseResult Ok(Unicolour color, ParsedColorFormat format)
        => new ColorParseResult(true, color, format, null);

    public static ColorParseResult Fail(string error)
        => new ColorParseResult(false, null, null, error);
}

// Base interface for color parsers

// Hex color parser (#RGB, #RGBA, #RRGGBB, #RRGGBBAA, RGB, RGBA, RRGGBB, RRGGBBAA)

// RGB/RGBA color parser (CSS Level 2/3 style with commas)

// HSL/HSLA color parser (CSS Level 2/3 style with commas)

// HSV/HSVA color parser (not official CSS but commonly supported)

// RGB color parser (CSS Level 4 style with spaces and slash syntax)

// HSL color parser (CSS Level 4 style with spaces and slash syntax)

// HWB color parser (CSS Level 4 - Hue, Whiteness, Blackness)

// LAB color parser (CSS Level 4 - L*a*b*)

// LCH color parser (CSS Level 4 - L*C*h*)

// RGB plain color parser (bare values without function syntax)

// Named color parser (CSS named colors)

// Main color parser that tries all parsers

// Usage example:
/*
var parser = new UnicColourParser();

// Parse various formats
var result1 = parser.Parse("#FF0000");          // Hex with hash
var result2 = parser.Parse("FF0000");           // Hex without hash
var result3 = parser.Parse("#F00");             // Short hex
var result4 = parser.Parse("F00");              // Short hex without hash
var result5 = parser.Parse("rgb(255, 0, 0)");   // RGB (CSS2/3)
var result6 = parser.Parse("255, 0, 0");        // Plain RGB with commas
var result7 = parser.Parse("255 0 0");          // Plain RGB with spaces
var result8 = parser.Parse("255 0 0 / 0.5");    // Plain RGB with alpha
var result9 = parser.Parse("hsl(0, 100%, 50%)"); // HSL (CSS2/3)
var result10 = parser.Parse("red");             // Named color

// CSS Level 4 formats
var result11 = parser.Parse("rgb(255 0 0)");    // RGB (modern)
var result12 = parser.Parse("hsl(0 100% 50%)"); // HSL (modern)
var result13 = parser.Parse("hwb(0 0% 0%)");    // HWB
var result14 = parser.Parse("lab(50% 40 30)");  // LAB
var result15 = parser.Parse("lch(50% 40 30)");  // LCH

// Check results
if (result1.Success)
{
    Console.WriteLine($"Parsed color as {result1.Format}: {result1.Color}");
}

// For conversion back to these formats, you could create separate formatter classes
*/