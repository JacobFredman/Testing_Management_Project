using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using BE;

namespace PLWPF.Converters
{
    /// <summary>
    ///     Convert the lessons to a string of license types to show on grid
    /// </summary>
    public class LessonAndTypeToString : IValueConverter
    {
        /// <summary>
        ///     Convert the lessons to a string of license types to show on grid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lesson = value as List<TrainingDetails>;
            return string.Join(", ", lesson.Select(x => x.License.ToString())).TrimEnd(',', ' ');
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