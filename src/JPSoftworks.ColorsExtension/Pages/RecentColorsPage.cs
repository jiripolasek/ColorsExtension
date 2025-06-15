// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class RecentColorsPage : ListPage, IDisposable
{
    private readonly ColorsExtensionPage _colorsPage;
    private readonly HistoryManager _historyManager;

    public RecentColorsPage(ColorsExtensionPage colorsPage)
    {
        ArgumentNullException.ThrowIfNull(colorsPage);

        this._colorsPage = colorsPage;

        this._historyManager = HistoryManager.Instance;
        this._historyManager.HistoryChanged += this.HistoryManagerOnHistoryChanged;

        this.Id = "jpsoftworks.colors.history";
        this.Title = "Color History";
        this.Name = "Show recent colors";
        this.PlaceholderText = "Search recent colors";
        this.Icon = Icons.Colorful.History;
        this.EmptyContent = new CommandItem
        {
            Title = "No colors yet",
            Subtitle = "Colors are saved when you copy them",
            Icon = Icons.Colorful.History
        };
    }

    private void HistoryManagerOnHistoryChanged(object? sender, EventArgs e)
    {
        this.RaiseItemsChanged(this._historyManager.GetColorHistoryEntries().Count);
    }

    public override IListItem[] GetItems()
    {
        if (this._historyManager.GetColorHistoryEntries().Count == 0)
        {
            return [];
        }

        return [.. from colorHistoryEntry in this._historyManager.GetColorHistoryEntries().Reverse()
            let rgb = new RgbColor(colorHistoryEntry.R, colorHistoryEntry.G, colorHistoryEntry.B)
            select new RecentColorListItem(this._colorsPage, colorHistoryEntry?.Query, colorHistoryEntry.Value, rgb)
            ];
    }

    public void Dispose()
    {
        this._historyManager.HistoryChanged -= this.HistoryManagerOnHistoryChanged;
    }
}