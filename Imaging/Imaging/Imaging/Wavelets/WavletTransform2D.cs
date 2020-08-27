using System;
using System.Threading.Tasks;

namespace Imaging
{
    public abstract class WaveletTransform2D
    {
        protected int width;
        protected int height;
        protected int minSize;
        protected int allowedMinSize;

        volatile bool enableParallel = true;
        object threadSync = new object();

        public bool EnableParallel
        {
            get { return enableParallel; }
            set { enableParallel = value; }
        }

        public WaveletTransform2D(int minSize, int allowedMinSize, int width, int height)
        {
            if (allowedMinSize < 1)
                throw new ArgumentException("allowedMinSize can't be less than one");
            if (minSize < allowedMinSize)
                throw new ArgumentException("minSize can't be smaller than " + allowedMinSize);
            if (width < minSize || height < minSize)
                throw new ArgumentException("width and height must be greater or equal to " + minSize);

            this.width = width;
            this.height = height;
            this.minSize = minSize;
            this.allowedMinSize = allowedMinSize;
        }

        void CheckArrayArgument(float[,] source, string name)
        {
            if (source == null)
                throw new ArgumentException(name + " can't be null");
            if (source.GetLength(0) < width)
                throw new ArgumentException("first dimension of " + name + " can't be smaller than " + width);
            if (source.GetLength(1) < height)
                throw new ArgumentException("second dimension of " + name + " can't be smaller than " + height);
        }

		virtual protected void TransformRow(float[,] source, float[,] dest, int y, int length)
		{
		}

		virtual protected void TransformCol(float[,] source, float[,] dest, int x, int length)
		{
		}

		virtual protected void InverseTransformRow(float[,] source, float[,] dest, int y, int length)
		{
		}

		virtual protected void InverseTransformCol(float[,] source, float[,] dest, int x, int length)
		{
		}

		virtual protected void TransformRows(float[,] source, float[,] dest, int w, int h)
		{
			if (enableParallel)
			{
				Parallel.For(0, h, (y, loopState) =>
				{					
					TransformRow(source, dest, y, w);
				});
			}
			else
			{
				for (int y = 0; y < h; y++)
				{					
					TransformRow(source, dest, y, w);
				}
			}
		}

		virtual protected void TransformCols(float[,] source, float[,] dest, int w, int h)
		{
			if (enableParallel)
			{
				Parallel.For(0, w, (x, loopState) =>
				{					
					TransformCol(source, dest, x, h);
				});
			}
			else
			{
				for (int x = 0; x < w; x++)
				{					
					TransformCol(source, dest, x, h);
				}
			}
		}

		virtual protected void InverseTransformRows(float[,] source, float[,] dest, int w, int h)
		{
			if (enableParallel)
			{
				Parallel.For(0, h, (y, loopState) =>
				{				
					InverseTransformRow(source, dest, y, w);
				});
			}
			else
			{
				for (int y = 0; y < h; y++)
				{					
					InverseTransformRow(source, dest, y, w);
				}
			}
		}

		virtual protected void InverseTransformCols(float[,] source, float[,] dest, int w, int h)
		{
			if (enableParallel)
			{
				Parallel.For(0, w, (x, loopState) =>
				{					
					InverseTransformCol(source, dest, x, h);
				});
			}
			else
			{
				for (int x = 0; x < w; x++)
				{					
					InverseTransformCol(source, dest, x, h);
				}
			}
		}

		virtual public void Transform2D(float[,] source)
		{
			lock (threadSync)
			{
				CheckArrayArgument(source, "source");

				float[,] temp = new float[width, height];
				int w = width;
				int h = height;

				while ((w >= minSize) && (h >= minSize))
				{
					TransformRows(source, temp, w, h);
					TransformCols(temp, source, w, h);
					w = -(-w >> 1);
					h = -(-h >> 1);
				}
			}
		}

		virtual public void ReverseTransform2D(float[,] source)
		{
			lock (threadSync)
			{
				CheckArrayArgument(source, "source");

				int log2 = 1;
				int test = 1;

				while (test < (width | height))
				{
					test <<= 1;
					log2++;
				}

				float[,] temp = new float[width, height];
				int i = 1;

				while (i <= log2)
				{
					int w = -(-width >> (log2 - i));
					int h = -(-height >> (log2 - i));

					if ((w >= minSize) && (h >= minSize))
					{
						InverseTransformCols(source, temp, w, h);
						InverseTransformRows(temp, source, w, h);
					}
					i++;
				}
			}
		}
	}
}
