using de.LandauSoftware.Core.WPF;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace de.LandauSoftware.WPFTranslate
{
    public class TranslateWindowViewModel : DialogCoordinatorNotifyBase
    {
        private CancellationTokenSource _CancellationTokenSource;
        private bool _CancelOnException;
        private int _CurrentPosition = -1;
        private LangValueCollection _CurrentTranslationKey;
        private IList<LangValueCollection> _KeyList;
        private IList<Language> _Languages;
        private RelayICommand _RestCommand;
        private Language _SelectedSourceLanguage;
        private Language _SelectedTargetLanguage;
        private RelayICommand _StartCommand;
        private RelayICommand _StopCommand;
        private List<Language> _TargetLanguages;
        private bool _TranslateJustEmpty;

        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                return _CancellationTokenSource;
            }
            set
            {
                _CancellationTokenSource = value;

                RaisePropertyChanged(nameof(CancellationTokenSource));
            }
        }

        public bool CancelOnException
        {
            get
            {
                return _CancelOnException;
            }
            set
            {
                _CancelOnException = value;

                RaisePropertyChanged(nameof(CancelOnException));
            }
        }

        public int CurrentPosition
        {
            get
            {
                return _CurrentPosition;
            }
            set
            {
                _CurrentPosition = value;

                RaisePropertyChanged(nameof(CurrentPosition));
                RaisePropertyChanged(nameof(CurrentPositionUINumber));
            }
        }

        public int CurrentPositionUINumber
        {
            get
            {
                return _CurrentPosition + 1;
            }
        }

        public LangValueCollection CurrentTranslationKey
        {
            get
            {
                return _CurrentTranslationKey;
            }
            set
            {
                _CurrentTranslationKey = value;

                RaisePropertyChanged(nameof(CurrentTranslationKey));
            }
        }

        public IList<LangValueCollection> KeyList
        {
            get
            {
                return _KeyList;
            }
            set
            {
                _KeyList = value;

                RaisePropertyChanged(nameof(KeyList));
            }
        }

        public IList<Language> Languages
        {
            get
            {
                return _Languages;
            }
            set
            {
                _Languages = value;

                RaisePropertyChanged(nameof(Languages));

                TargetLanguages = null;
            }
        }

        public ICommand RestCommand
        {
            get
            {
                if (_RestCommand == null)
                    _RestCommand = new RelayICommand(p => CancellationTokenSource == null, p => Rest());

                return _RestCommand;
            }
        }

        public Language SelectedSourceLanguage
        {
            get
            {
                return _SelectedSourceLanguage;
            }
            set
            {
                if (value != _SelectedSourceLanguage)
                    Rest();

                _SelectedSourceLanguage = value;

                RaisePropertyChanged(nameof(SelectedSourceLanguage));

                TargetLanguages = null;
            }
        }

        public Language SelectedTargetLanguage
        {
            get
            {
                return _SelectedTargetLanguage;
            }
            set
            {
                if (value != _SelectedTargetLanguage)
                    Rest();

                _SelectedTargetLanguage = value;

                RaisePropertyChanged(nameof(SelectedTargetLanguage));
            }
        }

        public ICommand StartCommand
        {
            get
            {
                if (_StartCommand == null)
                    _StartCommand = new RelayICommand(p => CancellationTokenSource == null && SelectedSourceLanguage != null && SelectedTargetLanguage != null, p =>
                    {
                        if (KeyList.Count - 1 == CurrentPosition)
                            CurrentPosition = -1;

                        CancellationTokenSource = new CancellationTokenSource();

                        Task.Run(() =>
                        {
                            CancellationToken cancelToken = CancellationTokenSource.Token;

                            while (!cancelToken.IsCancellationRequested)
                            {
                                if (!TranslateNextElement())
                                {
                                    CurrentTranslationKey = null;

                                    break;
                                }
                            }

                            CancellationTokenSource = null;
                        }, CancellationTokenSource.Token);
                    });

                return _StartCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (_StopCommand == null)
                    _StopCommand = new RelayICommand(p => CancellationTokenSource != null, p =>
                    {
                        CancellationTokenSource.Cancel();
                        CancellationTokenSource = null;
                    });

                return _StopCommand;
            }
        }

        public List<Language> TargetLanguages
        {
            get
            {
                if (_TargetLanguages == null && Languages != null && SelectedSourceLanguage != null)
                {
                    _TargetLanguages = new List<Language>();

                    foreach (Language item in Languages)
                    {
                        if (item != SelectedSourceLanguage)
                            _TargetLanguages.Add(item);
                    }
                }

                return _TargetLanguages;
            }
            set
            {
                _TargetLanguages = value;

                RaisePropertyChanged(nameof(TargetLanguages));
            }
        }

        public bool TranslateJustEmpty
        {
            get
            {
                return _TranslateJustEmpty;
            }
            set
            {
                _TranslateJustEmpty = value;

                RaisePropertyChanged(nameof(TranslateJustEmpty));
            }
        }

        private void Rest()
        {
            CurrentPosition = -1;
            CurrentTranslationKey = null;
        }

        private bool TranslateNextElement()
        {
            CurrentPosition = CurrentPosition + 1;

            if (CurrentPosition >= KeyList.Count)
                return false;

            CurrentTranslationKey = KeyList[CurrentPosition];

            LangValue targetValue = CurrentTranslationKey.FindValueByLang(SelectedTargetLanguage);

            if (TranslateJustEmpty && !string.IsNullOrWhiteSpace(targetValue.Value))
                return true;

            LangValue sourceValue = CurrentTranslationKey.FindValueByLang(SelectedSourceLanguage);

            if (string.IsNullOrWhiteSpace(sourceValue.Value))
                return true;

            try
            {
                string resp = Translate.StringTranslate(sourceValue.Value, SelectedSourceLanguage.LangKey, SelectedTargetLanguage.LangKey);

                targetValue.Value = resp;
            }
            catch (Exception ex)
            {
                if (CancelOnException)
                {
                    Task.Run(async () =>
                    {
                        await DialogCoordinator.ShowMessageAsync(this, "Fehler", "Beim Übersetzten ist ein Fehler aufgetreten!" + Environment.NewLine + ex.Message);
                    });

                    return false;
                }
            }

            return true;
        }
    }
}