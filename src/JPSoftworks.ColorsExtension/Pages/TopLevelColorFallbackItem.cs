// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers;
using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.Storage.Streams;

namespace JPSoftworks.ColorsExtension.Pages;

internal sealed partial class TopLevelColorFallbackItem : FallbackCommandItem, IDisposable
{
    private readonly ColorsExtensionPage _page;
    private Task<IRandomAccessStream?>? _currentTaskCreateColorSwatch;

    private RgbColor? _lastColor;

    public TopLevelColorFallbackItem(string displayTitle) : base(new NoOpCommand(), displayTitle)
    {
        // Let's create a new extension page so we can manipulate its name without affecting the top level one.
        this._page = new ColorsExtensionPage();
        this.Command = this._page;
        this.SetEmpty();
    }

    public override void UpdateQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
        {
            this.SetEmpty();
            return;
        }

        var result = new ColorParsingCoordinator(new NamedColorManager()).Parse(query, null);

        switch (result.Strategy)
        {
            case CombinedParseStrategy.ExactMatch:
                var rgbByte255 = result.ExactResult!.Color!.Rgb.Byte255;
                var color = (rgbByte255.R, rgbByte255.G, rgbByte255.B);
                this.SetColorAndText(query, SelectColorListItem.BuildSubtitle(color), color, query);
                this._page.Name = "Show details for " + color;
                break;
            case CombinedParseStrategy.AutoSelectNamed:
            case CombinedParseStrategy.ShowNamedOptions:

                string text;
                var namedColorResult = result.NamedResult;
                var bestMatch = namedColorResult!.BestMatch;

                string matchText
                    = string.Equals(bestMatch!.ColorName, query.Trim('"'), StringComparison.OrdinalIgnoreCase)
                        ? query
                        : $"{query} as {bestMatch.ColorName}";

                if (namedColorResult.AllMatches.Count > 1)
                {
                    text = $"Matching {matchText} and {(namedColorResult.AllMatches.Count - 1)} similar colors...";
                }
                else
                {
                    text = $"Matching {matchText}";
                }

                this.SetColorAndText(text,
                    bestMatch.GetQualifiedName() + " • " + SelectColorListItem.BuildSubtitle(namedColorResult.BestMatch!.Rgb!.Value),
                    namedColorResult.BestMatch!.Rgb!.Value,
                    query);

                if (namedColorResult.AllMatches.Count > 1)
                {
                    this._page.Name = $"Show matches for '{query}'";
                }
                else
                {
                    this._page.Name = $"Show details for {namedColorResult.BestMatch.GetQualifiedName()}";
                }

                break;
            default:
                this.SetEmpty();
                break;
        }
    }

    private void SetColorAndText(string title, string subtitle, RgbColor color, string query)
    {
        this.Title = title;
        this.Subtitle = subtitle;
        this._page.Name = title;
        this._page.SearchText = query;
        if (this._lastColor != color)
        {
            _ = this.SetIconAsync(color);
            this._lastColor = color;
        }
    }


    private async Task SetIconAsync(RgbColor rgbColor)
    {
        var taskCreateColorSwatch = BitmapStreamFactory.CreateRoundedColorStreamAsync(rgbColor.R, rgbColor.G, rgbColor.B);
        this._currentTaskCreateColorSwatch = taskCreateColorSwatch;
        IRandomAccessStream? iconStream = await taskCreateColorSwatch;
        if (this._currentTaskCreateColorSwatch == taskCreateColorSwatch)
        {
            this.Icon = IconInfo.FromStream(iconStream!);
            this._currentTaskCreateColorSwatch = null;
        }
    }

    private void SetEmpty()
    {
        this._page.Name = string.Empty;
        this.Title = string.Empty;
        this.Subtitle = string.Empty;
        this.MoreCommands = [];
        this.Icon = null;
        this._lastColor = null;
    }

    public void Dispose()
    {
        this._page.Dispose();
        this._currentTaskCreateColorSwatch?.Dispose();
    }
}