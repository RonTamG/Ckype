using AudioLibrary;
using Caliburn.Micro;
using Client;
using PacketLibrary;
using System.Diagnostics;
using System.Net;

namespace Ckype.ViewModels
{
    public class ChatListPersonControlViewModel
    {
        #region Public Properties

        /// <summary>
        /// Nickname for the client
        /// </summary>
        public string Nickname { get; set; }

        public Person Person { get; set; }

        public MessageListControlViewModel MessagePage { get; set; }

        public NetworkAudio Call { get; set; }

        #endregion

        #region Public Methods

        public void OpenMessageBox()
        {
            var chatScreen = IoC.Get<ChatScreenViewModel>();
            chatScreen.ChangeScreen(MessagePage);
        }

        public void CallPerson()
        {
            if (Call == null)
                Call = new NetworkAudio(new IPEndPoint(IPAddress.Parse(Person.ip), 7000));
            if (Call.connected)
                // clientSocket.EndCurrentCall(Person);
                Call.Disconnect();
            else
            {
                // clientSocket.CallPerson(Person); and then whenever this is received on the other side need to prompt the user and ask if he wants to accept or deny
                // written in the client's PacketHandler.
                Call.Start();
                //if (!Call.connected) // if the call failed to connect
                    // Ioc implementaion of a popup handler
            }

        }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with paramter input
        /// </summary>
        /// <param name="person">The chat list person</param>
        public ChatListPersonControlViewModel(Person person)
        {
            Nickname = person.name;
            Person = person;
            MessagePage = new MessageListControlViewModel(person);
        } 
        #endregion
    }
}
