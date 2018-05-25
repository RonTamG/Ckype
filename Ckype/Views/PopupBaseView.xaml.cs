using Ckype.ViewModels;
using Ckype.Views.Popups;
using System.Windows;

namespace Ckype.Views
{
    /// <summary>
    /// Interaction logic for PopupMessageView.xaml
    /// </summary>
    public partial class PopupBaseView : Window
    {
        private PopupBaseViewModel myVM;

        public PopupBaseView(PopupDialogViewModelBase VM)
        {
            InitializeComponent();

            myVM = new PopupBaseViewModel()
            {
                Title = VM.Title,
                CloseAction = new System.Action<object>((blank) => this.Close()),
                MinimizeAction = new System.Action<object>((blank) => this.WindowState = WindowState.Minimized),
            };
            DataContext = myVM;
            VM.CloseCommand = myVM.CloseCommand;

            SetContent(VM);
        }
        public void SetContent(PopupDialogViewModelBase VM)
        {
            switch(VM.Type)
            {
                case PopupType.Message:
                    {
                        myVM.Content = new PopupMessageView(VM);
                    };
                    break;
            }
        }
    }
}
