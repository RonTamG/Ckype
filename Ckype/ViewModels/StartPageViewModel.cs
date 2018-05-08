using Caliburn.Micro;
using System.Diagnostics;

namespace Ckype.ViewModels
{
    public class StartPageViewModel : Screen
    {
        private ShellViewModel _parent;
        public string ipTextBox { get; set; }
        public string portTextBox { get; set; }
        public string nicknameTextBox { get; set; }


        public StartPageViewModel(ShellViewModel VM)
        {
            _parent = VM;
        }

        /// <summary>
        /// Move to the next screen after connecting
        /// </summary>
        public void Connect()
        {
            _parent.ShowChatPage();

            Debug.WriteLine(ipTextBox);
            Debug.WriteLine(portTextBox);
            Debug.WriteLine(nicknameTextBox);
        }
    }
}
