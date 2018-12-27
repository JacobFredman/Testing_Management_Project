using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Linq;
using BE;

namespace PLWPF.Converters
{
    /// <summary>
    /// Convert the lessons to a string of license types to show on grid
    /// </summary>
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
