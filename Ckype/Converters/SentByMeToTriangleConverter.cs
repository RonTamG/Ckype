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
    /// A converter the takes a boolean if I sent the message
    /// and aligns the messages to the right if true.
    /// </summary>
    public class SentByMeToTriangleConverter : BaseValueConverter<SentByMeToTriangleConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "M 0,0 L 20,10 L 20,0 L 0,0" : "M 0,0 L 0,10 L 20,0 L 0,0";

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
