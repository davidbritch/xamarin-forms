using System.IO;
using System.Threading.Tasks;

namespace Imaging
{
    public interface IPhotoPickerService
    {
        Task<Stream> PickPhotoAsync();
        Task<bool> SavePhotoAsync(byte[] data, string folder, string filename);
    }
}
