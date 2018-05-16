using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            PacketHandler.FriendMessageReceivedEvent += MessageReceived;

            var client = IoC.Get<ClientSocket>();

            if (client.Friends.Count != 0)
            {
                foreach(var friend in client.Friends)
                {
                    List.Add(new ChatListPersonControlViewModel(friend));
                }
            }
        }

        private void MessageReceived(Person person, string message)
        {
            ChatListPersonControlViewModel temp = null;
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].Person.ip == person.ip && List[i].Person.port == person.port)
                {
                    temp = List[i];
                    break;
                }
            }

            var newMessage = new MessageControlViewModel
            {
                MessageType = MessageType.Text,
                Content = message,
            };

            App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
            {
                temp?.MessagePage.Messages.Add(newMessage);
            });
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

            //// Remove item on the list from UI thread
            //System.Threading.SynchronizationContext.Current.Post((System.Threading.SendOrPostCallback)delegate
            //{
            //    List.Remove(toRemove);
            //}, null);

           // This can also be used but is wpf specific !!
           App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
           {
               List.Remove(toRemove);
           });
        }
    }
}
