using de.LandauSoftware.WPFTranslate.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Standartnamespaces
    /// </summary>
    public abstract class XAMLSettings
    {
        private static ReadOnlyCollection<XAMLSettings> _Defaults;

        /// <summary>
        /// Ruft die Standart NSses ab
        /// </summary>
        public static ReadOnlyCollection<XAMLSettings> Defaults
        {
            get
            {
                if (_Defaults == null)
                {
                    List<XAMLSettings> tmp = new List<XAMLSettings>();
                    foreach (Type item in typeof(XAMLSettings).Assembly.GetTypes())
                    {
                        if (typeof(XAMLSettings).IsAssignableFrom(item) && !item.IsAbstract)
                            tmp.Add((XAMLSettings)Activator.CreateInstance(item));
                    }

                    _Defaults = tmp.AsReadOnly();
                }

                return _Defaults;
            }
        }

        /// <summary>
        /// Ruft die Liste als Collection ab
        /// </summary>
        public abstract ReadOnlyCollection<DictionaryNamespace> GetNamespacesAsCollection { get; }

        /// <summary>
        /// Main NS
        /// </summary>
        public abstract DictionaryNamespace MainNamepsace { get; }

        /// <summary>
        /// Prüft, ob xml:space="preserve" unterstützt wird.
        /// </summary>
        public abstract bool SupportesXmlPreserveSpace { get; }

        /// <summary>
        /// String NS
        /// </summary>
        public abstract DictionaryNamespace SystemNamespace { get; }

        /// <summary>
        /// Static NS
        /// </summary>
        public abstract DictionaryNamespace XNamespace { get; }

        /// <summary>
        /// Sucht nach einem NS
        /// </summary>
        /// <param name="mainNSSource"></param>
        /// <returns></returns>
        public static XAMLSettings TryFind(string mainNSSource)
        {
            foreach (XAMLSettings item in Defaults)
            {
                if (item.MainNamepsace.Source == mainNSSource)
                    return item;
            }

            return null;
        }
    }
}