using AutomaticWebDriver.WebScrapping;
using HtmlAgilityPack;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to download chrome drive.", ex);
            }
        }
        private static void Update()
        {
            try
            {
                object path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
                String chromeVersion = "",
                    from = "",
                    to = Directory.GetCurrentDirectory() + "\\chromedriver.exe",
                    driversPath = "PathChromeDrive",
                    driverFolder = "";

                if (path != null)
                    chromeVersion = FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion.Substring(0, 2);
                else
                    return;

                var dir = Directory.GetDirectories(driversPath);
                foreach (var item in dir)
                {
                    int inicialVersion = Convert.ToInt32(item.Replace(driversPath, "").Substring(2, 2));
                    int lastVersion = Convert.ToInt32(item.Replace(driversPath, "").Substring(5, 2));

                    if (inicialVersion <= Convert.ToInt32(chromeVersion) && lastVersion >= Convert.ToInt32(chromeVersion))
                    {
                        driverFolder = item;
                    }
                }
                if (driverFolder == "")
                    return;

                from = driverFolder + "\\chromedriver.exe";

                File.Copy(from, to, true);
            }
            catch (Exception ex)
            {

            }
        }
    }
}