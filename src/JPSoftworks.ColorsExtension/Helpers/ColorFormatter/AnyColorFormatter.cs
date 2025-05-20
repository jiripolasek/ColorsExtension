using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class AnyColorFormatter
{
    private readonly Dictionary<ParsedColorFormat, IColorFormatter> _formatters;

    public AnyColorFormatter()
    {
        this._formatters = new Dictionary<ParsedColorFormat, IColorFormatter>
        {
            { ParsedColorFormat.HexShort, new HexShortColorFormatter() },
            { ParsedColorFormat.HexLong, new HexLongColorFormatter() },
            { ParsedColorFormat.HexWithoutHash, new HexWithoutHashColorFormatter() },
            { ParsedColorFormat.Rgb, new RgbColorFormatter() },
            { ParsedColorFormat.RgbModern, new RgbModernColorFormatter() },
            { ParsedColorFormat.RgbPlain, new RgbPlainColorFormatter() },
            { ParsedColorFormat.Hsl, new HslColorFormatter() },
            { ParsedColorFormat.HslModern, new HslModernColorFormatter() },
            { ParsedColorFormat.Hsv, new HsvColorFormatter() },
            { ParsedColorFormat.HwbModern, new HwbModernColorFormatter() },
            { ParsedColorFormat.LabModern, new LabModernColorFormatter() },
            { ParsedColorFormat.LchModern, new LchModernColorFormatter() },
            { ParsedColorFormat.NamedColor, new NamedColorFormatter() }
        };
    }

    // Format color to specific format
    public string Format(Unicolour color, ParsedColorFormat format)
    {
        if (this._formatters.TryGetValue(format, out var formatter))
        {
            return formatter.Format(color);
        }

        throw new ArgumentException($"Unsupported format: {format}");
    }

    // Format color to multiple formats (useful for showing alternatives)
    public Dictionary<ParsedColorFormat, string> FormatAll(Unicolour color)
    {
        var result = new Dictionary<ParsedColorFormat, string>();

        foreach (var kvp in this._formatters)
        {
            try
            {
                result[kvp.Key] = kvp.Value.Format(color);
            }
            catch
            {
                // Skip formats that can't represent this color
            }
        }

        return result;
    }

    // Add custom formatter
    public void AddFormatter(IColorFormatter formatter)
    {
        this._formatters[formatter.TargetFormat] = formatter;
    }
}