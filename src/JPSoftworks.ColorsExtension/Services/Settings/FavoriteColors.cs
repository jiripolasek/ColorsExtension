using System.Text.Json.Serialization;

namespace JPSoftworks.ColorsExtension.Services.Settings;

[method: JsonConstructor]
public record FavoriteColors(List<ColorListEntryModel> Favorites);