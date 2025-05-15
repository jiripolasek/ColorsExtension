using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace JPSoftworks.ColorsExtension.Services;

/// <summary>
/// Maintains an in-memory MRU list with deduplication and a size limit for type <typeparamref name="T"/>.
/// </summary>
public class HistoryManager<T>
{
    private readonly HistoryStorage<T> _storage;
    private readonly List<T> _items;
    private readonly IEqualityComparer<T> _comparer;

    /// <summary>
    /// Read-only view of the current MRU items.
    /// </summary>
    public IReadOnlyList<T> Items => this._items.AsReadOnly();

    /// <summary>
    /// Maximum number of items retained.
    /// </summary>
    public int Limit { get; }

    /// <param name="fileName">Filename for backing storage (e.g. "mru.json").</param>
    /// <param name="limit">Maximum items to keep; must be > 0.</param>
    /// <param name="comparer">Comparer for detecting duplicates.</param>
    /// <param name="folder">Storage folder override.</param>
    /// <param name="jsonOptions">JSON serializer options.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if limit is not positive.</exception>
    public HistoryManager(
        string fileName,
        int limit = 20,
        IEqualityComparer<T>? comparer = null,
        StorageFolder? folder = null,
        JsonSerializerOptions? jsonOptions = null)
    {
        if (limit <= 0)
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be positive.");

        this.Limit = limit;
        this._comparer = comparer ?? EqualityComparer<T>.Default;
        this._storage = new HistoryStorage<T>(fileName, folder, jsonOptions);
        this._items = [];
    }

    /// <summary>
    /// Initializes the manager by loading existing items up to the configured limit.
    /// </summary>
    public async Task InitializeAsync()
    {
        var loaded = await this._storage.LoadAsync();
        this._items.Clear();
        this._items.AddRange(loaded.Take(this.Limit));
    }

    /// <summary>
    /// Adds an item to the top of the MRU, removes duplicates and enforces <see cref="Limit"/>. Persists changes.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public async Task AddAsync(T item)
    {
        this._items.RemoveAll(x => this._comparer.Equals(x, item));
        this._items.Insert(0, item);
        if (this._items.Count > this.Limit)
        {
            this._items.RemoveRange(this.Limit, this._items.Count - this.Limit);
        }

        await this._storage.SaveAsync(this._items);
    }
}