using Ckype.ViewModels;
using System.Windows;

namespace Ckype.Views
{
    /// <summary>
    /// Interaction logic for PopupMessageView.xaml
    /// </summary>
    public partial class PopupMessageView : Window
    {
        public PopupMessageView(PopupMessageViewModel VM)
        {
            InitializeComponent();

            DataContext = VM;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
