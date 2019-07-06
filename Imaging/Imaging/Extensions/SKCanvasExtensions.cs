using System;
using SkiaSharp;

namespace Imaging
{
    public static class SKCanvasExtensions
    {
        public static void DrawImage(this SKCanvas canvas, SKImage image, SKRect dest,
            ImageStretch stretch,
            ImageAlignment horizontal = ImageAlignment.Center,
            ImageAlignment vertical = ImageAlignment.Center,
            SKPaint paint = null)
        {
            if (stretch == ImageStretch.Fill)
            {
                canvas.DrawImage(image, dest, paint);
            }
            else
            {
                float scale = 1;                
                switch (stretch)
                {
                    case ImageStretch.None:
                        break;

                    case ImageStretch.Uniform:
                        scale = Math.Min(dest.Width / image.Width, dest.Height / image.Height);
                        break;

                    case ImageStretch.UniformToFill:
                        scale = Math.Max(dest.Width / image.Width, dest.Height / image.Height);
                        break;
                }

                SKRect display = CalculateDisplayRect(dest, scale * image.Width, scale * image.Height, horizontal, vertical);
                canvas.DrawImage(image, display, paint);
            }
        }

        public static void DrawImage(this SKCanvas canvas, SKImage image, SKRect source, SKRect dest,
            ImageStretch stretch,
            ImageAlignment horizontal = ImageAlignment.Center,
            ImageAlignment vertical = ImageAlignment.Center,
            SKPaint paint = null)
        {
            if (stretch == ImageStretch.Fill)
            {
                canvas.DrawImage(image, dest, paint);
            }
            else
            {
                float scale = 1;
                switch (stretch)
                {
                    case ImageStretch.None:
                        break;

                    case ImageStretch.Uniform:
                        scale = Math.Min(dest.Width / source.Width, dest.Height / source.Height);
                        break;

                    case ImageStretch.UniformToFill:
                        scale = Math.Max(dest.Width / source.Width, dest.Height / source.Height);
                        break;
                }

                SKRect display = CalculateDisplayRect(dest, scale * source.Width, scale * source.Height, horizontal, vertical);
                canvas.DrawImage(image, source, display, paint);
            }
        }

        static SKRect CalculateDisplayRect(SKRect dest, float imageWidth, float imageHeight,
            ImageAlignment horizontal, ImageAlignment vertical)
        {
            float x = 0;
            float y = 0;

            switch (horizontal)
            {
                case ImageAlignment.Center:
                    x = (dest.Width - imageWidth) / 2;
                    break;

                case ImageAlignment.Start:
                    break;

                case ImageAlignment.End:
                    x = dest.Width - imageWidth;
                    break;
            }

            switch (vertical)
            {
                case ImageAlignment.Center:
                    y = (dest.Height - imageHeight) / 2;
                    break;

                case ImageAlignment.Start:
                    break;

                case ImageAlignment.End:
                    x = dest.Height - imageHeight;
                    break;
            }

            x += dest.Left;
            y += dest.Top;
            return new SKRect(x, y, x + imageWidth, y + imageHeight);
        }
    }
}
