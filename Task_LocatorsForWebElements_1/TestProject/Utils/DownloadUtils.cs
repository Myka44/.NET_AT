using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLayer.Utils
{
    public static class DownloadUtils
    {
        public static string CreateDirectory(string baseDownloadDirectory)
        {
            string downloadDirectory = Path.GetFullPath(Path.Combine(baseDownloadDirectory, Guid.NewGuid().ToString()));
            Directory.CreateDirectory(downloadDirectory);

            return downloadDirectory;
        }

        public static bool WaitForFileToBeDownloaded(string filePath, TimeSpan timeout)
        {
            DateTime deadline = DateTime.UtcNow.Add(timeout);
            while (DateTime.UtcNow < deadline)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }

                Thread.Sleep(500);
            }
            return false;
        }
    }
}
