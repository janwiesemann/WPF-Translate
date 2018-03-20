using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace de.LandauSoftware.WPFTranslate
{
    public class LangValueCollection : ObservableCollection<LangValue>
    {
        public LangValueCollection(IEnumerable<Language> langs)
        {
            foreach (Language item in langs)
            {
                this.Add(new LangValue(item));
            }
        }

        private string _Key;

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

        private LangValue FindValueByLang(Language lang)
        {
            foreach (LangValue item in this)
            {
                if (item.Language == lang)
                    return item;
            }

            return null;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        public void SetValue(Language lang, string value)
        {
            LangValue val = FindValueByLang(lang);

            val.Value = value;
        }

        internal void RemoveLang(Language lang)
        {
            LangValue val = FindValueByLang(lang);

            this.Remove(val);
        }
    }
}
