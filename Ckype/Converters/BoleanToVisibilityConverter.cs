using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ckype.Converters
{
    /// <summary>
    /// A converter the takes a boolean
    /// returns visible if true and false otherwise
    /// if the parameter isnt null reverses.
    /// </summary>
    public class BoleanToVisibilityConverter : BaseValueConverter<BoleanToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;            
            else
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
