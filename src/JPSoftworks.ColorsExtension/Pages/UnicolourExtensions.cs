// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using Wacton.Unicolour;

namespace JPSoftworks.ColorsExtension.Pages;

public static class UnicolourExtensions
{
    /// <summary>
    /// Generates a 5-step shade palette from the given base color.
    /// Index 2 is the base color; indexes 0/1 are darker by 30%/15%,
    /// indexes 3/4 are lighter by 15%/30% (clamped to [0,1]).
    /// </summary>
    /// <param name="baseColor">The starting Unicolour.</param>
    /// <param name="stepFraction">
    /// The lightness increment per step (default 0.15 = 15%).
    /// </param>
    /// <returns>A list of 5 Unicolour shades.</returns>
    public static IReadOnlyList<Unicolour> GenerateShades(
        this Unicolour baseColor,
        double stepFraction = 0.15)
    {
        const int totalSteps = 5;
        var hsl = baseColor.Hsl; // Hue (°), Saturation (0–1), Lightness (0–1)

        var shades = new List<Unicolour>(totalSteps);
        const int midIndex = (totalSteps - 1) / 2; // 2 for 5 steps

        for (var i = 0; i < totalSteps; i++)
        {
            var offset = i - midIndex;
            var newLightness = Math.Clamp(hsl.L + (offset * stepFraction), 0.0, 1.0);

            // Reconstruct color in HSL space with same hue & saturation
            var shade = new Unicolour(
                ColourSpace.Hsl,
                hsl.H,
                hsl.S,
                newLightness
            );

            shades.Add(shade);
        }

        return shades;
    }
}