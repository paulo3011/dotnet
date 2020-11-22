using Moreira.FileStorage.Contracts;
using Moreira.FileStorage.Models;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Moreira.FileStorage.Clients
{
    public class FileSystemClient : IFileServiceClient
    {
        private string _path = System.IO.Path.GetTempPath();

        /// <summary>
        /// Path where files will be saved or fetched.
        /// </summary>
        public string Path { get => _path; set => _path = value; }

        /// <summary>
        /// Returns a path to the file system for the passed FilePath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual string MakePath(FilePath filePath)
        {
            var path = System.IO.Path.Combine(_path, filePath.ToString());
            var dir = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            return path;
        }

        public Task<HttpUploadResponse> UploadToAsync(string sourceFilePath, FilePath destination)
        {
            string destinationFile = MakePath(destination);
            File.Copy(sourceFilePath, destinationFile, true);
            return Task.FromResult<HttpUploadResponse>(new HttpUploadResponse(HttpStatusCode.OK, new Uri($"file://{destinationFile}")));
        }

        public async Task<HttpUploadResponse> UploadToAsync(MemoryStream sourceStream, FilePath destination)
        {
            return await UploadToAsync((Stream)sourceStream, destination);
        }

        public Task<HttpUploadResponse> UploadToAsync(Stream sourceStream, FilePath destination)
        {
            string destinationFile = MakePath(destination);

            sourceStream.Seek(0, SeekOrigin.Begin);

            using (FileStream fs = new FileStream(destinationFile, FileMode.OpenOrCreate))
            {
                sourceStream.CopyTo(fs);
                fs.Flush();
            }

            return Task.FromResult<HttpUploadResponse>(new HttpUploadResponse(HttpStatusCode.OK, new Uri($"file://{destinationFile}")));
        }

        public Task<HttpUploadResponse> UploadToAsync(byte[] sourceBytes, FilePath destination)
        {
            string destinationFile = MakePath(destination);

            using (MemoryStream ms = new MemoryStream(sourceBytes))
            {
                ms.Seek(0, SeekOrigin.Begin);

                using (FileStream fs = new FileStream(destinationFile, FileMode.OpenOrCreate))
                {
                    ms.CopyTo(fs);
                    fs.Flush();
                }
            }

            return Task.FromResult<HttpUploadResponse>(new HttpUploadResponse(HttpStatusCode.OK, new Uri($"file://{destinationFile}")));
        }

        public async Task<IFileResponse> CopyToAsync(FilePath source, FilePath destination)
        {
            return await CopyToAsync(source, destination, true);
        }

        public async Task<IFileResponse> CopyToAsync(FilePath source, FilePath destination, bool waitForCompletion = false, CancellationToken cancellationToken = default)
        {
            string sourceFile = MakePath(source);
            string destinationFile = MakePath(destination);
            File.Copy(sourceFile, destinationFile, true);
            return await Task.FromResult<IFileResponse>(new HttpFileResponse(HttpStatusCode.OK));
        }

        public async Task<string> DownloadFileStringAsync(FilePath sourceToDownload)
        {
            string sourceFile = MakePath(sourceToDownload);

            using (var stream = File.OpenText(sourceFile))
            {
                return await Task.FromResult(stream.ReadToEnd());
            }
        }

        public async Task<Stream> DownloadFileStreamAsync(FilePath sourceToDownload)
        {
            string sourceFile = MakePath(sourceToDownload);
            return await Task.FromResult<Stream>(File.OpenRead(sourceFile));
        }

        public async Task<bool> ExistsAsync(FilePath path)
        {
            string sourceFile = MakePath(path);
            return await Task.FromResult<bool>(File.Exists(sourceFile));
        }

        public async Task<bool> DeleteAsync(FilePath path)
        {
            string sourceFile = MakePath(path);
            File.Delete(sourceFile);
            return await Task.FromResult<bool>(true);
        }

        public async Task<bool> DeleteFolderAsync(string folderName)
        {
            string sourceFile = System.IO.Path.Combine(Path, folderName);
            Directory.Delete(sourceFile, true);
            return await Task.FromResult<bool>(true);
        }

        public async Task<bool> CheckConnectionAsync(string folderName)
        {
            return await Task.FromResult<bool>(true);
        }

        public string CreateAnFakeFile(String message = "")
        {
            string fileName = System.IO.Path.GetTempFileName();

            using (var stream = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
            {
                stream.WriteLine($"test file, created at: {DateTime.Now}");
                stream.WriteLine(message);
            }

            return fileName;
        }

        public async Task<string> CreateSharedUrl(FilePath destination)
        {
            return await Task.FromResult(destination.ToString());
        }

        public async Task<StreamInfo> DownloadFileStreamInfoAsync(FilePath sourceToDownload)
        {
            string sourceFile = MakePath(sourceToDownload);
            var stream = await Task.FromResult<Stream>(File.OpenRead(sourceFile));
            return new StreamInfo
            {
                Stream = stream,
                ContentType = sourceToDownload.ContentType,
                Extension = sourceToDownload.Extension
            };
        }

        public async Task<Uri> GetUri(FilePath destination)
        {
            return await Task.FromResult(new Uri(destination.ToString()));
        }
    }
}