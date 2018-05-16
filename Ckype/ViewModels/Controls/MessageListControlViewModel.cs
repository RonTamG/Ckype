using Caliburn.Micro;
using Client;
using PacketLibrary;
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

        public Person Person { get; set; }

        public MessageListControlViewModel(Person person)
        {
            Messages = new ObservableCollection<MessageControlViewModel>();
            Person = person;
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


            var MessagePacket = new MessagePacket(Message, Person);

            var client = IoC.Get<ClientSocket>();
            client.Send(MessagePacket.Data);
        }

        public void SendFile(string filename)
        {
            IoC.Get<ClientSocket>().SendFile(filename, Person);         
        }
    }
}
