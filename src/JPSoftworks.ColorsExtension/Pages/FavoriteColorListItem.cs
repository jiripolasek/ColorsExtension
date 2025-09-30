// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using Windows.System;
using JPSoftworks.ColorsExtension.Commands;
using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class FavoriteColorListItem : ListItem
{
    private readonly IDynamicListPage _listPage;
    private readonly ColorListEntryModel _favoriteColorEntry;

    public FavoriteColorListItem(IDynamicListPage listPage, ColorListEntryModel favoriteColorEntry)
    {
        ArgumentNullException.ThrowIfNull(listPage);

        this._listPage = listPage;
        this._favoriteColorEntry = favoriteColorEntry;
        this.Command = new AnonymousCommand(this.Action)
        {
            Icon = Icons.Info,
            Result = CommandResult.GoBack(), // Hack: GoBack command because I can't get back to the previous page
            Name = "Show details"
        };
        this.Title = this._favoriteColorEntry.Query;
        var rgbColor = new RgbColor(this._favoriteColorEntry.R, this._favoriteColorEntry.G, this._favoriteColorEntry.B);
        this.Subtitle = SelectColorListItem.BuildSubtitle(rgbColor);
        _ = this.SetIconAsync(rgbColor);

        this.MoreCommands =
        [
            new CommandContextItem(new CopyAndSaveColorCommand(this._favoriteColorEntry.Value, rgbColor))
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, true, false, (int)VirtualKey.C)
            },
            new CommandContextItem(new AnonymousCommand(this.DeleteFavoriteColor) { Result = CommandResult.KeepOpen(), Icon = Icons.Delete, Name = "Remove from favorites" } )
            {
                RequestedShortcut = KeyChordHelpers.FromModifiers(false, false, true, false, (int)VirtualKey.Delete),
            }
        ];
    }

    private void DeleteFavoriteColor()
    {
        FavoritesColorsManager.Instance.RemovePinnedColor(this._favoriteColorEntry);
    }

    private async Task SetIconAsync(RgbColor rgbColor)
    {
        var iconStream = await BitmapStreamFactory.CreateRoundedColorStreamAsync(rgbColor.R, rgbColor.G, rgbColor.B);
        this.Icon = IconInfo.FromStream(iconStream!);
    }

    private void Action()
    {
        this._listPage.SearchText = this._favoriteColorEntry.Query;
    }
}