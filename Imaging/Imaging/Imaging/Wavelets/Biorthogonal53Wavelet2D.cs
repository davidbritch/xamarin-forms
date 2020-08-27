namespace Imaging
{
    public class Biorthogonal53Wavelet2D : WaveletTransform2D
    {
        protected const int AllowedMinSize = 3;
        protected const float Scale = 2.0f;
        protected const float InverseScale = 0.5f;
        protected const float Mean = 0.5f;
        protected const float InverseMean = 2.0f;
        protected const float Smooth = 0.25f;
        protected const float InverseSmooth = 4.0f;

        public Biorthogonal53Wavelet2D(int width, int height)
            : base (AllowedMinSize, AllowedMinSize, width, height)
        {
        }

        public Biorthogonal53Wavelet2D(int width, int height, int minSize)
            : base (minSize, AllowedMinSize, width, height)
        {
        }

        protected override void TransformRow(float[,] source, float[,] dest, int y, int length)
        {
			if (length >= AllowedMinSize)
			{
				int half = length >> 1;
				if ((length & 1) == 0)
					half--;

				int offSource = 0;
				int offDest = half + 1;
				int numLFValues = offDest;

				float lastHF = 0.0f;
				for (int i = 0; i < half; i++)
				{
					float hf = source[offSource + 1, y] - (source[offSource, y] + source[offSource + 2, y]) * Mean;
					dest[i, y] = (source[offSource, y] + (lastHF + hf) * Smooth) * Scale;
					dest[offDest++, y] = hf;
					lastHF = hf;
					offSource += 2;
				}
				if ((length & 1) == 0)
				{
					dest[numLFValues - 1, y] = Scale * source[length - 2, y];
					dest[length - 1, y] = source[length - 1, y] - source[length - 2, y];
				}
				else
					dest[numLFValues - 1, y] = Scale * source[length - 1, y];
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
				if ((length & 1) == 0)
					half--;

				int offSource = 0;
				int offDest = half + 1;
				int numLFValues = offDest;

				float lastHF = 0.0f;
				for (int i = 0; i < half; i++)
				{
					float hf = source[x, offSource + 1] - (source[x, offSource] + source[x, offSource + 2]) * Mean;
					dest[x, i] = (source[x, offSource] + (lastHF + hf) * Smooth) * Scale;
					dest[x, offDest++] = hf;
					lastHF = hf;
					offSource += 2;
				}
				if ((length & 1) == 0)
				{
					dest[x, numLFValues - 1] = source[x, length - 2] * Scale;
					dest[x, length - 1] = source[x, length - 1] - source[x, length - 2];
				}
				else
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
				if ((length & 1) == 0)
					half--;

				int numLFValues = half + 1;
				float lastLF = InverseScale * source[0, y] - source[numLFValues, y] * Smooth;
				float lastHF = source[numLFValues, y];

				dest[0, y] = lastLF;
				dest[1, y] = lastHF + lastLF * InverseScale;

				for (int i = 1; i < half; i++)
				{
					float hf = source[numLFValues + i, y];
					float lf = InverseScale * source[i, y];
					float lfReconst = lf - (hf + lastHF) * Smooth;

					dest[2 * i, y] = lfReconst;
					dest[2 * i + 1, y] = lfReconst * Mean + hf;
					dest[2 * i - 1, y] += lfReconst * Mean;
					lastHF = hf;
					lastLF = lfReconst;
				}

				if ((length & 1) == 0)
				{
					dest[length - 3, y] += source[numLFValues - 1, y] * Mean * InverseScale;
					dest[length - 2, y] = source[numLFValues - 1, y] * InverseScale;
					dest[length - 1, y] = source[length - 1, y] + source[numLFValues - 1, y] * InverseScale;
				}
				else
				{
					dest[length - 2, y] += source[numLFValues - 1, y] * Mean * InverseScale;
					dest[length - 1, y] = source[numLFValues - 1, y] * InverseScale;
				}
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
				if ((length & 1) == 0)
					half--;

				int numLFValues = half + 1;
				float lastLF = InverseScale * source[x, 0] - source[x, numLFValues] * Smooth;
				float lastHF = source[x, numLFValues];

				dest[x, 0] = lastLF;
				dest[x, 1] = lastHF + lastLF * InverseScale;

				for (int i = 1; i < half; i++)
				{
					float hf = source[x, numLFValues + i];
					float lf = InverseScale * source[x, i];
					float lfReconst = lf - (hf + lastHF) * Smooth;

					dest[x, 2 * i] = lfReconst;
					dest[x, 2 * i + 1] = lfReconst * Mean + hf;
					dest[x, 2 * i - 1] += lfReconst * Mean;
					lastHF = hf;
					lastLF = lfReconst;
				}

				if ((length & 1) == 0)
				{
					dest[x, length - 3] += source[x, numLFValues - 1] * Mean * InverseScale;
					dest[x, length - 2] = source[x, numLFValues - 1] * InverseScale;
					dest[x, length - 1] = source[x, length - 1] + source[x, numLFValues - 1] * InverseScale;
				}
				else
				{
					dest[x, length - 2] += source[x, numLFValues - 1] * Mean * InverseScale;
					dest[x, length - 1] = source[x, numLFValues - 1] * InverseScale;
				}
			}
			else
			{
				for (int i = 0; i < length; i++)
					dest[x, i] = source[x, i];
			}
		}
    }
}
