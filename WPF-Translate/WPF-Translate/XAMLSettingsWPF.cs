using de.LandauSoftware.WPFTranslate.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace de.LandauSoftware.WPFTranslate
{
    internal class XAMLSettingsWPF : XAMLSettings
    {
        private DictionaryNamespace _MainNamepsace;
        private DictionaryNamespace _StaticNamepsace;
        private DictionaryNamespace _StringNamepsace;

        public override ReadOnlyCollection<DictionaryNamespace> GetNamespacesAsCollection
        {
            get
            {
                return (new List<DictionaryNamespace>
                {
                    MainNamepsace,
                    XNamespace,
                    SystemNamespace
                }).AsReadOnly();
            }
        }

        public override DictionaryNamespace MainNamepsace => _MainNamepsace ?? (_MainNamepsace = new DictionaryNamespace("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"));

        public override DictionaryNamespace SystemNamespace => _StringNamepsace ?? (_StringNamepsace = new DictionaryNamespace("sys", "clr-namespace:System;assembly=mscorlib"));

        public override DictionaryNamespace XNamespace => _StaticNamepsace ?? (_StaticNamepsace = new DictionaryNamespace("x", "http://schemas.microsoft.com/winfx/2006/xaml"));

        public override bool SupportesXmlPreserveSpace => true;
    }
}