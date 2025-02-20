using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using QAPDashboard.Shared.Configurations;
using System.Text;

namespace QAPDashboard.Shared.Utilities
{
    public static class AzureStorageHelper
    {
        public static void PutBlob(string testRunID, string identifier, string dataObject, string containerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(RunnerConfiguration.AZStorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient($"{testRunID}/{identifier}");
            blobClient.Upload(new MemoryStream(Encoding.UTF8.GetBytes(dataObject)), overwrite: true);
        }

        public static string GetBlob(string testRunID, string identifier, string containerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(RunnerConfiguration.AZStorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient($"{testRunID}/{identifier}");
            return new StreamReader(blobClient.OpenRead()).ReadToEnd();
        }

        public static Pageable<BlobItem> GetAllBlobs(string storageContainerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(RunnerConfiguration.AZStorageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(storageContainerName);
            return containerClient.GetBlobs();
        }

        public static List<string> GetAllBlobsContent(string identifier, string storageContainerName)
        {
            List<string> blobData = new();
            var allBlobs = GetAllBlobs(storageContainerName);
            foreach (var blobItem in allBlobs)
            {
                if (blobItem.Name.EndsWith($"{identifier}"))
                {
                    blobData.Add(GetBlob(blobItem.Name.Split("/")[0], identifier, storageContainerName));
                }
            }
            return blobData;
        }
    }
}
