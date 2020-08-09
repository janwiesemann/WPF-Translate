using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows.Data;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für MissingKeysWindow.xaml
    /// </summary>
    public partial class MissingKeysWindow : MetroWindow
    {
        /// <summary>
        /// Erstellt eine neue Instanz des MissingKeysWindow
        /// </summary>
        /// <param name="keys"></param>
        public MissingKeysWindow(IEnumerable<MissingKeyInfo> keys)
        {
            InitializeComponent();

            listView.ItemsSource = keys;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listView.ItemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(MissingKeyInfo.File)));
        }
    }
}