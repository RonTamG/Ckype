using Ckype.Interfaces;
using Ckype.ViewModels;
using Ckype.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ckype.IoCStuff
{
    class UIManager : IUIManager
    {
        public Task OpenMessageBox(PopupMessageViewModel viewModel)
        {
            return Task.Run(() => 
            {
                App.Current.Dispatcher.Invoke(() => new PopupMessageView(viewModel).ShowDialog());
            });
        }
    }
}
