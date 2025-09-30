// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Diagnostics;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal abstract class AsyncDynamicListPage : ListPage, IDynamicListPage, IDisposable
{
    private const int DebounceDelayMs = 300;
    private readonly Lock _itemsLock = new();
    private readonly Lock _searchLock = new();
    private readonly SemaphoreSlim _updateSemaphore = new(1, 1);
    private IListItem[] _currentItems = [];

    private Timer? _debounceTimer;
    private bool _isDisposed;
    private IListItem[]? _lastSearchResults;

    private string _lastSearchText = "";
    private CancellationTokenSource _updateCancellationSource = new();
    
    public override string SearchText
    {
        get => base.SearchText;
        set
        {
            var oldSearch = base.SearchText;
            base.SearchText = value;
            this.UpdateSearchText(oldSearch, value);
        }
    }

    protected AsyncDynamicListPage()
    {
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
    protected abstract Task<IListItem[]> SearchItemsAsync(string previousText, string searchText, CancellationToken cancellationToken);

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
            await this.UpdateItemsAsync(this.SearchText, this.SearchText);
        }
    }

    protected virtual void UpdateSearchText(string oldSearch, string newSearch)
    {
        lock (this._searchLock)
        {
            if (string.Equals(this._lastSearchText, newSearch, StringComparison.Ordinal))
            {
                return;
            }
        }

        this.ScheduleSearchUpdate(this._lastSearchText, newSearch);
    }

    private void ScheduleSearchUpdate(string previousText, string searchText)
    {
        var newTimer = new Timer(async void (_) =>
        {
            try
            {
                await this.UpdateItemsAsync(previousText, searchText);
            }
            catch (Exception ex)
            {
                this.HandleSearchError(ex, searchText);
            }
        }, null, DebounceDelayMs, Timeout.Infinite);

        var oldTimer = Interlocked.Exchange(ref this._debounceTimer, newTimer);
        oldTimer?.Dispose();
    }

    private async Task UpdateItemsAsync(string previousText, string searchText)
    {
        if (this._isDisposed)
        {
            return;
        }

        var acquired = await this._updateSemaphore.WaitAsync(0);
        if (!acquired)
        {
            this.ScheduleSearchUpdate(previousText, searchText);
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
                    newItems = await this.SearchItemsAsync(previousText, searchText, cancellationToken);
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
        Debug.WriteLine($"Error searching for '{searchText}': {ex.Message}");
    }

    private void UpdateItems(IListItem[] newItems)
    {
        if (this._isDisposed)
        {
            return;
        }

        // ReSharper disable once InconsistentlySynchronizedField
        if (ReferenceEquals(this._currentItems, newItems))
        {
            return;
        }

        var itemsChanged = false;

        lock (this._itemsLock)
        {
            var oldItems = this._currentItems;

            if (oldItems.Length != newItems.Length)
            {
                this._currentItems = newItems;
                itemsChanged = true;
            }
            else
            {
                for (var i = 0; i < oldItems.Length; i++)
                {
                    if (!Equals(oldItems[i], newItems[i]))
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

            this._updateCancellationSource.Cancel();
            this._updateCancellationSource.Dispose();

            this._updateSemaphore.Dispose();

            // ReSharper disable once InconsistentlySynchronizedField
            this._currentItems = [];
            this._lastSearchResults = null;
        }
    }
}