namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Stellt einen NameSpace in einem ResourceDictionary dar
    /// </summary>
    public class DictionaryNamespace
    {
        /// <summary>
        /// initialisiert eine neue Instanz eines Namespaces
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="source">Quelle</param>
        public DictionaryNamespace(string name, string source)
        {
            Name = name;
            Source = source;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Quelle
        /// </summary>
        public string Source { get; }
    }
}