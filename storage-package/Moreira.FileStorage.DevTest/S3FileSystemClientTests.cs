using Moreira.FileStorage.Clients;
using Moreira.FileStorage.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Moreira.FileStorage.DevTest
{
    [TestClass]
    public class S3FileSystemTests
    {
        [TestMethod]
        public async Task UploadAndDownloadLocalFile_Success()
        {
            var client = new FileSystemClient();
            var checkvalue = $"UploadAndDownloadLocalFile_Success_{DateTime.Now.Ticks}";
            var fileToUpload = client.CreateAnFakeFile(checkvalue);

            var destination = new FilePath
            {
                Folder = "localrootfolder",
                Path = @"Documents\UploadAndDownloadLocalFile_Success.txt"
            };

            await client.UploadToAsync(fileToUpload, destination);

            Assert.IsTrue(await client.ExistsAsync(destination));

            var text = await client.DownloadFileStringAsync(destination);

            Assert.IsTrue(text.Contains(checkvalue));
        }

        [TestMethod]
        public async Task UploadAndDownloadStream_Success()
        {
            var client = new FileSystemClient();
            var checkvalue = $"UploadAndDownloadStream_Success_{DateTime.Now.Ticks}";
            var fileToUpload = client.CreateAnFakeFile(checkvalue);

            var destination = new FilePath
            {
                Folder = "localrootfolder",
                Path = @"Documents\UploadAndDownloadStream_Success.txt"
            };

            using (var memoryStream = new MemoryStream())
            {
                File.OpenRead(fileToUpload).CopyTo(memoryStream);
                await client.UploadToAsync(memoryStream, destination);
                Assert.IsTrue(await client.ExistsAsync(destination));
            }

            using (var stream = await client.DownloadFileStreamAsync(destination))
            {
                var reader = new StreamReader(stream);
                var text = reader.ReadToEnd();
                Assert.IsTrue(text.Contains(checkvalue));
            }
        }
    }
}
