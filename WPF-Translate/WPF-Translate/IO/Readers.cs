using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace de.LandauSoftware.WPFTranslate.IO
{
    internal static class Readers
    {
        private static ReadOnlyCollection<IResourceFileReader> _FileReaders;

        public static ReadOnlyCollection<IResourceFileReader> FileReaders
        {
            get
            {
                if (_FileReaders == null)
                {
                    List<IResourceFileReader> tmp = new List<IResourceFileReader>();
                    foreach (Type item in Assembly.GetExecutingAssembly().GetTypes())
                    {
                        if (typeof(IResourceFileReader).IsAssignableFrom(item) && !item.IsAbstract && item.GetConstructors().Contains(i => i.GetParameters().Length == 0))
                            tmp.Add((IResourceFileReader)Activator.CreateInstance(item));
                    }
                    _FileReaders = new ReadOnlyCollection<IResourceFileReader>(tmp);
                }

                return _FileReaders;
            }
        }

        public static IResourceFileReader FindFileReader(string filename)
        {
            string extension = Path.GetExtension(filename);
            extension = extension.ToUpper();

            foreach (IResourceFileReader item in FileReaders)
            {
                if (item.FileExtension.ToUpper() == extension)
                    return item;
            }

            throw new InvalidOperationException("Unsupported file type!");
        }

        public static string GetFileDialogFilter()
        {
            List<string> extension = new List<string>();
            foreach (IResourceFileReader item in FileReaders)
                extension.Add($"*.{item.FileExtension}");

            return "ResourceFiles|" + string.Join(";", extension);
        }
    }
}