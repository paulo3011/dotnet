using System;
using System.Collections.Generic;
using System.IO;

namespace Moreira.FileStorage.Models
{
    public class StreamInfo : IDisposable
    {
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public IDictionary<string, string> Metadata { get; set; }

        public void Dispose()
        {
            if (Stream != null)
                Stream.Dispose();
        }
    }
}