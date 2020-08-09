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
        /// <summary>
        /// Ein neues Fentser zur Eingabe von Suchwerten
        /// </summary>
        public SearchWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Erstellt eine Suchmodul mit den aktuellen Einstellungen
        /// </summary>
        /// <returns></returns>
        public SearchModule CreateSearchModule()
        {
            return new SearchModule((bool)searchKey.IsChecked, (bool)searchValue.IsChecked, searchText.Text);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            VerifySearchEnabledAnEnable();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ControlzEx.KeyboardNavigationEx.Focus(searchText);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void SearchText_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            searchText.SelectAll();
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            VerifySearchEnabledAnEnable();
        }

        private void VerifySearchEnabledAnEnable()
        {
            search.IsEnabled = !string.IsNullOrWhiteSpace(searchText.Text) && (searchKey.IsChecked == true || searchValue.IsChecked == true);
        }

        /// <summary>
        /// Führt die Suche für einen bestimmten Wert aus
        /// </summary>
        public struct SearchModule
        {
            private readonly bool seachKeys;
            private readonly bool searchValues;
            private readonly string text;

            internal SearchModule(bool seachKeys, bool searchValues, string text)
            {
                this.seachKeys = seachKeys;
                this.searchValues = searchValues;
                this.text = text;
            }

            /// <summary>
            /// Prüft, ob der Wert ein Match ist
            /// </summary>
            /// <param name="langValues"></param>
            /// <returns></returns>
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