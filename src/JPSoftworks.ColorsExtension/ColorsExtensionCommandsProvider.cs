// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Pages;
using JPSoftworks.ColorsExtension.Resources;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension;

public sealed partial class ColorsExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly IFallbackCommandItem[] _fallbackCommands;

    private readonly SettingsManager _settingsManager = new();

    public ColorsExtensionCommandsProvider()
    {
        this.Id = "JPSoftworks.CmdPal.ColorsExtension";
        this.DisplayName = Strings.Colors!;
        this.Icon = IconHelpers.FromRelativePath("Assets\\Icons\\ColorsIcon.png");
        this.Settings = this._settingsManager.Settings;

        var colorsExtensionPage = new ColorsExtensionPage();
        this._commands =
        [
            new CommandItem(colorsExtensionPage) {
                Title = this.DisplayName,
                Subtitle = Strings.Colors_Subtitle!,
                MoreCommands = [
                    new CommandContextItem(this.Settings!.SettingsPage!),
                    new CommandContextItem(new HelpPage())
                ]}
        ];
        this._fallbackCommands =
        [
            new TopLevelColorFallbackItem(Strings.MatchColorFallbackItem_DisplayTitle!)
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return this._commands;
    }

    public override IFallbackCommandItem[] FallbackCommands()
    {
        return this._fallbackCommands;
    }
}