using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Stellt eine Samlung an Werten dar
    /// </summary>
    [DebuggerDisplay("Key = {Key} Count = {Count}")]
    public class LangValueCollection : ObservableCollection<LangValue>
    {
        private static CancellationTokenSource CancelTokenSource;
        private bool _BackgroundIsHighlited;
        private string _Key;

        /// <summary>
        /// Ersellt eine neue Smlung
        /// </summary>
        /// <param name="key"></param>
        /// <param name="langs"></param>
        public LangValueCollection(string key, IEnumerable<Language> langs)
        {
            _Key = key;

            foreach (Language item in langs)
            {
                this.Add(new LangValue(item));
            }
        }

        /// <summary>
        /// Legt fest, ob der Hintergrund dieser Spalte rot makiert ist
        /// </summary>
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

        /// <summary>
        /// Key
        /// </summary>
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

        /// <summary>
        /// Lässt den Hintergrund der Spalte für zwei Sekunden blinken
        /// </summary>
        public void BlinkBackgroundForTwoSeconds()
        {
            BlinkBackgroundSecond(250, 8);
        }

        /// <summary>
        /// Lässt den Hintergrund Blinken
        ///
        /// Es kann immer nur ein Hintergrund blinken!
        /// </summary>
        /// <param name="delay">Verzögerung zwischen den Wechseln</param>
        /// <param name="amount">Anzahl der Wechsel</param>
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

        /// <summary>
        /// Sucht nach einem Wert über die Sprache
        /// </summary>
        /// <param name="lang">Sprache</param>
        /// <returns>Null wenn nicht gefunden</returns>
        public LangValue FindValueByLang(Language lang)
        {
            foreach (LangValue item in this)
            {
                if (item.Language == lang)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Entfernt eine Sprache
        /// </summary>
        /// <param name="lang"></param>
        public void RemoveLang(Language lang)
        {
            LangValue val = FindValueByLang(lang);

            this.Remove(val);
        }

        /// <summary>
        /// Setzt einen Wert
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="value"></param>
        public void SetValue(Language lang, string value)
        {
            LangValue val = FindValueByLang(lang);

            val.Value = value;
        }

        /// <summary>
        /// OnPropertyChanged
        /// </summary>
        /// <param name="name">name der Eigenschaft</param>
        protected virtual void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(new PropertyChangedEventArgs(name));
        }
    }
}