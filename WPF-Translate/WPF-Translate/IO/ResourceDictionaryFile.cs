using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate.IO
{
    [DebuggerDisplay("{FileName} {Entrys.Count}")]
    public class ResourceDictionaryFile
    {
        public static readonly DictionaryNamespace EmptyNameSpace = new DictionaryNamespace("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");

        public static readonly DictionaryNamespace MsCoreLibSystemNameSpace = new DictionaryNamespace("sys", "clr-namespace:System;assembly=mscorlib");

        public static readonly DictionaryNamespace xNameSpace = new DictionaryNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml");

        private List<DictionaryNamespace> _AdditionalNamespaces = new List<DictionaryNamespace>();

        private List<RawDictionaryEntry> _Entrys = new List<RawDictionaryEntry>();

        private string _FileName;

        private List<MergedDictionary> _MergedDictionaries = new List<MergedDictionary>();

        public ResourceDictionaryFile(string fileName)
        {
            _FileName = fileName;
        }

        public static DictionaryNamespace[] DefaultNameSpaces
        {
            get
            {
                return new DictionaryNamespace[] { EmptyNameSpace, xNameSpace };
            }
        }

        public List<DictionaryNamespace> AdditionalNamespaces
        {
            get
            {
                return _AdditionalNamespaces;
            }
        }

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

        public List<RawDictionaryEntry> Entrys
        {
            get
            {
                return _Entrys;
            }
        }

        public string FileName
        {
            get
            {
                return _FileName;
            }
        }

        public List<MergedDictionary> MergedDictionaries
        {
            get
            {
                return _MergedDictionaries;
            }
        }

        public void RemoveAllStringRessoueces()
        {
            for (int i = Entrys.Count - 1; i >= 0; i--)
            {
                if (Entrys[i] is DictionaryEntry)
                    Entrys.RemoveAt(i);
            }
        }
    }
}