using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ckype.AttachedProperties
{
    /// <summary>
    /// Creates a clipping region from the parent <see cref="Border"/> <see cref="CornerRadius"/>
    /// </summary>
    public class ClipFromBorderProperty : BaseAttachedProperty<ClipFromBorderProperty, bool>
    {
        #region Private members

        /// <summary>
        /// Called when the parent border first loads
        /// </summary>
        private RoutedEventHandler mBorder_Loaded;

        /// <summary>
        /// Called when the border's size changes
        /// </summary>
        private SizeChangedEventHandler mBorder_SizeChanged;

        #endregion
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get self
            var self = (sender as FrameworkElement);

            // Check we have a parent border
            if (!(self.Parent is Border))
            {
                Debugger.Break();
                return;
            }

            // Setup loaded event
            mBorder_Loaded = (s1, e1) => Border_OnChange(s1, e1, self);

            // Setup size changed event
            mBorder_SizeChanged = (s1, e1) => Border_OnChange(s1, e1, self);

            Border border = (Border)self.Parent;
            // If true, hook into events
            if ((bool)e.NewValue)
            {
                border.Loaded += mBorder_Loaded;
                border.SizeChanged += mBorder_SizeChanged;
            }
            // Otherwise, unhook
            else
            {
                border.Loaded -= mBorder_Loaded;
                border.SizeChanged -= mBorder_SizeChanged;
            }
        }

        /// <summary>
        /// Called when the border is loaded and changes it's size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="child">The child element</param>
        private void Border_OnChange(object sender, RoutedEventArgs e, FrameworkElement child)
        {
            // Get Border
            var border = (Border)sender;

            // Check we have an actual size
            if (border.ActualWidth == 0 && border.ActualHeight == 0)
                return;

            // Setup the new child clipping area
            var rect = new RectangleGeometry();
            // Match the corner radius with the border's corner radius
            rect.RadiusX = rect.RadiusY = Math.Max(0, border.CornerRadius.TopLeft - (border.BorderThickness.Left * 0.5));
            // Set rectancle size to match child's actual size
            rect.Rect = new Rect(child.RenderSize);

            // Assign clipping area to the child
            child.Clip = rect;
        }
    }
}
