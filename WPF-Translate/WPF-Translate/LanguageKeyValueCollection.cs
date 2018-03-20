using de.LandauSoftware.Core.WPF;
using de.LandauSoftware.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.LandauSoftware.WPFTranslate
{
    public class LanguageKeyValueCollection : NotifyBase
    {
        public LanguageKeyValueCollection()
        {
            _Keys = new ObservableCollection<LangValueCollection>();
            _Languages = new ObservableCollection<Language>();
            _Languages.CollectionChanged += Languages_CollectionChanged;
        }

        public event EventHandler LanguagesChangedEvent;

        private void Languages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LanguagesChangedEvent?.Invoke(this, null);
        }

        private ObservableCollection<LangValueCollection> _Keys;
        private ObservableCollection<Language> _Languages;

        public ObservableCollection<LangValueCollection> Keys
        {
            get
            {
                return _Keys;
            }
            set
            {
                _Keys = value;

                RaisePropertyChanged(nameof(Keys));
            }
        }

        public ObservableCollection<Language> Languages
        {
            get
            {
                return _Languages;
            }
            set
            {
                _Languages = value;
            }
        }

        public bool ContainsLanguage(string langID)
        {
            return GetLangByID(langID) != null;
        }

        private Language GetLangByID(string langID)
        {
            foreach (Language item in Languages)
            {
                if (item.LangKey == langID)
                    return item;
            }

            return null;
        }

        private LangValueCollection GetLangValueCollectionByKey(string key)
        {
            foreach (LangValueCollection item in Keys)
            {
                if (item.Key == key)
                    return item;
            }

            return null;
        }

        public void AddSetValue(string langID, string key, string value)
        {
            Language lang = GetLangByID(langID);

            if (lang == null)
                lang = AddLanguage(langID);

            LangValueCollection langKey = GetLangValueCollectionByKey(key);

            if(langKey == null)
            {
                langKey = new LangValueCollection(Languages);

                Keys.Add(langKey);
            }

            langKey.SetValue(lang, value);
        }

        public void RemoveLanguage(string langID)
        {
            Language lang = GetLangByID(langID);

            if (lang == null)
                throw new KeyNotFoundException("Lang was not Found!");

            Languages.Remove(lang);

            foreach(LangValueCollection langkey in Keys)
            {
                langkey.RemoveLang(lang);
            }
        }

        public Language AddLanguage(string langID)
        {
            Language ret = new Language(langID);

            foreach (LangValueCollection langkey in Keys)
            {
                langkey.Add(new LangValue(ret));
            }

            Languages.Add(ret);

            return ret;
        }

        public void AddKey()
        {
            Keys.Add(new LangValueCollection(Languages));
        }

        public void RemoveKey(LangValueCollection key)
        {
            Keys.Remove(key);
        }

        public Language FindLang(string langID)
        {
            foreach (Language item in Languages)
            {
                if (item.LangKey == langID)
                    return item;
            }

            return null;
        }
    }
}
