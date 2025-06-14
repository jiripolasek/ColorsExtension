// ------------------------------------------------------------
//
// Copyright (c) Jiří Polášek. All rights reserved.
//
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.QueryParser;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class InputWarningListItem : ListItem
{
    public InputWarningListItem(ParseWarning warning)
    {
        this.Title = "Warning";
        this.Subtitle = warning.Message;
        this.Icon = Icons.Colorful.Warning;
    }
}