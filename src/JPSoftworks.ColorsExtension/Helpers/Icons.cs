// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Helpers;

internal static class Icons
{
    internal static IconInfo ColorWheel { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\ColorsIcon.png");

    internal static IconInfo ColorWheelLarge { get; } = IconHelpers.FromRelativePath("Assets\\StoreLogo.scale-100.png");

    internal static IconInfo Info => new("\uE946");

    internal static IconInfo Warning => new("\uE7BA");
}