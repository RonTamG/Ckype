using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckype.ViewModels
{
    public class PopupMessageViewModel : PopupDialogViewModelBase
    {
        /// <summary>
        /// Text that will appear on the "Ok" button that closes the popup
        /// </summary>
        public string ConfirmationBoxText { get; set; }

        /// <summary>
        /// Message that will be shown in the body of the popup
        /// </summary>
        public string Message { get; set; }

        public PopupMessageViewModel()
        {
            Type = PopupType.Message;
        }
    }
}
