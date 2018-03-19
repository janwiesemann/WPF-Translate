using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate
{
    [DebuggerDisplay("Key = {Key} Value = {Value}")]
    public class DictionaryEntry : RawDictionaryEntry
    {
        private string _Key;

        public DictionaryEntry(string key, string value) : base(value)
        {
            _Key = key;
        }

        public string Key
        {
            get
            {
                return _Key;
            }
        }
    }
}