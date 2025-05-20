using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public interface IColorFormatter
{
    ParsedColorFormat TargetFormat { get; }
    string Format(Unicolour color);
}