using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Perfect_Scan.Manager
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;
        private Func<Action<object>> alignControlCommand;

        public event EventHandler CanExecuteChanged;
         
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        
        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Func<Action<object>> alignControlCommand)
        {
            this.alignControlCommand = alignControlCommand;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

         
        public void Execute(object parameter = null)
        {
            _execute.Invoke(parameter);
        }
         
        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
