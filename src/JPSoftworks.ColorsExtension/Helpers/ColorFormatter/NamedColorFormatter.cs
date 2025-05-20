using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class NamedColorFormatter : IColorFormatter
{
    private readonly NamedColorManager _colorManager;
    private readonly bool _includeSetName;

    public NamedColorFormatter(NamedColorManager? colorManager = null, bool includeSetName = true)
    {
        this._colorManager = colorManager ?? new NamedColorManager();
        this._includeSetName = includeSetName;
    }

    public ParsedColorFormat TargetFormat => ParsedColorFormat.NamedColor;

    public string Format(Unicolour color)
    {
        var rgb = color.Rgb;
        var r = (int)Math.Round(rgb.R * 255);
        var g = (int)Math.Round(rgb.G * 255);
        var b = (int)Math.Round(rgb.B * 255);

        // Try exact match first
        var exactResult = this._colorManager.GetNameByRgb(r, g, b);
        if (exactResult.Success)
        {
            return this._includeSetName ? exactResult.GetQualifiedName() : exactResult.ColorName!;
        }

        // Fall back to closest match
        var closestResult = this._colorManager.GetClosestNamedColor(r, g, b);
        if (closestResult.Success)
        {
            return this._includeSetName ? closestResult.GetQualifiedName() : closestResult.ColorName!;
        }

        // Fallback to hex if no named color found
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}