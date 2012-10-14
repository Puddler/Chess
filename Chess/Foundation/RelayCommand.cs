using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Chess.Foundation
{
    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action _execute;
        #endregion

        #region Constructors
        public RelayCommand(Action execute)
        {
            _execute = execute;
        }
        #endregion

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion
    }
}
