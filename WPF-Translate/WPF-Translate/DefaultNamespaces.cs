using de.LandauSoftware.WPFTranslate.IO;
using System.Collections.ObjectModel;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Standartnamespaces
    /// </summary>
    public class DefaultNamespaces : Collection<DictionaryNamespace>
    {
        private static ReadOnlyCollection<DefaultNamespaces> _Defaults;

        /// <summary>
        /// Erstellt eine neue Instanz der DefaultNamespaces
        /// </summary>
        /// <param name="mainNamespace"></param>
        /// <param name="xNamespace"></param>
        /// <param name="sysNamesoace"></param>
        public DefaultNamespaces(string mainNamespace, string xNamespace, string sysNamesoace)
        {
            MainNamespace = new DictionaryNamespace("", mainNamespace);
            XNamespace = new DictionaryNamespace("x", xNamespace);
            SysNamespace = new DictionaryNamespace("sys", sysNamesoace);

            this.Add(MainNamespace);
            this.Add(XNamespace);
            this.Add(SysNamespace);
        }

        /// <summary>
        /// Standart namespaces
        /// </summary>
        public static ReadOnlyCollection<DefaultNamespaces> Defaults
        {
            get
            {
                if (_Defaults == null)
                    _Defaults = new ReadOnlyCollection<DefaultNamespaces>(new DefaultNamespaces[]
                    {
                        new DefaultNamespaces
                        (
                             "http://schemas.microsoft.com/winfx/2006/xaml/presentation",
                             "http://schemas.microsoft.com/winfx/2006/xaml",
                             "clr-namespace:System;assembly=mscorlib"
                        ),
                        new DefaultNamespaces
                        (
                            "http://xamarin.com/schemas/2014/forms",
                            "http://schemas.microsoft.com/winfx/2009/xaml",
                            "clr-namespace:System;assembly=netstandard"
                        )
                    });

                return _Defaults;
            }
        }

        /// <summary>
        /// Main NS
        /// </summary>
        public DictionaryNamespace MainNamespace { get; }
        /// <summary>
        /// Sys NS
        /// </summary>
        public DictionaryNamespace SysNamespace { get; }
        /// <summary>
        /// X NS
        /// </summary>
        public DictionaryNamespace XNamespace { get; }

        /// <summary>
        /// Sucht nach einem NS
        /// </summary>
        /// <param name="mainNamespace"></param>
        /// <returns></returns>
        public static DefaultNamespaces TryFind(string mainNamespace)
        {
            foreach (DefaultNamespaces item in Defaults)
            {
                if (item.MainNamespace.Source == mainNamespace)
                    return item;
            }

            return null;
        }
    }
}