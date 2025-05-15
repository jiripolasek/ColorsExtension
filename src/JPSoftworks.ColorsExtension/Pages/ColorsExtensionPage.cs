// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using JPSoftworks.ColorsExtension.Resources;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class ColorsExtensionPage : AsyncDynamicListPage
{
    private readonly NamedColorManager _namedColorManager = new();
    private readonly AnyColorParser _parser = new();
    private Unicolour? _color;
    private ColorParseResult? _result;

    public ColorsExtensionPage()
    {
        this.Icon = Icons.ColorWheel;
        this.Title = Strings.Colors!;
        this.Name = Strings.Open!;
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

    protected override async Task<IListItem[]> SearchItemsAsync(string searchText, CancellationToken cancellationToken)
    {
        try
        {
            this._result = this._parser.Parse(searchText);
            this._color = this._result.Success ? this._result.Color : null;
        }
        catch (Exception)
        {
            this._color = null;
        }

        if (this._result is not { Success: true })
        {
            this.EmptyContent = new CommandItem(new NoOpCommand())
            {
                Icon = Icons.ColorWheelLarge,
                Title = Strings.ColorNotRecognized!,
            };

            return [];
        }

        List<ColorListItem> namedColors = [];
        var rgbByte255 = this._color!.Rgb.Byte255;
        var colorName = this._namedColorManager.GetNameByRgb(rgbByte255.R, rgbByte255.G, rgbByte255.B);
        if (colorName.Success)
        {
            namedColors.Add(await ColorListItem.CreateAsync(this._color, ParsedColorFormat.NamedColor));
        }

        return
        [
            // color formats
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.HexLong),
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.RgbModern),
            ..namedColors,
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.HslModern),
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.Hsv),
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.HwbModern),
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.LchModern),
            await ColorListItem.CreateAsync(this._color, ParsedColorFormat.LabModern),

            // gradients
            .. await BuildBasicGradientAsync(this._color)
        ];
    }

    private static async Task<ColorListItem[]> BuildBasicGradientAsync(Unicolour baseColor)
    {
        return await Task.WhenAll(baseColor.GenerateShadesWithLightness()
            .Select(static color => ColorListItem.CreateAsync(color.Shade, color.Shade.Hex, GetLightnessString(color.Lightness), 10))
            .ToArray());
        
        static string GetLightnessString(double relativeLightness)
        {
            if (relativeLightness == 0)
                return "base color";

            return relativeLightness > 0 ? $"{relativeLightness:P0} lighter" : $"{-relativeLightness:P0} darker";
        }

    }
}