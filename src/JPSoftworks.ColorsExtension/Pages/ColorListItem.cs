// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using Windows.Storage.Streams;
using Windows.System;
using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorFormatter;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class ColorListItem : ListItem
{
    private ColorListItem(Unicolour color, ParsedColorFormat format, IRandomAccessStream? iconStream) : base(
        new NoOpCommand())
    {
        var c = new AnyColorFormatter();

        this.Title = c.Format(color, format);
        this.Command = new CopyTextCommand(this.Title);
        this.Tags = [new Tag(ColorFormatNames.GetDisplayName(format))];
        this.Subtitle = ColorFormatNames.GetDisplayName(format);
        this.Icon = iconStream == null ? null : IconInfo.FromStream(iconStream);
        this.MoreCommands =
        [
            new CommandContextItem(new CopyTextCommand(c.Format(color, format)))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, true, false, (int)VirtualKey.C, 0)
            }
        ];
    }

    private ColorListItem(Unicolour color, string text, string? subtitle, IRandomAccessStream? iconStream) : base(
        new CopyTextCommand(text))
    {
        this.Title = text;
        this.Subtitle = subtitle ?? "HSL: " + color.GetRepresentation(ColourSpace.Hsl);
        this.Icon = iconStream == null ? null : IconInfo.FromStream(iconStream);

        this.MoreCommands =
        [
            new CommandContextItem(new CopyTextCommand(text))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, true, false, (int)VirtualKey.C, 0)
            }
        ];
    }

    public static async Task<ColorListItem> CreateAsync(
        Unicolour color,
        string title,
        string? subtitle = null,
        int r = 4)
    {
        var stream = await BitmapStreamFactory.CreateRoundedColorStreamAsync((byte)color.Rgb.Byte255.R,
            (byte)color.Rgb.Byte255.G, (byte)color.Rgb.Byte255.B, 20, r);
        return new ColorListItem(color, title, subtitle, stream);
    }

    public static async Task<ColorListItem> CreateAsync(Unicolour color, ParsedColorFormat format, int r = 4)
    {
        var stream = await BitmapStreamFactory.CreateRoundedColorStreamAsync((byte)color.Rgb.Byte255.R,
            (byte)color.Rgb.Byte255.G, (byte)color.Rgb.Byte255.B, 20, r);
        return new ColorListItem(color, format, stream);
    }
}