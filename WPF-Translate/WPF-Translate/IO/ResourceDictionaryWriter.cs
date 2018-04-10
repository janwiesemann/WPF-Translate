using de.LandauSoftware.Core;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// Eine Klasse zum schreiben von ResourceDictionaryFiles
    /// </summary>
    public static class ResourceDictionaryWriter
    {
        /// <summary>
        /// Schreibt eine Datei auf die Festplatte
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        public static void Write(ResourceDictionaryFile rdfile)
        {
            using (MemoryStream ms = new MemoryStream()) //Zwischenspeicher zum formattieren
            {
                using (XmlWriter writer = XmlWriter.Create(ms))
                {
                    writer.WriteStartElement("ResourceDictionary", ResourceDictionaryFile.EmptyNameSpace.Source);
                    {
                        WriteNamepaces(rdfile, writer);

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
        /// <param name="entry">DictionaryStringEntry</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteEmptyString(DictionaryStringEntry entry, XmlWriter writer)
        {
            writer.WriteStartElement("Static", ResourceDictionaryFile.xNameSpace.Source);

            WriteKey(entry.Key, writer);

            writer.WriteAttributeString("Member", ResourceDictionaryFile.MsCoreLibSystemNameSpace.Name + ":String.Empty");

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
                DictionaryStringEntry entry = rawEntry as DictionaryStringEntry;

                if (entry != null)
                {
                    if (string.IsNullOrWhiteSpace(entry.Value))
                        WriteEmptyString(entry, writer);
                    else
                        WriteStringEntry(entry, writer);
                }
                else
                    writer.WriteRaw(rawEntry.Value + Environment.NewLine);
            }
        }

        /// <summary>
        /// Schreibt einen x:Key in die Datei
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteKey(string key, XmlWriter writer)
        {
            writer.WriteAttributeString("Key", ResourceDictionaryFile.xNameSpace.Source, key);
        }

        /// <summary>
        /// Schreibt die Namespaces in die Datei
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteNamepaces(ResourceDictionaryFile rdfile, XmlWriter writer)
        {
            foreach (DictionaryNamespace item in rdfile.AllNamespces)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                    writer.WriteAttributeString("xmlns", item.Name, null, item.Source);
            }
        }

        /// <summary>
        /// Schreibt einen String-Eintrag in die datei
        /// </summary>
        /// <param name="entry">DictionaryStringEntry</param>
        /// <param name="writer">XmlWriter</param>
        private static void WriteStringEntry(DictionaryStringEntry entry, XmlWriter writer)
        {
            writer.WriteStartElement("String", ResourceDictionaryFile.xNameSpace.Source);

            WriteKey(entry.Key, writer);

            if (entry.Value.Contains(c => c == '\r' || c == '\n'))
                writer.WriteAttributeString("xml", "space", "", "preserve");

            writer.WriteString(entry.Value);

            writer.WriteEndElement();
        }
    }
}