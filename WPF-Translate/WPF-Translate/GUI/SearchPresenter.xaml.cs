using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für SearchPresenter.xaml
    /// </summary>
    public partial class SearchPresenter : MetroWindow
    {
        private Action<LangValueCollection> elementClickedCallback;

        /// <summary>
        /// Erstellt einen neuen SearchPresenter
        /// </summary>
        /// <param name="langs">Alle möglichen Sprachen</param>
        /// <param name="matches">Alle anzuzeigenden Ergebnisse</param>
        /// <param name="elementClickedCallback">Callback für den Click auf ein Element</param>
        public SearchPresenter(IList<Language> langs, IEnumerable<LangValueCollection> matches, Action<LangValueCollection> elementClickedCallback)
        {
            InitializeComponent();

            this.elementClickedCallback = elementClickedCallback;

            AddColoumn("Key", nameof(LangValueCollection.Key));

            for (int i = 0; i < langs.Count; i++)
            {
                AddColoumn(langs[i].LangKey, "[" + i + "]." + nameof(LangValue.Value));
            }

            listView.ItemsSource = matches;
        }

        private void AddColoumn(string header, string bindingPath)
        {
            GridViewColumn gvc = new GridViewColumn
            {
                Header = header,
                DisplayMemberBinding = new Binding(bindingPath)
            };

            gridView.Columns.Add(gvc);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            elementClickedCallback?.Invoke(listView.SelectedItem as LangValueCollection);
        }
    }
}