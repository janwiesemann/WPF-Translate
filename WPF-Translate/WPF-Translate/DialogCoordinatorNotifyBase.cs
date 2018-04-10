using de.LandauSoftware.Core.WPF;
using MahApps.Metro.Controls.Dialogs;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Notify base mit Implementierung für den MahApps DialogCoordinator
    /// </summary>
    public class DialogCoordinatorNotifyBase : NotifyBase
    {
        private IDialogCoordinator _DialogCoordinator;

        /// <summary>
        /// Ruft den aktuellen DialogCoordinator ab.
        /// </summary>
        protected IDialogCoordinator DialogCoordinator
        {
            get
            {
                if (_DialogCoordinator == null)
                    _DialogCoordinator = MahApps.Metro.Controls.Dialogs.DialogCoordinator.Instance;

                return _DialogCoordinator;
            }
        }
    }
}