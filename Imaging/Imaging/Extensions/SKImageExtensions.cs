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

        static unsafe void ToYCbCrAArrays(this SKImage image, float[,] y, float[,] cb, float[,] cr, float[,] a)
        {
            if (y == null)
                throw new ArgumentException("y can't be null");
            if (cb == null)
                throw new ArgumentException("cb can't be null");
            if (cr == null)
                throw new ArgumentException("cr can't be null");
            if (a == null)
                throw new ArgumentException("a can't be null");

            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    byte red = *bmpPtr++;
                    byte green = *bmpPtr++;
                    byte blue = *bmpPtr++;
                    byte alpha = *bmpPtr++;

                    y[col, row] = (0.2990f * red + 0.5870f * green + 0.1140f * blue + 0.5f);
                    cb[col, row] = (-0.1687f * red - 0.3313f * green + 0.5000f * blue + 127.5f + 0.5f);
                    cr[col, row] = (0.5000f * red - 0.4187f * green - 0.0813f * blue + 127.5f + 0.5f);
                    a[col, row] = alpha;
                }
            }
        }

        static unsafe SKPixmap ToRGBAPixmap(this SKImage image, float[,] y, float[,] cb, float[,] cr, float[,] a)
        {
            if (y == null)
                throw new ArgumentException("y can't be null");
            if (cb == null)
                throw new ArgumentException("cb can't be null");
            if (cr == null)
                throw new ArgumentException("cr can't be null");
            if (a == null)
                throw new ArgumentException("a can't be null");

            SKPixmap pixmap = image.PeekPixels();
            byte* bmpPtr = (byte*)pixmap.GetPixels().ToPointer();
            int width = image.Width;
            int height = image.Height;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    float yValue = y[col, row];
                    float cbValue = cb[col, row] - 127.5f;
                    float crValue = cr[col, row] - 127.5f;
                    float aValue = a[col, row];

                    int red = (int)(yValue + 1.40200f * crValue + 0.5f);
                    int green = (int)(yValue - 0.34414f * cbValue - 0.71417f * crValue + 0.5f);
                    int blue = (int)(yValue + 1.77200f * cbValue + 0.5f);
                    int alpha = (int)aValue;

                    if (red < 0)
                        red = 0;
                    else if (red > 255)
                        red = 255;

                    if (green < 0)
                        green = 0;
                    else if (green > 255)
                        green = 255;

                    if (blue < 0)
                        blue = 0;
                    else if (blue > 255)
                        blue = 255;

                    if (alpha < 0)
                        alpha = 0;
                    else if (alpha > 255)
                        alpha = 255;

                    *bmpPtr++ = (byte)red;
                    *bmpPtr++ = (byte)green;
                    *bmpPtr++ = (byte)blue;
                    *bmpPtr++ = (byte)alpha;
                }
            }
            return pixmap;
        }

        public static unsafe SKPixmap WaveletUpscale(this SKImage image, Wavelet wavelet)
        {
            int width = image.Width;
            int height = image.Height;
            int upscaledWidth = width * 2;
            int upscaledHeight = height * 2;
                        
            float[,] y = new float[upscaledWidth, upscaledWidth];
            float[,] cb = new float[upscaledWidth, upscaledWidth];
            float[,] cr = new float[upscaledWidth, upscaledWidth];
            float[,] a = new float[upscaledWidth, upscaledWidth];

            image.ToYCbCrAArrays(y, cb, cr, a);

            WaveletTransform2D wavelet2D;
            WaveletTransform2D upscaledWavelet2D;

            switch (wavelet)
            {
                case Wavelet.Haar:
                    wavelet2D = new HaarWavelet2D(width, height);
                    upscaledWavelet2D = new HaarWavelet2D(upscaledWidth, upscaledHeight);
                    break;
                case Wavelet.Biorthogonal53:
                default:
                    wavelet2D = new Biorthogonal53Wavelet2D(width, height);
                    upscaledWavelet2D = new Biorthogonal53Wavelet2D(upscaledWidth, upscaledHeight);
                    break;
            }

            wavelet2D.Transform2D(y);
            wavelet2D.Transform2D(cb);
            wavelet2D.Transform2D(cr);
            wavelet2D.Transform2D(a);

            upscaledWavelet2D.ReverseTransform2D(y);
            upscaledWavelet2D.ReverseTransform2D(cb);
            upscaledWavelet2D.ReverseTransform2D(cr);
            upscaledWavelet2D.ReverseTransform2D(a);

            for (int row = 0; row < upscaledHeight; row++)
            {
                for (int col = 0; col < upscaledWidth; col++)
                {
                    y[col, row] *= 4.0f;
                    cb[col, row] *= 4.0f;
                    cr[col, row] *= 4.0f;
                    a[col, row] *= 4.0f;
                }
            }

            SKImageInfo info = new SKImageInfo(upscaledWidth, upscaledHeight, SKColorType.Rgba8888);
            SKImage output = SKImage.Create(info);

            SKPixmap pixmap = output.ToRGBAPixmap(y, cb, cr, a);
            return pixmap;                        
        }
    }
}
