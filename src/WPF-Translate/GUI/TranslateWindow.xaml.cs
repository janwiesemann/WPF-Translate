using MahApps.Metro.Controls;
using System.Collections.Generic;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für TranslateWindow.xaml
    /// </summary>
    public partial class TranslateWindow : MetroWindow
    {
        private TranslateWindow(IList<LangValueCollection> keyList, IList<Language> sprachen)
        {
            InitializeComponent();

            vModel.KeyList = keyList;
            vModel.Languages = sprachen;
        }

        /// <summary>
        /// Zeigt den Übersetzter in einem neun Dialog an.
        /// </summary>
        /// <param name="keyList">List mit allen Keys LangValueCollection</param>
        /// <param name="sprachen">Liste aller Sprachen</param>
        public static void ShowDialog(IList<LangValueCollection> keyList, IList<Language> sprachen)
        {
            TranslateWindow tw = new TranslateWindow(keyList, sprachen);
            tw.ShowDialog();
        }
    }
}