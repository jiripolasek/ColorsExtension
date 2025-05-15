// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Collections.Generic;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

internal static class ColorFormatNames
{
    // Dictionary mapping ParsedColorFormat to user-friendly names
    private static readonly Dictionary<ParsedColorFormat, string> FormatNames = new()
    {
        { ParsedColorFormat.HexShort, "Hex" },
        { ParsedColorFormat.HexLong, "Hex" },
        { ParsedColorFormat.HexWithoutHash, "Hex" },
        { ParsedColorFormat.Rgb, "RGB" },
        { ParsedColorFormat.RgbModern, "RGB" },
        { ParsedColorFormat.RgbPlain, "RGB" },
        { ParsedColorFormat.Hsl, "HSL" },
        { ParsedColorFormat.HslModern, "HSL" },
        { ParsedColorFormat.Hsv, "HSV" },
        { ParsedColorFormat.HwbModern, "HWB" },
        { ParsedColorFormat.LabModern, "LAB" },
        { ParsedColorFormat.LchModern, "LCH" },
        { ParsedColorFormat.NamedColor, "Named" }
    };

    // Get the user-friendly name for a format
    public static string GetName(ParsedColorFormat format)
    {
        return FormatNames.TryGetValue(format, out var name) ? name : format.ToString();
    }

    // Get the name in display format (suitable for prefixing)
    public static string GetDisplayName(ParsedColorFormat format)
    {
        var name = GetName(format);
        return $"{name}";
    }

    // Get all available format names
    public static IReadOnlyDictionary<ParsedColorFormat, string> GetAllNames()
    {
        return FormatNames;
    }

    // Alternative display names for when you want shorter labels
    private static readonly Dictionary<ParsedColorFormat, string> ShortNames = new Dictionary<ParsedColorFormat, string>
    {
        { ParsedColorFormat.HexShort, "Hex3" },
        { ParsedColorFormat.HexLong, "Hex6" },
        { ParsedColorFormat.HexWithoutHash, "Hex" },
        { ParsedColorFormat.Rgb, "RGB" },
        { ParsedColorFormat.RgbModern, "RGB4" },
        { ParsedColorFormat.RgbPlain, "Plain" },
        { ParsedColorFormat.Hsl, "HSL" },
        { ParsedColorFormat.HslModern, "HSL4" },
        { ParsedColorFormat.Hsv, "HSV" },
        { ParsedColorFormat.HwbModern, "HWB" },
        { ParsedColorFormat.LabModern, "LAB" },
        { ParsedColorFormat.LchModern, "LCH" },
        { ParsedColorFormat.NamedColor, "Name" }
    };

    // Get the short name for a format
    public static string GetShortName(ParsedColorFormat format)
    {
        return ShortNames.TryGetValue(format, out var name) ? name : format.ToString();
    }

    // Get the short name in display format
    public static string GetShortDisplayName(ParsedColorFormat format)
    {
        var name = GetShortName(format);
        return $"{name}:";
    }
}