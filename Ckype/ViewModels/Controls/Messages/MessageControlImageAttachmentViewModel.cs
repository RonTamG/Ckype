﻿
using Ckype.Interfaces;
using System;
using System.IO;

namespace Ckype.ViewModels
{
    public class MessageControlImageAttachmentViewModel : BaseViewModel
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
        /// the local path to the image file
        /// </summary>
        public string LocalFilePath { get; set; }
    }
}
