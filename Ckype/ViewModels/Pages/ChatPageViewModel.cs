﻿using Caliburn.Micro;
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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
            ChatListControl = IoC.Get<ChatListControlViewModel>();
            ChatScreen = IoC.Get<ChatScreenViewModel>();
        }

        #endregion

        /// <summary>
        /// Adds a message to the chat screen and sends it
        /// </summary>
        public void AddMessage()
        {
            ChatScreen.AddMessage(MessageText);
            MessageText = String.Empty;
        }

        public void Disconnect()
        {
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
                ChatScreen.AddFile(filename);
            }
        }
    }
}
