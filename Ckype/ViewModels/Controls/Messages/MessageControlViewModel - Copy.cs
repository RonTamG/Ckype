
using Ckype.Interfaces;
using System;

namespace Ckype.ViewModels
{
    /// <summary>
    /// Chat message attachment for images.
    /// </summary>
    public class MessageControlImageAttachmentViewModel : BaseViewModel, IMessage
    {
        /// <summary>
        /// The title ffor this image file
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// The path to file on the computer
        /// </summary>
        public string LocalFilePath { get; set; }
    }
}
