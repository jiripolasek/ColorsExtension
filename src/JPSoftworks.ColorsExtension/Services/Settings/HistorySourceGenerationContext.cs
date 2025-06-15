using System.Text.Json.Serialization;

namespace JPSoftworks.ColorsExtension.Services.Settings;

[JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(HistoryModel))]
[JsonSerializable(typeof(ColorListEntryModel))]
internal partial class HistorySourceGenerationContext : JsonSerializerContext
{
}