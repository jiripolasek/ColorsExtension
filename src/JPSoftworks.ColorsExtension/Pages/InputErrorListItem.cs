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
        this.Title = Format(type);
        this.Subtitle = error;
        this.Icon = Icons.Colorful.Error;
    }

    private static string Format(ParseErrorType type)
    {
        return type switch
        {
            ParseErrorType.UnknownSwitch => "Unknown switch",
            ParseErrorType.MissingArgument => "Missing argument for switch",
            ParseErrorType.UnexpectedArgument => "Unexpected argument",
            ParseErrorType.InvalidValue => "Invalid value for switch",
            ParseErrorType.MissingSwitchName => "Missing switch name",
            ParseErrorType.InvalidSwitchName => "Invalid switch name",
            _ => "Unknown error"
        };
    }
}