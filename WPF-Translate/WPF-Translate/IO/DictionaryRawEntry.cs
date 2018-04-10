using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Stellt einen Rohen Eintrag aus einem Wörderbuch dar.
    /// Dieser kann z.B. den rohen XML-Code für einen Style beinhalten
    /// </summary>
    [DebuggerDisplay("Value = {Value}")]
    public class DictionaryRawEntry
    {
        private string _Value;

        /// <summary>
        /// Erstellt eine neue Instanz, eines rohen Eintrages
        /// </summary>
        /// <param name="value">Wert</param>
        public DictionaryRawEntry(string value)
        {
            _Value = value;
        }

        /// <summary>
        /// Ruft den Wert ab
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
        }
    }
}