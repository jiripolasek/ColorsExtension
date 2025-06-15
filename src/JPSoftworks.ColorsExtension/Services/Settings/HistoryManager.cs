using System.Text.Json;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Services.Settings;

internal sealed class HistoryManager
{
    private const int RecentEntriesCount = 100;

    public event EventHandler? HistoryChanged;

    private readonly Lock _lock = new();

    private HistoryModel _model = new([]);
    public static HistoryManager Instance { get; } = new();

    private string FilePath { get; }

    private HistoryManager()
    {
        this.FilePath = HistoryJsonPath();
        this.LoadSettings();
    }

    private void LoadSettings()
    {
        lock (this._lock)
        {
            try
            {
                var json = File.ReadAllText(this.FilePath);
                this._model = JsonSerializer.Deserialize<HistoryModel>(json, HistorySourceGenerationContext.Default.Options) ?? new HistoryModel([]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                this._model = new HistoryModel([]);
            }
        }
    }

    private void SaveSettings()
    {
        try
        {
            var json = JsonSerializer.Serialize(this._model, HistorySourceGenerationContext.Default.Options);
            File.WriteAllText(this.FilePath, json);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }

    private static string HistoryJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "history.json");
    }

    public void AddColorHistoryEntry(string query, RgbColor rgbColor)
    {
        lock (this._lock)
        {
            this._model.Colors.RemoveAll(entry =>
                entry.R == rgbColor.R && entry.G == rgbColor.G && entry.B == rgbColor.B);

            var entry = new ColorListEntryModel(query, rgbColor.R, rgbColor.G, rgbColor.B, DateTimeOffset.UtcNow);
            this._model.Colors.Add(entry);

            if (this._model.Colors.Count > RecentEntriesCount)
            {
                this._model.Colors.RemoveRange(0, this._model.Colors.Count - RecentEntriesCount);
            }
        }

        this.HistoryChanged?.Invoke(this, EventArgs.Empty);

        this.SaveSettings();
    }

    public IReadOnlyList<ColorListEntryModel> GetColorHistoryEntries()
    {
        lock (this._lock)
        {
            return new List<ColorListEntryModel>(this._model.Colors);
        }
    }

    public void RemoveColorHistoryEntry(string query, RgbColor rgbColor)
    {
        lock (this._lock)
        {
            this._model.Colors.RemoveAll(entry =>
                entry.Query.Equals(query, StringComparison.OrdinalIgnoreCase) &&
                entry.R == rgbColor.R && entry.G == rgbColor.G && entry.B == rgbColor.B);
        }

        this.HistoryChanged?.Invoke(this, EventArgs.Empty);

        this.SaveSettings();
    }
}