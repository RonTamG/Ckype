using System;
using System.Windows.Controls;
using Caliburn.Micro;

namespace Ckype.ViewModels
{
    public class PopupBaseViewModel : BaseWindowViewModel
    {
        public string Title { get; set; }

        public Control Content { get; set; }
    }
}
