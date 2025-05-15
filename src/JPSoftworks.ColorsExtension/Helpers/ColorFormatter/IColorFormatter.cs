using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public interface IColorFormatter
{
    string Format(Unicolour color);
    ParsedColorFormat TargetFormat { get; }
}