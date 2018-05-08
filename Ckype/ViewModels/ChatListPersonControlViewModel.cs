using Caliburn.Micro;
using System.Diagnostics;

namespace Ckype.ViewModels
{
    public class ChatListPersonControlViewModel
    {
        #region Public Properties

        /// <summary>
        /// Nickname for the client
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// Bool value if this client is the selected client
        /// </summary>
        public bool Selected { get; set; }

        #endregion

        #region Public Methods

        public void OpenMessageBox()
        {

        }

        #endregion

        #region Constructors

        public ChatListPersonControlViewModel()
        {
        }

        /// <summary>
        /// Constructor with paramter input
        /// </summary>
        /// <param name="nickname">Nickname of the client</param>
        /// <param name="selected">If the current client is selected or the not Defaults to false</param>
        public ChatListPersonControlViewModel(string nickname, bool selected = false)
        {
            Nickname = nickname;
            Selected = selected;
        } 
        #endregion
    }
}
