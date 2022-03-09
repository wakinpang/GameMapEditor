using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MapEditorControl.InnerUtil
{
    class HistoryStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<string> converted = null;
            if (value != null)
            {
                var newList = from r in (value as ObservableCollection<HistorySection>)
                              select r.ProjectPath;
                converted = new ObservableCollection<string>(newList.ToList<string>()); ;
            }

            return converted;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
