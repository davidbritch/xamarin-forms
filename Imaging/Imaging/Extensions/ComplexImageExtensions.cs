using System;
using SkiaSharp;

namespace Imaging
{
    public static class ComplexImageExtensions
    {
        public static unsafe SKPixmap ToSKPixmap(this ComplexImage complexImage, SKImage image)
        {
            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;

            Complex[,] data = complexImage.Data;
            double scale = (complexImage.IsFourierTransformed) ? Math.Sqrt(width * height) : 1;

            bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    // Assuming SKColorType.Rgba8888 - used by iOS and Android
                    // (UWP uses SKColorType.Bgra8888)
                    byte result = (byte)Math.Max(0, Math.Min(255, data[row, col].Magnitude * scale * 255));
                    *bmpPtr++ = result; // red
                    *bmpPtr++ = result; // green
                    *bmpPtr++ = result; // blue
                    bmpPtr += 1; // Ignore alpha
                }
            }

            return pixmap;
        }
    }
}
