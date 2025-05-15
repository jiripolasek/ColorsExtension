namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public enum ParsedColorFormat
{
    HexShort,           // #RGB or #RGBA
    HexLong,            // #RRGGBB or #RRGGBBAA
    Rgb,                // rgb(r, g, b) or rgba(r, g, b, a)
    Hsl,                // hsl(h, s%, l%) or hsla(h, s%, l%, a)
    Hsv,                // hsv(h, s%, v%) or hsva(h, s%, v%, a)
    NamedColor,         // Named color (red, blue, etc.)
    RgbModern,          // rgb(r g b) or rgb(r g b / a) - CSS Level 4
    HslModern,          // hsl(h s% l%) or hsl(h s% l% / a) - CSS Level 4
    HwbModern,          // hwb(h w% b%) or hwb(h w% b% / a) - CSS Level 4 
    LabModern,          // lab(l% a b) or lab(l% a b / alpha) - CSS Level 4
    LchModern,          // lch(l% c h) or lch(l% c h / alpha) - CSS Level 4
    RgbPlain,           // "r, g, b" or "r,g,b,a" or "r g b" or "r g b / a" without function syntax
    HexWithoutHash      // RGB or RGBA without # prefix
}