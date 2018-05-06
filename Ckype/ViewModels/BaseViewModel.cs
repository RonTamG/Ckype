using PropertyChanged;
using System.ComponentModel;

namespace Ckype
{
    /// <summary>
    /// View model that implements the base requirements of all view models
    /// Fires propery changed event when needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that's fired
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };
    }
}
