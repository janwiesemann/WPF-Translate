using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace de.LandauSoftware.WPFTranslate
{
    [DebuggerDisplay("Key = {Key} Count = {Count}")]
    public class LangValueCollection : ObservableCollection<LangValue>
    {
        private static CancellationTokenSource CancelTokenSource;
        private bool _BackgroundIsHighlited;
        private string _Key;

        public LangValueCollection(string key, IEnumerable<Language> langs)
        {
            _Key = key;

            foreach (Language item in langs)
            {
                this.Add(new LangValue(item));
            }
        }

        public bool BackgroundIsHighlited
        {
            get
            {
                return _BackgroundIsHighlited;
            }
            set
            {
                _BackgroundIsHighlited = value;

                OnPropertyChanged(nameof(BackgroundIsHighlited));
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

        public void BlinkBackgroundForTwoSeconds()
        {
            BlinkBackgroundSecond(250, 8);
        }

        public void BlinkBackgroundSecond(int delay, int amount)
        {
            if (CancelTokenSource != null)
                CancelTokenSource.Cancel();

            CancelTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                CancellationToken token = CancelTokenSource.Token;

                BackgroundIsHighlited = false;

                while (amount > 0 && !token.IsCancellationRequested)
                {
                    BackgroundIsHighlited = !BackgroundIsHighlited;

                    amount--;

                    Task.Delay(delay).Wait();
                }

                BackgroundIsHighlited = false;
            }, CancelTokenSource.Token);
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