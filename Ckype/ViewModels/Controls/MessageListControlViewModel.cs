using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class MessageListControlViewModel : BaseViewModel
    {
        public ObservableCollection<MessageControlViewModel> Messages { get; set; }

        public MessageListControlViewModel()
        {
            Messages = new ObservableCollection<MessageControlViewModel>();
            AddMessage("paka");
        }

        /// <summary>
        /// Add string message to the message lists
        /// </summary>
        /// <param name="Message">The text message to add</param>
        public void AddMessage(string Message)
        {
            var newMessage = new MessageControlViewModel
            {
                MessageType = MessageType.Text,
                Content = Message,
            };

            Messages.Add(newMessage);
        }
    }
}
