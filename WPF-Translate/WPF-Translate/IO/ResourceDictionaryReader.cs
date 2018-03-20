using de.LandauSoftware.Core;
using System.Text.RegularExpressions;
using System.Xml;

namespace de.LandauSoftware.WPFTranslate.IO
{
    public static class ResourceDictionaryReader
    {
        public static ResourceDictionaryFile Read(string file)
        {
            ResourceDictionaryFile rdfile = new ResourceDictionaryFile(file);

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            if (doc.DocumentElement.Name != "ResourceDictionary")
                throw new XmlException("XML-Root is unknown!");

            ParseAdditionalNameSpaces(rdfile, doc.DocumentElement);

            foreach (XmlElement node in doc.DocumentElement.ChildNodes)
            {
                switch (node.LocalName)
                {
                    case "ResourceDictionary.MergedDictionaries":
                        ParseMergedDictonaries(rdfile, node);
                        break;

                    case "String":
                        ParseString(rdfile, node);
                        break;

                    case "Static":
                        ParseStatic(rdfile, node);
                        break;

                    default:
                        ParseUnknow(rdfile, node);
                        break;
                }
            }

            return rdfile;
        }

        private static string GetKey(XmlElement node)
        {
            string key = node.Attributes["x:Key"]?.Value;

            if (key == null)
                throw new XmlException("Key must be prenset!");

            return key;
        }

        private static void ParseAdditionalNameSpaces(ResourceDictionaryFile rdfile, XmlElement docElement)
        {
            foreach (XmlAttribute item in docElement.Attributes)
            {
                if (!item.Name.StartsWith("xmlns"))
                    continue;

                string name = item.Name.Substring(item.Name.Contains(":") ? 6 : 5);
                string source = item.Value;

                if (!ResourceDictionaryFile.DefaultNameSpaces.Contains(ns => (ns.Name == name && ns.Source == source) || source == ResourceDictionaryFile.MsCoreLibSystemNameSpace.Source))
                    rdfile.AdditionalNamespaces.Add(new DictionaryNamespace(name, source));
            }
        }

        private static void ParseMergedDictonaries(ResourceDictionaryFile rdfile, XmlElement node)
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

        private static void ParseStatic(ResourceDictionaryFile rdfile, XmlElement node)
        {
            string key = GetKey(node);

            bool memberIsEmptyString = node.Attributes["Member"]?.Value == "sys:String.Empty";

            if (memberIsEmptyString)
                rdfile.Entrys.Add(new DictionaryEntry(key, string.Empty));
            else
                rdfile.Entrys.Add(new RawDictionaryEntry(node.OuterXml));
        }

        private static void ParseString(ResourceDictionaryFile rdfile, XmlElement node)
        {
            string key = GetKey(node);

            rdfile.Entrys.Add(new DictionaryEntry(key, node.InnerText));
        }

        private static void ParseUnknow(ResourceDictionaryFile rdfile, XmlElement node)
        {
            Regex regex = new Regex(@"\s+xmlns\s*(:\w+)?\s*=\s*\""([^\""]*)\""", RegexOptions.IgnoreCase);

            MatchCollection mc = regex.Matches(node.OuterXml);

            string xml = regex.Replace(node.OuterXml, "");

            rdfile.Entrys.Add(new RawDictionaryEntry(xml));
        }
    }
}