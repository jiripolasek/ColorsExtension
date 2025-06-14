// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.QueryParser;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class InputErrorListItem : ListItem
{
    public InputErrorListItem(string error, ParseErrorType type)
    {
        this.Title = type.ToString();
        this.Subtitle = error;
        this.Icon = Icons.Colorful.Error;
    }
}