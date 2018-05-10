using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using Client;
using PacketLibrary;

namespace Ckype.ViewModels
{
    public delegate void FriendAdded(Person person);

    public class ChatListControlViewModel : BaseViewModel
    {
        public ObservableCollection<ChatListPersonControlViewModel> List { get; set; }
        

        public ChatListControlViewModel()
        {
            List = new ObservableCollection<ChatListPersonControlViewModel>();

            PacketHandler.FriendAddedEvent += FriendAdded;
            PacketHandler.FriendRemovedEvent += FriendRemoved;

            var client = IoC.Get<ClientSocket>();
            
            while(!client._socket.Connected) { }

            if (client.Friends.Count != 0)
            {
                foreach(var friend in client.Friends)
                {
                    List.Add(new ChatListPersonControlViewModel(friend));
                }
            }
        }

        public void FriendAdded(Person person)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
            {
                List.Add(new ChatListPersonControlViewModel(person));
            });
        }

        public void FriendRemoved(Person person)
        {
            ChatListPersonControlViewModel toRemove = null;
            foreach (var friend in List)
            {
                if (friend.Person.ip == person.ip && friend.Person.port == person.port)
                {
                    toRemove = friend;
                    break;
                }
            }

            App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
            {
                List.Remove(toRemove);
            });
        }
    }
}
