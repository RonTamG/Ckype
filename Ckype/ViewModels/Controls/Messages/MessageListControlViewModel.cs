using Caliburn.Micro;
using PacketLibrary;
using Server;
using System;
using System.Collections.ObjectModel;
using Client;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net;

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
            Task.Run(() =>
            {
                // Add if not accepted request
                Random randInt = new Random();
                int port = randInt.Next(1025, 65535);

                var client = IoC.Get<ClientSocket>();

                LinkPacket linkRequest = new LinkPacket(Person, port, 5000);
                client.Send(linkRequest.Data);
                bool Accepted = true;
                if (Accepted)
                {

                    #region Build File Packet

                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

                    uint packetLength;
                    ushort packetStart = (ushort)(filename.Length + 20 + client.me.Data.Length);

                    packetLength = (uint)(packetStart + fs.Length);

                    FilePacket packet = new FilePacket(filename, packetLength, (uint)fs.Length, packetStart, client.me);
                    fs.Read(packet.Data, packetStart, (int)fs.Length);

                    fs.Close();

                    #endregion

                    #region Create socket and send

                    Socket FileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    bool NotConnected = true;
                    while (NotConnected)
                    {
                        try
                        {
                            FileSocket.Connect(Person.ip, port);
                            NotConnected = false;
                        }
                        catch (SocketException)
                        {
                            NotConnected = true;
                        }
                    }
                    FileSocket.Send(packet.Data);
                    FileSocket.Shutdown(SocketShutdown.Both);
                    FileSocket.Close();

                    #endregion
                }
                else
                    return;
          });
        }
    }
}
