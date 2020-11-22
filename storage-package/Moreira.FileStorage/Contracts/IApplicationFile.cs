using Moreira.FileStorage.Models;
using System.IO;

namespace Moreira.FileStorage.Contracts
{
    /// <summary>
    /// Represents an application file.
    /// </summary>
    public interface IApplicationFile
    {
        /// <summary>
        /// Path to the file in the application's storage.
        /// </summary>
        FilePath RawFilePath { get; set; }

        /// <summary>
        /// Public URL for the file if it can be accessed publicly.
        /// URL for private files is generated dynamically and with expiration.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Stream data from the file to persist in storage.
        /// </summary>
        Stream FileStream { get; set; }
    }
}
