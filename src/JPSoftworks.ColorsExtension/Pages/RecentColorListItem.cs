// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Commands;
using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.System;
using JPSoftworks.ColorsExtension.Services.Settings;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class RecentColorListItem : ListItem
{
    private readonly DynamicListPage _listPage;
    private readonly RgbColor _rgbColor;
    private readonly string _query;

    public RecentColorListItem(DynamicListPage listPage, string? query, string? value, RgbColor rgbColor)
    {
        ArgumentNullException.ThrowIfNull(listPage);

        this._listPage = listPage;
        this._rgbColor = rgbColor;
        var hexColor = $"#{rgbColor.R:X2}{rgbColor.G:X2}{rgbColor.B:X2}";
        this._query = query ?? hexColor;

        this.Command = new AnonymousCommand(this.Action)
        {
            Icon = Icons.Info,
            Result = CommandResult.GoBack(), // Hack: GoBack command because I can't get back to the previous page
            Name = "Show details"
        };
        this.Title = query ?? $"Color {rgbColor}";
        this.Subtitle = SelectColorListItem.BuildSubtitle(rgbColor);
        _ = this.SetIconAsync(rgbColor);

        this.MoreCommands =
        [
            new CommandContextItem(new CopyAndSaveColorCommand(value ?? hexColor, rgbColor))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, true, false, (int)VirtualKey.C, 0)
            },
            new CommandContextItem(new AddToFavoritesCommand(this.Title, rgbColor))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(true, false, false, false, (int)VirtualKey.B, 0)
            },
            new CommandContextItem(new AnonymousCommand(this.DeleteRecentColor) { Result = CommandResult.KeepOpen(), Icon = Icons.Delete, Name = "Remove from recent colors" } )
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, false, true, false, (int)VirtualKey.Delete),
            }
        ];
    }

    private void DeleteRecentColor()
    {
        HistoryManager.Instance.RemoveColorHistoryEntry(this._query, this._rgbColor);
    }

    private async Task SetIconAsync(RgbColor rgbColor)
    {
        var iconStream = await BitmapStreamFactory.CreateRoundedColorStreamAsync(rgbColor.R, rgbColor.G, rgbColor.B);
        this.Icon = IconInfo.FromStream(iconStream!);
    }

    private void Action()
    {
        this._listPage.SearchText = this._query;
    }
}