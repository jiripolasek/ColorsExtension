// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Commands;
using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class SelectColorListItem : ListItem
{
    private readonly IDynamicListPage _listPage;
    private readonly string _query;

    public SelectColorListItem(IDynamicListPage listPage, NamedColorResult colorResult)
    {
        ArgumentNullException.ThrowIfNull(listPage);
        ArgumentNullException.ThrowIfNull(colorResult);

        this._listPage = listPage;
        this.Command = new AnonymousCommand(this.Action)
        {
            Result = CommandResult.GoToPage(new GoToPageArgs { PageId = this._listPage.Id }),
            Name = "Show details",
        };
        this.Title = colorResult.GetQualifiedName();
        this.Subtitle = BuildSubtitle(colorResult.Rgb!.Value);
        this.Tags = [new Tag(colorResult.ColorSetObject?.Name!) { Icon = Icons.ColorPalette }];
        _ = this.SetIconAsync(colorResult.Rgb!.Value);
        this._query = colorResult.GetQueryName();
        this.MoreCommands = [
            new CommandContextItem(new AddToFavoritesCommand(this.Title, colorResult.Rgb.Value))
        ];
    }

    internal static string BuildSubtitle(RgbColor rgb)
    {
        var unicolour = new Unicolour(ColourSpace.Rgb255, rgb.R, rgb.G, rgb.B);
        return $"{unicolour.Hex} • RGB {unicolour.Rgb.Byte255} • HSL {unicolour.Hsl}";
    }

    private async Task SetIconAsync(RgbColor rgbColor)
    {
        var iconStream = await BitmapStreamFactory.CreateRoundedColorStreamAsync(rgbColor.R, rgbColor.G, rgbColor.B);
        this.Icon = IconInfo.FromStream(iconStream!);
    }

    private void Action()
    {
        this._listPage.SearchText = this._query;
    }
}