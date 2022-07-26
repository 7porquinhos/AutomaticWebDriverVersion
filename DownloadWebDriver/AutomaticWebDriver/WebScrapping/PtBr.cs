using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AutomaticWebDriver.WebScrapping
{
    public class PtBr
    {
        private static CultureInfo _PtBrCulture = null;
        private static Encoding _PtBrEncoding = null;

        public static CultureInfo Culture
        {
            get
            {
                if (_PtBrCulture == null)
                    _PtBrCulture = new CultureInfo("pt-BR");
                return _PtBrCulture;
            }
        }

        public static Encoding Encoding
        {
            get
            {
                if (_PtBrEncoding == null)
                    _PtBrEncoding = Encoding.GetEncoding("ISO-8859-1");
                return PtBr._PtBrEncoding;
            }
            set
            {
                _PtBrEncoding = value;
            }
        }

        public static DateTime DateTimeParse(string Data)
        {
            return DateTimeParse(Data, "dd/MM/yyyy");
        }

        public static DateTime DateTimeParse(string Data, string Format)
        {
            return DateTime.ParseExact(HttpUtility.HtmlDecode(Data).Trim(), Format, Culture);
        }
    }
}
