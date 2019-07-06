using System;
using System.IO;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Imaging
{
    public partial class BasicImagingPage : ContentPage
    {
        SKImage image;
        SKPixmap pixmap;

        public BasicImagingPage()
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
            await Navigation.PushModalAsync(new NavigationPage(new SavePhotoPage(pixmap)));
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            processButton.IsEnabled = picker.SelectedIndex != -1 ? true : false;
        }

        void OnProcessButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            switch (picker.SelectedIndex)
            {
                case 0:
                    pixmap = image.ToGreyscale();
                    break;
                case 1:
                    pixmap = image.OtsuThreshold();
                    break;
                case 2:
                    pixmap = image.ToSepia();
                    break;
            }
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
                canvas.DrawImage(image, info.Rect, ImageStretch.Uniform);
            }
        }
    }
}
