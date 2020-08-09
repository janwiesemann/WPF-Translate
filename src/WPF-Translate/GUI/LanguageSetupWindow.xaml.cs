using de.LandauSoftware.WPFTranslate.IO;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für LanguageSetupWindow.xaml
    /// </summary>
    public partial class LanguageSetupWindow : MetroWindow
    {
        /// <summary>
        /// Erstellt ein neues LanguageSetupWindow
        /// </summary>
        public LanguageSetupWindow()
        {
            InitializeComponent();

            readerComboBox.ItemsSource = Readers.FileReaders;
            readerComboBox.SelectedIndex = Readers.FileReaders.Count == 0 ? -1 : 0;
        }

        /// <summary>
        /// Dateiname
        /// </summary>
        public string FileName => filePath.Text;

        /// <summary>
        /// Sprach ID
        /// </summary>
        public string LangID => sprachKey.Text;

        public IResourceFileReader Reader => readerComboBox.SelectedItem as IResourceFileReader;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChangeFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "File|*." + Reader.FileExtension
            };

            if (sfd.ShowDialog() == true)
                filePath.Text = sfd.FileName;
        }

        private void FilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputAndEnableOk();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ReaderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VerifyInputAndEnableOk();
        }

        private void SprachKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputAndEnableOk();
        }

        private void VerifyInputAndEnableOk()
        {
            changeFile.IsEnabled = Reader != null;

            ok.IsEnabled = Reader != null && !string.IsNullOrWhiteSpace(sprachKey.Text) && !string.IsNullOrWhiteSpace(filePath.Text);
        }
    }
}