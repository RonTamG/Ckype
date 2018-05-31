using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AudioLibrary;
using Caliburn.Micro;
using Ckype.Interfaces;
using Ckype.ViewModels.Popups;
using Client;
using PacketLibrary;

namespace Ckype.ViewModels
{
    public delegate void FriendAdded(Person person);

    public class ChatListControlViewModel : BaseViewModel
    {
        public ObservableCollection<ChatListPersonControlViewModel> List { get; set; }

        public ChatListPersonControlViewModel FindPersonControl(Person person)
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

            return temp;
        }
        

        public ChatListControlViewModel()
        {
            List = new ObservableCollection<ChatListPersonControlViewModel>();

            PacketHandler.FriendAddedEvent += FriendAdded;
            PacketHandler.FriendRemovedEvent += FriendRemoved;
            PacketHandler.FriendsReceivedEvent += FriendsReceived;
            PacketHandler.FriendMessageReceivedEvent += MessageReceived;
            PacketHandler.CallingEvent += FriendCalling;
            PacketHandler.AcceptedCallEvent += FriendAnswered;
            PacketHandler.DeclinedCallEvent += FriendDeclined;
            PacketHandler.CancelledCallEvent += FriendEndedCall;

            var client = IoC.Get<ClientSocket>();

            if (client.Friends.Count != 0)
            {
                foreach(var friend in client.Friends)
                {
                    List.Add(new ChatListPersonControlViewModel(friend));
                }
            }
        }        

        private void FriendCalling(ref CallPacket packet)
        {
            var Popup = IoC.Get<IUIManager>().OpenMessageBox(new PopupCallingViewModel()
            {
                Message = $"{packet.destClient.name} is calling...",
                Title = $"{packet.destClient.name}",
            });

            Popup.Wait();
            if (((PopupCallingViewModel)(Popup.Result)).AcceptedCall)
            {
                var PersonViewModel = this.FindPersonControl(packet.destClient);
                packet.SetAcceptedCall();
                packet.SetCheckType();
                IoC.Get<ClientSocket>().Send(packet.Data);
                if (PersonViewModel.Call == null)
                    PersonViewModel.Call = new NetworkAudio(new IPEndPoint(IPAddress.Parse(PersonViewModel.Person.ip), 7000));
                PersonViewModel.Call.Start();
            }
        }

        private void FriendAnswered(ref CallPacket packet)
        {
            var FriendToCall = this.FindPersonControl(packet.destClient);
            FriendToCall.Call.Start();
        }

        private void FriendDeclined(ref CallPacket packet)
        {
            IoC.Get<IUIManager>().OpenMessageBox(new PopupMessageViewModel()
            {
                Message = $"{packet.destClient.name} Declined Call",
                Title = $"{packet.destClient.name}",
                ConfirmationBoxText = "Ok",
            });
        }

        private void FriendEndedCall(ref CallPacket packet)
        {
            var FriendToEndCall = this.FindPersonControl(packet.destClient);
            FriendToEndCall.Call.Disconnect();
        }

        private void MessageReceived(Person person, string message)
        {
            var temp = this.FindPersonControl(person);

            var newMessage = new MessageControlViewModel
            {
                SentByMe = false,
                MessageSentTime = DateTimeOffset.UtcNow,
                SenderName = person.name,
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

        public void AllFriendsRemoved()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
            {
                List.Clear();
            });
        }

        public void FriendsReceived(List<Person> lst)
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate // <--- HERE
            {
                for(int i = 0;i<lst.Count;i++)
                {
                    List.Add(new ChatListPersonControlViewModel(lst[i]));
                }
            });
        }
    }
}
