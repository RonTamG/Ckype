
using Ckype.Interfaces;
using System;
using System.IO;
using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Diagnostics;

namespace Ckype.ViewModels
{
    /// <summary>
    /// An attachment to a message.
    /// Contains everything related to files.
    /// </summary>
    public class MessageControlFileAttachmentViewModel : BaseViewModel
    {
        /// <summary>
        /// name of the image file
        /// </summary>
        public string FileName { get { return Path.GetFileName(LocalFilePath); } }

        /// <summary>
        /// The size of the image file
        /// </summary>
        public long FileSize
        {
            get
            {
                if (LocalFilePath == null)
                    return 0;
                FileInfo file = new FileInfo(LocalFilePath);
                return file.Length;
            }
        }

        /// <summary>
        /// This file type's windows icon
        /// </summary>
        public ImageSource WinIcon
        {
            get
            {
                if (LocalFilePath == null)
                    return null;
                Icon icon = Icon.ExtractAssociatedIcon(LocalFilePath);
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                return imageSource;
            }
        }

        /// <summary>
        /// The files extention (.jpg, .mp3, .pdf ...)
        /// </summary>
        public string Extension { get { return Path.GetExtension(LocalFilePath); } }

        /// <summary>
        /// the local path to the image file
        /// </summary>
        public string LocalFilePath { get; set; }

    }
}
