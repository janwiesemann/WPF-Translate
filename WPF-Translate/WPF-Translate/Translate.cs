using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace de.LandauSoftware.WPFTranslate
{
    public static class Translate
    {
        public static string StringTranslate(string text, string sourceLanguage, string targetLanguage)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("q", text);

            using (WebClient client = new WebClient())
            {
                byte[] data = client.UploadValues(string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&ie=UTF-8&oe=UTF-8", sourceLanguage, targetLanguage), nvc);

                string response = Encoding.UTF8.GetString(data);

                return response;
            }
        }
    }
}
