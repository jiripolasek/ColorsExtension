// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers.ColorParser;

public class ColorParseResult
{
    public bool Success { get; }
    public Unicolour? Color { get; }
    public ParsedColorFormat? Format { get; }
    public string? Error { get; }

    private ColorParseResult(bool success, Unicolour? color, ParsedColorFormat? format, string? error)
    {
        this.Success = success;
        this.Color = color;
        this.Format = format;
        this.Error = error;
    }

    public static ColorParseResult Ok(Unicolour color, ParsedColorFormat format)
    {
        return new ColorParseResult(true, color, format, null);
    }

    public static ColorParseResult Fail(string error)
    {
        return new ColorParseResult(false, null, null, error);
    }
}