using Caliburn.Micro;
using System.Diagnostics;

namespace Ckype.ViewModels
{
    public class StartPageViewModel : Screen
    { 
        public string IpTextBox { get; set; }
        public string PortTextBox { get; set; }
        public string NicknameTextBox { get; set; }

        /// <summary>
        /// Move to the next screen after connecting
        /// </summary>
        public void Connect()
        {
            ShellViewModel shellViewModel = IoC.Get<ShellViewModel>();
            shellViewModel.ShowChatPage();

            Debug.WriteLine(IpTextBox);
            Debug.WriteLine(PortTextBox);
            Debug.WriteLine(NicknameTextBox);
        }
    }
}
