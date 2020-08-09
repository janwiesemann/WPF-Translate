using System;
using System.Windows.Input;

namespace de.LandauSoftware.WPFTranslate
{
    public class RelayICommand<T> : ICommand
    {
        private Predicate<T> canExecute;
        private Action<T> execute;

        public RelayICommand(Action<T> execute) : this(null, execute)
        { }

        public RelayICommand(Predicate<T> canExecute, Action<T> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            T parameterT = ParamToT(parameter);

            return canExecute(parameterT);
        }

        public void Execute(object parameter)
        {
            T parameterT = ParamToT(parameter);

            execute(parameterT);
        }

        private static T ParamToT(object param)
        {
            if (param is T t)
                return t;

            return (T)Convert.ChangeType(param, typeof(T));
        }
    }
}