using Ckype.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.Interfaces
{
    public interface IUIManager
    {
        Task OpenMessageBox(PopupMessageViewModel viewModel);
    }
}
