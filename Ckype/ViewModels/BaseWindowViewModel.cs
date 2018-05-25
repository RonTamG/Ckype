
using Caliburn.Micro;
using Client;
using System;
using System.Windows;
using System.Windows.Input;

namespace Ckype.ViewModels
{
    public class BaseWindowViewModel
    {
        public System.Action<object> CloseAction { get; set; }
        public System.Action<object> MinimizeAction { get; set; }
        public System.Action<object> MaximizeAction { get; set; }

        private RelayCommand<object> closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                    closeCommand = new RelayCommand<object>(CloseAction);
                return closeCommand;
            }
        }

        private RelayCommand<object> minimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                if (minimizeCommand == null)
                    minimizeCommand = new RelayCommand<object>(MinimizeAction);
                return minimizeCommand;
            }
        }

    }
}
