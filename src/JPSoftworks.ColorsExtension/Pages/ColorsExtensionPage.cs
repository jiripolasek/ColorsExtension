// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using JPSoftworks.ColorsExtension.Helpers.QueryParser;
using JPSoftworks.ColorsExtension.Resources;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class ColorsExtensionPage : AsyncDynamicListPage
{
    private readonly ColorParsingCoordinator _colorParsingCoordinator;
    private readonly NamedColorManager _namedColorManager = new();

    public ColorsExtensionPage()
    {
        this.Icon = Icons.ColorWheel;
        this.Title = Strings.Colors!;
        this.Name = Strings.Open!;
        this.PlaceholderText = "Type color code or name (e.g. #FFCC00 or red)...";

        this._colorParsingCoordinator = new ColorParsingCoordinator(this._namedColorManager);
    }

    protected override Task<IListItem[]> LoadInitialItemsAsync(CancellationToken cancellationToken)
    {
        IListItem[] initialItems = [];

        this.EmptyContent = new CommandItem(new NoOpCommand())
        {
            Icon = Icons.ColorWheelLarge,
            Title = Strings.ColorSearchPlaceholder!
        };

        return Task.FromResult(initialItems);
    }
    
    protected override async Task<IListItem[]> SearchItemsAsync(string previousText, string searchText, CancellationToken cancellationToken)
    {
        var cursorPosition = TextUtilities.FindLastCommonStringIndex(previousText, searchText);
        var queryParserResult = ColorQueryParser.Instance.Parse(searchText, cursorPosition);

        if (queryParserResult.HasSuggestions)
        {
            var suggestionItems = queryParserResult.Context.Suggestions.Select(suggestion => new InputSuggestionListItem(suggestion, this)).ToList();
            if (queryParserResult.Context.Suggestions.Any(static t => t.Type == SuggestionType.Value))
            {
                // Add a command to cancel value the suggestions.
                // We can remember the current query and don't display suggestions again for it until the user types something else.
            }
            return [.. suggestionItems];
        }

        if (queryParserResult.HasErrors)
        {
            return [.. queryParserResult.Errors.Select(static error => new InputErrorListItem(error.Message, error.Type))];
        }

        List<IListItem> result = [];
        if (queryParserResult.HasWarnings)
        {
            result.AddRange(queryParserResult.Context.Warnings.Select(static warning => new InputWarningListItem(warning)));
        }

        var combinedParserResult = this._colorParsingCoordinator.Parse(queryParserResult.Query, queryParserResult.Options.Palette);

        var colorResults = combinedParserResult.Strategy switch
        {
            CombinedParseStrategy.ExactMatch => await this.SingleColorResults(combinedParserResult.ExactResult!.Color!),
            CombinedParseStrategy.AutoSelectNamed => await this.SingleColorResults(new Unicolour(ColourSpace.Rgb255,
                combinedParserResult.NamedResult!.BestMatch!.Rgb!.Value.R,
                combinedParserResult.NamedResult.BestMatch.Rgb.Value.G,
                combinedParserResult.NamedResult.BestMatch.Rgb.Value.B)),
            CombinedParseStrategy.ShowNamedOptions => this.SelectColorResults(combinedParserResult.NamedResult!.AllMatches),
            _ => this.NoMatchResults()
        };
        result.AddRange(colorResults);

        return [.. result];
    }

    private IListItem[] NoMatchResults()
    {
        this.EmptyContent = new CommandItem(new NoOpCommand())
        {
            Icon = Icons.ColorWheelLarge,
            Title = Strings.ColorNotRecognized!
        };
        return [];
    }

    private IListItem[] SelectColorResults(List<NamedColorResult> matches)
    {
        return
        [
            ..matches.Where(static t => t.Success)
                .Select(namedColorResult => new SelectColorListItem(this, namedColorResult))
        ];
    }

    private async ValueTask<IListItem[]> SingleColorResults(Unicolour color)
    {
        return
        [
            // color formats
            await ColorListItem.CreateAsync(color, ParsedColorFormat.HexLong),
            await ColorListItem.CreateAsync(color, ParsedColorFormat.RgbModern),
            ..await this.GetColorsNameItems(color),
            await ColorListItem.CreateAsync(color, ParsedColorFormat.HslModern),
            await ColorListItem.CreateAsync(color, ParsedColorFormat.Hsv),
            await ColorListItem.CreateAsync(color, ParsedColorFormat.HwbModern),
            await ColorListItem.CreateAsync(color, ParsedColorFormat.LchModern),
            await ColorListItem.CreateAsync(color, ParsedColorFormat.LabModern),

            // gradients
            .. await BuildBasicGradientAsync(color)
        ];
    }

    private async ValueTask<List<ColorListItem>> GetColorsNameItems(Unicolour? color)
    {
        List<ColorListItem> colorListItems = [];
        var rgbByte255 = color!.Rgb.Byte255;

        var namedColor = this._namedColorManager.GetNameByRgb(rgbByte255.R, rgbByte255.G, rgbByte255.B);
        foreach (var namedColorResult in namedColor)
        {
            colorListItems.Add(await ColorListItem.CreateAsync(color, namedColorResult.GetQualifiedName()));
        }

        return colorListItems;
    }

    private static async Task<ColorListItem[]> BuildBasicGradientAsync(Unicolour baseColor)
    {
        return await Task.WhenAll(baseColor.GenerateShadesWithLightness()
            .Select(static color =>
                ColorListItem.CreateAsync(color.Shade, color.Shade.Hex, GetLightnessString(color.Lightness), 10))
            .ToArray());

        static string GetLightnessString(double relativeLightness)
        {
            if (relativeLightness == 0)
            {
                return "base color";
            }

            return relativeLightness > 0 ? $"{relativeLightness:P0} lighter" : $"{-relativeLightness:P0} darker";
        }
    }
}