using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate.IO
{
    [DebuggerDisplay("Value = {Value}")]
    public class RawDictionaryEntry
    {
        private string _Value;

        public RawDictionaryEntry(string value)
        {
            _Value = value;
        }

        public string Value
        {
            get
            {
                return _Value;
            }
        }
    }
}