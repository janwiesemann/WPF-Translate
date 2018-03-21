using de.LandauSoftware.Core.WPF;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
