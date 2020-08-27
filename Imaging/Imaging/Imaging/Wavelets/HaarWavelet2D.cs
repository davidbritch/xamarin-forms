namespace Imaging
{
    public class HaarWavelet2D : WaveletTransform2D
    {
        protected const int AllowedMinSize = 2;
        protected const float Scale = 2.0f;
        protected const float InverseScale = 0.5f;
        protected const float Mean = 0.5f;
        protected const float InverseMean = 2.0f;

        public HaarWavelet2D(int width, int height)
            : base(AllowedMinSize, AllowedMinSize, width, height)
        {
        }

        public HaarWavelet2D(int width, int height, int minSize)
            : base (minSize, AllowedMinSize, width, height)
        {
        }

        protected override void TransformRow(float[,] source, float[,] dest, int y, int length)
        {
            if (length >= AllowedMinSize)
            {
                int half = length >> 1;
                int offSource = 0;
                int numLFValues = half + (length & 1);

                for (int i = 0; i < half; i++)
                {
                    float a = source[offSource, y];
                    float b = source[offSource + 1, y];

                    dest[i, y] = (a + b);
                    dest[i + numLFValues, y] = (b - a) * Mean;
                    offSource += 2;
                }
                if ((length & 1) != 0)
                    dest[numLFValues - 1, y] = source[length - 1, y] * Scale;
            }
            else
            {
                for (int i = 0; i < length; i++)
                    dest[i, y] = source[i, y];
            }
        }

        protected override void TransformCol(float[,] source, float[,] dest, int x, int length)
        {
            if (length >= AllowedMinSize)
            {
                int half = length >> 1;
                int offSource = 0;
                int numLFValues = half + (length & 1);

                for (int i = 0; i < half; i++)
                {
                    float a = source[x, offSource];
                    float b = source[x, offSource + 1];

                    dest[x, i] = (a + b);
                    dest[x, i + numLFValues] = (b - a) * Mean;
                    offSource += 2;
                }
                if ((length & 1) != 0)
                    dest[x, numLFValues - 1] = source[x, length - 1] * Scale;
            }
            else
            {
                for (int i = 0; i < length; i++)
                    dest[x, i] = source[x, i];
            }
        }

        protected override void InverseTransformRow(float[,] source, float[,] dest, int y, int length)
        {
            if (length >= AllowedMinSize)
            {
                int half = length >> 1;
                int offDest = 0;
                int numLFValues = half + (length & 1);

                for (int i = 0; i < half; i++)
                {
                    float a = source[i, y] * InverseScale;
                    float b = source[i + numLFValues, y];
                    dest[offDest, y] = a - b;
                    dest[offDest + 1, y] = a + b;
                    offDest += 2;
                }
                if ((length & 1) != 0)
                    dest[length - 1, y] = source[numLFValues - 1, y] * InverseScale;
            }
            else
            {
                for (int i = 0; i < length; i++)
                    dest[i, y] = source[i, y];
            }
        }

        protected override void InverseTransformCol(float[,] source, float[,] dest, int x, int length)
        {
            if (length >= AllowedMinSize)
            {
                int half = length >> 1;
                int offDest = 0;
                int numLFValues = half + (length & 1);

                for (int i = 0; i < half; i++)
                {
                    float a = source[x, i] * InverseScale;
                    float b = source[x, i + numLFValues];
                    dest[x, offDest] = a - b;
                    dest[x, offDest + 1] = a + b;
                    offDest += 2;
                }
                if ((length & 1) != 0)
                    dest[x, length - 1] = source[x, numLFValues - 1] * InverseScale;
            }
            else
            {
                for (int i = 0; i < length; i++)
                    dest[x, i] = source[x, i];
            }
        }
    }
}
