using System.Text.Json.Serialization;

namespace JPSoftworks.ColorsExtension.Services.Settings;

[JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ColorListEntryModel))]
[JsonSerializable(typeof(FavoriteColors))]
internal partial class FavoritesSourceGenerationContext : JsonSerializerContext
{
}