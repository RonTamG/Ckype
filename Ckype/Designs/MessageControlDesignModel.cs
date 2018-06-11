
using Ckype.Interfaces;
using Ckype.ViewModels;
using System;

namespace Ckype.Designs
{
    public class MessageControlDesignModel : BaseViewModel, IMessage
    {

        public static MessageControlDesignModel Instance = new MessageControlDesignModel()
        {
            SenderName = "Ron",
            SentByMe = false,
            MessageSentTime = DateTimeOffset.Parse("11:45 PM"),
            FileAttachment = new MessageControlFileAttachmentViewModel
            {
                LocalFilePath = @"C:\Users\Owner\Desktop\Poker.jpg"
            }
        };

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
        /// File attachment
        /// </summary>
        public MessageControlFileAttachmentViewModel FileAttachment { get; set; }

        /// <summary>
        /// True if the message contains text
        /// </summary>
        public bool HasMessage => Content != null;

        /// <summary>
        /// True if the message contains an image attachment
        /// </summary>
        public bool HasImageAttachment
        {
            get
            {
                return HasFileAttachment && (FileAttachment.Extension == ".jpg" || FileAttachment.Extension == ".jpeg" || FileAttachment.Extension == ".png");
            }
        }

        /// <summary>
        /// True if the messsage contains a generic file, not an image.
        /// </summary>
        public bool HasGenericFileAttachment
        {
            get
            {
                return HasFileAttachment && !HasImageAttachment;
            }
        }

        /// <summary>
        /// True if the message contains any file
        /// </summary>
        public bool HasFileAttachment
        {
            get
            {
                return FileAttachment != null;
            }
        }
    }
}
