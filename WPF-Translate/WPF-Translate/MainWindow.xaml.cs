using de.LandauSoftware.Core.WPF;
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
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_LoadedSinglefire;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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

            GridViewColumn col = new GridViewColumn();

            col.CellTemplate = new DataTemplate() { DataType = typeof(Button), VisualTree = ff };

            gridView.Columns.Add(col);
        }

        private void CreateColoumn(object header, DataTemplate cellTemplate)
        {
            GridViewColumn col = new GridViewColumn();

            col.Header = header?.ToString();

            col.CellTemplate = cellTemplate;

            gridView.Columns.Add(col);
        }

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

            vModel_LanguageCollectionChangedEvent(this, EventArgs.Empty);
        }

        private void vModel_LanguageCollectionChangedEvent(object sender, EventArgs e)
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

        private void vModel_LanguageCollectionScrollIntoViewRequest(object sender, LangValueCollection e)
        {
            Keyboard.ClearFocus();
            listView.ScrollIntoView(e);
            e.BlinkBackgroundForTwoSeconds();
        }
    }
}