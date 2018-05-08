using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class ChatListControlViewModel
    {
        public List<ChatListPersonControlViewModel> List { get; set; }

        public ChatListControlViewModel()
        {
            List = new List<ChatListPersonControlViewModel>
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
