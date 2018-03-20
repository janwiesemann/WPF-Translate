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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private  DataTemplate CreateCellTemplate(string langKey, Binding binding)
        {
            FrameworkElementFactory ff = new FrameworkElementFactory(typeof(TextBox));
            ff.SetBinding(TextBox.TextProperty, binding);
            ff.SetValue(TextBox.AcceptsReturnProperty, true);
            ff.SetValue(TextBox.AcceptsTabProperty, true);

            if (langKey != null)
            {
                ff.SetValue(SpellCheck.IsEnabledProperty, true);
                ff.SetValue(TextBox.LanguageProperty, XmlLanguage.GetLanguage(langKey));
            }

            return new DataTemplate() { DataType = typeof(TextBox), VisualTree = ff };
        }

        private void CreateColoumn(object header, string langKey, Binding cellbinding)
        {
            GridViewColumn col = new GridViewColumn();

            if (header is string)
                col.Header = header.ToString();
            else if (header is BindingBase)
                BindingOperations.SetBinding(col, GridViewColumn.HeaderProperty, header as BindingBase);
            else
                throw new ArgumentException(nameof(header), "Header must be string or binding");

            col.CellTemplate = CreateCellTemplate(langKey, cellbinding);

            gridView.Columns.Add(col);
        }

        private void vModel_LanguageCollectionChangedEvent(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                gridView.Columns.Clear();

                CreateColoumn("Keys", null, new Binding(nameof(LangValueCollection.Key)));

                for (int i = 0; i < vModel.LangData.Languages.Count; i++)
                {
                    Language lang = vModel.LangData.Languages[i];

                    CreateColoumn(lang.LangKey, null, new Binding("[" + i + "]." + nameof(LangValue.Value)));
                }
            });
        }
    }
}
