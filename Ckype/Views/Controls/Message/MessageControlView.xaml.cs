using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ckype.Views
{
    /// <summary>
    /// Interaction logic for MessageControlView.xaml
    /// </summary>
    public partial class MessageControlView : UserControl
    {
        public MessageControlView()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (!floatingTip.IsOpen) { floatingTip.IsOpen = true; }

            Point currentPos = e.GetPosition(rect);

            // The + 20 part is so your mouse pointer doesn't overlap.
            floatingTip.HorizontalOffset = currentPos.X + 20;
            floatingTip.VerticalOffset = currentPos.Y;
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            floatingTip.IsOpen = false;
        }
    }
}
