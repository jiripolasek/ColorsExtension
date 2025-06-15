// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Commands;

internal sealed class CopyAndSaveColorCommand : InvokableCommand
{
    private readonly HistoryManager _historyManager = HistoryManager.Instance;
    private readonly RgbColor _color;
    private readonly string _query;

    private CommandResult Result { get; } = CommandResult.ShowToast("Copied to clipboard");


    public CopyAndSaveColorCommand(string query, RgbColor color)
    {
        this._color = color;
        this._query = query;
        this.Name = "Copy";
        this.Icon = new IconInfo("\uE8C8");
    }

    public CopyAndSaveColorCommand(string query, Unicolour color) : this(query,
        new RgbColor(color.Rgb.Byte255.R, color.Rgb.Byte255.G, color.Rgb.Byte255.B))
    {

    }

    public override ICommandResult Invoke()
    {
        ClipboardHelper.SetText(this._query);
        this._historyManager.AddColorHistoryEntry(this._query, this._color);
        return this.Result;
    }
}