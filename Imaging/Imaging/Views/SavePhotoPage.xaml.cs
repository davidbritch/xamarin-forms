using System;
using System.IO;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;

namespace Imaging
{
    public partial class SavePhotoPage : ContentPage, IModalPage
    {
        SKPixmap pixmap;

        public SavePhotoPage()
        {
            InitializeComponent();
        }

        public SavePhotoPage(SKPixmap skPixmap)
            : this()
        {
            pixmap = skPixmap;
        }

        public async Task Dismiss()
        {
            await Navigation.PopModalAsync();
        }

        void OnFormatPickerChanged(object sender, EventArgs e)
        {
            if (formatPicker.SelectedIndex != -1)
            {
                SKEncodedImageFormat imageFormat = (SKEncodedImageFormat)formatPicker.SelectedItem;
                fileNameEntry.Text = Path.ChangeExtension(fileNameEntry.Text, imageFormat.ToString().ToLower());
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            SKEncodedImageFormat imageFormat = (SKEncodedImageFormat)formatPicker.SelectedItem;
            int quality = (int)qualitySlider.Value;

            using (MemoryStream memStream = new MemoryStream())
            {
                using (SKManagedWStream wstream = new SKManagedWStream(memStream))
                {
                    bool result = pixmap.Encode(wstream, imageFormat, quality);
                    byte[] data = memStream.ToArray();

                    if (data == null || data.Length == 0)
                        statusLabel.Text = "Encode failed";
                    else
                    {
                        bool success = await DependencyService.Get<IPhotoPickerService>().SavePhotoAsync(data, folderNameEntry.Text, fileNameEntry.Text);

                        if (!success)
                            statusLabel.Text = "Save failed";
                        else
                            statusLabel.Text = "Save succeeded";
                    }
                }
            }
        }
    }
}
