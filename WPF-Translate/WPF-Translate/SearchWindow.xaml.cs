using de.LandauSoftware.Core.WPF;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für SeachWindow.xaml
    /// </summary>
    public partial class SearchWindow : MetroWindow
    {
        private SaveWindowStates saveWindowStates;

        public SearchWindow()
        {
            InitializeComponent();

            saveWindowStates = WindowHelper.GetSaveWindowStatesStorage(this);
            saveWindowStates.AdditionalDataLoaded += SaveWindowStates_AdditionalDataLoaded;
            saveWindowStates.PreviewAdditionalDataSaved += SaveWindowStates_PreviewAdditionalDataSaved;
        }

        public SearchModule CreateSearchModule()
        {
            return new SearchModule((bool)searchKey.IsChecked, (bool)searchValue.IsChecked, searchText.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void checkBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            VerifySearchEnabledAnEnable();
        }

        private void SaveWindowStates_AdditionalDataLoaded(object sender, object e)
        {
            if (e == null || !(e is SaveWindowStatesAdditionalData))
                return;

            SaveWindowStatesAdditionalData ad = (SaveWindowStatesAdditionalData)e;
            searchText.Text = ad.text;
            searchKey.IsChecked = ad.searchKeys;
            searchValue.IsChecked = ad.searchValues;
        }

        private void SaveWindowStates_PreviewAdditionalDataSaved(object sender, EventArgs e)
        {
            SaveWindowStatesAdditionalData ad = new SaveWindowStatesAdditionalData();
            ad.text = searchText.Text;
            ad.searchKeys = (bool)searchKey.IsChecked;
            ad.searchValues = (bool)searchValue.IsChecked;

            saveWindowStates.AdditionalData = ad;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifySearchEnabledAnEnable();
        }

        private void VerifySearchEnabledAnEnable()
        {
            search.IsEnabled = !string.IsNullOrWhiteSpace(searchText.Text) && (searchKey.IsChecked == true || searchValue.IsChecked == true);
        }

        [Serializable]
        private struct SaveWindowStatesAdditionalData
        {
            public bool searchKeys;
            public bool searchValues;
            public string text;
        }

        public class SearchModule
        {
            private bool seachKeys;
            private bool searchValues;
            private string text;

            public SearchModule(bool seachKeys, bool searchValues, string text)
            {
                this.seachKeys = seachKeys;
                this.searchValues = searchValues;
                this.text = text;
            }

            public bool IsMatch(LangValueCollection langValues)
            {
                if (seachKeys)
                {
                    if (langValues.Key.IndexOf(text, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }

                if (searchValues)
                {
                    foreach (LangValue item in langValues)
                    {
                        if (item.Value != null && item.Value.IndexOf(text, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                            return true;
                    }
                }

                return false;
            }
        }
    }
}