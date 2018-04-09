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
        public LanguageSetupWindow()
        {
            InitializeComponent();
        }

        public string FileName
        {
            get
            {
                return filePath.Text;
            }
            set
            {
                filePath.Text = value;
            }
        }

        public string LangID
        {
            get
            {
                return sprachKey.Text;
            }
            set
            {
                sprachKey.Text = value;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void changeFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XAML-Datei|*.xaml";
            sfd.FileName = "StringResource_" + sprachKey.Text + ".xaml";

            if (sfd.ShowDialog() == true)
                filePath.Text = sfd.FileName;
        }

        private void filePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputAndEnableOk();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void sprachKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifyInputAndEnableOk();
        }

        private void VerifyInputAndEnableOk()
        {
            ok.IsEnabled = !string.IsNullOrWhiteSpace(sprachKey.Text) && !string.IsNullOrWhiteSpace(filePath.Text);
        }
    }
}