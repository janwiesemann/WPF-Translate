using System;

namespace de.LandauSoftware.WPFTranslate
{
    internal class RelayICommand : RelayICommand<object>
    {
        public RelayICommand(Action<object> execute) : base(execute)
        { }

        public RelayICommand(Predicate<object> canExecute, Action<object> execute) : base(canExecute, execute)
        { }
    }
}