using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Moreira.FileStorage.Contracts;
using Moreira.FileStorage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Moreira.AzureUtils.Clients
{
    public class BlobStorageClient : IFileServiceClient
    {
        /// <summary>
        /// Parameters of the string connection.
        /// </summary>
        private Dictionary<string, string> _connectionStringKeyValues;
        private const string AccountKeyName = "AccountKey";
        private const string AccountNameKey = "AccountName";
        private string _connectionString;

        public async Task<bool> CheckConnectionAsync(string folderName)
        {
            return await ExistsAsync(new FilePath { Folder = folderName });
        }

        public async Task<IFileResponse> CopyToAsync(FilePath source, FilePath destination, bool waitForCompletion = false, CancellationToken cancellationToken = default)
        {
            var blobClientSource = await CreateBlobClient(source);

            var blobClientDestination = await CreateBlobClient(destination);

            // Copy from one path to another
            var responseStart = await blobClientDestination.StartCopyFromUriAsync(blobClientSource.Uri);

            if (waitForCompletion)
            {
                var response = await responseStart.WaitForCompletionAsync();
            }

            return new HttpFileResponse(System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> DeleteAsync(FilePath filePath)
        {
            var containerClient = await CreateBlobContainerClient(filePath, false);
            var response = await containerClient.DeleteBlobIfExistsAsync(filePath.Path);
            return true;
        }

        public async Task<bool> DeleteFolderAsync(string folderName)
        {
            var containerClient = await CreateBlobContainerClient(new FilePath { Folder = folderName }, false);
            var response = await containerClient.DeleteIfExistsAsync();
            return true;
        }

        public async Task<Stream> DownloadFileStreamAsync(FilePath sourceToDownload)
        {
            var blobClient = await CreateBlobClient(sourceToDownload);

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            return download.Content;
        }

        public async Task<StreamInfo> DownloadFileStreamInfoAsync(FilePath sourceToDownload)
        {
            var blobClient = await CreateBlobClient(sourceToDownload);

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            var streamInfo = new StreamInfo 
            { 
                Stream = download.Content, 
                ContentType = download.ContentType,
            };

            if(download.Details != null && download.Details.Metadata != null)
            {
                if (download.Details.Metadata.ContainsKey("Extension"))
                    streamInfo.Extension = download.Details.Metadata["Extension"];
                if (download.Details.Metadata.ContainsKey("ContentType"))
                    streamInfo.ContentType = download.Details.Metadata["ContentType"];
            }

            return streamInfo;
        }

        public async Task<string> DownloadFileStringAsync(FilePath sourceToDownload)
        {
            using(var stream = await DownloadFileStreamAsync(sourceToDownload))
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public async Task<bool> ExistsAsync(FilePath filePath)
        {
            var containerClient = await CreateBlobContainerClient(filePath, false);
            var response = await containerClient.ExistsAsync();
            return response.Value;
        }

        public async Task<HttpUploadResponse> UploadToAsync(string sourceFilePath, FilePath destination)
        {
            using(var uploadFileStream = File.OpenRead(sourceFilePath))
            {
                return await UploadToAsync(uploadFileStream, destination);
            }
        }

        public async Task<HttpUploadResponse> UploadToAsync(MemoryStream sourceStream, FilePath destination)
        {
            return await UploadToAsync((Stream)sourceStream, destination);
        }

        public async Task<HttpUploadResponse> UploadToAsync(Stream sourceStream, FilePath destination)
        {
            var blobClient = await CreateBlobClient(destination);

            BlobHttpHeaders httpHeaders = new BlobHttpHeaders();
            httpHeaders.ContentType = destination.ContentType;

            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("Extension", destination.Extension);
            metadata.Add("ContentType", destination.ContentType);

            await blobClient.UploadAsync(sourceStream, httpHeaders, metadata);
            
            return new HttpUploadResponse(System.Net.HttpStatusCode.OK, blobClient.Uri);
        }

        public async Task<HttpUploadResponse> UploadToAsync(byte[] sourceBytes, FilePath destination)
        {
            using (var uploadFileStream = new MemoryStream(sourceBytes))
            {
                return await UploadToAsync(uploadFileStream, destination);
            }
        }

        public async Task<Uri> GetUri(FilePath destination)
        {
            var containerClient = await CreateBlobContainerClient(destination);
            return containerClient.GetBlobClient(destination.Path).Uri;
        }

        private async Task<BlobClient> CreateBlobClient(FilePath destination)
        {
            // create a container client
            var containerClient = await CreateBlobContainerClient(destination);

            // Creates a reference to the file
            return containerClient.GetBlobClient(destination.Path);
        }

        private string GetConnectionString(bool forceGet = false)
        {
            if (string.IsNullOrWhiteSpace(_connectionString) == false && forceGet == false)
                return _connectionString;

            // Retrieve the connection string for use with the application. The storage
            // connection string is stored in an environment variable on the machine
            // running the application called AZURE_STORAGE_CONNECTION_STRING. If the
            // environment variable is created after the application is launched in a
            // console or with Visual Studio, the shell or application needs to be closed
            // and reloaded to take the environment variable into account.
            _connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new Exception("Env variable AZURE_STORAGE_CONNECTION_STRING was not set.");
                //windows
                //setx AZURE_STORAGE_CONNECTION_STRING "<yourconnectionstring>"
                //linux
                //export AZURE_STORAGE_CONNECTION_STRING="<yourconnectionstring>"
            }
            
            if(_connectionString.Contains(AccountNameKey) == false)
            {
                throw new Exception("Wrong AZURE_STORAGE_CONNECTION_STRING env variable.");
            }

            return _connectionString;
        }

        /// <summary>
        /// Returns a dictionary with the values ​​of the connection string.
        /// DefaultEndpointsProtocol=https;AccountName=moreira;AccountKey=valuehere;EndpointSuffix=core.windows.net
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        private Dictionary<string,string> GetConnectionStringValues(string connectionstring)
        {
            var values = connectionstring.Split(";");
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            foreach(var item in values)
            {
                if(item.Contains(AccountKeyName))
                {
                    keyValuePairs.Add(AccountKeyName, item.Replace($"{AccountKeyName}=", ""));
                }
                else
                {
                    var data = item.Split("=");
                    keyValuePairs.Add(data[0], data[1]);
                }

            }

            return keyValuePairs;
        }

        private string GetConnectionStringSetting(string key)
        {
            if (_connectionStringKeyValues == null)
            {
                var connectionstring = GetConnectionString();
                _connectionStringKeyValues = GetConnectionStringValues(connectionstring);
            }

            return _connectionStringKeyValues[key];
        }

        private string GetAccountKeyFromConnectionString()
        {
            return GetConnectionStringSetting(AccountKeyName);
        }

        private string GetAccountNameFromConnectionString()
        {
            return GetConnectionStringSetting(AccountNameKey);
        }


        /// <summary>
        /// Create blob container client.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="createContainerIfNotExists"></param>
        /// <seealso cref="https://www.nuget.org/packages/Azure.Storage.Blobs"/>
        /// <seealso cref="https://docs.microsoft.com/pt-br/azure/storage/blobs/storage-quickstart-blobs-dotnet#configure-your-storage-connection-string"/>
        /// <seealso cref="https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/storage"/>
        /// <returns></returns>
        private async Task<BlobContainerClient> CreateBlobContainerClient(FilePath destination, bool createContainerIfNotExists = true)
        {
            string containerName = destination.Folder;

            string connectionString = GetConnectionString();

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            //check if the container exists
            var containerExists = await containerClient.ExistsAsync();

            if (containerClient != null && createContainerIfNotExists == false || containerExists)
                return containerClient;

            try
            {
                // Create the container and return a container client object
                return await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a public link to access a file that expires in 1 hour.
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        /// <seealso cref="https://www.nuget.org/packages/Azure.Storage.Blobs"/>
        /// <seealso cref="https://docs.microsoft.com/pt-br/azure/storage/blobs/storage-quickstart-blobs-dotnet#configure-your-storage-connection-string"/>
        /// <seealso cref="https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/storage"/>
        public async Task<string> CreateSharedUrl(FilePath destination)
        {
            // Create a SharedKeyCredential that we can use to sign the SAS token
            var accountName = GetAccountNameFromConnectionString();
            var accountKey = GetAccountKeyFromConnectionString();
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, accountKey);

            // Create a SAS token that's valid for one hour.
            // Be careful with SAS start time. If you set the start time for a SAS to now, then due to clock skew (differences in current time according to different machines), failures may be observed intermittently for the first few minutes. In general, set the start time to be at least 15 minutes in the past. Or, don't set it at all, which will make it valid immediately in all cases. The same generally applies to expiry time as well--remember that you may observe up to 15 minutes of clock skew in either direction on any request. For clients using a REST version prior to 2012-02-12, the maximum duration for a SAS that does not reference a stored access policy is 1 hour, and any policies specifying longer term than that will fail.
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = destination.Folder,
                BlobName = destination.Path,
                Resource = "b",
                //StartsOn = DateTimeOffset.UtcNow.AddMinutes(-15),
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Specify read permissions for the SAS.
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Use the key to get the SAS token.
            //sasBuilder.ToSasQueryParameters()
            string sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();

            // Construct the full URI, including the SAS token.
            UriBuilder fullUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", accountName),
                Path = string.Format("{0}/{1}", destination.Folder, destination.Path),
                Query = sasToken
            };

            return await Task.FromResult(fullUri.Uri.ToString());
        }
    }
}
