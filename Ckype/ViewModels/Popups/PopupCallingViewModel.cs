using PacketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels.Popups
{
    public class PopupCallingViewModel : PopupDialogViewModelBase
    {
        public RelayCommand<object> AcceptCall { get; set; }
        public RelayCommand<object> RejectCall { get; set; }
        public bool AcceptedCall;

        private void _AcceptCall()
        {
            AcceptedCall = true;
            CloseCommand.Execute(null);
        }

        private void _RejectCall()
        {
            AcceptedCall = false;
            CloseCommand.Execute(null);
        }
        /// <summary>
        /// Message that will be shown in the body of the popup
        /// </summary>
        public string Message { get; set; }

        public PopupCallingViewModel()
        {
            Type = PopupType.Call;
            AcceptCall = new RelayCommand<object>((blank) => _AcceptCall());
            RejectCall = new RelayCommand<object>((blank) => _RejectCall());
        }
    }
}
