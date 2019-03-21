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

        /// <summary>
        /// Erstellt eine neue Instanz eines ResourceDictionaryFiles
        /// </summary>
        /// <param name="fileName">Dateiname</param>
        public ResourceDictionaryFile(string fileName)
        {
            FileName = fileName;
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
        public List<DictionaryNamespace> AdditionalNamespaces { get; } = new List<DictionaryNamespace>();

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
        public List<DictionaryRawEntry> Entrys { get; } = new List<DictionaryRawEntry>();

        /// <summary>
        /// Ruft den Dateinamen ab
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Ruft alle MergedDictionaries ab
        /// </summary>
        public List<MergedDictionary> MergedDictionaries { get; } = new List<MergedDictionary>();

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