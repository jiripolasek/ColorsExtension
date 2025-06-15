// ------------------------------------------------------------
// 
// Copyright (c) Jiří Polášek. All rights reserved.
// 
// ------------------------------------------------------------

using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace JPSoftworks.ColorsExtension.Helpers;

internal static class BitmapStreamFactory
{
    /// <summary>
    /// Creates an in-memory bitmap stream containing a filled rounded-square
    /// of the specified solid color. CPU-drawn so it works in a headless COM server.
    /// </summary>
    /// <param name="r">Red channel (0–255).</param>
    /// <param name="g">Green channel (0–255).</param>
    /// <param name="b">Blue channel (0–255).</param>
    /// <param name="size">Width & height in pixels (square). Default is 20.</param>
    /// <param name="cornerRadius"> Corner radius in pixels (clamped to half of size). Default is 4. </param>
    public static async Task<IRandomAccessStream?> CreateRoundedColorStreamAsync(
        byte r,
        byte g,
        byte b,
        uint size = 20,
        double cornerRadius = 4.0)
    {
        try
        {
            // 1) Clamp corner radius so it never exceeds half the side length
            var radius = Math.Min(cornerRadius, size / 2.0);
            // We'll treat each pixel as covering [x, x+1)×[y, y+1)
            // and test its center (x+0.5, y+0.5) against the corner circles.

            var totalPixels = (int)size * (int)size;
            var pixels = new byte[totalPixels * 4]; // BGRA8

            // 2) Walk every pixel
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var inside = true;

                    // Top-left corner?
                    if (x < radius && y < radius)
                    {
                        var dx = radius - 0.5 - x;
                        var dy = radius - 0.5 - y;
                        if ((dx * dx) + (dy * dy) > radius * radius)
                        {
                            inside = false;
                        }
                    }
                    // Top-right corner?
                    else if (x >= size - radius && y < radius)
                    {
                        var dx = x - (size - radius - 0.5);
                        var dy = radius - 0.5 - y;
                        if ((dx * dx) + (dy * dy) > radius * radius)
                        {
                            inside = false;
                        }
                    }
                    // Bottom-left corner?
                    else if (x < radius && y >= size - radius)
                    {
                        var dx = radius - 0.5 - x;
                        var dy = y - (size - radius - 0.5);
                        if ((dx * dx) + (dy * dy) > radius * radius)
                        {
                            inside = false;
                        }
                    }
                    // Bottom-right corner?
                    else if (x >= size - radius && y >= size - radius)
                    {
                        var dx = x - (size - radius - 0.5);
                        var dy = y - (size - radius - 0.5);
                        if ((dx * dx) + (dy * dy) > radius * radius)
                        {
                            inside = false;
                        }
                    }

                    if (!inside)
                    {
                        continue;
                    }

                    // Fill the pixel
                    var idx = ((y * (int)size) + x) * 4;
                    pixels[idx] = b; // Blue
                    pixels[idx + 1] = g; // Green
                    pixels[idx + 2] = r; // Red
                    pixels[idx + 3] = 255; // Alpha
                }
            }

            // 3) Encode to PNG
            var stream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream)!;

            encoder.SetPixelData(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Premultiplied,
                size, size,
                96, 96,
                pixels);

            await encoder.FlushAsync()!;
            stream.Seek(0);
            return stream;
        }
        catch (Exception)
        {
            return null;
        }
    }
}