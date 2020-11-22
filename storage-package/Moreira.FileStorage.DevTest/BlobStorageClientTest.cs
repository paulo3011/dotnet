using Moreira.FileStorage.Contracts;
using Moreira.FileStorage.DevTest;
using Moreira.FileStorage.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Moreira.AzureUtils.Clients;

namespace Moreira.DevTest.Moreira.Azure.Storage
{
    [TestClass]
    public class BlobStorageClientTest : TestBase
    {
        private string folder = "moreiradev";

        [TestMethod]
        public async Task CrudAndCopy_Success()
        {
            var client = new BlobStorageClient();            
            string text = "texto gravado";
            string sourceFilePath = CreateNewTxtFile(text);
            string filename = Path.GetFileName(sourceFilePath);

            FilePath destination = new FilePath { 
                Folder = folder,
                Path= $"forms\\{filename}"
            };            

            var response = await client.UploadToAsync(sourceFilePath, destination);

            Assert.IsTrue(((HttpFileResponse)response).HttpStatusCode == System.Net.HttpStatusCode.OK);

            var downloadStream = await client.DownloadFileStreamAsync(destination);
            using(var reader = new StreamReader(downloadStream))
            {
                var texto = reader.ReadToEnd();
                Assert.IsTrue(text == texto);
            }

            var downloadString = await client.DownloadFileStringAsync(destination);

            Assert.IsTrue(text == downloadString);

            FilePath destinationCopy = new FilePath
            {
                Folder = folder,
                Path = $"forms\\copy\\{filename}"
            };

            var result = await client.CopyToAsync(destination, destinationCopy, true);

            var downloadStringCopy = await client.DownloadFileStringAsync(destinationCopy);

            Assert.IsTrue(text == downloadStringCopy);

            var exists = await client.ExistsAsync(destinationCopy);
            Assert.IsTrue(exists);

            var deleted = await client.DeleteAsync(destinationCopy);

            Assert.IsTrue(deleted);

            var destinationExists = await client.ExistsAsync(destination);
            Assert.IsTrue(destinationExists);

            await client.DeleteFolderAsync(destination.Folder);
        }
    }
}