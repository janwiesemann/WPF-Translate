using de.LandauSoftware.Core.WPF;
using de.LandauSoftware.Core;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using de.LandauSoftware.WPFTranslate.IO;
using System.Text.RegularExpressions;
using System.IO;

namespace de.LandauSoftware.WPFTranslate
{
    public class MainWindowViewModel : NotifyBase
    {
        private LanguageKeyValueCollection _LangData;
        private IDialogCoordinator _DialogCoordinator;
        private RelayICommand _LoadFileCommand;
        private Dictionary<Language, ResourceDictionaryFile> _FileList;
        private RelayICommand _ClearCommand;
        private RelayICommand _AddKeyCommand;
        private RelayICommand<LangValueCollection> _RemoveKeyCommand;

        public event EventHandler LanguageCollectionChangedEvent;
        public event EventHandler<LangValueCollection> LanguageCollectionScrollIntoViewRequest;

        private void RaiseLanguageCollectionChangedEvent()
        {
            LanguageCollectionChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        public LanguageKeyValueCollection LangData
        {
            get
            {
                if(_LangData == null)
                {
                    _LangData = new LanguageKeyValueCollection();
                    _LangData.LanguagesChangedEvent += LangData_LanguagesChangedEvent;
                }

                return _LangData;
            }
            set
            {
                _LangData = value;

                RaisePropertyChanged(nameof(LangData));

                RaiseLanguageCollectionChangedEvent();
            }
        }

        private void LangData_LanguagesChangedEvent(object sender, EventArgs e)
        {
            LanguageCollectionChangedEvent?.Invoke(sender, e);
        }

        public IDialogCoordinator DialogCoordinator
        {
            get
            {
                if (_DialogCoordinator == null)
                    _DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;

                return _DialogCoordinator;
            }
        }

        public Dictionary<Language, ResourceDictionaryFile> FileList
        {
            get
            {
                if (_FileList == null)
                    _FileList = new Dictionary<Language, ResourceDictionaryFile>();

                return _FileList;
            }
        }

        private bool FileListContainsFile(string filename)
        {
            return FileList.Contains(kvp => kvp.Value.FileName == filename);
        }

        private string TryFindLangKey(string filename)
        {
            filename = Path.GetFileName(filename);

            Match match = Regex.Match(filename, @"(\w{2}-\w{2})");

            if (match.Success)
                return match.Groups[1].Value;
            else
                return null;
        }

        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                    _SearchCommand = new RelayICommand(async p =>
                    {
                        SearchWindow sw = new SearchWindow();

                        if (sw.ShowDialog() != true)
                            return;

                        SearchWindow.SearchModule searchModule = sw.CreateSearchModule();

                        ProgressDialogController progresscontroller = await DialogCoordinator.ShowProgressAsync(this, "Suche", "Bitte warten...", true);

                        progresscontroller.Maximum = LangData.Keys.Count;

                        for (int i = 0; i < LangData.Keys.Count; i++)
                        {
                            progresscontroller.SetProgress(i);

                            if (searchModule.IsMatch(LangData.Keys[i]))
                            {
                                LanguageCollectionScrollIntoViewRequest?.Invoke(this, LangData.Keys[i]);

                                break;
                            }
                            else if (progresscontroller.IsCanceled)
                                break;
                        }

                        await progresscontroller.CloseAsync();
                    });

                return _SearchCommand;
            }
        }

        public ICommand AddKeyCommand
        {
            get
            {
                if (_AddKeyCommand == null)
                    _AddKeyCommand = new RelayICommand(p =>
                    {
                        LangValueCollection langValue = LangData.AddKey();

                        LanguageCollectionScrollIntoViewRequest?.Invoke(this, langValue);
                    });

                return _AddKeyCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_ClearCommand == null)
                    _ClearCommand = new RelayICommand(async p => {
                    MessageDialogResult res = await DialogCoordinator.ShowMessageAsync(this, "Löschen", "Es werden alle Einträge gelöscht. Soll die Aktion wirklich ausgeführt werden?", MessageDialogStyle.AffirmativeAndNegative, DialogSettingsYesNo);

                        if(res == MessageDialogResult.Affirmative)
                        {
                            FileList.Clear();
                            LangData = null;
                        }
                    });

                return _ClearCommand;
            }
        }

        public static readonly MetroDialogSettings DialogSettingsYesNo = new MetroDialogSettings() { AffirmativeButtonText = "Ja", NegativeButtonText = "Nein", DefaultButtonFocus = MessageDialogResult.Negative };
        private RelayICommand<LangValueCollection> _TranslateKeyCommand;
        private RelayICommand _AddLanguageCommand;
        private RelayICommand _RemoveLanguageCommand;
        private RelayICommand _SearchCommand;

        public ICommand RemoveKeyCommand
        {
            get
            {
                if (_RemoveKeyCommand == null)
                    _RemoveKeyCommand = new RelayICommand<LangValueCollection>(async lk =>
                    {
                        MessageDialogResult res = await DialogCoordinator.ShowMessageAsync(this, "Löschen", "Soll der Eintrag wirklich gelöscht werden?", MessageDialogStyle.AffirmativeAndNegative, DialogSettingsYesNo);

                        if(res == MessageDialogResult.Affirmative)
                            LangData.RemoveKey(lk);
                    });

                return _RemoveKeyCommand;
            }
        }

        public ICommand RemoveLanguageCommand
        {
            get
            {
                if (_RemoveLanguageCommand == null)
                    _RemoveLanguageCommand = new RelayICommand(p => LangData.Languages.Count > 0, p =>
                    {
                        RemoveLanguageWindow rlw = new RemoveLanguageWindow(FileList);

                        if(rlw.ShowDialog() == true)
                        {
                            Language lang = rlw.Selectedlanguage;

                            FileList.Remove(lang);
                            LangData.RemoveLanguage(lang);
                        }
                    });

                return _RemoveLanguageCommand;
            }
        }

        public ICommand AddLanguageCommand
        {
            get
            {
                if (_AddLanguageCommand == null)
                    _AddLanguageCommand = new RelayICommand(async p =>
                    {
                        LanguageSetupWindow lsw = new LanguageSetupWindow();

                        if(lsw.ShowDialog() == true)
                        {
                            ResourceDictionaryFile rdf = new ResourceDictionaryFile(lsw.FileName);

                            await AddResourceDictionaryFileToLangData(rdf, lsw.LangID);
                        }
                    });

                return _AddLanguageCommand;
            }
        }

        public ICommand TranslateKeyCommand
        {
            get
            {
                if (_TranslateKeyCommand == null)
                    _TranslateKeyCommand = new RelayICommand<LangValueCollection>(lk => {
                    });

                return _TranslateKeyCommand;
            }
        }

        private async Task AddResourceDictionaryFileToLangData(ResourceDictionaryFile rdf, string langID)
        {
            while (langID == null || LangData.ContainsLanguage(langID))
            {
                langID = await DialogCoordinator.ShowInputAsync(this, "Sprache vorhanden!", "Die Sprache konnte nicht festgelegt werden! Möglicherweise ist diese bereits vorhanden oder der SprachKey konnte nicht gefunden werden.");

                if (string.IsNullOrWhiteSpace(langID))
                    return;
            }

            for (int i = rdf.Entrys.Count - 1; i >= 0; i--)
            {
                DictionaryEntry entry = rdf.Entrys[i] as DictionaryEntry;

                if (entry != null)
                {
                    LangData.AddSetValue(langID, entry.Key, entry.Value);

                    rdf.Entrys.RemoveAt(i);
                }
            }

            Language lang = LangData.FindLang(langID);

            if (lang == null)
                lang = LangData.AddLanguage(langID);

            FileList.Add(lang, rdf);
        }

        public ICommand LoadFileCommand
        {
            get
            {
                if (_LoadFileCommand == null)
                    _LoadFileCommand = new RelayICommand( async o =>
                    {
                        try
                        {
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Multiselect = true;
                            ofd.Filter = "XAML-Datei|*.xaml";

                            if (ofd.ShowDialog() != true)
                                return;

                            foreach (string file in ofd.FileNames)
                            {
                                if (FileListContainsFile(file))
                                    continue;

                                ResourceDictionaryFile rdf = ResourceDictionaryReader.Read(file);

                                string langID = TryFindLangKey(file);

                                await AddResourceDictionaryFileToLangData(rdf, langID);
                            }

                            LangData.Keys = new ObservableCollection<LangValueCollection>(LangData.Keys.OrderBy(k => k.Key));
                        }
                        catch(Exception ex)
                        {
                            await DialogCoordinator.ShowMessageAsync(this, "Fehler", ex.Message);
                        }
                    });

                return _LoadFileCommand;
            }
        }
    }
}
