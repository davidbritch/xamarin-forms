using System.Threading.Tasks;
using Android.Content;
using Android.Media;
using Android.OS;
using Imaging.Droid.Services;
using Java.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace Imaging.Droid.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public Task<System.IO.Stream> PickPhotoAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Photo"),
                MainActivity.PickImageId);

            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<System.IO.Stream>();
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }

        public async Task<bool> SavePhotoAsync(byte[] data, string folder, string filename)
        {
            try
            {
                File picturesDirectory = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures);
                File folderDirectory = picturesDirectory;

                if (!string.IsNullOrEmpty(folder))
                {
                    folderDirectory = new File(picturesDirectory, folder);
                    folderDirectory.Mkdirs();
                }

                using (File bitmapFile = new File(folderDirectory, filename))
                {
                    bitmapFile.CreateNewFile();

                    using (FileOutputStream outputStream = new FileOutputStream(bitmapFile))
                    {
                        await outputStream.WriteAsync(data);
                    }

                    MediaScannerConnection.ScanFile(MainActivity.Instance,
                                                    new string[] { bitmapFile.Path },
                                                    new string[] { "image/png", "image/jpeg" }, null);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
