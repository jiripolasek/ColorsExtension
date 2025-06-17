// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Commands;
using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorFormatter;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;
using Windows.Storage.Streams;
using Windows.System;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class ColorListItem : ListItem
{
    private static readonly AnyColorFormatter Formatter = new();

    private ColorListItem(Unicolour color, ParsedColorFormat format, IRandomAccessStream? iconStream) : base(
        new NoOpCommand())
    {
        this.Title = Formatter.Format(color, format);
        this.Command = new CopyAndSaveColorCommand(this.Title, color);
        this.Tags = [new Tag(ColorFormatNames.GetDisplayName(format))];
        this.Subtitle = ColorFormatNames.GetDisplayName(format);
        this.Icon = iconStream == null ? null : IconInfo.FromStream(iconStream);
        this.MoreCommands =
        [
            new CommandContextItem(new AddToFavoritesCommand(this.Title, color.ToRgbColor()))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.B)
            },
            new CommandContextItem(new CopyAndSaveColorCommand(this.Title, color))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, true, false, (int)VirtualKey.C)
            }
        ];
    }

    private ColorListItem(Unicolour color, string text, string? subtitle, IRandomAccessStream? iconStream, bool allowSelect = false, IDynamicListPage? targetPage = null) : base(
        new CopyAndSaveColorCommand(text, color))
    {
        this.Title = text;
        this.Subtitle = subtitle ?? "HSL: " + color.GetRepresentation(ColourSpace.Hsl);
        this.Icon = iconStream == null ? null : IconInfo.FromStream(iconStream);

        this.MoreCommands =
        [
            ..(allowSelect && targetPage != null ? new[] {new CommandContextItem(new UpdateSearchTextCommand(this.Title, targetPage) { Name = "Show detail" })} : []),
            new CommandContextItem(new AddToFavoritesCommand(text, color.ToRgbColor())),
            new CommandContextItem(new CopyAndSaveColorCommand(text, color))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, true, false, (int)VirtualKey.C)
            },
        ];
    }

    public static async Task<ColorListItem> CreateAsync(
        Unicolour color,
        string title,
        string? subtitle = null,
        int r = 4,
        IDynamicListPage? targetForUpdate = null)
    {
        var stream = await BitmapStreamFactory.CreateRoundedColorStreamAsync((byte)color.Rgb.Byte255.R,
            (byte)color.Rgb.Byte255.G, (byte)color.Rgb.Byte255.B, 20, r);
        return new ColorListItem(color, title, subtitle, stream, targetForUpdate != null, targetForUpdate);
    }

    public static async Task<ColorListItem> CreateAsync(Unicolour color, ParsedColorFormat format, int r = 4)
    {
        var stream = await BitmapStreamFactory.CreateRoundedColorStreamAsync((byte)color.Rgb.Byte255.R,
            (byte)color.Rgb.Byte255.G, (byte)color.Rgb.Byte255.B, 20, r);
        return new ColorListItem(color, format, stream);
    }
}