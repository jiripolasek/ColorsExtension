using System.Text.Json;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Services.Settings;

internal sealed class FavoritesColorsManager
{
    public static FavoritesColorsManager Instance { get; } = new();

    public event EventHandler? PinnedColorsChanged;

    private readonly Lock _lock = new();
    private FavoriteColors _model = new([]);
    private string FilePath { get; }

    private FavoritesColorsManager()
    {
        this.FilePath = JsonPath();
        this.LoadSettings();
    }

    private void LoadSettings()
    {
        lock (this._lock)
        {
            try
            {
                var json = File.ReadAllText(this.FilePath);
                this._model = JsonSerializer.Deserialize<FavoriteColors>(json, FavoritesSourceGenerationContext.Default.Options) ?? new FavoriteColors([]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                this._model = new FavoriteColors([]);
            }
        }
    }

    private void SaveSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(this._model, FavoritesSourceGenerationContext.Default.Options);
            File.WriteAllText(this.FilePath, json);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    private static string JsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "favorites.json");
    }

    public void AddPinnedColor(ColorListEntryModel entry)
    {
        lock (this._lock)
        {
            this._model.Favorites.RemoveAll(e => e.Query.Equals(entry.Query, StringComparison.OrdinalIgnoreCase));
            this._model.Favorites.Add(entry);
        }

        this.PinnedColorsChanged?.Invoke(this, EventArgs.Empty);
        this.SaveSettings();
    }

    public IReadOnlyList<ColorListEntryModel> GetPinnedColors()
    {
        lock (this._lock)
        {
            return new List<ColorListEntryModel>(this._model.Favorites);
        }
    }

    public void RemovePinnedColor(ColorListEntryModel entry)
    {
        lock (this._lock)
        {
            this._model.Favorites.RemoveAll(e => e.Query.Equals(entry.Query, StringComparison.OrdinalIgnoreCase));
        }

        this.PinnedColorsChanged?.Invoke(this, EventArgs.Empty);
        this.SaveSettings();
    }
}