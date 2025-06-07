// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public static class ColorQueryParser
{
    private static readonly Lazy<CommandPaletteParser<ColorQueryOptions>> InstanceLazy = new(Create);

    public static CommandPaletteParser<ColorQueryOptions> Instance => InstanceLazy.Value;

    private static CommandPaletteParser<ColorQueryOptions> Create()
    {
        return new CommandPaletteParser<ColorQueryOptions>()
            .AddValueSwitch(
                name: "palette",
                handler: static (query, arg) => query.Palette = arg);
    }
}