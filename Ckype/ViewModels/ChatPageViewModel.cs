using Caliburn.Micro;
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
    }
}
