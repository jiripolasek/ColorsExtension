// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Helpers;

internal static class Icons
{
    internal static IconInfo ColorWheel { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\ColorsIcon.png");

    internal static IconInfo ColorWheelLarge { get; } = IconHelpers.FromRelativePath("Assets\\StoreLogo.scale-100.png");

    internal static IconInfo Info => new("\uE946");

    internal static IconInfo Warning => new("\uE7BA");

    internal static IconInfo Error => new("\uE783");

    internal static IconInfo Rename { get; } = new("\uE8AC");

    internal static IconInfo Help { get; } = new("\uE897");

    internal static IconInfo ColorPalette { get; } = new("\uE790");

    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    internal static class Colorful
    {
        internal static IconInfo Warning { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\FluentColorWarning20.svg");

        internal static IconInfo Question { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\FluentColorQuestionCircle20.svg");

        internal static IconInfo Error { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\FluentColorDismissCircle20.svg");

        internal static IconInfo Suggestion { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\FluentColorLightbulbFilament20.svg");

        internal static IconInfo Edit { get; } = IconHelpers.FromRelativePath("Assets\\Icons\\FluentColorEdit20.svg");
    }
}