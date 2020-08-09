using de.LandauSoftware.WPFTranslate.IO;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für RemoveLanguageWindow.xaml
    /// </summary>
    public partial class RemoveLanguageWindow : MetroWindow
    {
        /// <summary>
        /// Erstellt ein neues Fenster zurm entfernen einer Sprache
        /// </summary>
        /// <param name="fileList"></param>
        public RemoveLanguageWindow(Dictionary<Language, ResourceDictionaryFile> fileList)
        {
            InitializeComponent();

            langs.ItemsSource = fileList;
        }

        /// <summary>
        /// Ausgewählte Sprache
        /// </summary>
        public Language Selectedlanguage
        {
            get
            {
                if (langs.SelectedItem == null)
                    return null;

                return ((KeyValuePair<Language, ResourceDictionaryFile>)langs.SelectedItem).Key;
            }
        }

        private void Langs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            yes.IsEnabled = langs.SelectedItem != null;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}