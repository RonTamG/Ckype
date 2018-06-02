using Ckype.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for ChatPageView.xaml
    /// </summary>
    public partial class ChatPageView : Page
    {
        public ChatPageView()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  Handles Enter key press to send or add new line according to situation
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">Key pressed</param>
        private void MessageText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textbox = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    var index = textbox.CaretIndex;

                    textbox.Text = textbox.Text.Insert(index, Environment.NewLine);

                    textbox.CaretIndex = index + Environment.NewLine.Length;
                }
                else
                    (DataContext as ChatPageViewModel).AddMessage();

                e.Handled = true;
            }
        }
    }
}
