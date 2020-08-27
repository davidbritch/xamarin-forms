using System;

namespace Imaging
{
    public class ComplexImage
    {
        Complex[,] data;
        int width;
        int height;
        bool isFourierTransformed = false;

        public Complex[,] Data { get { return data; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public bool IsFourierTransformed { get { return isFourierTransformed; } }
        
        public ComplexImage(int w, int h)
        {
            width = w;
            height = h;
            data = new Complex[height, width];
            isFourierTransformed = false;                
        }

        public void FastFourierTransform()
        {
            if (!isFourierTransformed)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (((x + y) & 0x1) != 0)
                        {
                            data[y, x].Re *= -1;
                            data[y, x].Im *= -1;
                        }
                    }
                }

                FourierTransform.FFT2D(data, Direction.Forward);
                isFourierTransformed = true;
            }
        }

        public void ReverseFastFourierTransform()
        {
            if (isFourierTransformed)
            {
                FourierTransform.FFT2D(data, Direction.Reverse);
                isFourierTransformed = false;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (((x + y) & 0x1) != 0)
                        {
                            data[y, x].Re *= -1;
                            data[y, x].Im *= -1;
                        }
                    }
                }
            }
        }
    }
}
