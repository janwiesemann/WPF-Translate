using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Erstellt ein neues Hauptfenster
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_LoadedSinglefire;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Erstellt eine neue Spalte mit einem Button
        /// </summary>
        /// <param name="content">Button Content</param>
        /// <param name="toolTip">Button Tooltip</param>
        /// <param name="commandBindingPath">Command Binding string</param>
        private void CreateButtonColoumn(string content, string toolTip, string commandBindingPath)
        {
            FrameworkElementFactory ff = new FrameworkElementFactory(typeof(Button));
            ff.SetValue(Button.ContentProperty, content);
            ff.SetValue(Button.VerticalAlignmentProperty, VerticalAlignment.Top);
            ff.SetValue(Button.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            ff.SetValue(Button.StyleProperty, App.Current.FindResource("BlueCircleButtonStyle"));
            ff.SetValue(Button.ToolTipProperty, toolTip);
            ff.SetBinding(Button.WidthProperty, new Binding(nameof(Button.ActualHeight)) { RelativeSource = new RelativeSource(RelativeSourceMode.Self), Mode = BindingMode.OneWay });
            ff.SetBinding(Button.CommandProperty, new Binding(nameof(BindingProxy.Data) + "." + commandBindingPath) { Source = Resources["BindingProxy"], Mode = BindingMode.OneWay });
            ff.SetBinding(Button.CommandParameterProperty, new Binding());

            GridViewColumn col = new GridViewColumn
            {
                CellTemplate = new DataTemplate
                {
                    DataType = typeof(Button),
                    VisualTree = ff
                }
            };

            gridView.Columns.Add(col);
        }

        /// <summary>
        /// Erstellt eine neue Spalte
        /// </summary>
        /// <param name="header">Header</param>
        /// <param name="cellTemplate">Cellen Vorlage</param>
        private void CreateColoumn(object header, DataTemplate cellTemplate)
        {
            GridViewColumn col = new GridViewColumn
            {
                Header = header?.ToString(),
                CellTemplate = cellTemplate
            };

            gridView.Columns.Add(col);
        }

        /// <summary>
        /// Erstellt eine neue Textzellen Vorlage.
        /// </summary>
        /// <param name="langKey">SprachKey</param>
        /// <param name="binding">Binding</param>
        /// <param name="allowNewLines">Neue zeilen und Tabs erlauben</param>
        /// <returns></returns>
        private DataTemplate CreateTextCellTemplate(string langKey, Binding binding, bool allowNewLines)
        {
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            FrameworkElementFactory ff = new FrameworkElementFactory(typeof(TextBox));
            ff.SetBinding(TextBox.TextProperty, binding);
            ff.SetValue(TextBox.VerticalAlignmentProperty, VerticalAlignment.Stretch);
            ff.SetValue(TextBox.VerticalContentAlignmentProperty, VerticalAlignment.Top);
            ff.SetValue(TextBox.BackgroundProperty, null);

            if (langKey != null)
            {
                ff.SetValue(SpellCheck.IsEnabledProperty, true);
                ff.SetValue(TextBox.LanguageProperty, XmlLanguage.GetLanguage(langKey));
            }

            if (allowNewLines)
            {
                ff.SetValue(TextBox.AcceptsReturnProperty, true);
                ff.SetValue(TextBox.AcceptsTabProperty, true);
            }

            return new DataTemplate() { DataType = typeof(TextBox), VisualTree = ff };
        }

        private void MainWindow_LoadedSinglefire(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainWindow_LoadedSinglefire;

            VModel_LanguageCollectionChangedEvent(this, EventArgs.Empty);
        }

        /// <summary>
        /// Wird beim hinzufügen, ändern oder löschen einer Sprache aufgerufen und updatet die
        /// Spalten des Fensters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VModel_LanguageCollectionChangedEvent(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                gridView.Columns.Clear();

                CreateButtonColoumn("X", "Löschen", nameof(MainWindowViewModel.RemoveKeyCommand));
                CreateButtonColoumn("T", "Übersetzten", nameof(MainWindowViewModel.TranslateKeyCommand));

                CreateColoumn("Key", CreateTextCellTemplate(null, new Binding(nameof(LangValueCollection.Key)), false));

                for (int i = 0; i < vModel.LangData.Languages.Count; i++)
                {
                    Language lang = vModel.LangData.Languages[i];

                    CreateColoumn(lang.LangKey, CreateTextCellTemplate(lang.LangKey, new Binding("[" + i + "]." + nameof(LangValue.Value)), true));
                }
            });
        }

        private void VModel_LanguageCollectionScrollIntoViewRequest(object sender, LangValueCollection e)
        {
            Keyboard.ClearFocus();

            listView.Focus();
            listView.ScrollIntoView(e);
            e.BlinkBackgroundForTwoSeconds();
        }
    }
}