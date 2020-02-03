using de.LandauSoftware.WPFTranslate.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Standartnamespaces
    /// </summary>
    public abstract class DefaultNamespacesBase
    {
        private static ReadOnlyCollection<DefaultNamespacesBase> _Defaults;

        /// <summary>
        /// Ruft die Standart NSses ab
        /// </summary>
        public static ReadOnlyCollection<DefaultNamespacesBase> Defaults
        {
            get
            {
                if (_Defaults == null)
                {
                    List<DefaultNamespacesBase> tmp = new List<DefaultNamespacesBase>();
                    foreach (Type item in typeof(DefaultNamespacesBase).Assembly.GetTypes())
                    {
                        if (typeof(DefaultNamespacesBase).IsAssignableFrom(item) && !item.IsAbstract)
                            tmp.Add((DefaultNamespacesBase)Activator.CreateInstance(item));
                    }

                    _Defaults = tmp.AsReadOnly();
                }

                return _Defaults;
            }
        }

        /// <summary>
        /// Ruft die Liste als Collection ab
        /// </summary>
        public abstract ReadOnlyCollection<DictionaryNamespace> GetAsCollection { get; }

        /// <summary>
        /// Main NS
        /// </summary>
        public abstract DictionaryNamespace MainNamepsace { get; }

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
        public static DefaultNamespacesBase TryFind(string mainNSSource)
        {
            foreach (DefaultNamespacesBase item in Defaults)
            {
                if (item.MainNamepsace.Source == mainNSSource)
                    return item;
            }

            return null;
        }
    }
}