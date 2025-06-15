using System.Text.Json.Serialization;

namespace JPSoftworks.ColorsExtension.Services.Settings;

[method: JsonConstructor]
public record HistoryModel(List<ColorListEntryModel> Colors);