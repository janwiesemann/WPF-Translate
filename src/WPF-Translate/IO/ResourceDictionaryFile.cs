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
        /// Erstellt eine neue Instanz eines ResourceDictionaryFiles
        /// </summary>
        /// <param name="fileName">Dateiname</param>
        public ResourceDictionaryFile(string fileName, IResourceFileReader reader)
        {
            FileName = fileName;
            Reader = reader;
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
        /// Eine Sammlung aller zusätzlichen Namespaces
        /// </summary>
        public List<DictionaryNamespace> Namespaces { get; } = new List<DictionaryNamespace>();

        public IResourceFileReader Reader { get; }

        /// <summary>
        /// Settings für die Datei
        /// </summary>
        public XAMLSettings Settings { get; set; }

        /// <summary>
        /// X:Class
        /// </summary>
        public string XClass { get; set; }

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