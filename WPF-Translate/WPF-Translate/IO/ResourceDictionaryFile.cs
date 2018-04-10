using System.Collections.Generic;
using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Stellt ein WPF-Ressource-File dar.
    /// </summary>
    [DebuggerDisplay("{FileName} {Entrys.Count}")]
    public class ResourceDictionaryFile
    {
        /// <summary>
        /// Leerer namespace (Standartdefinition)
        /// </summary>
        public static readonly DictionaryNamespace EmptyNameSpace = new DictionaryNamespace("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

        /// <summary>
        /// Namespace für MSCoreLib
        /// </summary>
        public static readonly DictionaryNamespace MsCoreLibSystemNameSpace = new DictionaryNamespace("sys", "clr-namespace:System;assembly=mscorlib");

        /// <summary>
        /// Namespace für winFX definitionen
        /// </summary>
        public static readonly DictionaryNamespace xNameSpace = new DictionaryNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");

        private List<DictionaryNamespace> _AdditionalNamespaces = new List<DictionaryNamespace>();

        private List<DictionaryRawEntry> _Entrys = new List<DictionaryRawEntry>();

        private string _FileName;

        private List<MergedDictionary> _MergedDictionaries = new List<MergedDictionary>();

        /// <summary>
        /// Erstellt eine neue Instanz eines ResourceDictionaryFiles
        /// </summary>
        /// <param name="fileName">Dateiname</param>
        public ResourceDictionaryFile(string fileName)
        {
            _FileName = fileName;
        }

        /// <summary>
        /// Eine Sammlung aller Standart-Namespaces
        /// </summary>
        public static DictionaryNamespace[] DefaultNameSpaces
        {
            get
            {
                return new DictionaryNamespace[] { EmptyNameSpace, xNameSpace };
            }
        }

        /// <summary>
        /// Eine Sammlung aller zusätzlichen Namespaces
        /// </summary>
        public List<DictionaryNamespace> AdditionalNamespaces
        {
            get
            {
                return _AdditionalNamespaces;
            }
        }

        /// <summary>
        /// Ruft eine Liste aller Namespaces ab. Zusamengesetzt aus AdditionalNamespaces und DefaultNameSpaces
        /// </summary>
        public List<DictionaryNamespace> AllNamespces
        {
            get
            {
                List<DictionaryNamespace> ret = new List<DictionaryNamespace>();
                ret.AddRange(DefaultNameSpaces);
                ret.AddRange(AdditionalNamespaces);
                ret.Add(MsCoreLibSystemNameSpace);

                return ret;
            }
        }

        /// <summary>
        /// Ruft eine Liste aller Einträge ab
        /// </summary>
        public List<DictionaryRawEntry> Entrys
        {
            get
            {
                return _Entrys;
            }
        }

        /// <summary>
        /// Ruft den Dateinamen ab
        /// </summary>
        public string FileName
        {
            get
            {
                return _FileName;
            }
        }

        /// <summary>
        /// Ruft alle MergedDictionaries ab
        /// </summary>
        public List<MergedDictionary> MergedDictionaries
        {
            get
            {
                return _MergedDictionaries;
            }
        }

        /// <summary>
        /// Entfernt alle String-Ressourcen (DictionaryStringEntry) aus den Einträgen
        /// </summary>
        public void RemoveAllStringRessoueces()
        {
            for (int i = Entrys.Count - 1; i >= 0; i--)
            {
                if (Entrys[i] is DictionaryStringEntry)
                    Entrys.RemoveAt(i);
            }
        }
    }
}