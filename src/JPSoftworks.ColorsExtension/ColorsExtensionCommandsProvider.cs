// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Pages;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension;

public sealed partial class ColorsExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public ColorsExtensionCommandsProvider()
    {
        this.DisplayName = "Colors";
        this.Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        this._commands =
        [
            new CommandItem(new ColorsExtensionPage()) { Title = this.DisplayName }
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return this._commands;
    }
}