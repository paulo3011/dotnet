using Moreira.FileStorage.Contracts;
using Moreira.FileStorage.Utils;
using Moreira.FileStorage.Utils.Extensions;
using System.IO;

namespace Moreira.FileStorage.Models
{
    /// <summary>
    /// Generic class for any type of application file.
    /// Attention: do not implement validations and features that are restricted to some type
    /// specific file or application rule, do this at the specific instance to avoid problems
    /// compatibility and validation rules.
    /// </summary>
    public abstract class ApplicationFile : IApplicationFile
    {
        /// <summary>
        /// Path to the original file sent by the user.
        /// (without any processing or editing)
        /// </summary>
        public virtual FilePath RawFilePath { get; set; }

        /// <summary>
        /// Public URL for the file.
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// Stream with the file data.
        /// </summary>
        public virtual Stream FileStream { get; set; }

        protected ApplicationFile() { }

        protected ApplicationFile(string base64, string extension)
        {
            FileStream = base64.GetStreamFromBase64();
            RawFilePath = FilePath.FromExtension(extension);
        }

        protected ApplicationFile(Stream fileStream, FilePath rawFilePath)
        {
            FileStream = fileStream;
            RawFilePath = rawFilePath;
        }

        /// <summary>
        /// Returns true if filling the file (ready to be saved / persisted in the application's storage or has already been persisted and already has the path to the file).
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFilled()
        {
            if (RawFilePath != null && RawFilePath.IsValid())
                return true;

            return false;
        }

        /// <summary>
        /// Returns true if filling the file (sending to be saved / persisted in the application's storage).
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFilling()
        {
            return FileStream != null && FileStream.Length > 0;
        }

        /// <summary>
        /// Returns true if it is a filled file (if FilePath has already been filled or if it is filling / preparing to persist in the application's storage).
        /// Does not validate format or any other rule just checks if it is filled out or if it is filling in / sending the file to be saved).
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFilledOrFilling()
        {
            if (IsFilled())
                return true;
            if (IsFilling())
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Overwrite and validate in the concrete class
        /// </remarks>
        public virtual bool ContentIsValid()
        {
            return true;
        }

        /// <summary>
        /// Returns true if the content type of the file / extension is valid for the application and false otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool ContentTypeIsValid()
        {
            var mime = MimeTypes.GetMimeType(RawFilePath.Extension, RawFilePath.ContentType);

            if (mime.ContentType == null || mime.IsSupported == false)
                return false;

            return true;
        }

        /// <summary>
        /// Returns true if the image type is valid and false otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidImage()
        {
            var mime = MimeTypes.GetMimeType(RawFilePath.Extension, RawFilePath.ContentType);

            if (mime.ContentType == null || mime.IsSupported == false)
                return false;

            if (mime.ContentType.Contains("image") == false)
                return false;

            return true;
        }
    }
}