using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class ChatScreenViewModel : BaseViewModel
    {
        //public ObservableCollection<MessageListControlViewModel> MessageLists { get; set; }

        public MessageListControlViewModel CurrentMessageList { get; set; }

        public void ChangeScreen(MessageListControlViewModel messageList)
        {
            CurrentMessageList = messageList;
        }

        public void AddMessage(string message)
        {
            CurrentMessageList.AddMessage(message);
        }

    }
}
