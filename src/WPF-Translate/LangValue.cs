using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Stellt einen Wertz einer bestimmten Sprache dar
    /// </summary>
    [DebuggerDisplay("Value = {Value}")]
    public class LangValue : NotifyBase
    {
        private string _Value;

        /// <summary>
        /// Erstellt einen neuen Wert
        /// </summary>
        /// <param name="lang">Sprache</param>
        public LangValue(Language lang)
        {
            Language = lang;
        }

        /// <summary>
        /// Ruft die Sprache des Wertes ab
        /// </summary>
        public Language Language { get; }

        /// <summary>
        /// Der Wert
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;

                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}