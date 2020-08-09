using System;
using System.Globalization;
using System.Windows;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public static string FindString(string key)
        {
            return App.Current.FindResource(key).ToString();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            LoadCultureStrings();

            base.OnStartup(e);
        }

        private void LoadCultureStrings()
        {
            CultureInfo current = CultureInfo.CurrentCulture;
            string name = current.Name;

            int i = name.IndexOf('-');
            if (i > 0)
                name = name.Substring(0, i);

            if (name == "en") //default and its already loaded
                return;

            try
            {
                ResourceDictionary rd = new ResourceDictionary()
                {
                    Source = new Uri($"pack://application:,,,/WPF-Translate;component/Resources/Langs/Strings_{name}.xaml", UriKind.RelativeOrAbsolute)
                };

                Resources.MergedDictionaries.Add(rd);
            }
            catch (Exception) //not found
            { }
        }
    }
}