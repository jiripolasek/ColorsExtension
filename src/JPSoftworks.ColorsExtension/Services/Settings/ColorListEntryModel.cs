using System.Text.Json.Serialization;

namespace JPSoftworks.ColorsExtension.Services.Settings;

[method: JsonConstructor]
public record ColorListEntryModel(string Value, string Query, int R, int G, int B, DateTimeOffset DateCopied)
{
    public ColorListEntryModel(string query, int r, int g, int b, DateTimeOffset dateCopied)
        : this(query, query, r, g, b, dateCopied)
    {
    }
}