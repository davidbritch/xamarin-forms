using System;

namespace Imaging
{
    public class FrequencyFilter
    {
        FrequencyRange frequencyRange = new FrequencyRange(0, 1024);

        public FrequencyFilter(FrequencyRange range)
        {
            frequencyRange = range;
        }

        public void Apply(ComplexImage complexImage)
        {
            if (!complexImage.IsFourierTransformed)
            {
                throw new ArgumentException("The source complex image should be Fourier transformed.");
            }

            int width = complexImage.Width;
            int height = complexImage.Height;
            int halfWidth = width >> 1;
            int halfHeight = height >> 1;
            int min = frequencyRange.Min;
            int max = frequencyRange.Max;

            Complex[,] data = complexImage.Data;
            for (int i = 0; i < height; i++)
            {
                int y = i - halfHeight;
                for (int j = 0; j < width; j++)
                {
                    int x = j - halfWidth;
                    int d = (int)Math.Sqrt(x * x + y * y);

                    if ((d > max) || (d < min))
                    {
                        data[i, j].Re = 0;
                        data[i, j].Im = 0;
                    }
                }
            }
        }
    }
}
