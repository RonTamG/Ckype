using Caliburn.Micro;
using Client;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class StartPageViewModel : Screen
    {
        public string IpTextBox { get; set; } = "192.168.1.24";
        public string PortTextBox { get; set; } = "6556";
        public string NicknameTextBox { get; set; } = "Ron";

        public bool AttemptingConnection { get; set; } = false;

        /// <summary>
        /// Move to the next screen after connecting
        /// </summary>
        public async Task ConnectAsync()
        {
            var client = IoC.Get<ClientSocket>();
            client.nickname = NicknameTextBox;

            var Connected = await Task<bool>.Run(() =>
            {
                client.Connect(IpTextBox, int.Parse(PortTextBox));
                return client._socket.Connected;
            });

            if (Connected)
                IoC.Get<ShellViewModel>().ShowChatPage();
            else
            {
                // popup cant connect
                PortTextBox = "Couldn't Connect";
            }
        }

    }
}
