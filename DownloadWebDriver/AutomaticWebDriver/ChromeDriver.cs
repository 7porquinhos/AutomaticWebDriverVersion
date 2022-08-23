using AutomaticWebDriver.Util.FilesUtil;
using AutomaticWebDriver.WebScrapping;
using HtmlAgilityPack;
using Microsoft.Win32;
using System.Collections.Specialized;
using System.Diagnostics;

namespace AutomaticWebDriver
{
    public static class ChromeDriver
    {
        /// <summary>
        /// Check the version of google chrome installed on the machine.
        /// </summary>
        /// <param></param>
        /// <returns>String containing the google chrome browser version.</returns>
        /// <exception cref="Exception"></exception>
        public static string Version()
        {
            return GetVersionGoogleChrome();
        }
        private static string GetVersionGoogleChrome()
        {
            #region Variables
            object path;
            string chromeVersion = "";
            #endregion

            try
            {
                path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
                if (path is null)
                    throw new Exception("Invalid directory, HKEY_LOCAL_MACHINE.");

                chromeVersion = FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion;
                if (chromeVersion is null)
                    throw new Exception("Could not identify google chrome browser version.");

                return chromeVersion;
            }
            catch (Exception ex)
            {
                throw new Exception("Chrome version not found, check if it was installed on the machine.", ex);
            }
        }

        /// <summary>
        /// Download, specific chrome drive for using local browser with selenium.
        /// </summary>
        /// <param name="DestinationPath">Path where to save chromedrive.exe</param>
        /// <returns>True, if download completed, false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool Download(string DestinationPath)
        {
            return DownloadChromeDrive(DestinationPath);
        }
        private static bool DownloadChromeDrive(string DestinationPath)
        {
            if (DestinationPath is null)
                throw new Exception("Parameter Destination path cannot be null.", new ArgumentNullException());
            if (!Directory.Exists(DestinationPath))
                throw new Exception("Directory does not exist.", new DirectoryNotFoundException());

            #region Variables
            string VersionToDownload = "";
            string HTML = "";
            string versionChrome = "";
            string URLHtml = "";
            string URLDownload = "";

            RequisicaoWebClient client = new RequisicaoWebClient();
            HtmlDocument document = new HtmlDocument();
            NameValueCollection parametrosCol = new NameValueCollection();
            #endregion

            #region Assigning values
            versionChrome = GetVersionGoogleChrome().Substring(0, 3);
            URLHtml = "https://chromedriver.chromium.org/downloads";
            URLDownload = "https://chromedriver.storage.googleapis.com";
            #endregion

            if (!versionChrome.Any())
                throw new Exception("Unidentified chrome drive version.");

            if (File.Exists($"{DestinationPath}/chromedriver.exe"))
            {
                //throw new Exception("chromedriver.exe already exists in the target directory.");
                return false;
            }

            try
            {
                HTML = client.CarregarHtml(URLHtml);

                document.LoadHtml(HTML);
                var htmlNodes = document.DocumentNode.SelectNodes("//ul/li/p/span/a");
                foreach (var nodeA in htmlNodes)
                {
                    if (nodeA.InnerText.Contains(versionChrome))
                    {
                        VersionToDownload = nodeA.InnerText.Replace("ChromeDriver ", "");
                        break;
                    }
                }

                var response = client.CarregarDados($"{URLDownload}/{VersionToDownload}/chromedriver_win32.zip");

                if (response is not null)
                    File.WriteAllBytes($"{DestinationPath}/chromedriver_win32.zip", response);

                if (File.Exists($"{DestinationPath}/chromedriver_win32.zip"))
                    FilesUtil.ExtractFileZip($"{DestinationPath}/chromedriver_win32.zip", DestinationPath);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to download chrome drive.", ex);
            }
        }

        /// <summary>
        /// Exists, check if there is already a chrome drive on the destination.
        /// </summary>
        /// <param name="OriginPath">Path where to check if chromedrive.exe already exists</param>
        /// <returns>True, if chromedrive.exe already exists, false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool Exists(string OriginPath)
        {
            if (OriginPath is null)
                throw new Exception("Parameter Destination path cannot be null.", new ArgumentNullException());
            if (!Directory.Exists(OriginPath))
                throw new Exception("Directory does not exist.", new DirectoryNotFoundException());

            return File.Exists($"{OriginPath}\\chromedriver.exe");
        }

        /// <summary>
        /// Overwrite, check if there is already a chrome drive on the destination and Overwrite.
        /// </summary>
        /// <param name="OriginPath">Path where to check if chromedrive.exe already exists</param>
        /// <returns>True, if chromedrive.exe already exists, false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool Overwrite(string OriginPath)
        {
            return OverwriteChromeDrive(OriginPath);
        }
        private static bool OverwriteChromeDrive(string OriginPath)
        {
            if (Exists(OriginPath))
            {
                File.Delete($"{OriginPath}chromedriver.exe");
                return DownloadChromeDrive(OriginPath);
            }
            else
                return false;
        }
    }
}