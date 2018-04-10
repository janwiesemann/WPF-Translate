using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Stellt ein Merged Dictionary dar.
    /// </summary>
    [DebuggerDisplay("{Source}")]
    public class MergedDictionary
    {
        private string _Source;

        /// <summary>
        /// Initialisiert eine neue Instanz des Merged Dictionarys
        /// </summary>
        /// <param name="source">Quelle</param>
        public MergedDictionary(string source)
        {
            _Source = source;
        }

        /// <summary>
        /// Quellpfad
        /// </summary>
        public string Source
        {
            get
            {
                return _Source;
            }
        }
    }
}