// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Resources;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Pages;

public sealed partial class HelpPage : ContentPage
{
    private readonly IContent[] _content;

    public HelpPage()
    {
        this.Icon = Icons.Colorful.Question;
        this.Title = Strings.Command_ShowHelp_Title!;
        this.Name = Strings.Command_ShowHelp_Name!;
        this.Id = "com.jpsoftworks.cmdpal.colors.help";

        this._content = [new MarkdownContent(HelpPages.Help!)];
    }

    public override IContent[] GetContent() => this._content;
}