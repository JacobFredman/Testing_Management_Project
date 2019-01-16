using System;
using System.Globalization;
using System.Windows.Data;

namespace PLWPF.Converters
{
    /// <summary>
    ///     Convert meters to Km
    /// </summary>
    public class MetersToKM : IValueConverter
    {
        /// <summary>
        ///     Convert meters to Km
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString(), NumberStyles.AllowExponent) / 1000;
        }

        /// <summary>
        ///     Convert Km to meters
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString()) * 1000;
        }
    }
}