using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticWebDriver.WebScrapping
{
    public class WebClientBase : WebClient
    {
        public WebClientBase(CookieContainer container)
        {
            this.container = container;

        }
        public WebClientBase()
        {
        }

        public string UltimaUrl { get; set; }

        public CookieContainer CookieContainer
        {
            get { return container; }
            set { container = value; }
        }

        private CookieContainer container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            var request = r as HttpWebRequest;

            if (request != null)
                request.CookieContainer = container;

            r.Timeout = 20 * 60 * 1000;
            return r;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;

            if (response != null)
            {
                this.UltimaUrl = response.ResponseUri.AbsoluteUri;
                CookieCollection cookies = response.Cookies;
                container.Add(cookies);
            }
        }
    }
}
