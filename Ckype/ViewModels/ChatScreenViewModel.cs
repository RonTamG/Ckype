using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class ChatScreenViewModel : BaseViewModel
    {
        public MessageListControlViewModel MessageList { get; set; }

        public ChatScreenViewModel()
        {
            MessageList = new MessageListControlViewModel();
        }

        public void AddMessage(string message)
        {
            MessageList.AddMessage(message);
            
        }
    }
}
