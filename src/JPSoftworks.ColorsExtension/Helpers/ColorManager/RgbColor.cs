// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

internal record struct RgbColor(byte R, byte G, byte B)
{
    public RgbColor(int r, int g, int b) : this((byte)r, (byte)g, (byte)b)
    {
    }

    public static implicit operator (int r, int g, int b)(RgbColor value)
    {
        return (value.R, value.G, value.B);
    }

    public static implicit operator RgbColor((int r, int g, int b) value)
    {
        return new RgbColor((byte)value.r, (byte)value.g, (byte)value.b);
    }
}