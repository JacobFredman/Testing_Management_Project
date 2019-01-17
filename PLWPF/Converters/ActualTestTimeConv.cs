using System;
using System.Globalization;
using System.Windows.Data;

namespace PLWPF.Converters
{
    /// <summary>
    ///     Show the actual test time only if it is set (not min value)
    /// </summary>
    public class ActualTestTimeConv : IValueConverter
    {
        /// <summary>
        ///     Show the actual test time only if it is set (not min value)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((DateTime) value == DateTime.MinValue) return "";
            return ((DateTime) value).ToString("g");
        }

        /// <summary>
        ///     Not Implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}