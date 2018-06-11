using AudioLibrary;
using Caliburn.Micro;
using Ckype.Interfaces;
using Client;
using PacketLibrary;
using System.Diagnostics;
using System.Net;

namespace Ckype.ViewModels
{
    public class ChatListPersonControlViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Nickname for the client
        /// </summary>
        public string Nickname { get; set; }

        public Person Person { get; set; }

        public MessageListControlViewModel MessagePage { get; set; }

        public bool Selected { get; set; }

        public NetworkAudio Call { get; set; }

        public bool Connected { get; set; }

        public bool Muted { get; set; }

        public bool NoNewMessage { get; set; }

        #endregion

        #region Public Methods

        public void OpenMessageBox()
        {
            // selected
            var PeopleList = IoC.Get<ChatListControlViewModel>();
            if (PeopleList.PersonSelected != null)
                PeopleList.PersonSelected.Selected = false;
            Selected = true;

            NoNewMessage = true;

            PeopleList.PersonSelected = this;
            var chatScreen = IoC.Get<ChatScreenViewModel>();
            chatScreen.ChangeScreen(MessagePage);
        }

        public void CallPerson()
        {
            if (Call != null && Call.connected)
            {
                IoC.Get<ClientSocket>().EndCurrentCall(Person);
                Connected = false;
                Call.Disconnect();
            }
            else
            {
                IoC.Get<ClientSocket>().CallPerson(Person);
                // clientSocket.CallPerson(Person); and then whenever this is received on the other side need to prompt the user and ask if he wants to accept or deny
                // written in the client's PacketHandler.
                // Call.Start();
                //if (!Call.connected) // if the call failed to connect
                // Ioc implementaion of a popup handler
            }

        }

        public void ToggleMuteMicrophone()
        {
            Call.ToggleSelfMute();
            Muted = !Muted;
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
            NoNewMessage = true;
            Connected = false;
            Call = new NetworkAudio(new IPEndPoint(IPAddress.Parse(Person.ip), 7000));
            Call.ConnectedToPerson += ConnectionStatusChanged;
        } 
        #endregion


        public void ConnectionStatusChanged(bool Status)
        {
            Connected = Status;
        }
    }
}
