using AudioLibrary;
using Caliburn.Micro;
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

        #endregion

        #region Public Methods

        public void OpenMessageBox()
        {
            var chatScreen = IoC.Get<ChatScreenViewModel>();
            chatScreen.ChangeScreen(MessagePage);
        }

        public void CallPerson()
        {
            var Call = new NetworkAudio(new IPEndPoint(IPAddress.Parse(Person.ip) , 7000));
            Call.Start();
        }

        #endregion

        #region Constructors

        public ChatListPersonControlViewModel()
        {
            MessagePage = new MessageListControlViewModel();
        }

        /// <summary>
        /// Constructor with paramter input
        /// </summary>
        /// <param name="nickname">Nickname of the client</param>
        /// <param name="selected">If the current client is selected or the not Defaults to false</param>
        public ChatListPersonControlViewModel(Person person)
        {
            Nickname = person.name;
            Person = person;
            MessagePage = new MessageListControlViewModel();
        } 
        #endregion
    }
}
