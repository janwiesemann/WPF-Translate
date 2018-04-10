namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Stellt einen NameSpace in einem ResourceDictionary dar
    /// </summary>
    public class DictionaryNamespace
    {
        private string _Name;

        private string _Source;

        /// <summary>
        /// initialisiert eine neue Instanz eines Namespaces
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="source">Quelle</param>
        public DictionaryNamespace(string name, string source)
        {
            _Name = name;
            _Source = source;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        /// <summary>
        /// Quelle
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