using Caliburn.Micro;
using Client;
using System.Diagnostics;

namespace Ckype.ViewModels
{
    public class StartPageViewModel : Screen
    {
        public string IpTextBox { get; set; } = "192.168.1.24";
        public string PortTextBox { get; set; } = "6556";
        public string NicknameTextBox { get; set; } = "Ron";

        /// <summary>
        /// Move to the next screen after connecting
        /// </summary>
        public void Connect()
        {
            var client = IoC.Get<ClientSocket>();
            client.nickname = NicknameTextBox;

            try
            {
                client.Connect(IpTextBox, int.Parse(PortTextBox));

            }
            catch (System.Exception)
            {

                PortTextBox = "Connection falied try again";
                return;
            }

            while (!client._socket.Connected)
            {
            }

            ShellViewModel shellViewModel = IoC.Get<ShellViewModel>();
            shellViewModel.ShowChatPage();
        }
    }
}
