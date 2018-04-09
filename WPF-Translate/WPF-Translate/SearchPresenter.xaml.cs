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
            GridViewColumn gvc = new GridViewColumn();
            gvc.Header = header;
            gvc.DisplayMemberBinding = new Binding(bindingPath);

            gridView.Columns.Add(gvc);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            elementClickedCallback?.Invoke(listView.SelectedItem as LangValueCollection);
        }
    }
}