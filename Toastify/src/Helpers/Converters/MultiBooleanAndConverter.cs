using System;
using System.Linq;
using System.Windows.Data;

namespace Toastify.Helpers.Converters
{
    public class MultiBooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.All(value => !(value is bool) || (bool)value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("MultiBooleanAndConverter is a OneWay converter.");
        }
    }
}