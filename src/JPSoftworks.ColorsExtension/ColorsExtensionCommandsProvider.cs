// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Pages;
using JPSoftworks.ColorsExtension.Resources;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension;

public sealed partial class ColorsExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly IFallbackCommandItem[] _fallbackCommands;

    public ColorsExtensionCommandsProvider()
    {
        this.Id = "JPSoftworks.CmdPal.ColorsExtension";
        this.DisplayName = Strings.Colors!;
        this.Icon = IconHelpers.FromRelativePath("Assets\\Icons\\ColorsIcon.png");
        var colorsExtensionPage = new ColorsExtensionPage();
        this._commands =
        [
            new CommandItem(colorsExtensionPage) { Title = this.DisplayName, Subtitle = Strings.Colors_Subtitle! }
        ];
        this._fallbackCommands =
        [
            new TopLevelColorFallbackItem("")
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return this._commands;
    }

    public override IFallbackCommandItem[]? FallbackCommands()
    {
        return this._fallbackCommands;
    }
}