using System;
using System.IO;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Imaging
{
    public partial class ConvolutionKernelsPage : ContentPage
    {
        SKImage image;
        SKSizeI sizeI;
        float[] kernel;
        bool kernelSelected = false;

        public ConvolutionKernelsPage()
        {
            InitializeComponent();
        }

        async void OnLoadPhotoClicked(object sender, EventArgs e)
        {
            (sender as ToolbarItem).IsEnabled = false;

            using (Stream stream = await DependencyService.Get<IPhotoPickerService>().PickPhotoAsync())
            {
                if (stream != null)
                {
                    image = SKImage.FromEncodedData(stream);
                    canvasView.InvalidateSurface();
                    picker.IsEnabled = true;
                }
            }

            (sender as ToolbarItem).IsEnabled = true;
        }

        async void OnSavePhotoClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SavePhotoPage(image.PeekPixels())));
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            processButton.IsEnabled = picker.SelectedIndex != -1 ? true : false;
        }

        void OnProcessButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            // Most kernels are 3x3
            sizeI = new SKSizeI(3, 3);

            switch (picker.SelectedIndex)
            {
                case 0:
                    kernel = ConvolutionKernels.BoxBlur;
                    break;
                case 1:
                    kernel = ConvolutionKernels.Blur;
                    break;
                case 2:
                    kernel = ConvolutionKernels.EdgeDetection;
                    break;
                case 3:
                    kernel = ConvolutionKernels.Emboss;
                    break;
                case 4:
                    kernel = ConvolutionKernels.GaussianBlur;
                    sizeI = new SKSizeI(7, 7);
                    break;
                case 5:
                    kernel = ConvolutionKernels.Identity;
                    break;
                case 6:
                    kernel = ConvolutionKernels.LaplacianOfGaussian;
                    sizeI = new SKSizeI(5, 5);
                    break;
                case 7:
                    kernel = ConvolutionKernels.Sharpen;;
                    break;
                case 8:
                    kernel = ConvolutionKernels.SobelBottom;
                    break;
                case 9:
                    kernel = ConvolutionKernels.SobelLeft;
                    break;
                case 10:
                    kernel = ConvolutionKernels.SobelRight;
                    break;
                case 11:
                    kernel = ConvolutionKernels.SobelTop;
                    break;
            }

            kernelSelected = true;
            canvasView.InvalidateSurface();

            (sender as Button).IsEnabled = true;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear();
            if (image != null)
            {
                if (kernelSelected)
                {
                    using (SKPaint paint = new SKPaint())
                    {
                        paint.FilterQuality = SKFilterQuality.High;
                        paint.IsAntialias = false;
                        paint.IsDither = false;
                        paint.ImageFilter = SKImageFilter.CreateMatrixConvolution(
                            sizeI, kernel, 1f, 0f, new SKPointI(1, 1),
                            SKMatrixConvolutionTileMode.Clamp, false);

                        canvas.DrawImage(image, info.Rect, ImageStretch.Uniform, paint: paint);
                        image = e.Surface.Snapshot();
                        kernel = null;
                        kernelSelected = false;
                    }
                }
                else
                {
                    canvas.DrawImage(image, info.Rect, ImageStretch.Uniform);
                }
            }
        }
    }
}
