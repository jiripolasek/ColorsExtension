// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorManager;

namespace JPSoftworks.ColorsExtension.Helpers.QueryParser;

public static class ColorQueryParser
{
    private static readonly Lazy<CommandPaletteParser<ColorQueryOptions>> InstanceLazy = new(Create);

    public static CommandPaletteParser<ColorQueryOptions> Instance => InstanceLazy.Value;

    private static CommandPaletteParser<ColorQueryOptions> Create()
    {
        NamedColorManager namedColorManager = new NamedColorManager();

        return new CommandPaletteParser<ColorQueryOptions>()
            .AddEnumValueSwitch(
                name: "palette",
                handler: static (query, arg) => query.Palette = arg,
                description: "Select a color palette from the available options.",
                valueSuggestions: [.. namedColorManager.ListRegisteredColorSets().Select(static t => new SwitchValueDefinition(t.Id, "Select color palette " + t.Name))]);
    }
}