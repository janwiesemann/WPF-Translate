using de.LandauSoftware.UI.WPF;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            SaveWindowStates.ForceDirectory = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "WindowStates"); //Setzt den Pfad für das Speichern von Fensterpositionen in einen Lokalen Ordner.
        }
    }
}