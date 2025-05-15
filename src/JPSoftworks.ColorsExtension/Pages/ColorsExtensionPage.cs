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
        this.Icon = IconHelpers.FromRelativePath("Assets\\Icons\\ColorsIcon.png");
        this.Title = Strings.Colors!;
        this.Name = Strings.Open!;
    }

    protected override async Task<IListItem[]> LoadInitialItemsAsync(CancellationToken cancellationToken)
    {
        return
        [
            new ListItem(new NoOpCommand())
            {
                Title = "Enter color code or name to start...", Icon = new IconInfo("\uE946")
            }
        ];
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

        if (this._result == null)
        {
            return [new ListItem(new NoOpCommand()) { Title = "Enter color code or name to start...", Icon = new IconInfo("\uE946") }];

        }

        if (!this._result.Success)
        {
            return [new ListItem(new NoOpCommand()) { Title = "Color not recognized", Icon = new IconInfo("\uE7BA") }];
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
        return await Task.WhenAll(baseColor.GenerateShades()
            .Select(static color => ColorListItem.CreateAsync(color, color.Hsl.ToString(), 10))
            .ToArray());
    }
}