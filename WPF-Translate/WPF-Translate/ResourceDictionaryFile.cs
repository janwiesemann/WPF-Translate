using System.Collections.Generic;
using System.Diagnostics;

namespace de.LandauSoftware.WPFTranslate
{
    [DebuggerDisplay("{FileName} {Entrys.Count}")]
    public class ResourceDictionaryFile
    {
        private List<RawDictionaryEntry> _Entrys = new List<RawDictionaryEntry>();
        private string _FileName;
        private List<MergedDictionary> _MergedDictionarys = new List<MergedDictionary>();

        public ResourceDictionaryFile(string fileName)
        {
            _FileName = fileName;
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