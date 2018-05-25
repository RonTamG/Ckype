using Ckype.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ckype.Views.Popups
{
    /// <summary>
    /// Interaction logic for PopupMessageView.xaml
    /// </summary>
    public partial class PopupMessageView : UserControl
    {
        public PopupMessageView(PopupDialogViewModelBase VM)
        {
            InitializeComponent();

            DataContext = VM;
        }
    }
}
