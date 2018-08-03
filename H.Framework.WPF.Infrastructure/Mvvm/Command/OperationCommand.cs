using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace H.Framework.WPF.Infrastructure.Mvvm.Command
{
    public class OperationCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            CanExecuteChanged?.Invoke(this, new ParameterEventArgs() { Parameter = parameter });
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Executing?.Invoke(this, new ParameterEventArgs() { Parameter = parameter });
        }

        public event EventHandler<ParameterEventArgs> Executing;
    }
}