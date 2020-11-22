using Moreira.FileStorage.Contracts;
using Moreira.FileStorage.Utils;
using System.Linq;

namespace Moreira.FileStorage.Models
{
    public class FilePath : IFilePath
    {
        public virtual string Folder { get; set; }
        public virtual string Path { get; set; }

        /// <summary>
        /// Mime Type of the file compliant with the IANA definition.
        /// </summary>
        /// <seealso cref="http://www.iana.org/assignments/media-types/media-types.xhtml"/>
        /// <seealso cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Basico_sobre_HTTP/MIME_types"/>
        public virtual string ContentType { get; set; }

        /// <summary>
        /// File extension
        /// </summary>
        public virtual string Extension { get; set; }

        /// <summary>
        /// By default it uses a local path delimiter.
        /// The path is in the same pattern used by the Azure Store, so it makes downloading easier.
        /// </summary>
        /// <remarks>
        /// Use Path.Combine if there are any more problems with the downloads.
        /// </remarks>
        public virtual string PathDelimiter { get; protected set; } = "/";

        public FilePath() { }

        public FilePath(string path, string folder)
        {
            Path = path;
            Folder = folder;
        }

        public FilePath(string path, string folder, string delimiter)
        {
            PathDelimiter = delimiter;
            Path = path;
            Folder = folder;
        }

        /// <summary>
        /// Checks if it is a valid path and returns true if yes.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Folder))
                return false;

            if (string.IsNullOrWhiteSpace(Path))
                return false;

            return true;
        }

        public void DetectMimeType()
        {
            MimeType mimetype = GetMimeType();

            if (mimetype.ContentType == null)
            {
                //invalid or unknown extension or mimetype reported.
                return;
            }

            Extension = mimetype.Extensions.First();
            ContentType = mimetype.ContentType;
        }

        private MimeType GetMimeType()
        {
            MimeType mimetype = default(MimeType);

            if (string.IsNullOrWhiteSpace(Extension) == false)
            {
                mimetype = MimeTypes.MimeTypeForExtension(Extension);
            }

            if (mimetype.ContentType == null && string.IsNullOrWhiteSpace(ContentType) == false)
            {
                mimetype = MimeTypes.MimeTypeForContentType(ContentType);
            }

            return mimetype;
        }

        public bool ContentTypeAndExtensionIsValid()
        {
            return MimeTypes.Exists(Extension,ContentType);
        }

        /// <summary>
        /// Creates a file path by detecting the mime type by the file extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static FilePath FromExtension(string extension)
        {
            var filePah = new FilePath() { Extension = extension };
            filePah.DetectMimeType();
            return filePah;
        }

        /// <summary>
        /// Returns the storage path for the file in the format: {Folder}{PathDelimiter}{Path}.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Folder}{PathDelimiter}{Path}";
        }
    }
}
