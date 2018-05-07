﻿using Caliburn.Micro;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Ckype.ViewModels
{
    public class ShellViewModel : BaseViewModel, IViewAware
    {
        public StartPageViewModel PageViewModel { get; set; } 

        public ShellViewModel()
        {
            PageViewModel = new StartPageViewModel();
        }

        #region Public Methods

        /// <summary>
        /// Maximize the shell window
        /// </summary>
        public void Maximize()
        {
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// Minimize the shell window
        /// </summary>
        public void Minimize()
        {
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Close the shell window
        /// </summary>
        public void Close()
        {
            window.Close();
        }

        #endregion

        #region IViewAware Implementation

        public event EventHandler<ViewAttachedEventArgs> ViewAttached;
        private Window window;
        public void AttachView(object view, object context = null)
        {
            window = view as Window;
            if (ViewAttached != null)
                ViewAttached(this, new ViewAttachedEventArgs() { Context = context, View = view });
        }

        public object GetView(object context = null)
        {
            return window;
        } 

        #endregion
    }
}
