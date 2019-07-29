namespace Imaging
{
    public class ConvolutionKernels
    {
        public static float[] Identity => new float[9]
        {
            0,0,0,
            0,1,0,
            0,0,0
        };

        public static float[] BoxBlur => new float[9]
        {
            0.111f, 0.111f, 0.111f,
            0.111f, 0.111f, 0.111f,
            0.111f, 0.111f, 0.111f
        };

        public static float[] GaussianBlur => new float[49]
        {
            0,  0,   0,     5,  0,  0, 0,
            0,  5,  18,   32,  18,  5, 0,
            0, 18,  64,  100,  64, 18, 0,
            5, 32, 100,  100, 100, 32, 5,
            0, 18,  64,  100,  64, 18, 0,
            0,  5,  18,   32,  18,  5, 0,
            0,  0,   0,    5,   0,  0, 0
        };

        public static float[] Blur => new float[9]
        {
            0.0625f, 0.125f, 0.0625f,
             0.125f,  0.25f,  0.125f,
            0.0625f, 0.125f, 0.0625f
        };

        public static float[] EdgeDetection => new float[9]
        {
            -1, -1, -1,
            -1,  8, -1,
            -1, -1, -1
        };

        public static float[] Sharpen => new float[9]
        {
            0, -1,  0,
            -1, 5, -1,
            0, -1,  0
        };

        public static float[] LaplacianOfGaussian => new float[25]
        {
             0,  0, -1,  0,  0,
             0, -1, -2, -1,  0,
            -1, -2, 16, -2, -1,
             0, -1, -2, -1,  0,
             0,  0, -1,  0,  0
        };

        public static float[] Emboss => new float[9]
        {
            -2, -1, 0,
            -1,  1, 1,
             0,  1, 2
        };

        public static float[] SobelBottom => new float[9]
        {
            -1, -2, -1,
             0,  0,  0,
             1,  2,  1
        };

        public static float[] SobelLeft => new float[9]
        {
            1, 0, -1,
            2, 0, -2,
            1, 0, -1
        };

        public static float[] SobelRight => new float[9]
        {
            -1, 0, 1,
            -2, 0, 2,
            -1, 0, 1
        };

        public static float[] SobelTop => new float[9]
        {
             1,  2,  1,
             0,  0,  0,
            -1, -2, -1
        };
    }
}
