
using Ckype.Interfaces;
using System;

namespace Ckype.ViewModels
{
    public class MessageControlViewModel : BaseViewModel, IMessage
    {
        /// <summary>
        /// Name of the person who sent the message.
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Text message, file.
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// The text or file.
        /// </summary>
        public object Content { get; set; }


        /// <summary>
        /// True if this message was sent by me.
        /// </summary>
        public bool SentByMe { get; set; }

        /// <summary>
        /// True if this item is currently selected.
        /// </summary>
        public bool IsSelected { get; set; }


        /// <summary>
        /// The time at which the messave was sent.
        /// </summary>
        public DateTimeOffset MessageSentTime { get; set; }

        /// <summary>
        /// Image attachment
        /// </summary>
        public MessageControlImageAttachmentViewModel ImageAttachment { get; set; }

        /// <summary>
        /// True if the message contains text
        /// </summary>
        public bool HasMessage => Content != null;

        /// <summary>
        /// True if the message contains an image attachment
        /// </summary>
        public bool HasImageAttachment => ImageAttachment != null;

        public bool HasFileAttachment = false; // CHANGE LATER TESTTTTT


    }
}
