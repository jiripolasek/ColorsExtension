// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using JPSoftworks.ColorsExtension.Helpers.ColorManager;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Helpers;

internal static class UnicolourExtensions
{
    /// <summary>
    /// Generates a 5-step shade palette from the given base color, including lightness values.
    /// Index 2 is the base color; indexes 0/1 are darker by 30%/15%,
    /// indexes 3/4 are lighter by 15%/30% (clamped to [0,1]).
    /// </summary>
    /// <param name="baseColor">The starting Unicolour.</param>
    /// <param name="stepFraction">
    /// The lightness increment per step (default 0.15 = 15%).
    /// </param>
    /// <returns>A list of 5 tuples containing Unicolour shades and their lightness values.</returns>
    public static IReadOnlyList<(Unicolour Shade, double Lightness)> GenerateShadesWithLightness(
        this Unicolour baseColor,
        double stepFraction = 0.15)
    {
        const int totalSteps = 5;
        var hsl = baseColor.Hsl; // Hue (°), Saturation (0–1), Lightness (0–1)

        var shades = new List<(Unicolour, double)>(totalSteps);
        const int midIndex = (totalSteps - 1) / 2; // 2 for 5 steps

        for (var i = 0; i < totalSteps; i++)
        {
            var relativeL = (i - midIndex) * stepFraction;
            var newLightness = Math.Clamp(hsl.L + relativeL, 0.0, 1.0);

            // Reconstruct color in HSL space with same hue & saturation
            var shade = new Unicolour(
                ColourSpace.Hsl,
                hsl.H,
                hsl.S,
                newLightness
            );

            shades.Add((shade, relativeL));
        }

        return shades;
    }

    public static IReadOnlyList<(Unicolour Shade, double ValueChange)> GenerateShadesWithValue(
        this Unicolour baseColor)
    {
        var hsv = baseColor.Hsb; // Hue (°), Saturation (0–1), Value (0–1)

        // Calculate hue coefficients based on brightness thresholds
        var hueCoefficient = (1 - hsv.B < 0.15) ? 1 : 0;   // If Value > 0.85 (very bright)
        var hueCoefficient2 = (hsv.B - 0.3 < 0) ? 1 : 0;   // If Value < 0.3 (very dark)

        var shades = new List<(Unicolour, double)>(5);

        // Variation 1: Brightest (+0.3 Value, +8° Hue if very bright)
        var newHue1 = Math.Min(hsv.H + (hueCoefficient * 8), 360);
        var newValue1 = Math.Min(hsv.B + 0.3, 1.0);
        var shade1 = new Unicolour(ColourSpace.Hsb, newHue1, hsv.S, newValue1);
        shades.Add((shade1, 0.3));

        // Variation 2: Lighter (+0.15 Value, +4° Hue if very bright)
        var newHue2 = Math.Min(hsv.H + (hueCoefficient * 4), 360);
        var newValue2 = Math.Min(hsv.B + 0.15, 1.0);
        var shade2 = new Unicolour(ColourSpace.Hsb, newHue2, hsv.S, newValue2);
        shades.Add((shade2, 0.15));

        // Original color (no change)
        shades.Add((baseColor, 0.0));

        // Variation 3: Darker (-0.2 Value, -4° Hue if very dark)
        var newHue3 = Math.Max(hsv.H - (hueCoefficient2 * 4), 0);
        var newValue3 = Math.Max(hsv.B - 0.2, 0.0);
        var shade3 = new Unicolour(ColourSpace.Hsb, newHue3, hsv.S, newValue3);
        shades.Add((shade3, -0.2));

        // Variation 4: Darkest (-0.3 Value, -8° Hue if very dark)
        var newHue4 = Math.Max(hsv.H - (hueCoefficient2 * 8), 0);
        var newValue4 = Math.Max(hsv.B - 0.3, 0.0);
        var shade4 = new Unicolour(ColourSpace.Hsb, newHue4, hsv.S, newValue4);
        shades.Add((shade4, -0.3));

        return shades;
    }


    public static RgbColor ToRgbColor(this Unicolour unicolour)
    {
        return new RgbColor(unicolour.Rgb.Byte255.R, unicolour.Rgb.Byte255.G, unicolour.Rgb.Byte255.B);
    }
}