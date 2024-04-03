using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVMExample.Converters
{
    public class FlagCheckboxContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFlagRaised = (bool)value;
            return isFlagRaised ? "Lower Flag" : "Raise Flag";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
