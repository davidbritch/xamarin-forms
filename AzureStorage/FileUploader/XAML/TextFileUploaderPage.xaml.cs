using System;
using System.IO;
using System.Text;
using Xamarin.Forms;
using FileUploader.Services;

namespace FileUploader.XAML
{
    public partial class TextFileUploaderPage : ContentPage
    {
        IAzureStorageService _storageService;
        string _uploadedFilename;

        public TextFileUploaderPage()
        {
            InitializeComponent();

            _storageService = DependencyService.Resolve<IAzureStorageService>();
        }

        async void OnUploadButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(uploadEditor.Text))
            {
                activityIndicator.IsRunning = true;

                var byteData = Encoding.UTF8.GetBytes(uploadEditor.Text);
                _uploadedFilename = await _storageService.UploadFileAsync(ContainerType.Text, new MemoryStream(byteData));

                downloadButton.IsEnabled = true;
                activityIndicator.IsRunning = false;
            }
        }

        async void OnDownloadButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_uploadedFilename))
            {
                activityIndicator.IsRunning = true;

                var byteData = await _storageService.GetFileAsync(ContainerType.Text, _uploadedFilename);
                var text = Encoding.UTF8.GetString(byteData);
                downloadEditor.Text = text;

                activityIndicator.IsRunning = false;
            }
        }
    }
}

