using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

// Named color parser (CSS named colors)
public class NamedColorParser : IColorParser
{
    private readonly NamedColorManager _colorManager;

    public NamedColorParser(NamedColorManager? colorManager = null)
    {
        this._colorManager = colorManager ?? new NamedColorManager();
    }

    public ColorParseResult TryParse(string input)
    {
        var result = this._colorManager.GetColorByName(input.Trim());

        if (result.Success)
        {
            return ColorParseResult.Ok(
                new Unicolour(ColourSpace.Rgb, result.Rgb!.Value.r / 255.0, result.Rgb!.Value.g / 255.0,
                    result.Rgb!.Value.b / 255.0),
                ParsedColorFormat.NamedColor
            );
        }

        return ColorParseResult.Fail($"Unknown named color: {input}");
    }
}