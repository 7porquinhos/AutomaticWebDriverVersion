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
        public static string GetVersion()
        {
            try
            {
                object path = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe", "", null);
                string chromeVersion = "";
                if (path != null)
                    chromeVersion = FileVersionInfo.GetVersionInfo(path.ToString()).FileVersion;
                return chromeVersion;
            }
            catch (Exception ex)
            {
                //Versão do chrome não encontrada, verifique se encontrasse instalado na maquina.
                throw new Exception("Chrome version not found, check if it was installed on the machine.",ex);
            }
        }

        public static void Download(string DestinationPath)
        {
            RequisicaoWebClient client = new RequisicaoWebClient();
            HtmlDocument document = new HtmlDocument();
            NameValueCollection parametrosCol = new NameValueCollection();

            var response = client.CarregarDados("https://chromedriver.storage.googleapis.com/103.0.5060.24/chromedriver_win32.zip");
            //byte[] bytes = Encoding.UTF8.GetBytes(response);
            if (response != null)
                File.WriteAllBytes($"{DestinationPath}/chromedriver_win32.zip", response);
            
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
