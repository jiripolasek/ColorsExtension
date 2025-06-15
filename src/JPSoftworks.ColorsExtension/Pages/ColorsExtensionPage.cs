// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Commands;
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
    private readonly HelpPage _helpPageInstance = new();
    private readonly ListItem _helpPageItem;
    private IListItem[]? _emptyItems;

    public ColorsExtensionPage()
    {
        this.Icon = Icons.ColorWheel;
        this.Title = Strings.Colors!;
        this.Name = Strings.Open!;
        this.PlaceholderText = "Type a color code or color name (e.g., #FFCC00 or red) or type “/” for more options";

        this._colorParsingCoordinator = new ColorParsingCoordinator(this._namedColorManager);

        this._helpPageItem = new ListItem(this._helpPageInstance)
        {
            Icon = Icons.Colorful.Question,
            Title = "Show help",
            Subtitle = "Learn how to use the color extension, including available commands and options",
        };
    }

    protected override Task<IListItem[]> LoadInitialItemsAsync(CancellationToken cancellationToken)
    {
        this._emptyItems ??= [
            new ListItem(new RecentColorsPage(this))
            {
                Icon = Icons.Colorful.History,
                Title = "Recent colors",
                Subtitle = "List of recently copied colors"
            },
            new ListItem(new FavoriteColorsPage(this))
            {
                Icon = Icons.Colorful.Favorite,
                Title = "Favorite colors",
                Subtitle = "Browse your favorite colors"
            },
            this._helpPageItem];

        return Task.FromResult(this._emptyItems);
    }

    protected override async Task<IListItem[]> SearchItemsAsync(string previousText, string searchText, CancellationToken cancellationToken)
    {
        var cursorPosition = TextUtilities.FindLastCommonStringIndex(previousText, searchText);
        var queryParserResult = ColorQueryParser.Instance.Parse(searchText, cursorPosition);

        if (queryParserResult.HasSuggestions)
        {
            var suggestionItems = queryParserResult.Context.Suggestions.Select(suggestion => new InputSuggestionListItem(suggestion, this)).ToList();
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

        if (queryParserResult.Options.ShowHelp)
        {
            result.Add(this._helpPageItem);
        }
        else
        {
            var combinedParserResult = this._colorParsingCoordinator.Parse(queryParserResult.Query, queryParserResult.Options.Palette);

            var colorResults = combinedParserResult.Strategy switch
            {
                CombinedParseStrategy.ExactMatch => await this.SingleColorResults(combinedParserResult.ExactResult!.Color!, searchText),
                CombinedParseStrategy.AutoSelectNamed => await this.SingleColorResults(new Unicolour(ColourSpace.Rgb255,
                    combinedParserResult.NamedResult!.BestMatch!.Rgb!.Value.R,
                    combinedParserResult.NamedResult.BestMatch.Rgb.Value.G,
                    combinedParserResult.NamedResult.BestMatch.Rgb.Value.B),
                    searchText),
                CombinedParseStrategy.ShowNamedOptions => this.SelectColorResults(combinedParserResult.NamedResult!.AllMatches),
                _ => this.NoMatchResults(queryParserResult)
            };
            result.AddRange(colorResults);
        }

        return [.. result];
    }

    private IListItem[] NoMatchResults(ParseResult<ColorQueryOptions> queryParserResult)
    {
        if (string.IsNullOrWhiteSpace(queryParserResult.Query))
        {
            return this._emptyItems!;
        }

        if (!string.IsNullOrWhiteSpace(queryParserResult.Options.Palette))
        {
            var palette = this._namedColorManager.ListRegisteredColorSets().FirstOrDefault(t => t.Id == queryParserResult.Options.Palette);
            var searchTextWithoutPaletteSwitch = TextUtilities.RemoveSwitches(this.SearchText, "palette");
            
            return
            [
                new ListItem(this._helpPageInstance)
                {
                    Title = $"No color found matching the input in palette {palette?.Name ?? "???"}",
                    Subtitle = "Try selecting a different palette, entering a different color code or name, or type “/” for more options",
                    Icon = Icons.Emojis.CryingFace
                },
                new ListItem(new UpdateSearchTextCommand(searchTextWithoutPaletteSwitch, this))
                {
                    Title = "Search all palettes",
                    Subtitle = "Remove palette switch to search all colors",
                    Icon = Icons.Colorful.Suggestion
                },
                this._helpPageItem
            ];
        }
        else
        {
            return
            [
                new ListItem(this._helpPageInstance)
                {
                    Title = "No color found matching the input",
                    Subtitle = "Try entering a different color code or name, or type “/” for more options",
                    Icon = Icons.Emojis.CryingFace
                },
                this._helpPageItem,
            ];
        }
    }

    private IListItem[] SelectColorResults(List<NamedColorResult> matches)
    {
        return
        [
            ..matches.Where(static t => t.Success)
                .Select(namedColorResult => new SelectColorListItem(this, namedColorResult))
        ];
    }

    private async ValueTask<IListItem[]> SingleColorResults(Unicolour color, string input)
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
            await ColorListItem.CreateAsync(color, ParsedColorFormat.CmykModern),

            // gradients
            .. await BuildBasicGradientAsync(color),

            // commands
            new ListItem(new AddToFavoritesCommand(input, color.ToRgbColor())),
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
        return await Task.WhenAll(baseColor.GenerateShadesWithValue()
            .Select(static color => ColorListItem.CreateAsync(color.Shade, color.Shade.Hex, GetBrightnessDescription(color.ValueChange), 10))
            .ToArray());

        static string GetBrightnessDescription(double relativeBrightness)
        {
            if (relativeBrightness == 0)
            {
                return "base color";
            }

            return relativeBrightness > 0 ? $"{relativeBrightness:P0} lighter" : $"{-relativeBrightness:P0} darker";
        }
    }
}