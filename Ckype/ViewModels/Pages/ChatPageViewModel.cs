using Caliburn.Micro;
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    class ChatPageViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// List of people to chat with
        /// </summary>
        public ChatListControlViewModel ChatListControl { get; set; }
        /// <summary>
        /// The screen on which the chat appears
        /// </summary>
        public ChatScreenViewModel ChatScreen { get; set; }
        /// <summary>
        /// The message to send
        /// </summary>
        public string MessageText { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChatPageViewModel()
        {
            ChatListControl = new ChatListControlViewModel();
            ChatScreen = IoC.Get<ChatScreenViewModel>();
        }

        #endregion

        public void AddMessage()
        {
            ChatScreen.AddMessage(MessageText);
        }

        public void Disconnect()
        {
//           var client = IoC.Get<ClientSocket>();
//            client.Disconnect();
//            client = new ClientSocket();
            IoC.Get<ClientSocket>().Disconnect();
            IoC.Get<ShellViewModel>().ShowStartPage();
        }

        public void Refresh()
        {
            ChatListControl.AllFriendsRemoved();
            IoC.Get<ClientSocket>().RefreshRequest();
        }

        public void Browse()
        {
            string filename;
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                ChatScreen.CurrentMessageList.SendFile(filename);
            }
        }
    }
}
