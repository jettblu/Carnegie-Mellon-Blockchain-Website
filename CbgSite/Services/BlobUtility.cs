using Azure.Storage.Blobs;
using CrypticPay;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Services
{
    public class BlobUtility
    {
        CloudBlobClient BlobClient { get; set; }
        string ConnectionString = string.Empty;
        private readonly StorageAccountOptions _settings;
        private readonly Areas.Identity.Data.CbgUser _user;
        public string BlobURI { get; set; }


        public BlobUtility(StorageAccountOptions settings, Areas.Identity.Data.CbgUser user)
        {
            _settings = settings;
            _user = user;
            ConnectionString = _settings.StorageAccountConnectionString;
        }

        // update to handle scaling
        public async Task UploadImage(Stream fileStream, string fileName)
        {
            var containerName = _settings.AvatarImagesContainerNameOption;
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // Get a reference to a blob
            string blobName = GenerateFileName(fileName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream, true);
            BlobURI = _settings.UriBase + containerName + "/" + blobName;
        }

        // generate file name for blob object
        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');

            strFileName = _user.Id + "/" + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "/" + fileName;
            return strFileName;
        }

        public string GetBlobURI(bool isScaled = true)
        {
            return BlobURI;
        }
    }
}
