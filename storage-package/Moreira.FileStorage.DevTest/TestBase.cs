using System;
using System.IO;

namespace Moreira.FileStorage.DevTest
{
    public abstract class TestBase
    {
        /// <summary>
        /// Creates a new file and saves it in a temporary dir.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        protected string CreateNewTxtFile(string content = "Hello, World!", string filename = "temp_file_", bool uniqueName = true)
        {
            string localPath = Path.GetTempPath();
            string fileName = $"{filename}" + Guid.NewGuid().ToString() + ".txt";

            if (uniqueName == false)
                fileName = filename;

            string localFilePath = Path.Combine(localPath, fileName);

            // Write text to the file
            File.WriteAllText(localFilePath, content);

            return localFilePath;
        }
    }
}
