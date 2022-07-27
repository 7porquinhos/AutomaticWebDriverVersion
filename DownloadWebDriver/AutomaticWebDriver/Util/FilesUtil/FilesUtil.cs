using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticWebDriver.Util.FilesUtil
{
    public static class FilesUtil
    {
        public static bool ExtractFileZip(string OriginPathZip, string DestinationPath)
        {
            try
            {
                using (ZipArchive zip = ZipFile.Open(OriginPathZip, ZipArchiveMode.Read))
                {
                    zip.ExtractToDirectory(DestinationPath);
                }

                bool fileRemove = RemoveFile(DestinationPath);

                return File.Exists($"{DestinationPath}/chromedriver.exe") && fileRemove;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to extract the file", ex);
            }
        }

        private static bool RemoveFile(string OriginPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(OriginPath);
                foreach (FileInfo f in dir.GetFiles("*.zip"))
                {
                    File.Delete(f.FullName);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to remove file.", ex);
            }
        }
    }
}
