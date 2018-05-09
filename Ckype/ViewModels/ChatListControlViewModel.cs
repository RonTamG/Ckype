using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class ChatListControlViewModel
    {
        public ObservableCollection<ChatListPersonControlViewModel> List { get; set; }

        public ChatListControlViewModel()
        {
            List = new ObservableCollection<ChatListPersonControlViewModel>
            {
                new ChatListPersonControlViewModel
                {
                    Nickname = "Ron"
                },
                new ChatListPersonControlViewModel
                {
                    Nickname = "Maya"
                },
                new ChatListPersonControlViewModel
                {
                    Nickname = "Bratha"
                },
                new ChatListPersonControlViewModel
                {
                    Nickname = "Mr Poncho"
                }
            };
        }
    }
}
