using Moreira.FileStorage.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Moreira.FileStorage.Contracts
{
    /// <summary>
    /// Represents a client for a storage service (Azure blob, S3 or local file system)
    /// </summary>
    public interface IFileServiceClient
    {
        /// <summary>
        /// Send the file (sourceFilePath) to the desired destination on the storage.
        /// </summary>
        /// <param name="sourceFilePath">Full path to the file to be sent</param>
        /// <param name="destination">Where the file will be saved</param>
        /// <returns></returns>
        Task<HttpUploadResponse> UploadToAsync(string sourceFilePath, FilePath destination);

        /// <summary>
        /// Sends the memorystream to the desired destination on the storage.
        /// </summary>
        /// <param name="sourceStream">File memory stream</param>
        /// <param name="destination">Where the file will be saved</param>
        /// <returns></returns>
        Task<HttpUploadResponse> UploadToAsync(MemoryStream sourceStream, FilePath destination);

        /// <summary>
        /// Sends the file stream to the desired destination on the storage.
        /// </summary>
        /// <param name="sourceStream">File stream</param>
        /// <param name="destination">Where the file will be saved</param>
        /// <returns></returns>
        Task<HttpUploadResponse> UploadToAsync(Stream sourceStream, FilePath destination);

        /// <summary>
        /// Sends the array of bytes to the desired destination on the storage.
        /// </summary>
        /// <param name="sourceBytes">Array of bytes</param>
        /// <param name="destination">Where the file will be saved</param>
        /// <returns></returns>
        Task<HttpUploadResponse> UploadToAsync(byte[] sourceBytes, FilePath destination);

        /// <summary>
        /// Copies source to destination and waits until the copy has been completed if waitForCompletion is true, 
        /// otherwise it returns after starting the copy, but without waiting to complete the copy.
        /// </summary>
        /// <param name="source">File to be copied</param>
        /// <param name="destination">Where the file will be saved</param>
        /// <param name="waitForCompletion">If true, wait for the complete copy to finish</param>
        /// <param name="cancellationToken">Token to cancel asynchronous copy</param>
        /// <returns></returns>
        Task<IFileResponse> CopyToAsync(FilePath source, FilePath destination, bool waitForCompletion = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file and returns the file data string.
        /// </summary>
        /// <param name="sourceToDownload">File path to download</param>
        /// <returns></returns>
        Task<string> DownloadFileStringAsync(FilePath sourceToDownload);

        /// <summary>
        /// Downloads a file and returns the file reading stream.
        /// Use the returned stream within a block using to make it correctly disposed.
        /// </summary>
        /// <param name="sourceToDownload">File path to download</param>
        /// <returns></returns>
        Task<Stream> DownloadFileStreamAsync(FilePath sourceToDownload);

        /// <summary>
        /// Downloads a file and returns the streaminfo to read the file.
        /// Use the returned stream within a block using to make it correctly disposed.
        /// </summary>
        /// <param name="sourceToDownload">File path to download</param>
        /// <returns></returns>
        Task<StreamInfo> DownloadFileStreamInfoAsync(FilePath sourceToDownload);

        /// <summary>
        /// Checks whether the file exists in Storage and returns true if yes
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(FilePath path);

        /// <summary>
        /// Deleta um arquivo no Storage e retorna true em caso de sim
        /// </summary>
        /// <param name="path">Delete a file in Storage and return true if yes</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(FilePath path);

        /// <summary>
        /// Deletes a folder (Bucket on s3, blob containers on Azure, or Directory on the local storage system)
        /// </summary>
        /// <param name="folderName">Folder name</param>
        /// <returns></returns>
        Task<bool> DeleteFolderAsync(string folderName);

        /// <summary>
        /// Checks if there is a connection with Storage and returns true if yes.
        /// </summary>
        /// <param name="folderName">Folder name in Storage</param>
        /// <returns></returns>
        Task<bool> CheckConnectionAsync(string folderName);

        /// <summary>
        /// Creates a link / url for public access to the file.
        /// </summary>
        /// <param name="destination">File for which you want to create the public access link (read-only)</param>
        /// <returns></returns>
        Task<string> CreateSharedUrl(FilePath destination);

        /// <summary>
        /// Returns the base uri of the file path storage.
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        Task<Uri> GetUri(FilePath destination);
    }
}