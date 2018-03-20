namespace de.LandauSoftware.WPFTranslate.IO
{
    public class DictionaryNamespace
    {
        private string _Name;

        private string _Source;

        public DictionaryNamespace(string name, string source)
        {
            _Name = name;
            _Source = source;
        }

        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public string Source
        {
            get
            {
                return _Source;
            }
        }

        public static bool operator !=(DictionaryNamespace left, DictionaryNamespace right)
        {
            return !(left == right);
        }

        public static bool operator ==(DictionaryNamespace left, DictionaryNamespace right)
        {
            return left.Name == right.Name && left.Source == right.Source;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DictionaryNamespace) || object.ReferenceEquals(obj, null))
                return false;

            return this == (DictionaryNamespace)obj;
        }

        public override int GetHashCode()
        {
            return _Name.GetHashCode() * 13 ^ _Source.GetHashCode();
        }
    }
}