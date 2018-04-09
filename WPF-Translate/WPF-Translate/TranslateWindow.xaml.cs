using MahApps.Metro.Controls;
using System.Collections.Generic;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für TranslateWindow.xaml
    /// </summary>
    public partial class TranslateWindow : MetroWindow
    {
        public TranslateWindow(IList<LangValueCollection> keyList, IList<Language> sprachen)
        {
            InitializeComponent();

            vModel.KeyList = keyList;
            vModel.Languages = sprachen;
        }

        public static void ShowDialog(IList<LangValueCollection> keyList, IList<Language> sprachen)
        {
            TranslateWindow tw = new TranslateWindow(keyList, sprachen);
            tw.ShowDialog();
        }
    }
}