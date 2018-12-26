using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using BE;

namespace PLWPF.Converters
{
    public class LessonAndTypeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lesson = value as List<LessonsAndType>;
            return String.Join(", ", lesson.Select(x => x.License.ToString())).TrimEnd(',', ' ');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
