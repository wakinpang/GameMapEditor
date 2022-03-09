using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MapEditorControl.InnerUtil
{
    public class SelectedToBorderVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (bool)value;

            //return visible ? Brushes.White : Brushes.Transparent;
            return visible ? Colors.White : Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
