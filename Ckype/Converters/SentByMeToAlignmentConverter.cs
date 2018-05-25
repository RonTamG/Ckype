﻿using System;
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
    public class SentByMeToAlignmentConverter : BaseValueConverter<SentByMeToAlignmentConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? HorizontalAlignment.Right : HorizontalAlignment.Left;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
