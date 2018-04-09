using de.LandauSoftware.Core.WPF;
using MahApps.Metro.Controls.Dialogs;

namespace de.LandauSoftware.WPFTranslate
{
    public class DialogCoordinatorNotifyBase : NotifyBase
    {
        private IDialogCoordinator _DialogCoordinator;

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