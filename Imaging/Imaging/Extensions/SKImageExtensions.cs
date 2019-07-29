using System;
using SkiaSharp;

namespace Imaging
{
    public static class SKImageExtensions
    {
        public static unsafe SKPixmap ToGreyscale(this SKImage image)
        {
            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;
            byte* tempPtr;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    tempPtr = bmpPtr;
                    byte red = *bmpPtr++;
                    byte green = *bmpPtr++;
                    byte blue = *bmpPtr++;
                    byte alpha = *bmpPtr++;

                    // Assuming SKColorType.Rgba8888 - used by iOS and Android
                    // (UWP uses SKColorType.Bgra8888)
                    byte result = (byte)(0.2126 * red + 0.7152 * green + 0.0722 * blue);

                    bmpPtr = tempPtr;
                    *bmpPtr++ = result; // red
                    *bmpPtr++ = result; // green
                    *bmpPtr++ = result; // blue
                    *bmpPtr++ = alpha;  // alpha
                }
            }
            return pixmap;
        }

        public static unsafe SKPixmap ToSepia(this SKImage image)
        {
            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;
            byte* tempPtr;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    tempPtr = bmpPtr;
                    byte red = *bmpPtr++;
                    byte green = *bmpPtr++;
                    byte blue = *bmpPtr++;
                    byte alpha = *bmpPtr++;

                    // Assuming SKColorType.Rgba8888 - used by iOS and Android
                    // (UWP uses SKColorType.Bgra8888)
                    byte intensity = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);

                    bmpPtr = tempPtr;
                    *bmpPtr++ = (byte)((intensity > 206) ? 255 : intensity + 49); // red
                    *bmpPtr++ = (byte)((intensity < 14) ? 0 : intensity - 14);    // green
                    *bmpPtr++ = (byte)((intensity < 56) ? 0 : intensity - 56);    // blue
                    *bmpPtr++ = alpha;  // alpha                    
                }
            }
            return pixmap;
        }

        public static unsafe SKPixmap OtsuThreshold(this SKImage image)
        {
            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;
            int[] intHistogram = new int[256];
            double[] histogram = new double[256];
            int threshold = 0;

            // Build a histogram
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    byte red = *bmpPtr++;
                    byte green = *bmpPtr++;
                    byte blue = *bmpPtr++;

                    // Assuming SKColorType.Rgba8888 - used by iOS and Android
                    // (UWP uses SKColorType.Bgra8888)
                    int result = (byte)(0.2126 * red + 0.7152 * green + 0.0722 * blue);
                    intHistogram[result]++;
                }
            }

            int pixelCount = width * height;
            double imageMean = 0;

            for (int i = 0; i < 256; i++)
            {
                histogram[i] = (double)intHistogram[i] / pixelCount;
                imageMean += histogram[i] * i;
            }

            double max = double.MinValue;
            double c1Prob = 0;
            double c2Prob = 1;
            double c1MeanInit = 0;

            for (int i = 0; i < 256 && c2Prob > 0; i++)
            {
                double c1Mean = c1MeanInit;
                double c2Mean = (imageMean - (c1Mean * c1Prob)) / c2Prob;
                double classVariance = c1Prob * (1.0 - c1Prob) * Math.Pow(c1Mean - c2Mean, 2);

                if (classVariance > max)
                {
                    max = classVariance;
                    threshold = i;
                }

                c1MeanInit *= c1Prob;
                c1Prob += histogram[i];
                c2Prob -= histogram[i];
                c1MeanInit += (double)i * (double)histogram[i];

                if (Math.Abs(c1Prob) > 0)
                    c1MeanInit /= c1Prob;
            }

            bmpPtr = (byte*)pixmap.GetPixels().ToPointer();

            // Apply threshold
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (*bmpPtr <= threshold)
                    {
                        *bmpPtr++ = 0; // red
                        *bmpPtr++ = 0; // green
                        *bmpPtr++ = 0; // blue
                    }
                    else
                    {
                        *bmpPtr++ = 255; // red
                        *bmpPtr++ = 255; // green
                        *bmpPtr++ = 255; // blue
                    }
                    bmpPtr += 1;
                }
            }

            return pixmap;
        }

        static unsafe ComplexImage ToComplexImage(this SKImage image)
        {
            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;

            if ((!MathHelpers.IsPowerOf2(width)) || (!MathHelpers.IsPowerOf2(height)))
            {
                throw new ArgumentException("Image width and height must be a power of 2.");
            }

            ComplexImage complexImage = new ComplexImage(width, height);
            Complex[,] data = complexImage.Data;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    byte red = *bmpPtr++;
                    byte green = *bmpPtr++;
                    byte blue = *bmpPtr++;
                    bmpPtr += 1; // Ignore alpha

                    // Assuming SKColorType.Rgba8888 - used by iOS and Android
                    // (UWP uses SKColorType.Bgra8888)
                    byte result = (byte)(0.2126 * red + 0.7152 * green + 0.0722 * blue);
                    data[row, col].Re = (float)result / 255;
                }
            }
            return complexImage;
        }

        public static unsafe SKPixmap FrequencyFilter(this SKImage image, int min, int max)
        {
            ComplexImage complexImage = image.ToComplexImage();

            complexImage.FastFourierTransform();
            FrequencyFilter filter = new FrequencyFilter(new FrequencyRange(min, max));
            filter.Apply(complexImage);
            complexImage.ReverseFastFourierTransform();

            SKPixmap pixmap = complexImage.ToSKPixmap(image);
            return pixmap;
        }
    }
}
