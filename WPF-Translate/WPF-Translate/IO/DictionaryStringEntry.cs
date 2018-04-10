using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Diese Klasse Stellt einen String-Eintrag in einem Wörterbuch dar.
    /// </summary>
    [DebuggerDisplay("Key = {Key} Value = {Value}")]
    public class DictionaryStringEntry : DictionaryRawEntry
    {
        private string _Key;

        /// <summary>
        /// Initialisiert einen neuen String Eintrag
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Wert</param>
        public DictionaryStringEntry(string key, string value) : base(value)
        {
            _Key = key;
        }

        /// <summary>
        /// Ruft den Key des Eintrages ab
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
        }
    }
}