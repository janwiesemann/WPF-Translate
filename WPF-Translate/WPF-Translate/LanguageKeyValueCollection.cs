using de.LandauSoftware.Core.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace de.LandauSoftware.WPFTranslate
{
    public class LanguageKeyValueCollection : NotifyBase
    {
        private ObservableCollection<LangValueCollection> _Keys;

        private ObservableCollection<Language> _Languages;

        public LanguageKeyValueCollection()
        {
            _Keys = new ObservableCollection<LangValueCollection>();
            _Languages = new ObservableCollection<Language>();
            _Languages.CollectionChanged += Languages_CollectionChanged;
        }

        public event EventHandler LanguagesChangedEvent;

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

        public LangValueCollection AddKey()
        {
            LangValueCollection langVal = new LangValueCollection("noname", Languages);

            Keys.Add(langVal);

            return langVal;
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

        public void AddSetValue(string langID, string key, string value)
        {
            Language lang = GetLangByID(langID);

            if (lang == null)
                lang = AddLanguage(langID);

            LangValueCollection langKey = GetLangValueCollectionByKey(key);

            if (langKey == null)
            {
                langKey = new LangValueCollection(key, Languages);

                Keys.Add(langKey);
            }

            langKey.SetValue(lang, value);
        }

        public bool ContainsLanguage(string langID)
        {
            return GetLangByID(langID) != null;
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

        public void RemoveKey(LangValueCollection key)
        {
            Keys.Remove(key);
        }

        public void RemoveLanguage(Language lang)
        {
            Languages.Remove(lang);

            for (int i = Keys.Count - 1; i >= 0; i--)
            {
                LangValueCollection langvals = Keys[i];

                langvals.RemoveLang(lang);

                if (langvals.Count <= 0)
                    Keys.RemoveAt(i);
            }
        }

        public void RemoveLanguage(string langID)
        {
            Language lang = GetLangByID(langID);

            if (lang == null)
                throw new KeyNotFoundException("Lang was not Found!");

            RemoveLanguage(lang);
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

        private void Languages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LanguagesChangedEvent?.Invoke(this, null);
        }

        //public void OrderByKey()
        //{
        //    Keys = new ObservableCollection<LangValueCollection>(Keys.OrderBy(k => k.Key));
        //}
    }
}