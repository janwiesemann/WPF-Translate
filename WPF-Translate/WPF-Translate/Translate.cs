using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Diese Klasse beinhalten due Funktion des Übsersetztens
    /// </summary>
    public static class Translate
    {
        /// <summary>
        /// Übersetzt einen String
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="sourceLanguage">Quell Sprache ID</param>
        /// <param name="targetLanguage">Ziel Sprach ID</param>
        /// <returns></returns>
        public static string StringTranslate(string text, string sourceLanguage, string targetLanguage)
        {
            NameValueCollection nvc = new NameValueCollection
            {
                { "q", text }
            };

            using (WebClient client = new WebClient())
            {
                byte[] data = client.UploadValues(string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&ie=UTF-8&oe=UTF-8", sourceLanguage, targetLanguage), nvc);

                string response = Encoding.UTF8.GetString(data);

                string result = string.Empty;

                JArray arr = JArray.Parse(response);

                foreach (JArray item in arr.First)
                {
                    result += item.First.Value<string>();
                }

                return result;
            }
        }
    }
}