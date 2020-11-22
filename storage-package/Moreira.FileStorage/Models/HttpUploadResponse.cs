using System;
using System.Net;

namespace Moreira.FileStorage.Models
{
    public class HttpUploadResponse : HttpFileResponse
    {
        public HttpUploadResponse(HttpStatusCode httpStatusCode, Uri uri) : base(httpStatusCode)
        {
            Uri = uri;
        }

        /// <summary>
        /// URI of the file persisted in storage.
        /// </summary>
        public Uri Uri { get; set; }
    }
}