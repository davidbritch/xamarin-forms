using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using Imaging.iOS.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace Imaging.iOS.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        TaskCompletionSource<Stream> taskCompletionSource;
        UIImagePickerController imagePicker;

        Task<Stream> IPhotoPickerService.PickPhotoAsync()
        {
            imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
            imagePicker.Canceled += OnImagePickerCancelled;

            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController = window.RootViewController;
            viewController.PresentModalViewController(imagePicker, true);

            taskCompletionSource = new TaskCompletionSource<Stream>();
            return taskCompletionSource.Task;
        }

        void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            UIImage image = e.EditedImage ?? e.OriginalImage;
            if (image != null)
            {
                NSData data = image.AsJPEG(1);
                Stream stream = data.AsStream();

                UnregisterEventHandlers();
                taskCompletionSource.SetResult(stream);
            }
            else
            {
                UnregisterEventHandlers();
                taskCompletionSource.SetResult(null);
            }
            imagePicker.DismissModalViewController(true);
        }

        void OnImagePickerCancelled(object sender, EventArgs e)
        {
            UnregisterEventHandlers();
            taskCompletionSource.SetResult(null);
            imagePicker.DismissModalViewController(true);
        }

        void UnregisterEventHandlers()
        {
            imagePicker.FinishedPickingMedia -= OnImagePickerFinishedPickingMedia;
            imagePicker.Canceled -= OnImagePickerCancelled;
        }

        public Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        {
            NSData nsData = NSData.FromArray(data);
            UIImage image = new UIImage(nsData);
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            image.SaveToPhotosAlbum((UIImage img, NSError error) =>
            {
                tcs.SetResult(error == null);
            });
            return tcs.Task;
        }
    }
}
