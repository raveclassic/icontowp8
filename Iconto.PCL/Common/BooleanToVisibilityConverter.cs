using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Iconto.PCL.Common
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //  If the data isn't a bool, bail.
            if (value is bool == false)
                return null;

            //  Cast the data.
            bool boolean = (bool)value;

            //  If we have the invert string, return the inverted value.
            if (parameter != null && parameter.ToString() == "Invert")
            {
                return boolean ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return boolean ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
