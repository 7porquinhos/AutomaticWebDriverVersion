using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.IO;
using AutomaticWebDriver.WebScrapping;

namespace AutomaticWebDriver.WebScrapping
{
    public class RequisicaoWebClient
    {
        public WebClientBase WebClient { get; set; }
        private Encoding _DefaultEncoding = null;
        public Encoding DefaultEncoding
        {
            get
            {
                if (_DefaultEncoding == null)
                    _DefaultEncoding = Encoding.Default;
                return _DefaultEncoding;
            }
            set
            {
                _DefaultEncoding = value;
                if (WebClient != null) WebClient.Encoding = value;
                PtBr.Encoding = value;
            }
        }

        public RequisicaoWebClient()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            //SecurityProtocolType.Tls12
            WebClient = new WebClientBase();
            DefaultEncoding = Encoding.Default;

            IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
            defaultWebProxy.Credentials = CredentialCache.DefaultCredentials;

            //WebProxy proxy = new WebProxy();
            //proxy.Credentials = CredentialCache.DefaultCredentials;
            WebClient.Proxy = defaultWebProxy;
        }
        public RequisicaoWebClient(CookieContainer container)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            WebClient = new WebClientBase(container);
            DefaultEncoding = Encoding.Default;
        }
        public void HttpRequestHeaderDefault()
        {
            //WebClient.Headers.Clear();
            //WebClient.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
            //WebClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            //WebClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,pt-BR;q=0.8,pt;q=0.7");
            WebClient.Headers.Add(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Add(HttpRequestHeader.Accept, "*/*");
        }
        public bool LogaProxy()
        {
            //WebClient = new WebClientBase();
            try
            {
                var str = CarregarHtml("https://www.google.com.tr/search?sclient=psy-ab&site=&source=hp&q=cars+&btnG=Ara");
                return true;
            }
            catch (WebException we)
            {
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }

        public string CarregarHtml(string url)
        {
            //WebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            return WebClient.DownloadString(url);
        }

        public string CarregarHtml(string url, string referer)
        {
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return CarregarHtml(url);
        }

        public string CarregarHtml(string url, NameValueCollection parametrosCol, string referer)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
                strParametros.AppendFormat("{0}={1}&", GetEncodeIso(Param), GetEncodeIso(parametrosCol[Param]));
            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarHtml(url, parametros, referer);
        }

        public string CarregarHtmlSoftware(string url, NameValueCollection parametrosCol, string referer, string accept, string userAgent)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
                strParametros.AppendFormat("{0}={1}&", GetEncodeIso(Param), GetEncodeIso(parametrosCol[Param]));
            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarHtmlSoftware2(url, parametros, referer, accept, userAgent);
        }

        public string CarregarHtmlSoftware2(string url, NameValueCollection parametrosCol, string referer, string accept, string userAgent)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
                strParametros.AppendFormat("{0}={1}&", GetEncodeIso(Param), GetEncodeIso(parametrosCol[Param]));
            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarHtmlSoftware3(url, parametros, referer, accept, userAgent);
        }

        public string CarregarHtmlSoftware3(string url, string referer, string accept, string userAgent)
        {
            return CarregarHtmlSoftware4(url, referer, accept, userAgent);
        }

        public string CarregarHtmlSoftware4(string url, string referer, string accept, string userAgent)
        {
            WebClient.Headers.Clear();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, userAgent);
            WebClient.Headers.Set(HttpRequestHeader.Accept, accept);
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            WebClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            WebClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            WebClient.Headers.Add("Upgrade-Insecure-Requests", "1");
            WebClient.Headers.Add("sec-ch-ua", @"""Google Chrome"";v=""89"", ""Chromium"";v=""89"", "";Not A Brand"";v=""99""");
            WebClient.Headers.Add("sec-ch-ua-mobile", @"?0");
            WebClient.Headers.Add("Sec-Fetch-Site", @"same-origin");
            WebClient.Headers.Add("Sec-Fetch-Mode", @"navigate");
            WebClient.Headers.Add("Sec-Fetch-Dest", @"iframe");

            return WebClient.UploadString(url, "GET");
        }

        public string CarregarHtmlSoftware3(string url, string parametros, string referer, string accept, string userAgent)
        {
            WebClient.Headers.Clear();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, userAgent);
            WebClient.Headers.Set(HttpRequestHeader.Accept, accept);
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            WebClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            WebClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            //WebClient.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            //WebClient.Headers.Add("Upgrade-Insecure-Requests", "1");
            //WebClient.Headers.Add("sec-ch-ua", @"""Google Chrome"";v=""89"", ""Chromium"";v=""89"", "";Not A Brand"";v=""99""");
            //WebClient.Headers.Add("sec-ch-ua-mobile", @"?0");
            //WebClient.Headers.Add("Sec-Fetch-Site", @"same-origin");
            //WebClient.Headers.Add("Sec-Fetch-Mode", @"navigate");
            //WebClient.Headers.Add("Sec-Fetch-Dest", @"iframe");
            //WebClient.Headers.Add("Sec-Fetch-User", @"?1");

            return WebClient.UploadString(url, "POST", parametros);
        }

        public string CarregarHtmlSoftware2(string url, string parametros, string referer, string accept, string userAgent)
        {
            WebClient.Headers.Clear();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, userAgent);
            WebClient.Headers.Set(HttpRequestHeader.Accept, accept);
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            WebClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            WebClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            WebClient.Headers.Add("Origin", "https://maximo.userede");
            WebClient.Headers.Add("Upgrade-Insecure-Requests", "1");
            WebClient.Headers.Add("sec-ch-ua", @"""Google Chrome"";v=""89"", ""Chromium"";v=""89"", "";Not A Brand"";v=""99""");
            WebClient.Headers.Add("sec-ch-ua-mobile", @"?0");
            WebClient.Headers.Add("Sec-Fetch-Site", @"same-origin");
            WebClient.Headers.Add("Sec-Fetch-Mode", @"navigate");
            WebClient.Headers.Add("Sec-Fetch-User", @"?1");
            WebClient.Headers.Add("Sec-Fetch-Dest", @"iframe");

            return WebClient.UploadString(url, "POST", parametros);
        }

        public string CarregarHtmlForcaCampo(string url, NameValueCollection parametrosCol, string referer)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;

            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
                strParametros.AppendFormat("{0}={1}&", GetEncodeIso(Param), GetEncodeIso(parametrosCol[Param]));

            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarHtmlForcaCampoMaximo(url, parametros, referer);
        }

        public string CarregarHtmlPost(string url, NameValueCollection parametrosCol, string referer)
        {
            HttpRequestHeaderDefault();
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            byte[] responsebytes = WebClient.UploadValues(url, "POST", parametrosCol);
            return Encoding.UTF8.GetString(responsebytes);
        }

        public string CarregarHtml(string url, List<KeyValuePair<string, string>> parametrosCol, string referer)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (var Param in parametrosCol)
                strParametros.AppendFormat("{0}={1}&", GetEncodeIso(Param.Key), GetEncodeIso(Param.Value));

            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarHtml(url, parametros, referer);
        }

        public string CarregarHtml(string url, string parametros, string referer)
        {
            WebClient.Headers.Clear();
            //HttpRequestHeaderDefault();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "application/json,application/x-www-form-urlencoded");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return WebClient.UploadString(url, "POST", parametros);
        }

        public string CarregarHtml(string url, string parametros, string referer, string contentType)
        {
            WebClient.Headers.Clear();
            //HttpRequestHeaderDefault();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Add(HttpRequestHeader.ContentType, contentType);
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return WebClient.UploadString(url, "POST", parametros);
        }

        public string CarregarHtmlForcaCampoMaximo(string url, string parametros, string referer)
        {
            WebClient.Headers.Clear();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            WebClient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            WebClient.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);

            return WebClient.UploadString(url, "POST", parametros);
        }

        public string CarregarHtmlXml(string url, string parametros, string referer)
        {
            WebClient.Headers.Clear();
            //HttpRequestHeaderDefault();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "text/xml");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return WebClient.UploadString(url, "POST", parametros);
        }

        public string CarregarHtmlBoundary(string url, NameValueCollection parametrosCol, string referer)
        {
            string boundary = "----WebKitFormBoundaryO318gEOHYnlfi28i";
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
            {
                strParametros.AppendLine(string.Format("--{0}", boundary));
                strParametros.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", Param)).AppendLine();
                strParametros.AppendLine(parametrosCol[Param]);
            }
            strParametros.AppendLine(string.Format("--{0}--", boundary));
            parametros = strParametros.ToString();
            return CarregarHtmlBoundary(url, parametros, referer, boundary);
        }
        public string CarregarHtmlBoundary(string url, NameValueCollection parametrosCol, string referer, Dictionary<string, string> dicAnexos)
        {

            MemoryStream postDataStream = new MemoryStream();
            StreamWriter postDataWriter = new StreamWriter(postDataStream);
            string boundary = "----WebKitFormBoundaryO318gEOHYnlfi28i";
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
            {
                strParametros.AppendLine(string.Format("--{0}", boundary));
                strParametros.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", Param)).AppendLine();
                strParametros.AppendLine(parametrosCol[Param]);
            }
            this.EscreverStream(ref postDataWriter, ref strParametros);
            if (dicAnexos != null && dicAnexos.Keys.Count > 0)
            {
                int index = 1;
                foreach (var item in dicAnexos)
                {
                    string fileName = Path.GetFileName(item.Value);
                    strParametros.AppendLine(string.Format("--{0}", boundary));
                    strParametros.AppendLine(string.Concat(string.Format("Content-Disposition: form-data; name=\"{0}\"; ", item.Key), "filename=\"" + fileName + "\""));
                    //strParametros.AppendLine("Content-Type: " + MimeMapping.GetMimeMapping(fileName));
                    strParametros.AppendLine();

                    this.EscreverStream(ref postDataWriter, ref strParametros);
                    this.LerArquivo(item.Value, ref postDataWriter, ref postDataStream);
                    this.EscreverStream(ref postDataWriter, ref strParametros);

                    var arr = item.Key.Split('$');

                    strParametros.AppendLine(string.Format("--{0}", boundary));
                    strParametros.AppendLine(string.Concat(string.Format("Content-Disposition: form-data; name=\"fileInput_{0}_{1}\"; ", index, arr[arr.Length - 1]), "filename=\"\""));
                    strParametros.AppendLine("Content-Type: application/octet-stream");
                    strParametros.AppendLine();
                    this.EscreverStream(ref postDataWriter, ref strParametros);
                    index++;
                }
            }
            strParametros.AppendLine(string.Format("--{0}--", boundary));
            this.EscreverStream(ref postDataWriter, ref strParametros);
            postDataWriter.Flush();
            byte[] data = postDataStream.ToArray();
            return CarregarHtmlBoundary(url, data, referer, boundary);
        }
        public string CarregarHtmlBoundary(string url, NameValueCollection parametrosCol, string referer, Dictionary<string, string> dicAnexos, string contentType, string host, string origin)
        {

            MemoryStream postDataStream = new MemoryStream();
            StreamWriter postDataWriter = new StreamWriter(postDataStream);

            string boundary = "----WebKitFormBoundaryO318gEOHYnlfi28i";
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
            {
                strParametros.AppendLine(string.Format("--{0}", boundary));
                strParametros.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", Param)).AppendLine();
                strParametros.AppendLine(parametrosCol[Param]);
            }
            this.EscreverStream(ref postDataWriter, ref strParametros);
            if (dicAnexos != null && dicAnexos.Keys.Count > 0)
            {
                foreach (var item in dicAnexos)
                {
                    string fileName = Path.GetFileName(item.Value);
                    strParametros.AppendLine(string.Format("--{0}", boundary));
                    strParametros.AppendLine(string.Concat(string.Format("Content-Disposition: form-data; name=\"{0}\"; ", item.Key), "filename=\"" + fileName + "\""));
                    strParametros.AppendLine("Content-Type: " + contentType);
                    strParametros.AppendLine();
                    this.EscreverStream(ref postDataWriter, ref strParametros);
                    this.LerArquivo(item.Value, ref postDataWriter, ref postDataStream);
                    this.EscreverStream(ref postDataWriter, ref strParametros);
                }
            }
            strParametros.AppendLine();
            strParametros.AppendLine(string.Format("--{0}--", boundary));
            this.EscreverStream(ref postDataWriter, ref strParametros);
            postDataWriter.Flush();
            byte[] data = postDataStream.ToArray();
            return CarregarHtmlBoundaryHost(url, data, referer, host, origin, boundary);
        }
        private void EscreverStream(ref StreamWriter streamWriter, ref StringBuilder postInfo)
        {
            streamWriter.Write(postInfo.ToString());
            postInfo = new StringBuilder();
        }
        private void LerArquivo(string filePath, ref StreamWriter postDataWriter, ref MemoryStream postDataStream)
        {
            //Leitura do arquivo
            postDataWriter.Flush();
            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    postDataStream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();
            }
        }
        public string CarregarHtmlBoundary(string url, string parametros, string referer, string boundary = "----WebKitFormBoundary")
        {

            WebClient.Headers.Clear();

            //WebClient.Headers.Set(HttpRequestHeader.Host, "workflow:14000");
            //WebClient.Headers.Set(HttpRequestHeader.KeepAlive, "true");
            //WebClient.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
            //WebClient.Headers.Set("Upgrade-Insecure-Requests", @"1");
            WebClient.Headers.Set(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + boundary);
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Set(HttpRequestHeader.Referer, referer);
            //WebClient.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            //WebClient.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,pt-BR;q=0.8,pt;q=0.7");
            return WebClient.UploadString(url, "POST", parametros);


            //byte[] b = WebClient.UploadData(url, "POST",DefaultEncoding.GetBytes(parametros));
            //MemoryStream output = new MemoryStream();
            //using (GZipStream g = new GZipStream(new MemoryStream(b), CompressionMode.Decompress))
            //{
            //    g.CopyTo(output);
            //}

            //return Encoding.UTF8.GetString(output.ToArray());            
        }
        public string CarregarHtmlBoundaryHost(string url, byte[] parametros, string referer, string host, string origin, string boundary = "----WebKitFormBoundary")
        {
            WebClient.Headers.Clear();
            WebClient.Headers.Set(HttpRequestHeader.Host, host);
            WebClient.Headers.Set(HttpRequestHeader.CacheControl, @"max-age=0");
            WebClient.Headers.Set("Origin", origin);
            WebClient.Headers.Set("Upgrade-Insecure-Requests", @"1");
            WebClient.Headers.Set(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + boundary);
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Set(HttpRequestHeader.Referer, referer);
            WebClient.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            WebClient.Headers.Set(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            byte[] b = WebClient.UploadData(url, "POST", parametros);
            return Encoding.UTF8.GetString(b);
        }
        public string CarregarHtmlBoundary(string url, byte[] parametros, string referer, string boundary = "----WebKitFormBoundary")
        {

            WebClient.Headers.Clear();

            //WebClient.Headers.Set(HttpRequestHeader.Host, "workflow:14000");
            //WebClient.Headers.Set(HttpRequestHeader.KeepAlive, "true");
            //WebClient.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
            //WebClient.Headers.Set("Upgrade-Insecure-Requests", @"1");
            WebClient.Headers.Set(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + boundary);
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Set(HttpRequestHeader.Referer, referer);
            //WebClient.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            //WebClient.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,pt-BR;q=0.8,pt;q=0.7");
            byte[] b = WebClient.UploadData(url, "POST", parametros);

            return Encoding.UTF8.GetString(b);

            //byte[] b = WebClient.UploadData(url, "POST",DefaultEncoding.GetBytes(parametros));
            //MemoryStream output = new MemoryStream();
            //using (GZipStream g = new GZipStream(new MemoryStream(b), CompressionMode.Decompress))
            //{
            //    g.CopyTo(output);
            //}

            //return Encoding.UTF8.GetString(output.ToArray());            
        }
        public byte[] CarregarDados(string url)
        {
            return WebClient.DownloadData(url);
        }
        public byte[] CarregarDados(string url, string referer)
        {
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return CarregarDados(url);
        }
        public byte[] CarregarDados(string url, NameValueCollection parametrosCol, string referer)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (string Param in parametrosCol.AllKeys.Where(d => d != null))
                strParametros.AppendFormat("{0}={1}&", GetEncodeIso(Param), GetEncodeIso(parametrosCol[Param]));
            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarDados(url, parametros, referer);
        }

        public byte[] CarregarDados(string url, List<KeyValuePair<string, string>> parametrosCol, string referer)
        {
            StringBuilder strParametros = new StringBuilder();
            string parametros = string.Empty;
            foreach (var Param in parametrosCol)
                strParametros.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(PtBr.Encoding.GetBytes(Param.Key)),
                    HttpUtility.UrlEncode(PtBr.Encoding.GetBytes(Param.Value == null ? string.Empty : Param.Value)));

            parametros = strParametros.ToString();
            parametros = parametros.Remove(parametros.Length - 1);

            return CarregarDados(url, parametros, referer);
        }

        public byte[] CarregarDados(string url, string parametros, string referer)
        {
            WebClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return WebClient.UploadData(url, "POST", DefaultEncoding.GetBytes(parametros));
        }
        public HtmlDocument CarregarHtmlDocument(string url)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(CarregarHtml(url));
            return doc;
        }
        public HtmlDocument ObterHtmlDocument(string htmlDocument)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlDocument);
            return doc;
        }
        public HtmlDocument CarregarHtmlDocument(string url, string referer)
        {
            WebClient.Headers.Clear();
            WebClient.Headers.Set(HttpRequestHeader.UserAgent, UserAgentCrawler.UserAgent());
            WebClient.Headers.Set(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            WebClient.Headers.Add(HttpRequestHeader.Referer, referer);
            return CarregarHtmlDocument(url);
        }
        public NameValueCollection GetValoresInput(HtmlNode node)
        {
            NameValueCollection colParametros = new NameValueCollection();
            HtmlNode.ElementsFlags.Remove("form");
            if (node != null)
            {
                var lista = node.SelectNodes(".//input[@type='hidden' and @name and @value]");
                if (lista == null && node.NextSibling != null)
                    lista = node.NextSibling.SelectNodes(".//input[@type='hidden' and @name and @value]");
                if (lista == null && node.NextSibling.NextSibling != null)
                    lista = node.NextSibling.NextSibling.SelectNodes(".//input[@type='hidden' and @name and @value]");
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.Attributes["value"];

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }
                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.Value;
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (valor.Contains("&quot;"))
                                valor = valor.Replace("&quot;", @"""");
                            colParametros.Add(name, GetEncodeIso(valor));
                        }
                    }
                }

                lista = node.SelectNodes(".//input[@type='text' and @name and @value]");
                if (lista == null && node.NextSibling != null)
                    lista = node.NextSibling.SelectNodes(".//input[@type='text' and @name and @value]");
                if (lista == null && node.NextSibling.NextSibling != null)
                    lista = node.NextSibling.NextSibling.SelectNodes(".//input[@type='text' and @name and @value]");
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.Attributes["value"];

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }
                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.Value;
                        }
                        if (!string.IsNullOrEmpty(name))
                        {

                            colParametros.Add(name, GetEncodeIso(valor));
                        }
                    }
                }

                lista = node.SelectNodes(".//input[@type='radio' and @name and @value and @checked]");
                if (lista != null && node.NextSibling != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.Attributes["value"];

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }
                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.Value;
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (valor.Contains("&quot;"))
                                valor = valor.Replace("&quot;", @"""");
                            colParametros.Add(name, GetEncodeIso(valor));
                        }
                    }
                }
                lista = node.SelectNodes(".//input[@type='email' and @name and @value]");
                if (lista == null && node.NextSibling != null)
                    lista = node.NextSibling.SelectNodes(".//input[@type='email' and @name and @value]");
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.Attributes["value"];

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }
                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.Value;
                        }
                        if (!string.IsNullOrEmpty(name))
                        {

                            colParametros.Add(name, GetEncodeIso(valor));
                        }
                    }
                }
            }
            return colParametros;
        }

        public NameValueCollection GetValoresInput(HtmlDocument doc)
        {
            NameValueCollection colParametros = new NameValueCollection();
            HtmlNode.ElementsFlags.Remove("form");
            if (doc != null)
            {
                var lista = doc.DocumentNode.SelectNodes("//input");
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.Attributes["value"];
                        var valueType = item.Attributes["type"];

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }
                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.Value;
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (valor.Contains("&quot;"))
                                valor = valor.Replace("&quot;", @"""");
                            if (valor.Contains("&lt;"))
                                valor = valor.Replace("&lt;", @"<");

                            colParametros.Add(name, valor);
                        }
                    }
                }
            }
            return colParametros;
        }

        public NameValueCollection GetValoresTextArea(HtmlDocument doc)
        {
            NameValueCollection colParametros = new NameValueCollection();
            HtmlNode.ElementsFlags.Remove("form");

            if (doc != null)
            {
                var lista = doc.DocumentNode.SelectNodes(".//textarea");

                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;
                        string valorSelected = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.InnerText;

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }

                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.ToString();
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            if (valor.Contains("&quot;"))
                                valor = valor.Replace("&quot;", @"""");
                            if (valor.Contains("&lt;"))
                                valor = valor.Replace("&lt;", @"<");

                            colParametros.Add(name, valor);
                        }
                    }
                }
            }
            return colParametros;
        }

        public NameValueCollection GetValoresSelectOption(HtmlDocument doc)
        {
            NameValueCollection colParametros = new NameValueCollection();
            HtmlNode.ElementsFlags.Remove("form");

            if (doc != null)
            {
                var lista = doc.DocumentNode.SelectNodes(".//select");

                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        string name = string.Empty;
                        string valor = string.Empty;
                        string valorSelected = string.Empty;

                        var valueName = item.Attributes["name"];
                        var valueAtributo = item.Attributes["value"];
                        var valueType = item.Attributes["type"];
                        var valueSelected = item.Attributes["selected"];

                        if (valueName != null)
                        {
                            name = valueName.Value;
                        }

                        if (valueAtributo != null)
                        {
                            valor = valueAtributo.Value;
                        }

                        if (valueSelected != null)
                        {
                            valorSelected = valueSelected.Value;
                        }

                        if (!string.IsNullOrEmpty(name))
                        {
                            if (valor.Contains("&quot;"))
                                valor = valor.Replace("&quot;", @"""");
                            if (valor.Contains("&lt;"))
                                valor = valor.Replace("&lt;", @"<");

                            colParametros.Add(name, valor);
                        }
                    }
                }
            }
            return colParametros;
        }

        public string GetEncodeIso(string valor)
        {
            return HttpUtility.UrlEncode(PtBr.Encoding.GetBytes(valor == null ? string.Empty : valor));
        }
        public string GetDecodeIso(string valor)
        {
            return HttpUtility.UrlDecode(valor);
        }
        public string GetHtmlDecode(string valor)
        {
            return HttpUtility.HtmlDecode(valor);
        }

        public IList<KeyValuePair<string, string>> ConvertNameValueKeyValue(NameValueCollection colParametros)
        {
            IList<KeyValuePair<string, string>> listaKey = new List<KeyValuePair<string, string>>();
            foreach (var param in colParametros.AllKeys)
            {
                if (colParametros[param].Split(',').Length > 1)
                {
                    foreach (var item in colParametros[param].Split(','))
                    {
                        listaKey.Add(new KeyValuePair<string, string>(param, GetDecodeIso(item).Replace("&amp;", "&")));
                    }
                }
                else
                {
                    listaKey.Add(new KeyValuePair<string, string>(param, GetDecodeIso(colParametros[param]).Replace("&amp;", "&")));
                }
            }
            return listaKey;
        }
        public HtmlNode ObterSingleNode(HtmlDocument doc, string path)
        {
            try
            {
                HtmlNode nod = doc.DocumentNode.SelectSingleNode(path);
                return nod;
            }
            catch { return null; }

        }
    }
}
