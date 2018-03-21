using de.LandauSoftware.Core.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.LandauSoftware.WPFTranslate
{
    public class Language : NotifyBase
    {
        public Language()
        { }

        public Language(string key)
        {
            _LangKey = key;
        }

        private string _LangKey;

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

        public static bool operator ==(Language left, Language right)
        {
            if (object.ReferenceEquals(left, right))
                return true;
            else if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
                return false;
            else
                return left.LangKey == right.LangKey;
        }

        public static bool operator !=(Language left, Language right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return LangKey.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null) || !(obj is Language))
                return false;

            return ((Language)obj).LangKey == LangKey;
        }

    }
}
