﻿using Caliburn.Micro;
using Ckype.Interfaces;
using Client;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class StartPageViewModel : Screen
    {
        public string IpTextBox { get; set; } = "127.0.0.1";
        public string PortTextBox { get; set; } = "6556";
        public string NicknameTextBox { get; set; } = "Ron";

        /// <summary>
        /// Move to the next screen after connecting
        /// </summary>
        public async Task ConnectAsync()
        {
            var client = IoC.Get<ClientSocket>();
            client.nickname = NicknameTextBox;
            bool Connected;
            try
            {

                Connected = await Task<bool>.Run(() =>
                {
                    client.Connect(IpTextBox, int.Parse(PortTextBox));
                    return client._socket.Connected;
                });

            }
            catch (System.Exception) { Connected = false; }
            if (Connected)
            {
                IoC.Get<ShellViewModel>().ShowChatPage();
            }
            else
            {
                //client.ResetSocket();
                // Reset client
                // popup cant connect
                PortTextBox = "Couldn't Connect";

                await IoC.Get<IUIManager>().OpenMessageBox(new PopupMessageViewModel() { Title = "Connection Issue", ConfirmationBoxText = "OK", Message = "Could not connect to server, Try Again" });
            }
        }

    }
}
