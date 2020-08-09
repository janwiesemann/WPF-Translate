using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Eine Klasse zum schreiben von ResourceDictionaryFiles
    /// </summary>
    public class ResourceDictionaryWriter : IResourceFileWriter
    {
        /// <summary>
        /// Schreibt eine Datei auf die Festplatte
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        public void Write(ResourceDictionaryFile rdfile)
        {
            using (MemoryStream ms = new MemoryStream()) //Zwischenspeicher zum formattieren
            {
                using (XmlWriter writer = XmlWriter.Create(ms))
                {
                    writer.WriteStartElement("ResourceDictionary", rdfile.Settings.MainNamepsace.Source);
                    {
                        WriteNamepaces(rdfile.Settings.GetNamespacesAsCollection, writer);
                        WriteNamepaces(rdfile.Namespaces, writer);

                        WriteXClass(rdfile, writer);

                        WriteAdditionalResourceDictionaries(rdfile, writer);

                        WriteEntrys(rdfile, writer);
                    }
                    writer.WriteEndElement();
                }

                FormatAndWriteXmToFile(ms, rdfile.FileName);
            }
        }

        /// <summary>
        /// Ließt XML aus einem Stream und speichert diese Daten anschließend formatiert in einer Datei
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="file">Dateipfad</param>
        private static void FormatAndWriteXmToFile(Stream stream, string file)
        {
            stream.Position = 0;

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            XmlWriterSettings setting = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                NewLineHandling = NewLineHandling.Replace,
                NewLineChars = Environment.NewLine
            };

            using (XmlWriter writer = XmlWriter.Create(file, setting))
            {
                doc.WriteTo(writer);
            }
        }

        /// <summary>
        /// Schreibt alle zusätzlichen Ressourcen Wörterbücher in die Datei
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteAdditionalResourceDictionaries(ResourceDictionaryFile rdfile, XmlWriter writer)
        {
            if (rdfile.MergedDictionaries.Count <= 0)
                return;

            writer.WriteStartElement("ResourceDictionary.MergedDictionaries");
            foreach (MergedDictionary item in rdfile.MergedDictionaries)
            {
                writer.WriteStartElement("ResourceDictionary");
                writer.WriteAttributeString("Source", item.Source);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Schreibt einen leeren String in die Datei
        /// </summary>
        /// <param name="rdfile"></param>
        /// <param name="entry">DictionaryStringEntry</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteEmptyString(ResourceDictionaryFile rdfile, DictionaryStringEntry entry, XmlWriter writer)
        {
            writer.WriteStartElement("Static", rdfile.Settings.XNamespace.Source);

            WriteKey(rdfile, entry.Key, writer);

            writer.WriteAttributeString("Member", rdfile.Settings.SystemNamespace.Name + ":String.Empty");

            writer.WriteEndElement();
        }

        /// <summary>
        /// Schreibt alle Einträge in die Datei
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteEntrys(ResourceDictionaryFile rdfile, XmlWriter writer)
        {
            foreach (DictionaryRawEntry rawEntry in rdfile.Entrys)
            {
                if (rawEntry is DictionaryStringEntry entry)
                {
                    if (string.IsNullOrWhiteSpace(entry.Value))
                        WriteEmptyString(rdfile, entry, writer);
                    else
                        WriteStringEntry(rdfile, entry, writer);
                }
                else
                    writer.WriteRaw(rawEntry.Value + Environment.NewLine);
            }
        }

        /// <summary>
        /// Schreibt einen x:Key in die Datei
        /// </summary>
        /// <param name="rdfile"></param>
        /// <param name="key">Key</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteKey(ResourceDictionaryFile rdfile, string key, XmlWriter writer)
        {
            writer.WriteAttributeString("Key", rdfile.Settings.XNamespace.Source, key);
        }

        /// <summary>
        /// Schreibt die Namespaces in die Datei
        /// </summary>
        /// <param name="dics">ResourceDictionaryFile</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteNamepaces(IEnumerable<DictionaryNamespace> dics, XmlWriter writer)
        {
            foreach (DictionaryNamespace item in dics)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                    writer.WriteAttributeString("xmlns", item.Name, null, item.Source);
            }
        }

        /// <summary>
        /// Schreibt einen String-Eintrag in die datei
        /// </summary>
        /// <param name="rdfile"></param>
        /// <param name="entry">DictionaryStringEntry</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteStringEntry(ResourceDictionaryFile rdfile, DictionaryStringEntry entry, XmlWriter writer)
        {
            writer.WriteStartElement("String", rdfile.Settings.SystemNamespace.Source);

            WriteKey(rdfile, entry.Key, writer);

            if (rdfile.Settings.SupportesXmlPreserveSpace && entry.Value.ContainsChar('\r', '\n'))
                writer.WriteAttributeString("xml", "space", "", "preserve");

            writer.WriteString(entry.Value);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Schreibt das X.Class Attribut in dem Root element
        /// </summary>
        /// <param name="rdfile"></param>
        /// <param name="writer"></param>
        private static void WriteXClass(ResourceDictionaryFile rdfile, XmlWriter writer)
        {
            if (string.IsNullOrWhiteSpace(rdfile.XClass))
                return;

            writer.WriteAttributeString("Class", rdfile.Settings.XNamespace.Source, rdfile.XClass);
        }
    }
}