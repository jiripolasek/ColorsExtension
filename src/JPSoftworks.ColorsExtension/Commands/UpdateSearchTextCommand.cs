// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Commands;

internal sealed partial class UpdateSearchTextCommand : InvokableCommand
{
    private readonly string _searchText;
    private readonly IDynamicListPage _target;

    public UpdateSearchTextCommand(string searchText, IDynamicListPage target)
    {
        ArgumentNullException.ThrowIfNull(target);

        this._searchText = searchText;
        this._target = target;
    }
    public override ICommandResult Invoke(object? sender)
    {
        this._target.SearchText = this._searchText;
        return CommandResult.KeepOpen();
    }
}