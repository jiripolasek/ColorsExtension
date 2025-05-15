// ------------------------------------------------------------
//
// Copyright (c) Jiří Polášek. All rights reserved.
//
// ------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;


public abstract class AsyncDynamicListPage : DynamicListPage
{
    private CancellationTokenSource _updateCancellationSource;
    private readonly SemaphoreSlim _updateSemaphore = new(1, 1);
    private IListItem[] _currentItems = [];

    private Timer? _debounceTimer;
    private const int DebounceDelayMs = 300;
    private bool _isDisposed;

    private string _lastSearchText = "";
    private IListItem[]? _lastSearchResults;
    private readonly Lock _itemsLock = new();
    private readonly Lock _searchLock = new();

    protected AsyncDynamicListPage()
    {
        this._updateCancellationSource = new CancellationTokenSource();

        _ = Task.Run(async () =>
        {
            try
            {
                await this.InitializeAsync();
            }
            catch (Exception ex)
            {
                this.HandleSearchError(ex, "");
            }
        });
    }

    /// <summary>
    /// Override this method in your derived class to implement the actual search logic
    /// </summary>
    protected abstract Task<IListItem[]> SearchItemsAsync(string searchText, CancellationToken cancellationToken);

    /// <summary>
    /// Override to implement initial loading if needed
    /// </summary>
    protected virtual Task<IListItem[]> LoadInitialItemsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Array.Empty<IListItem>());
    }

    private async Task InitializeAsync()
    {
        if (string.IsNullOrEmpty(this.SearchText))
        {
            await this.UpdateItemsAsync(this.SearchText);
        }
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        lock (this._searchLock)
        {
            if (string.Equals(this._lastSearchText, newSearch, StringComparison.Ordinal))
            {
                return;
            }
        }

        this.ScheduleSearchUpdate(newSearch);
    }

    private void ScheduleSearchUpdate(string searchText)
    {
        Timer newTimer = new Timer(async void (_) =>
        {
            try
            {
                await this.UpdateItemsAsync(searchText);
            }
            catch (Exception ex)
            {
                this.HandleSearchError(ex, searchText);
            }
        }, null, DebounceDelayMs, Timeout.Infinite);

        var oldTimer = Interlocked.Exchange(ref this._debounceTimer, newTimer);
        oldTimer?.Dispose();
    }

    private async Task UpdateItemsAsync(string searchText)
    {
        if (this._isDisposed) return;

        bool acquired = await this._updateSemaphore.WaitAsync(0);
        if (!acquired)
        {
            this.ScheduleSearchUpdate(searchText);
            return;
        }

        try
        {
            bool useCachedResults;
            IListItem[]? cachedResults = null;

            lock (this._searchLock)
            {
                useCachedResults = string.Equals(this._lastSearchText, searchText, StringComparison.Ordinal)
                    && this._lastSearchResults != null;
                if (useCachedResults)
                {
                    cachedResults = this._lastSearchResults;
                }
            }

            if (useCachedResults)
            {
                this.UpdateItems(cachedResults ?? []);
                return;
            }

            var oldSource = Interlocked.Exchange(ref this._updateCancellationSource, new CancellationTokenSource());
            await oldSource.CancelAsync();
            oldSource.Dispose();

            var cancellationToken = this._updateCancellationSource.Token;

            this.IsLoading = true;

            try
            {
                IListItem[] newItems;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    newItems = await this.LoadInitialItemsAsync(cancellationToken);
                }
                else
                {
                    newItems = await this.SearchItemsAsync(searchText, cancellationToken);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    this.UpdateItems(newItems);

                    lock (this._searchLock)
                    {
                        this._lastSearchText = searchText;
                        this._lastSearchResults = newItems;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                this.HandleSearchError(ex, searchText);
            }
            finally
            {
                this.SetLoadingState(false);
            }
        }
        finally
        {
            this._updateSemaphore.Release();
        }
    }

    protected virtual void HandleSearchError(Exception ex, string searchText)
    {
        System.Diagnostics.Debug.WriteLine($"Error searching for '{searchText}': {ex.Message}");
    }

    private void UpdateItems(IListItem[] newItems)
    {
        if (this._isDisposed)
        {
            return;
        }

        if (ReferenceEquals(this._currentItems, newItems))
        {
            return;
        }

        bool itemsChanged = false;

        lock (this._itemsLock)
        {
            var oldItems = this._currentItems;

            if (oldItems.Length != newItems.Length)
            {
                this._currentItems = newItems ?? [];
                itemsChanged = true;
            }
            else
            {
                for (int i = 0; i < oldItems.Length; i++)
                {
                    if (!ReferenceEquals(oldItems[i], newItems[i]) &&
                        !string.Equals(oldItems[i].Title, newItems[i].Title, StringComparison.Ordinal))
                    {
                        this._currentItems = newItems;
                        itemsChanged = true;
                        break;
                    }
                }
            }
        }

        if (itemsChanged)
        {
            this.OnItemsChanged();
        }
    }

    public override IListItem[] GetItems()
    {
        lock (this._itemsLock)
        {
            return this._currentItems.ToArray();
        }
    }

    public override void LoadMore()
    {
        base.LoadMore();
    }

    private void SetLoadingState(bool isLoading)
    {
        this.IsLoading = isLoading;
    }

    private void OnItemsChanged()
    {
        if (!this._isDisposed)
        {
            this.RaiseItemsChanged();
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !this._isDisposed)
        {
            this._isDisposed = true;

            this._debounceTimer?.Dispose();
            this._debounceTimer = null;

            this._updateCancellationSource?.Cancel();
            this._updateCancellationSource?.Dispose();
            this._updateCancellationSource = null;

            this._updateSemaphore?.Dispose();

            this._currentItems = [];
            this._lastSearchResults = null;
        }
    }
}