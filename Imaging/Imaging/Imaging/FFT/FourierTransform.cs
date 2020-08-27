using System;

namespace Imaging
{
    public static class FourierTransform
    {
        const int minLength = 2;
        const int maxLength = 16384;
        const int minBits = 1;
        const int maxBits = 14;
        static int[][] reversedBits = new int[maxBits][];
        static Complex[,][] complexRotation = new Complex[maxBits, 2][];

        public static void FFT(Complex[] data, Direction direction)
        {
            int n = data.Length;
            int m = MathHelpers.Log2(n);

            // Reorder data
            ReorderData(data);

            // Compute FFT
            int tn = 1, tm;

            for (int k = 1; k <= m; k++)
            {
                Complex[] rotation = GetComplexRotation(k, direction);
                tm = tn;
                tn <<= 1;

                for (int i = 0; i < tm; i++)
                {
                    Complex t = rotation[i];
                    for (int even = i; even < n; even += tn)
                    {
                        int odd = even + tm;
                        Complex ce = data[even];
                        Complex co = data[odd];

                        double tr = co.Re * t.Re - co.Im * t.Im;
                        double ti = co.Re * t.Im + co.Im * t.Re;

                        data[even].Re += tr;
                        data[even].Im += ti;
                        data[odd].Re = ce.Re - tr;
                        data[odd].Im = ce.Im - ti;
                    }
                }
            }

            if (direction == Direction.Forward)
            {
                for (int i = 0; i < n; i++)
                {
                    data[i].Re /= (double)n;
                    data[i].Im /= (double)n;
                }
            }
        }

        public static void FFT2D(Complex[,] data, Direction direction)
        {
            int k = data.GetLength(0);
            int n = data.GetLength(1);

            // Check data size
            if ((!MathHelpers.IsPowerOf2(k)) || (!MathHelpers.IsPowerOf2(n)) ||
                (k < minLength) || (k > maxLength) ||
                (n < minLength) || (n > maxLength))
            {
                throw new ArgumentException("Incorrect data length.");
            }

            // Process rows
            Complex[] row = new Complex[n];
            for (int i = 0; i < k; i++)
            {
                // Copy row
                for (int j = 0; j < n; j++)
                    row[j] = data[i, j];
                // Transform it
                FFT(row, direction);
                // Copy back
                for (int j = 0; j < n; j++)
                    data[i, j] = row[j];
            }

            // Process columns
            Complex[] col = new Complex[k];
            for (int j = 0; j < n; j++)
            {
                // Copy column
                for (int i = 0; i < k; i++)
                    col[i] = data[i, j];
                // Transform it
                FFT(col, direction);
                // Copy back
                for (int i = 0; i < k; i++)
                    data[i, j] = col[i];
            }
        }

        static int[] GetReversedBits(int numberOfBits)
        {
            if ((numberOfBits < minBits) || (numberOfBits > maxBits))
                throw new ArgumentOutOfRangeException();

            // Check if the array is already calculated
            if (reversedBits[numberOfBits - 1] == null)
            {
                int n = MathHelpers.Pow2(numberOfBits);
                int[] rBits = new int[n];

                // Calculate the array
                for (int i = 0; i < n; i++)
                {
                    int oldBits = i;
                    int newBits = 0;

                    for (int j = 0; j < numberOfBits; j++)
                    {
                        newBits = (newBits << 1) | (oldBits & 1);
                        oldBits = (oldBits >> 1);
                    }
                    rBits[i] = newBits;
                }
                reversedBits[numberOfBits - 1] = rBits;
            }
            return reversedBits[numberOfBits - 1];
        }

        static Complex[] GetComplexRotation(int numberOfBits, Direction direction)
        {
            int directionIndex = (direction == Direction.Forward) ? 0 : 1;

            // Check if the array is already calculated
            if (complexRotation[numberOfBits - 1, directionIndex] == null)
            {
                int n = 1 << (numberOfBits - 1);
                double uR = 1.0;
                double uI = 0.0;
                double angle = Math.PI / n * (int)direction;
                double wR = Math.Cos(angle);
                double wI = Math.Sin(angle);
                double t;
                Complex[] rotation = new Complex[n];

                for (int i = 0; i < n; i++)
                {
                    rotation[i] = new Complex(uR, uI);
                    t = uR * wI + uI * wR;
                    uR = uR * wR - uI * wI;
                    uI = t;
                }

                complexRotation[numberOfBits - 1, directionIndex] = rotation;
            }
            return complexRotation[numberOfBits - 1, directionIndex];
        }

        static void ReorderData(Complex[] data)
        {
            int length = data.Length;

            // Check data length
            if ((length < minLength) || (length > maxLength) || (!MathHelpers.IsPowerOf2(length)))
                throw new ArgumentException("Incorrect data length.");

            int[] rBits = GetReversedBits(MathHelpers.Log2(length));

            for (int i = 0; i < length; i++)
            {
                int s = rBits[i];
                if (s > i)
                {
                    Complex t = data[i];
                    data[i] = data[s];
                    data[s] = t;
                }
            }
        }
    }
}
