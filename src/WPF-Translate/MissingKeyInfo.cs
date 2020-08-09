namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Stellt einen Fehlenden Schlüssel in einer Datei dar
    /// </summary>
    public class MissingKeyInfo : NotifyBase
    {
        private string _File;
        private string _FullMatch;
        private string _Key;

        /// <summary>
        /// Datei
        /// </summary>
        public string File
        {
            get
            {
                return _File;
            }
            set
            {
                _File = value;

                RaisePropertyChanged(nameof(File));
            }
        }

        /// <summary>
        /// Voller Match
        /// </summary>
        public string FullMatch
        {
            get
            {
                return _FullMatch;
            }
            set
            {
                _FullMatch = value;

                RaisePropertyChanged(nameof(FullMatch));
            }
        }

        /// <summary>
        /// Schlüssel
        /// </summary>
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;

                RaisePropertyChanged(nameof(Key));
            }
        }
    }
}