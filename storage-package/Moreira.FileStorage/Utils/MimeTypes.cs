using Moreira.FileStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moreira.FileStorage.Utils
{
    public static class MimeTypes
    {
        public static List<MimeType> ContentTypes { get; }

        /// <summary>
        /// Registration of the main mime types(needs to be updated if any are needed that are not here)
        /// </summary>
        /// <seealso cref="https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types"/>
        /// <seealso cref="https://www.becsv.com/table-csv.php"/>
        static MimeTypes()
        {
            ContentTypes = new List<MimeType>();

            #region Images
            ContentTypes.Add(new MimeType("image/bmp", "Windows OS/ 2 Bitmap Graphics", new string[] { ".bmp" }, true));
            ContentTypes.Add(new MimeType("image/gif", "Graphics Interchange Format(GIF)", new string[] { ".gif" }, true));
            ContentTypes.Add(new MimeType("image/jpeg", "JPEG images", new string[] { ".jpeg", ".jpg" }, true));
            ContentTypes.Add(new MimeType("image/png", "Portable Network Graphics", new string[] { ".png" }, true));
            ContentTypes.Add(new MimeType("image/tiff", "Tagged Image File Format(TIFF)", new string[] { ".tif", ".tiff" }));
            ContentTypes.Add(new MimeType("image/vnd.microsoft.icon", "Icon format", new string[] { ".ico"}));           
            ContentTypes.Add(new MimeType("image/svg+xml", "Scalable Vector Graphics(SVG)", new string[] { ".svg" }));            
            ContentTypes.Add(new MimeType("image/webp", "WEBP image", new string[] { ".webp" }));
            #endregion

            #region Documents
            ContentTypes.Add(new MimeType("application/x-httpd-php", "Hypertext Preprocessor (Personal Home Page)", new string[] { ".php" }));
            ContentTypes.Add(new MimeType("application/pdf", "Adobe Portable Document Format (PDF)", new string[] { ".pdf" }, true));
            ContentTypes.Add(new MimeType("application/ogg", "OGG", new string[] { ".ogx" }));
            ContentTypes.Add(new MimeType("application/vnd.oasis.opendocument.text", "OpenDocument text document", new string[] { ".odt" }, true));
            ContentTypes.Add(new MimeType("application/vnd.oasis.opendocument.spreadsheet", "OpenDocument spreadsheet document", new string[] { ".ods" }, true));
            ContentTypes.Add(new MimeType("application/vnd.oasis.opendocument.presentation", "OpenDocument presentation document", new string[] { ".odp" }, true));
            ContentTypes.Add(new MimeType("application/vnd.apple.installer+xml", "Apple Installer Package", new string[] { ".mpkg" }));
            ContentTypes.Add(new MimeType("text/javascript", "JavaScript module", new string[] { ".mjs" }));
            ContentTypes.Add(new MimeType("application/ld+json", "JSON-LD format", new string[] { ".jsonld" }));
            ContentTypes.Add(new MimeType("application/json", "JSON format", new string[] { ".json" }, true));
            ContentTypes.Add(new MimeType("text/javascript", "JavaScript", new string[] { ".js" }));
            ContentTypes.Add(new MimeType("application/java-archive", "Java Archive (JAR)", new string[] { ".jar" }));
            ContentTypes.Add(new MimeType("text/calendar", "iCalendar format", new string[] { ".ics" }));
            ContentTypes.Add(new MimeType("text/html", "HyperText Markup Language (HTML)", new string[] { ".html",".htm" }));
            ContentTypes.Add(new MimeType("application/gzip", "GZip Compressed Archive", new string[] { ".gz" }, true));
            ContentTypes.Add(new MimeType("application/epub+zip", "Electronic publication (EPUB)", new string[] { ".epub" }));
            ContentTypes.Add(new MimeType("application/vnd.ms-fontobject", "MS Embedded OpenType fonts", new string[] { ".eot" }));
            ContentTypes.Add(new MimeType("application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Microsoft Word (OpenXML)", new string[] { ".docx" }, true));
            ContentTypes.Add(new MimeType("application/msword", "Microsoft Word", new string[] { ".doc" }, true));
            ContentTypes.Add(new MimeType("text/csv", "Comma-separated values (CSV)", new string[] { ".csv" }, true));
            ContentTypes.Add(new MimeType("text/css", "Cascading Style Sheets (CSS)", new string[] { ".css" }));
            ContentTypes.Add(new MimeType("application/x-csh", "C-Shell script", new string[] { ".csh" }));
            ContentTypes.Add(new MimeType("application/x-bzip2", "BZip2 archive", new string[] { ".bz2" }));
            ContentTypes.Add(new MimeType("application/x-bzip", "BZip archive", new string[] { ".bz" }));
            ContentTypes.Add(new MimeType("application/octet-stream", "Any kind of binary data", new string[] { ".bin" }));
            ContentTypes.Add(new MimeType("application/vnd.amazon.ebook", "Amazon Kindle eBook format", new string[] { ".azw" }));
            ContentTypes.Add(new MimeType("application/x-freearc", "Archive document (multiple files embedded)", new string[] { ".arc" }));
            ContentTypes.Add(new MimeType("application/x-abiword", "AbiWord document", new string[] { ".abw" }));
            ContentTypes.Add(new MimeType("application/x-7z-compressed", "7-zip archive", new string[] { ".7z" }, true));
            ContentTypes.Add(new MimeType("application/zip", "ZIP archive", new string[] { ".zip" }, true));
            ContentTypes.Add(new MimeType("application/vnd.mozilla.xul+xml", "XUL", new string[] { ".xul" }));
            
            ContentTypes.Add(new MimeType("application/xml", "XML if not readable from casual users (RFC 3023, section 3) ", new string[] { ".xml" }));
            ContentTypes.Add(new MimeType("text/xml", "XML readable from casual users (RFC 3023, section 3)", new string[] { ".xml" }));

            ContentTypes.Add(new MimeType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Microsoft Excel (OpenXML)", new string[] { ".xlsx" }));
            ContentTypes.Add(new MimeType("application/vnd.ms-excel", "Microsoft Excel", new string[] { ".xls" }, true));
            ContentTypes.Add(new MimeType("application/xhtml+xml", "XHTML", new string[] { ".xhtml" }));
            ContentTypes.Add(new MimeType("application/vnd.visio", "Microsoft Visio", new string[] { ".vsd" }, true));
            ContentTypes.Add(new MimeType("text/plain", "Text, (generally ASCII or ISO 8859-n)", new string[] { ".txt" }, true));
            ContentTypes.Add(new MimeType("application/x-tar", "Tape Archive (TAR)", new string[] { ".tar" }));
            ContentTypes.Add(new MimeType("application/x-shockwave-flash", "Small web format (SWF) or Adobe Flash document", new string[] { ".swf" }));
            ContentTypes.Add(new MimeType("application/x-sh", "Bourne shell script", new string[] { ".sh" }));
            ContentTypes.Add(new MimeType("application/rtf", "Rich Text Format (RTF)", new string[] { ".rtf" }, true));
            ContentTypes.Add(new MimeType("application/vnd.rar", "RAR archive", new string[] { ".rar" }, true));
            ContentTypes.Add(new MimeType("application/vnd.openxmlformats-officedocument.presentationml.presentation", "Microsoft PowerPoint (OpenXML)", new string[] { ".pptx" }, true));
            ContentTypes.Add(new MimeType("application/vnd.ms-powerpoint", "Microsoft PowerPoint", new string[] { ".ppt" }, true));
            #endregion

            #region Audios / Videos
            ContentTypes.Add(new MimeType("audio/aac", "AAC audio", new string[] { ".aac" }));            
            ContentTypes.Add(new MimeType("audio/midi", "Musical Instrument Digital Interface (MIDI)", new string[] { ".mid", ".midi" }));
            ContentTypes.Add(new MimeType("audio/x-midi", "Musical Instrument Digital Interface (MIDI)", new string[] { ".mid", ".midi" }));
            ContentTypes.Add(new MimeType("audio/mpeg", "MP3 audio", new string[] { ".mp3" }, true));
            ContentTypes.Add(new MimeType("audio/ogg", "OGG audio", new string[] { ".oga" }));
            ContentTypes.Add(new MimeType("audio/opus", "Opus audio", new string[] { "opus" }));
            ContentTypes.Add(new MimeType("audio/wav", "Waveform Audio Format", new string[] { ".wav" }, true));
            ContentTypes.Add(new MimeType("audio/webm", "WEBM audio", new string[] { ".weba" }));

            ContentTypes.Add(new MimeType("video/mpeg", "MPEG Video", new string[] { ".mpeg" }, true));
            ContentTypes.Add(new MimeType("video/ogg", "OGG video", new string[] { ".ogv" }));
            ContentTypes.Add(new MimeType("video/mp2t", "MPEG transport stream", new string[] { ".ts" }));
            ContentTypes.Add(new MimeType("video/webm", "WEBM video", new string[] { ".webm" }));
            ContentTypes.Add(new MimeType("video/x-msvideo", "AVI: Audio Video Interleave", new string[] { ".avi" }));
            
            //TODO: ver forma melhor de retornar corretamente caso busque por extensão.
            ContentTypes.Add(new MimeType("audio/3gpp", "3GPP audio container if it doesn't contain video", new string[] { ".3gp" }));
            ContentTypes.Add(new MimeType("video/3gpp", "3GPP audio/video container", new string[] { ".3gp" }));
            ContentTypes.Add(new MimeType("video/3gpp2", "3GPP2 video container", new string[] { ".3g2" }));
            ContentTypes.Add(new MimeType("audio/3gpp2", "3GPP2 audio container if it doesn't contain video", new string[] { ".3g2" }));
            #endregion
        }

        public static bool Exists(string extension, string contentType)
        {
            var mime = GetMimeType(extension, contentType);

            if (mime.ContentType == null)
                return false;

            return true;
        }

        public static MimeType GetMimeType(string extension, string contentType)
        {
            return ContentTypes.FirstOrDefault(x => x.Extensions.Contains(extension) && x.ContentType == contentType);
        }

        public static List<MimeType> GetMimeTypeOfSupportedFiles()
        {
            return ContentTypes.Where(x => x.ContentType.Contains("image") == false && x.IsSupported).ToList();
        }

        public static List<MimeType> GetMimeTypeOfSupportedImages()
        {
            return ContentTypes.Where(x => x.ContentType.Contains("image") && x.IsSupported).ToList();
        }

        public static List<string> GetExtensionsOfSupportedImages()
        {
            return GetMimeTypeOfSupportedImages().SelectMany(x => x.Extensions).ToList();
        }

        public static List<string> GetExtensionsOfSupportedFiles()
        {
            return GetMimeTypeOfSupportedFiles().SelectMany(x => x.Extensions).ToList();
        }

        public static MimeType MimeTypeForExtension(string extension)
        {
            return ContentTypes.FirstOrDefault(x => x.Extensions.Contains(extension));
        }

        public static MimeType MimeTypeForContentType(string contentType)
        {
            return ContentTypes.FirstOrDefault(x => x.ContentType == contentType);
        }
    }
}
