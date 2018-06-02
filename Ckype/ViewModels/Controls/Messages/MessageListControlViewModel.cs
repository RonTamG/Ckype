using Caliburn.Micro;
using PacketLibrary;
using Server;
using System;
using System.Collections.ObjectModel;
using Client;
using System.Threading;

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
                SenderName = Person.name,
                MessageSentTime = DateTimeOffset.UtcNow,
                SentByMe=true,
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

            // if gonna change then build a client here
            // then whenever you send a file you are only a client
            // so if people want to send multiple files to *you* you can use the same socket.
            Random randInt = new Random();
            int port = randInt.Next(1025, 65535);

            var client = IoC.Get<ClientSocket>();

            LinkPacket linkRequest = new LinkPacket(Person, port);
            client.Send(linkRequest.Data);

            ServerSocket LinkedServer = new ServerSocket();
            LinkedServer.Bind(port);
            LinkedServer.Listen(0);
            LinkedServer.Accept();
            while (LinkedServer.connected.Count < 1)
                Thread.Sleep(1000);
            LinkedServer.SendFile(filename, client.me);
            LinkedServer.CloseAllSockets();
/*
            LinkPacket linkClose = new LinkPacket(Person, (ushort)(type.LinkClose));
            client.Send(linkClose.Data);

            LinkedServer = null;*/
        }
    }
}
