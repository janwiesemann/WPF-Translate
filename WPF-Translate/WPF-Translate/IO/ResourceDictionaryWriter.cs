using de.LandauSoftware.Core;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace de.LandauSoftware.WPFTranslate.IO
{
    public static class ResourceDictionaryWriter
    {
        public static void Write(ResourceDictionaryFile rdfile)
        {
            using (MemoryStream ms = new MemoryStream())
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

        private static void WriteEmptyString(DictionaryEntry entry, XmlWriter writer)
        {
            writer.WriteStartElement("Static", ResourceDictionaryFile.xNameSpace.Source);

            WriteKey(entry.Key, writer);

            writer.WriteAttributeString("Member", ResourceDictionaryFile.MsCoreLibSystemNameSpace.Name + ":String.Empty");

            writer.WriteEndElement();
        }

        private static void WriteEntrys(ResourceDictionaryFile rdfile, XmlWriter writer)
        {
            foreach (RawDictionaryEntry rawEntry in rdfile.Entrys)
            {
                DictionaryEntry entry = rawEntry as DictionaryEntry;

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

        private static void WriteKey(string key, XmlWriter writer)
        {
            writer.WriteAttributeString("Key", ResourceDictionaryFile.xNameSpace.Source, key);
        }

        private static void WriteNamepaces(ResourceDictionaryFile rdfile, XmlWriter writer)
        {
            foreach (DictionaryNamespace item in rdfile.AllNamespces)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                    writer.WriteAttributeString("xmlns", item.Name, null, item.Source);
            }
        }

        private static void WriteStringEntry(DictionaryEntry entry, XmlWriter writer)
        {
            writer.WriteStartElement("String", ResourceDictionaryFile.xNameSpace.Source);

            WriteKey(entry.Key, writer);

            if (entry.Value.Contains(c => c == '\r' || c == '\n'))
                writer.WriteAttributeString("xml", "space", "", "preserve");

            writer.WriteRaw(entry.Value);

            writer.WriteEndElement();
        }
    }
}