namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public class MaterialColorSet : IColorSet
{
    private readonly Dictionary<string, (int r, int g, int b)> _colors
        = new(StringComparer.OrdinalIgnoreCase)
        {
            // Material Design Primary Colors (500 variants)
            { "material-red", (244, 67, 54) },
            { "material-pink", (233, 30, 99) },
            { "material-purple", (156, 39, 176) },
            { "material-deep-purple", (103, 58, 183) },
            { "material-indigo", (63, 81, 181) },
            { "material-blue", (33, 150, 243) },
            { "material-light-blue", (3, 169, 244) },
            { "material-cyan", (0, 188, 212) },
            { "material-teal", (0, 150, 136) },
            { "material-green", (76, 175, 80) },
            { "material-light-green", (139, 195, 74) },
            { "material-lime", (205, 220, 57) },
            { "material-yellow", (255, 235, 59) },
            { "material-amber", (255, 193, 7) },
            { "material-orange", (255, 152, 0) },
            { "material-deep-orange", (255, 87, 34) },
            { "material-brown", (121, 85, 72) },
            { "material-grey", (158, 158, 158) },
            { "material-blue-grey", (96, 125, 139) },

            // Material You Dynamic Colors
            { "material-you-primary", (0, 100, 210) },
            { "material-you-secondary", (87, 106, 126) },
            { "material-you-tertiary", (109, 85, 122) },
            { "material-you-error", (186, 26, 26) },
            { "material-you-surface", (255, 251, 254) },
            { "material-you-on-surface", (26, 28, 30) },
            { "material-you-surface-variant", (223, 227, 236) },
            { "material-you-on-surface-variant", (67, 71, 78) }
        };

    public string Name => "material color";

    public IReadOnlyDictionary<string, (int r, int g, int b)> Colors => this._colors;
}