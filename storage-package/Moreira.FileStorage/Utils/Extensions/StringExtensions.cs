using System;
using System.IO;

namespace Moreira.FileStorage.Utils.Extensions
{
    /// <summary>
    /// Storage-related string extensions only
    /// </summary>
    public static class StringExtensions
    {
        public static Stream GetStreamFromBase64(this string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return null;

            return new MemoryStream(Convert.FromBase64String(base64));
        }
    }
}