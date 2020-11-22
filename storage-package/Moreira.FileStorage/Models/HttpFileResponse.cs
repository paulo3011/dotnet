using Moreira.FileStorage.Contracts;
using System.Net;

namespace Moreira.FileStorage.Models
{
    public class HttpFileResponse : IFileResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public HttpFileResponse() { }
        public HttpFileResponse(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}