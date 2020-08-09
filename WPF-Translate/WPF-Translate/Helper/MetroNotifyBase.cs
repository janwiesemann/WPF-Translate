using MahApps.Metro.Controls.Dialogs;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Metro NotifyBase
    ///
    /// https://mahapps.com/docs/dialogs/mvvm-dialog
    /// </summary>
    public class MetroNotifyBase : NotifyBase
    {
        private IDialogCoordinator _DialogCoordinator;

        public IDialogCoordinator DialogCoordinator => _DialogCoordinator ?? (_DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance);
    }
}