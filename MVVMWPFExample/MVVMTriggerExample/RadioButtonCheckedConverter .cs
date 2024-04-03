using System.Globalization;
using System.Windows.Data;

namespace MVVMExample
{
    public class RadioButtonCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the parameter matches the value, return true, indicating the radio button should be checked
            return value?.ToString().ToLower() == parameter?.ToString().ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the radio button is checked, return the parameter as the new value
            if ((bool)value)
                return parameter?.ToString();
            return null;
        }
    }
}
