using System.Globalization;
using JPSoftworks.ColorsExtension.Helpers.ColorParser;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorFormatter;

public class LabModernColorFormatter : IColorFormatter
{
    public ParsedColorFormat TargetFormat => ParsedColorFormat.LabModern;

    public string Format(Unicolour color)
    {
        var lab = color.Lab;
        var l = Math.Round(lab.L, 1);
        var a = Math.Round(lab.A, 1);
        var b = Math.Round(lab.B, 1);

        return
            $"lab({l.ToString(CultureInfo.InvariantCulture)}% {a.ToString(CultureInfo.InvariantCulture)} {b.ToString(CultureInfo.InvariantCulture)})";
    }
}