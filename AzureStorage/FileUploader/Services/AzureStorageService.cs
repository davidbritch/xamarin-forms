using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace FileUploader.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        CloudStorageAccount _storageAccount;
        CloudBlobClient _client;

        public AzureStorageService()
        {
            _storageAccount = CloudStorageAccount.Parse(Constants.StorageConnection);
            _client = CreateBlobClient();
        }

        CloudBlobClient CreateBlobClient()
        {
            CloudBlobClient client = _storageAccount.CreateCloudBlobClient();
            client.DefaultRequestOptions = new BlobRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(3), 4),
                LocationMode = LocationMode.PrimaryThenSecondary,
                MaximumExecutionTime = TimeSpan.FromSeconds(20)
            };
            return client;
        }

        public async Task<IList<string>> GetFilesListAsync(ContainerType containerType)
        {
            var container = _client.GetContainerReference(containerType.ToString().ToLower());

            var allBlobsList = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Any())
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobsList.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);

            return allBlobsList;
        }

        public async Task<byte[]> GetFileAsync(ContainerType containerType, string name)
        {
            var container = _client.GetContainerReference(containerType.ToString().ToLower());

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public async Task<string> UploadFileAsync(ContainerType containerType, Stream stream)
        {
            var container = _client.GetContainerReference(containerType.ToString().ToLower());
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);

            return name;
        }

        public async Task<bool> DeleteFileAsync(ContainerType containerType, string name)
        {
            var container = _client.GetContainerReference(containerType.ToString().ToLower());
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> DeleteContainerAsync(ContainerType containerType)
        {
            var container = _client.GetContainerReference(containerType.ToString().ToLower());
            return await container.DeleteIfExistsAsync();
        }
    }
}
