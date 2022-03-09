using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MapEditorControl.InnerUtil
{
    class ValueConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)(value);
            return d / 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)(value);
            return d * 100;
        }
    }
}
