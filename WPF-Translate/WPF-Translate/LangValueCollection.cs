using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate
{
    [DebuggerDisplay("Key = {Key} Count = {Count}")]
    public class LangValueCollection : ObservableCollection<LangValue>
    {
        private string _Key;

        public LangValueCollection(string key, IEnumerable<Language> langs)
        {
            _Key = key;

            foreach (Language item in langs)
            {
                this.Add(new LangValue(item));
            }
        }

        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;

                OnPropertyChanged(nameof(Key));
            }
        }

        public LangValue FindValueByLang(Language lang)
        {
            foreach (LangValue item in this)
            {
                if (item.Language == lang)
                    return item;
            }

            return null;
        }

        public void RemoveLang(Language lang)
        {
            LangValue val = FindValueByLang(lang);

            this.Remove(val);
        }

        public void SetValue(Language lang, string value)
        {
            LangValue val = FindValueByLang(lang);

            val.Value = value;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }
    }
}