using System;
using System.Globalization;
using System.Windows.Data;

namespace PLWPF.Converters
{
    public class ActualTestTimeConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((DateTime)value == DateTime.MinValue) return "";
            else return ((DateTime) value).ToString("g");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
