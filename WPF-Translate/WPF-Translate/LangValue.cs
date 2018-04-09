using de.LandauSoftware.Core.WPF;

namespace de.LandauSoftware.WPFTranslate
{
    public class LangValue : NotifyBase
    {
        private Language _Language;

        private string _Value;

        public LangValue(Language lang)
        {
            _Language = lang;
        }

        public LangValue(Language lang, string value) : this(lang)
        {
            _Value = value;
        }

        public Language Language
        {
            get
            {
                return _Language;
            }
        }

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