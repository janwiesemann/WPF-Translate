using de.LandauSoftware.WPFTranslate.IO;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für RemoveLanguageWindow.xaml
    /// </summary>
    public partial class RemoveLanguageWindow : MetroWindow
    {
        public Language Selectedlanguage
        {
            get
            {
                if (langs.SelectedItem == null)
                    return null;

                return ((KeyValuePair<Language, ResourceDictionaryFile>)langs.SelectedItem).Key;
            }
        }

        public RemoveLanguageWindow(Dictionary<Language, ResourceDictionaryFile> fileList)
        {
            InitializeComponent();

            langs.ItemsSource = fileList;
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void langs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            yes.IsEnabled = langs.SelectedItem != null;
        }
    }
}
