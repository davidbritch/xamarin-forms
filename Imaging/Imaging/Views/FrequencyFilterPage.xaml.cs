using System;
using System.IO;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Imaging
{
    public partial class FrequencyFilterPage : ContentPage
    {
        SKImage image;
        SKPixmap pixmap;

        public FrequencyFilterPage()
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

                    minSlider.IsEnabled = true;
                    maxSlider.IsEnabled = true;
                    processButton.IsEnabled = true;
                }
            }

            (sender as ToolbarItem).IsEnabled = true;
        }

        async void OnSavePhotoClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SavePhotoPage(pixmap)));
        }

        void OnProcessButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            pixmap = image.FrequencyFilter((int)minSlider.Value, (int)maxSlider.Value);
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
