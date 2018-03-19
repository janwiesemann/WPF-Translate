using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate
{
    [DebuggerDisplay("{Source}")]
    public class MergedDictionary
    {
        private string _Source;

        public MergedDictionary(string source)
        {
            _Source = source;
        }

        public string Source
        {
            get
            {
                return _Source;
            }
        }
    }
}