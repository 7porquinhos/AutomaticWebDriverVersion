using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticWebDriver.WebScrapping
{
    public static class UserAgentCrawler
    {
        private static string? UserAgentText { get; set; }

        public static string UserAgent()
        {
            // From Web
            var url = "https://www.whatismybrowser.com/pt/detect/what-is-my-user-agent/";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            foreach (var item in doc.DocumentNode.SelectNodes("//*[@id=\"detected_value\"]"))
            {
                UserAgentText  = item.InnerText;
            }
            return UserAgentText;
        }
    }
}
