using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace de.LandauSoftware.WPFTranslate.IO
{
    /// <summary>
    /// eine Klasse zum lesen von XAML-RessourceDictionary dateien.
    /// </summary>
    public class ResourceDictionaryReader : IResourceFileReader
    {
        public const string LangKeyKey = "languageKey";
        public string FileExtension => "xaml";

        public string GetLanguageKey(ResourceDictionaryFile file)
        {
            string key = TryGetLangKeyViaFilename(file);
            if (key != null)
                return key;

            key = TryGetLangKeyViaContent(file);
            if (key != null)
                return key;

            return null;
        }

        public IResourceFileWriter GetWriter()
        {
            return new ResourceDictionaryWriter();
        }

        /// <summary>
        /// Ließt eine Datei in ein ResourceDictionaryFile
        /// </summary>
        /// <param name="file">Dateipfad</param>
        /// <returns></returns>
        public ResourceDictionaryFile Read(string file)
        {
            ResourceDictionaryFile rdfile = new ResourceDictionaryFile(file, this);

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            if (doc.DocumentElement.Name != "ResourceDictionary")
                throw new XmlException("XML-Root is unknown!");

            ParseAdditionalNameSpaces(rdfile, doc.DocumentElement);

            ParseXClass(rdfile, doc.DocumentElement);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes) //einlesen aller Elemente
            {
                if (!(node is XmlElement elem))
                    continue;

                switch (node.LocalName)
                {
                    case "ResourceDictionary.MergedDictionaries":
                        ParseMergedDictonariesElement(rdfile, elem);
                        break;

                    case "String":
                        ParseStringElement(rdfile, elem);
                        break;

                    case "Static":
                        ParseStaticElement(rdfile, elem);
                        break;

                    default:
                        ParseUnknowElement(rdfile, elem);
                        break;
                }
            }

            return rdfile;
        }

        /// <summary>
        /// Ruft den x:Key eines String-Knotens ab.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static string GetKey(XmlElement node)
        {
            string key = node.Attributes["x:Key"]?.Value;

            if (key == null)
                throw new XmlException("Key must be prenset!");

            return key;
        }

        /// <summary>
        /// Ließt ggf. zusätzliche Namespaces ein und fügt diese in die Datei hinzu
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="docElement">Xml-Root-Element</param>
        private static void ParseAdditionalNameSpaces(ResourceDictionaryFile rdfile, XmlElement docElement)
        {
            foreach (XmlAttribute item in docElement.Attributes)
            {
                if (!item.Name.StartsWith("xmlns"))
                    continue;

                string name = item.Name.Substring(item.Name.Contains(":") ? 6 : 5);
                string source = item.Value;

                if (string.IsNullOrEmpty(name))
                {
                    rdfile.Settings = XAMLSettings.TryFind(source);

                    continue;
                }

                rdfile.Namespaces.Add(new DictionaryNamespace(name, source));
            }

            if (rdfile.Settings == null)
                throw new Exception("Default Namesapces nicht gefunden!");

            foreach (DictionaryNamespace item in rdfile.Settings.GetNamespacesAsCollection)
            {
                while (true)
                {
                    DictionaryNamespace duplicate = rdfile.Namespaces.Find(i => i.Name == item.Name && i.Source == item.Source);

                    if (duplicate != null)
                        rdfile.Namespaces.Remove(duplicate);
                    else
                        break;
                }
            }
        }

        /// <summary>
        /// Ließt einen MergedDictionaries Knoten ein
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="node">Knoten</param>
        private static void ParseMergedDictonariesElement(ResourceDictionaryFile rdfile, XmlElement node)
        {
            foreach (XmlElement item in node.ChildNodes)
            {
                if (item.Name != "ResourceDictionary")
                    throw new XmlException("MergedDictionaries unknown type found!");

                string source = item.Attributes["Source"]?.Value ?? null;

                if (source == null)
                    throw new XmlException("ResourceDictionary Source can not be null");

                rdfile.MergedDictionaries.Add(new MergedDictionary(source));
            }
        }

        /// <summary>
        /// Ließt einen leeres String-Element ein.
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="node">Knoten</param>
        private static void ParseStaticElement(ResourceDictionaryFile rdfile, XmlElement node)
        {
            string key = GetKey(node);

            bool memberIsEmptyString = node.Attributes["Member"]?.Value == "sys:String.Empty";

            if (memberIsEmptyString)
                rdfile.Entrys.Add(new DictionaryStringEntry(key, string.Empty));
            else
                rdfile.Entrys.Add(new DictionaryRawEntry(node.OuterXml));
        }

        /// <summary>
        /// Ließt ein String-Element ein
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="node">Knoten</param>
        private static void ParseStringElement(ResourceDictionaryFile rdfile, XmlElement node)
        {
            string key = GetKey(node);

            rdfile.Entrys.Add(new DictionaryStringEntry(key, node.InnerText));
        }

        /// <summary>
        /// Ließt ein unbekanntes Element ein
        /// </summary>
        /// <param name="rdfile">ResourceDictionaryFile</param>
        /// <param name="node">Knoten</param>
        private static void ParseUnknowElement(ResourceDictionaryFile rdfile, XmlElement node)
        {
            Regex regex = new Regex(@"\s+xmlns\s*(:\w+)?\s*=\s*\""([^\""]*)\""", RegexOptions.IgnoreCase);

            string xml = regex.Replace(node.OuterXml, ""); //entfernt alle lokalen namespaces welche von dem XML-Reader hinzugefügt wurden

            rdfile.Entrys.Add(new DictionaryRawEntry(xml));
        }

        /// <summary>
        /// Ließt den X:Class Eintrag aus dem rootElement. Dieses wird bei Xamarin.Forms benötigt.
        /// </summary>
        /// <param name="rdfile"></param>
        /// <param name="documentElement"></param>
        private static void ParseXClass(ResourceDictionaryFile rdfile, XmlElement documentElement)
        {
            foreach (XmlAttribute item in documentElement.Attributes)
            {
                if (item.Name == "x:Class")
                    rdfile.XClass = item.Value;
            }
        }

        private static string TryGetLangKeyViaFilename(ResourceDictionaryFile file)
        {
            string filename = Path.GetFileName(file.FileName);
            Match match = Regex.Match(filename, @"(\w{2}-\w{2})");

            return match.Success ? match.Groups[1].Value : null;
        }

        private string TryGetLangKeyViaContent(ResourceDictionaryFile file)
        {
            foreach (DictionaryRawEntry item in file.Entrys)
            {
                if (item is DictionaryStringEntry dsi && dsi.Key == LangKeyKey)
                    return item.Value;
            }

            return null;
        }
    }
}