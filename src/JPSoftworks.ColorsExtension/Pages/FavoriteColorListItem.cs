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
    private readonly DynamicListPage _listPage;
    private readonly ColorListEntryModel _favoriteColorEntry;

    public FavoriteColorListItem(DynamicListPage listPage, ColorListEntryModel favoriteColorEntry)
    {
        ArgumentNullException.ThrowIfNull(listPage);

        this._listPage = listPage;
        this._favoriteColorEntry = favoriteColorEntry;
        var rgbColor = new RgbColor(_favoriteColorEntry.R, _favoriteColorEntry.G, this._favoriteColorEntry.B);
        var hexColor = $"#{rgbColor.R:X2}{rgbColor.G:X2}{rgbColor.B:X2}";

        this.Command = new AnonymousCommand(this.Action)
        {
            Icon = Icons.Info,
            Result = CommandResult.GoBack(), // Hack: GoBack command because I can't get back to the previous page
            Name = "Show details"
        };
        this.Title = this._favoriteColorEntry.Query ?? this._favoriteColorEntry.Value ?? $"Color {rgbColor}";
        this.Subtitle = SelectColorListItem.BuildSubtitle(rgbColor);
        _ = this.SetIconAsync(rgbColor);

        this.MoreCommands =
        [
            new CommandContextItem(new CopyAndSaveColorCommand(this._favoriteColorEntry.Value ?? hexColor, rgbColor)),
            new CommandContextItem(new AnonymousCommand(this.DeleteFavoriteColor) { Result = CommandResult.KeepOpen(), Icon = Icons.Delete, Name = "Delete" } )
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