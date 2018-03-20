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

        public event EventHandler LanguageCollectionChangedEvent;

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
                    _LangData.LanguagesChangedEvent += (s, e) => LanguageCollectionChangedEvent?.Invoke(s, e);
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

        public ICommand ClearCommand
        {
            get
            {
                if (_ClearCommand == null)
                    _ClearCommand = new RelayICommand(async p => {
                        MessageDialogResult res = await DialogCoordinator.ShowMessageAsync(this, "Löschen", "Es werden alle Einträge gelöscht. Soll die Aktion wirklich ausgeführt werden?", MessageDialogStyle.AffirmativeAndNegative);

                        if(res == MessageDialogResult.Affirmative)
                        {
                            FileList.Clear();
                            LangData = null;
                        }
                    });

                return _ClearCommand;
            }
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
                            ofd.Filter = "XAML-Datei|*.xaml";

                            if (ofd.ShowDialog() != true)
                                return;

                            if (FileListContainsFile(ofd.FileName))
                                throw new InvalidOperationException("Die Datei wurde bereits geladen!");

                            ResourceDictionaryFile rdf = ResourceDictionaryReader.Read(ofd.FileName);

                            string langID = TryFindLangKey(rdf.FileName);

                            while (langID == null || LangData.ContainsLanguage(langID))
                            {
                                langID = await DialogCoordinator.ShowInputAsync(this, "Sprache vorhanden!", "Die Sprache konnte nicht Festgelegt werden! Möglicherweise ist diese bereits vorhanden oder der SprachKey konnte nicht gefunden werden.");

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

                            FileList.Add(LangData.FindLang(langID), rdf);
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
