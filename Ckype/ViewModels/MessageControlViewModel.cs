
using Ckype.Interfaces;

namespace Ckype.ViewModels
{
    public class MessageControlViewModel : BaseViewModel, IMessage
    {
        public MessageType MessageType { get; set; }

        public object Content { get; set; }
    }
}
