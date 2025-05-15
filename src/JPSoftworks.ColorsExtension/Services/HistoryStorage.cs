// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace JPSoftworks.ColorsExtension.Services;


/// <summary>
/// Handles JSON-based persistence for a history list of <typeparamref name="T"/> items.
/// </summary>
public class HistoryStorage<T>
{
    private readonly StorageFolder _folder;
    private readonly string _fileName;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <param name="fileName">Filename for storing the JSON (e.g. "history.json").</param>
    /// <param name="folder">Storage folder override (defaults to LocalFolder).</param>
    /// <param name="jsonOptions">Custom serializer options.</param>
    public HistoryStorage(
        string fileName,
        StorageFolder? folder = null,
        JsonSerializerOptions? jsonOptions = null)
    {
        this._fileName = fileName;
        this._folder = folder ?? ApplicationData.Current!.LocalFolder!;
        this._jsonOptions = jsonOptions ?? new() { WriteIndented = false };
    }

    /// <summary>
    /// Loads the persisted history, returning an empty list if none or on error.
    /// </summary>
    public async Task<List<T>> LoadAsync()
    {
        var item = await this._folder.TryGetItemAsync(this._fileName);
        if (item is not StorageFile file)
        {
            return [];
        }

        try
        {
            string json = await FileIO.ReadTextAsync(file);
            return JsonSerializer.Deserialize<List<T>>(json, this._jsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    /// <summary>
    /// Saves the given items to disk, overwriting any existing file.
    /// </summary>
    public async Task SaveAsync(IEnumerable<T> items)
    {
        var file = await this._folder.CreateFileAsync(this._fileName,
            CreationCollisionOption.ReplaceExisting);

        string json = JsonSerializer.Serialize(items, this._jsonOptions);
        await FileIO.WriteTextAsync(file, json);
    }
}