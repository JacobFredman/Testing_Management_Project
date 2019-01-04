using System;
using System.Globalization;
using System.Windows.Data;

namespace PLWPF.Converters
{
    public class MetersToKM : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString(), NumberStyles.AllowExponent) / 1000;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString()) * 1000;
        }
    }
}