namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public class FluentColorSet : IColorSet
{
    private readonly Dictionary<string, (int r, int g, int b)> _colors
        = new(StringComparer.OrdinalIgnoreCase)
        {
            // Microsoft Fluent Design System Colors

            // Primary Theme Colors (Light Theme)
            { "fluent-blue-primary", (0, 120, 212) },
            { "fluent-blue-shade-10", (0, 99, 177) },
            { "fluent-blue-shade-20", (0, 78, 142) },
            { "fluent-blue-shade-30", (0, 59, 107) },
            { "fluent-blue-tint-10", (50, 138, 216) },
            { "fluent-blue-tint-20", (77, 155, 220) },
            { "fluent-blue-tint-30", (102, 171, 224) },
            { "fluent-blue-tint-40", (127, 186, 229) },

            // Communication Colors
            { "fluent-cyan", (0, 178, 148) },
            { "fluent-dark-blue", (0, 57, 162) },
            { "fluent-dark-green", (0, 89, 0) },
            { "fluent-dark-orange", (141, 38, 0) },
            { "fluent-dark-purple", (85, 0, 119) },
            { "fluent-dark-red", (161, 13, 0) },
            { "fluent-dark-teal", (2, 68, 89) },
            { "fluent-dark-yellow", (102, 61, 0) },
            { "fluent-light-blue", (0, 153, 255) },
            { "fluent-light-green", (125, 201, 56) },
            { "fluent-light-orange", (255, 138, 0) },
            { "fluent-light-purple", (186, 85, 211) },
            { "fluent-light-red", (255, 55, 95) },
            { "fluent-light-teal", (0, 183, 195) },
            { "fluent-light-yellow", (255, 207, 0) },

            // Shared Colors
            { "fluent-pink", (239, 97, 131) },
            { "fluent-rose", (255, 115, 124) },
            { "fluent-gold", (255, 185, 0) },
            { "fluent-bronze", (167, 130, 48) },
            { "fluent-brown", (80, 53, 40) },
            { "fluent-green", (34, 121, 0) },
            { "fluent-dark-cyan", (0, 103, 131) },
            { "fluent-teal", (2, 120, 138) },
            { "fluent-purple", (92, 26, 195) },
            { "fluent-maroon", (129, 53, 53) },
            { "fluent-orange", (191, 87, 0) },
            { "fluent-red", (196, 43, 28) },
            { "fluent-yellow", (223, 166, 0) },

            // Semantic Colors
            { "fluent-success", (49, 140, 54) },
            { "fluent-warning", (255, 144, 0) },
            { "fluent-danger", (164, 38, 44) },
            { "fluent-info", (0, 99, 177) },

            // Neutral Colors (Light Theme)
            { "fluent-gray-10", (250, 249, 248) },
            { "fluent-gray-20", (243, 242, 241) },
            { "fluent-gray-30", (237, 235, 233) },
            { "fluent-gray-40", (225, 223, 221) },
            { "fluent-gray-50", (210, 208, 206) },
            { "fluent-gray-60", (200, 198, 196) },
            { "fluent-gray-70", (190, 185, 184) },
            { "fluent-gray-80", (179, 176, 173) },
            { "fluent-gray-90", (161, 159, 157) },
            { "fluent-gray-100", (151, 149, 146) },
            { "fluent-gray-110", (138, 136, 134) },
            { "fluent-gray-120", (121, 119, 117) },
            { "fluent-gray-130", (96, 94, 92) },
            { "fluent-gray-140", (72, 70, 68) },
            { "fluent-gray-150", (50, 49, 48) },
            { "fluent-gray-160", (37, 36, 35) },
            { "fluent-gray-170", (26, 25, 24) },
            { "fluent-gray-180", (19, 18, 17) },
            { "fluent-gray-190", (16, 15, 14) },
            { "fluent-gray-200", (11, 10, 9) },

            // Microsoft Brand Colors
            { "fluent-microsoft-blue", (0, 103, 184) },
            { "fluent-microsoft-green", (16, 124, 16) },
            { "fluent-microsoft-red", (232, 17, 35) },
            { "fluent-microsoft-yellow", (255, 185, 0) },

            // Office App Colors
            { "fluent-excel-green", (16, 124, 16) },
            { "fluent-word-blue", (43, 87, 154) },
            { "fluent-powerpoint-orange", (183, 71, 42) },
            { "fluent-outlook-blue", (0, 114, 198) },
            { "fluent-teams-purple", (75, 59, 160) },
            { "fluent-onenote-purple", (128, 57, 123) },
            { "fluent-sharepoint-blue", (0, 120, 212) },
            { "fluent-yammer-blue", (0, 115, 178) },
            { "fluent-sway-green", (0, 158, 97) },
            { "fluent-delve-orange", (219, 129, 15) }
        };

    public string Name => "fluent color";

    public IReadOnlyDictionary<string, (int r, int g, int b)> Colors => this._colors;
}