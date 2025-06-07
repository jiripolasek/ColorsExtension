// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class SelectColorListItem : ListItem
{
    private readonly NamedColorResult _colorResult;
    private readonly DynamicListPage _listPage;

    public SelectColorListItem(DynamicListPage listPage, NamedColorResult colorResult)
    {
        ArgumentNullException.ThrowIfNull(listPage);
        ArgumentNullException.ThrowIfNull(colorResult);

        this._listPage = listPage;
        this._colorResult = colorResult;
        this.Command = new AnonymousCommand(this.Action)
        {
            Result = CommandResult.GoToPage(new GoToPageArgs { PageId = this._listPage.Id })
        };
        this.Title = this._colorResult.GetQualifiedName();
        this.Subtitle = BuildSubtitle(colorResult.Rgb!.Value);
        this.Tags = [new Tag(colorResult.ColorSetObject?.Name!) { Icon = Icons.ColorPalette }];
        _ = this.SetIconAsync(colorResult.Rgb!.Value);
    }

    private static string BuildSubtitle(RgbColor rgb)
    {
        // format as hex • rgb • hsl

        var unicolour = new Unicolour(ColourSpace.Rgb255, rgb.R, rgb.G, rgb.B);

        return $"{unicolour.Hex}  • RGB {unicolour.Rgb.Byte255}  • HSL {unicolour.Hsl}";
    }

    private async Task SetIconAsync(RgbColor rgbColor)
    {
        var iconStream
            = await BitmapStreamFactory.CreateRoundedColorStreamAsync((byte)rgbColor.R, (byte)rgbColor.G,
                (byte)rgbColor.B);
        this.Icon = IconInfo.FromStream(iconStream!);
    }

    private void Action()
    {
        this._listPage.SearchText = this._colorResult.GetQueryName();
    }
}