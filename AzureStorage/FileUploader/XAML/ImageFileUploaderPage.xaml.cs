using System;
using System.IO;
using Xamarin.Forms;
using FileUploader.Services;

namespace FileUploader.XAML
{
    public partial class ImageFileUploaderPage : ContentPage
    {
        IAzureStorageService _storageService;
        string _uploadedFilename;
        byte[] _byteData;

        public ImageFileUploaderPage()
        {
            InitializeComponent();

            _storageService = DependencyService.Resolve<IAzureStorageService>();
            _byteData = Convert.ToByteArray("FileUploader.waterfront.jpg");
            imageToUpload.Source = ImageSource.FromStream(() => new MemoryStream(_byteData));
        }

        async void OnUploadImageButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            _uploadedFilename = await _storageService.UploadFileAsync(ContainerType.Image, new MemoryStream(_byteData));

            uploadButton.IsEnabled = false;
            downloadButton.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void OnDownloadImageButtonClicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(_uploadedFilename))
            {
                activityIndicator.IsRunning = true;

                var imageData = await _storageService.GetFileAsync(ContainerType.Image, _uploadedFilename);
                downloadedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));

                activityIndicator.IsRunning = false;
            }
        }
    }
}
