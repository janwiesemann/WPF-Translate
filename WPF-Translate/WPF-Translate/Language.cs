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
