using de.LandauSoftware.Core.WPF;

namespace de.LandauSoftware.WPFTranslate
{
    public class Language : NotifyBase
    {
        private string _LangKey;

        public Language()
        { }

        public Language(string key)
        {
            _LangKey = key;
        }

        public string LangKey
        {
            get
            {
                return _LangKey;
            }
            set
            {
                _LangKey = value;
            }
        }

        public static bool operator !=(Language left, Language right)
        {
            return !(left == right);
        }

        public static bool operator ==(Language left, Language right)
        {
            if (object.ReferenceEquals(left, right))
                return true;
            else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
                return false;
            else
                return left.LangKey == right.LangKey;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null) || !(obj is Language))
                return false;

            return ((Language)obj).LangKey == LangKey;
        }

        public override int GetHashCode()
        {
            return LangKey.GetHashCode();
        }
    }
}