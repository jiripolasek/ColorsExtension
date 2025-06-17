// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Commands;

internal sealed partial class CopyAndSaveColorCommand : InvokableCommand
{
    private readonly HistoryManager _historyManager = HistoryManager.Instance;
    private readonly RgbColor _color;
    private readonly string _value;

    private CommandResult Result { get; } = CommandResult.ShowToast("Copied to clipboard");
    
    public CopyAndSaveColorCommand(string value, RgbColor color)
    {
        this._color = color;
        this._value = value;
        this.Name = "Copy " + value;
        this.Icon = Icons.Copy;
    }

    public CopyAndSaveColorCommand(string value, Unicolour color) : this(value,
        new RgbColor(color.Rgb.Byte255.R, color.Rgb.Byte255.G, color.Rgb.Byte255.B))
    {

    }

    public override ICommandResult Invoke()
    {
        ClipboardHelper.SetText(this._value);
        this._historyManager.AddColorHistoryEntry(this._value, this._color);
        return this.Result;
    }
}