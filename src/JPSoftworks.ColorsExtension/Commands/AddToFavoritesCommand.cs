// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using JPSoftworks.ColorsExtension.Services.Settings;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace JPSoftworks.ColorsExtension.Commands;

internal sealed partial class AddToFavoritesCommand : InvokableCommand
{
    private readonly string _text;
    private readonly RgbColor _color;
    private readonly FavoritesColorsManager _favoriteColorsManager;

    public AddToFavoritesCommand(string text, RgbColor color)
    {
        this._text = text;
        this._color = color;
        this._favoriteColorsManager = FavoritesColorsManager.Instance;
        this.Name = "Add to favorites";
        this.Icon = Icons.Colorful.Favorite;
    }

    public override ICommandResult Invoke()
    {
        var model = new ColorListEntryModel(this._text, this._color.R, this._color.G, this._color.B, DateTimeOffset.UtcNow);
        this._favoriteColorsManager.AddPinnedColor(model);
        return CommandResult.ShowToast(new ToastArgs { Message = "Added to favorites", Result = CommandResult.KeepOpen() });
    }
}