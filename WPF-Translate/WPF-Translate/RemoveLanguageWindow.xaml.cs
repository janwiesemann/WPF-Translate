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
        public RemoveLanguageWindow(Dictionary<Language, ResourceDictionaryFile> fileList)
        {
            InitializeComponent();

            langs.ItemsSource = fileList;
        }

        public Language Selectedlanguage
        {
            get
            {
                if (langs.SelectedItem == null)
                    return null;

                return ((KeyValuePair<Language, ResourceDictionaryFile>)langs.SelectedItem).Key;
            }
        }

        private void langs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            yes.IsEnabled = langs.SelectedItem != null;
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}