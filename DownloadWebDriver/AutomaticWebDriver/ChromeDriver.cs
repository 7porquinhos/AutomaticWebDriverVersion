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
        public static string GetVersion()
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
        /// Download specific chrome drive for using local browser with selenium.
        /// </summary>
        /// <param name="DestinationPath">Caminho onde ira salvar o chromedrive.exe.</param>
        /// <returns>True download completed, false.</returns>
        /// <exception cref="Exception"></exception>
        public static bool Download(string DestinationPath)
        {
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
            versionChrome = GetVersion().Substring(0, 3);
            URLHtml = "https://chromedriver.chromium.org/downloads";
            URLDownload = "https://chromedriver.storage.googleapis.com";
            #endregion

            if (!versionChrome.Any())
                throw new Exception("Unidentified chrome drive version.");

            if (File.Exists(DestinationPath))
                throw new Exception("Invalid directory.");

            if (File.Exists($"{DestinationPath}/chromedriver.exe"))
                throw new Exception("chromedriver.exe already exists in the target directory.");

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
    }
}