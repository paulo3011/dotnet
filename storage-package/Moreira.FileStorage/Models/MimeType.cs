namespace Moreira.FileStorage.Models
{
    /// <summary>
    /// Represent a Mime Type
    /// </summary>
    public struct MimeType
    {
        public string ContentType { get; }
        public string[] Extensions { get; }
        public string Description { get; }
        /// <summary>
        /// If false, file type is not supported by the system.
        /// </summary>
        public bool IsSupported { get; }

        public MimeType(string contentType, string description, string[] extensions, bool isSupported = false)
        {
            ContentType = contentType;
            Description = description;
            Extensions = extensions;
            IsSupported = isSupported;
        }
    }
}