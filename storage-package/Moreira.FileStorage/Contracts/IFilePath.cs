namespace Moreira.FileStorage.Contracts
{
    /// <summary>
    /// Represents an abstraction for a file path, whether in s3, azure blob or local file system.
    /// </summary>
    public interface IFilePath
    {
        /// <summary>
        /// Name of the root directory.
        /// In S3 it is the name of the bucket.
        /// In Azure Blob it is the name of the container.
        /// In a common file system it is the name of the directory.
        /// </summary>
        /// <remarks>
        /// ATTENTION: azure does not accept names with uppercase letters, generating exception of invalid character.
        /// </remarks>
        string Folder { get; set; }

        /// <summary>
        /// Path relative to the folder for the file.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Delimiter used to identify the separation between directories.
        /// </summary>
        string PathDelimiter { get; }

        /// <summary>
        /// Mime Type of the file compliant with the IANA definition.
        /// </summary>
        /// <seealso cref="http://www.iana.org/assignments/media-types/media-types.xhtml"/>
        /// <seealso cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Basico_sobre_HTTP/MIME_types"/>
        string ContentType { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// Validates that ContentType and Extension are valid values.
        /// </summary>
        /// <returns></returns>
        bool ContentTypeAndExtensionIsValid();
    }
}
