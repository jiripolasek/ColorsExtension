namespace JPSoftworks.ColorsExtension.Helpers.ColorManager;

public class PantoneColorSet : IColorSet
{
    private readonly Dictionary<string, (int r, int g, int b)> _colors
        = new(StringComparer.OrdinalIgnoreCase)
        {
            // Pantone Color of the Year (2009-2024)
            { "pantone-peach-fuzz", (255, 191, 149) }, // 2024
            { "pantone-viva-magenta", (187, 52, 91) }, // 2023
            { "pantone-very-peri", (102, 103, 171) }, // 2022
            { "pantone-ultimate-gray", (147, 149, 151) }, // 2021
            { "pantone-illuminating", (245, 223, 77) }, // 2021
            { "pantone-classic-blue", (15, 76, 129) }, // 2020
            { "pantone-living-coral", (255, 122, 88) }, // 2019
            { "pantone-ultra-violet", (88, 24, 69) }, // 2018
            { "pantone-greenery", (136, 176, 75) }, // 2017
            { "pantone-rose-quartz", (247, 202, 201) }, // 2016
            { "pantone-serenity", (145, 168, 208) }, // 2016
            { "pantone-marsala", (188, 72, 77) }, // 2015
            { "pantone-radiant-orchid", (181, 101, 167) }, // 2014
            { "pantone-emerald", (0, 158, 96) }, // 2013
            { "pantone-tangerine-tango", (221, 73, 4) }, // 2012
            { "pantone-honeysuckle", (214, 76, 102) }, // 2011
            { "pantone-turquoise", (0, 173, 181) }, // 2010
            { "pantone-mimosa", (255, 239, 0) }, // 2009Q

            // Classic Pantone Colors
            { "pantone-red-032", (237, 41, 57) },
            { "pantone-yellow-012", (254, 221, 0) },
            { "pantone-blue-072", (0, 56, 168) },
            { "pantone-green-376", (0, 133, 67) },
            { "pantone-orange-021", (254, 80, 0) },
            { "pantone-purple-269", (179, 32, 144) },
            { "pantone-cyan-306", (0, 174, 239) },
            { "pantone-magenta-806", (236, 0, 140) },
            { "pantone-warm-gray-7", (162, 152, 134) },
            { "pantone-cool-gray-7", (151, 153, 155) },
            { "pantone-black", (35, 31, 32) },
            { "pantone-white", (245, 245, 245) },

            // Corporate Colors
            { "pantone-reflex-blue", (0, 37, 130) },
            { "pantone-process-blue", (0, 133, 202) },
            { "pantone-bright-red", (245, 0, 41) },
            { "pantone-warm-red", (245, 54, 92) },
            { "pantone-rubine-red", (206, 18, 86) },
            { "pantone-rhodamine-red", (227, 6, 123) },
            { "pantone-purple", (177, 66, 157) },
            { "pantone-violet", (123, 84, 168) },
            { "pantone-process-cyan", (0, 174, 239) },
            { "pantone-green", (0, 158, 73) },
            { "pantone-bright-green", (0, 225, 118) },
            { "pantone-yellow", (254, 221, 0) },
            { "pantone-warm-yellow", (254, 211, 48) },
            { "pantone-warm-orange", (254, 105, 32) },
            { "pantone-brown", (111, 78, 55) },
            { "pantone-warm-gray", (177, 169, 159) },
            { "pantone-cool-gray", (149, 154, 157) },
            { "pantone-neutral-gray", (170, 171, 172) }
        };

    public string Name => "pantone";

    public IReadOnlyDictionary<string, (int r, int g, int b)> Colors => this._colors;
}