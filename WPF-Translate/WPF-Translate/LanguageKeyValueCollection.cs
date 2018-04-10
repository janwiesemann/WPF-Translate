using de.LandauSoftware.Core.WPF;
using de.LandauSoftware.WPFTranslate.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Die Klasse ist eine Samlung aller Keys uns Sprachen
    /// </summary>
    public class LanguageKeyValueCollection : NotifyBase
    {
        private ObservableCollection<LangValueCollection> _Keys;

        private ObservableCollection<Language> _Languages;

        /// <summary>
        /// Erstellt eine neue LanguageKeyValueCollection
        /// </summary>
        public LanguageKeyValueCollection()
        {
            _Keys = new ObservableCollection<LangValueCollection>();
            _Languages = new ObservableCollection<Language>();
            _Languages.CollectionChanged += Languages_CollectionChanged;
        }

        /// <summary>
        /// Wurd aufgerufen wenn eine Sprache hinzugefügt, geändert oder gelöscht wird.
        /// </summary>
        public event EventHandler LanguagesChangedEvent;

        /// <summary>
        /// Beinhaltet alle Keys
        /// </summary>
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

        /// <summary>
        /// Eine Liste aller Sprachen
        /// </summary>
        public ObservableCollection<Language> Languages
        {
            get
            {
                return _Languages;
            }
        }

        /// <summary>
        /// Fügt einen leeren Key hinzu
        /// </summary>
        /// <returns></returns>
        public LangValueCollection AddKey()
        {
            LangValueCollection langVal = new LangValueCollection("noname", Languages);

            Keys.Add(langVal);

            return langVal;
        }

        /// <summary>
        /// Fügt eine neue Sprache hinzu
        /// </summary>
        /// <param name="langID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Fügt einen neuen Wert hinzu oder ändert diesen
        /// </summary>
        /// <param name="langID"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// Orpft, ob eine Sprache vorhanden ist
        /// </summary>
        /// <param name="langID"></param>
        /// <returns></returns>
        public bool ContainsLanguage(string langID)
        {
            return GetLangByID(langID) != null;
        }

        /// <summary>
        /// Ruft eine Sprache über die ID ab.
        /// </summary>
        /// <param name="langID">Sprach ID</param>
        /// <returns>Null wenn icht gefunden</returns>
        public Language GetLangByID(string langID)
        {
            foreach (Language item in Languages)
            {
                if (item.LangKey == langID)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Ruft alle Einträge als Dictionary Entry ab
        /// </summary>
        /// <param name="lang">Sprachen</param>
        /// <returns></returns>
        public IEnumerable<DictionaryRawEntry> GetLangEntrysAsDictionaryEntry(Language lang)
        {
            List<DictionaryRawEntry> res = new List<DictionaryRawEntry>();

            foreach (LangValueCollection item in Keys)
            {
                LangValue lval = item.FindValueByLang(lang);

                if (lval != null)
                    res.Add(new DictionaryStringEntry(item.Key, lval.Value));
            }

            return res;
        }

        /// <summary>
        /// Sucht nach einer LangValueCollection über den Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Null wenn nicht gefunden</returns>
        public LangValueCollection GetLangValueCollectionByKey(string key)
        {
            foreach (LangValueCollection item in Keys)
            {
                if (item.Key == key)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Entfernt einen Key
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(LangValueCollection key)
        {
            Keys.Remove(key);
        }

        /// <summary>
        /// Entfernt eine Sprache
        /// </summary>
        /// <param name="lang">Sprache</param>
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

        private void Languages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LanguagesChangedEvent?.Invoke(this, null);
        }
    }
}