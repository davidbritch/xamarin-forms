using System;
using System.Text;
using Xamarin.Forms;
using FileUploader.Services;

namespace FileUploader.XAML
{
    public partial class TextFileBrowserPage : ContentPage
    {
        IAzureStorageService _storageService;
        string _fileName;

        public TextFileBrowserPage()
        {
            InitializeComponent();

            _storageService = DependencyService.Resolve<IAzureStorageService>();
        }

        async void OnGetFileListButtonClicked(object sender, EventArgs e)
        {
            var fileList = await _storageService.GetFilesListAsync(ContainerType.Text);
            listView.ItemsSource = fileList;
            editor.Text = string.Empty;
            deleteButton.IsEnabled = false;
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _fileName = e.SelectedItem.ToString();
            var byteData = await _storageService.GetFileAsync(ContainerType.Text, _fileName);
            var text = Encoding.UTF8.GetString(byteData);
            editor.Text = text;
            deleteButton.IsEnabled = true;
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                bool isDeleted = await _storageService.DeleteFileAsync(ContainerType.Text, _fileName);
                if (isDeleted)
                {
                    OnGetFileListButtonClicked(sender, e);
                }
            }
        }
    }
}
