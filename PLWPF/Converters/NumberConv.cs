using System;
using System.Globalization;
using System.Windows.Data;

namespace PLWPF.Converters
{
    /// <summary>
    ///     Convert Nullable number to not Nullable number
    /// </summary>
    public class NumberConv : IValueConverter
    {
        /// <summary>
        ///     Don't change anything
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        ///     if value is null return 0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? 0 : value;
        }
    }
}