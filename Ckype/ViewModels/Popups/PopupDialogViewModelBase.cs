using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ckype.ViewModels
{
    public abstract class PopupDialogViewModelBase
    {
        public ICommand CloseCommand { get; set; }

        public string Title { get; set; }

        public PopupType Type { get; set; }
    }
}
