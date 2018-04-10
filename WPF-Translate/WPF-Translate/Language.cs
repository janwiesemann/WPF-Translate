using de.LandauSoftware.Core.WPF;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Stellt eine Sprache dar
    /// </summary>
    public class Language : NotifyBase
    {
        private string _LangKey;

        /// <summary>
        /// Initialisiert eine neue Instantz der Sprache
        /// </summary>
        public Language()
        { }

        /// <summary>
        /// Initialisiert eine neue Instantz der Sprache
        /// </summary>
        /// <param name="key">Sprachkey z.B. en-us</param>
        public Language(string key)
        {
            _LangKey = key;
        }

        /// <summary>
        /// Ruft den Sprachkey ab oder setzt diesen
        /// </summary>
        public string LangKey
        {
            get
            {
                return _LangKey;
            }
            set
            {
                _LangKey = value;

                RaisePropertyChanged(nameof(LangKey));
            }
        }

        /// <summary>
        /// Ungleich
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Language left, Language right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Gleich
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Language left, Language right)
        {
            if (object.ReferenceEquals(left, right))
                return true;
            else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
                return false;
            else
                return left.LangKey == right.LangKey;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null) || !(obj is Language))
                return false;

            return ((Language)obj).LangKey == LangKey;
        }

        /// <summary>
        /// Ruft den Hash-Code der Sprache ab
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return LangKey.GetHashCode();
        }
    }
}