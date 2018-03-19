using System.Collections.Generic;
using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate
{
    [DebuggerDisplay("{FileName} {Entrys.Count}")]
    public class ResourceDictionaryFile
    {
        public const string MsCoreLibSystemNameSpace = "clr-namespace:System;assembly=mscorlib";

        public static readonly DictionaryNamespace[] DefaultNameSpaces = new DictionaryNamespace[]
                {
            new DictionaryNamespace("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"),
            new DictionaryNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml")
        };

        private List<DictionaryNamespace> _AdditionalNamespaces = new List<DictionaryNamespace>();
        private List<RawDictionaryEntry> _Entrys = new List<RawDictionaryEntry>();
        private string _FileName;
        private List<MergedDictionary> _MergedDictionarys = new List<MergedDictionary>();

        public ResourceDictionaryFile(string fileName)
        {
            _FileName = fileName;
        }

        public List<DictionaryNamespace> AdditionalNamespaces
        {
            get
            {
                return _AdditionalNamespaces;
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

        public List<MergedDictionary> MergedDictionarys
        {
            get
            {
                return _MergedDictionarys;
            }
        }
    }
}