// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class FavoriteColorsPage : ListPage, IDisposable
{
    private readonly ColorsExtensionPage _colorsPage;
    private readonly FavoritesColorsManager _favoriteColorsManager;

    public FavoriteColorsPage(ColorsExtensionPage colorsPage)
    {
        ArgumentNullException.ThrowIfNull(colorsPage);

        this._colorsPage = colorsPage;

        this._favoriteColorsManager = FavoritesColorsManager.Instance;
        this._favoriteColorsManager.PinnedColorsChanged += this.FavoriteColorsChanged;

        this.Id = "jpsoftworks.colors.favorites";
        this.Title = "Favorite Colors";
        this.Name = "Show favorite colors";
        this.PlaceholderText = "Search favorite colors";
        this.Icon = Icons.Colorful.Favorite;
        this.EmptyContent = new CommandItem
        {
            Title = "No favorite colors yet",
            Subtitle = "Mark colors as favorite",
            Icon = Icons.Colorful.Favorite
        };
    }

    private void FavoriteColorsChanged(object? sender, EventArgs e)
    {
        this.RaiseItemsChanged(this._favoriteColorsManager.GetPinnedColors().Count);
    }

    public override IListItem[] GetItems()
    {
        var favoriteColors = this._favoriteColorsManager.GetPinnedColors();
        return favoriteColors.Count == 0
            ? []
            : [.. favoriteColors.Select(entry => new FavoriteColorListItem(this._colorsPage, entry))];
    }

    public void Dispose()
    {
        this._favoriteColorsManager.PinnedColorsChanged -= this.FavoriteColorsChanged;
    }
}