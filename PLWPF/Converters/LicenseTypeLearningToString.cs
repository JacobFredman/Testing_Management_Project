using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using BE;

namespace PLWPF.Converters
{
    /// <summary>
    ///     Convert the lessons to a string of license types to show on grid
    /// </summary>
    internal class LicenseTypeLearningToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Join(" ", value as List<LicenseType>);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}