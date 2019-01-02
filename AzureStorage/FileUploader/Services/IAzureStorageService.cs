using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileUploader.Services
{
    public interface IAzureStorageService
    {
        Task<IList<string>> GetFilesListAsync(ContainerType containerType);
        Task<byte[]> GetFileAsync(ContainerType containerType, string name);
        Task<string> UploadFileAsync(ContainerType containerType, Stream stream);
        Task<bool> DeleteFileAsync(ContainerType containerType, string name);
        Task<bool> DeleteContainerAsync(ContainerType containerType);
    }
}
