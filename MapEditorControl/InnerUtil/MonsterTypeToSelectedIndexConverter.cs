using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MapEditorControl.InnerUtil
{
    class MonsterTypeToSelectedIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var objIndex = (short)value;

            switch(objIndex)
            {
                case 5:
                    return 0;
                case 6:
                    return 1;
                case 8:
                    return 2;
                case 10:
                    return 3;
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var controlIndex = (int)value;

            switch (controlIndex)
            {
                case 0:
                    return 5;
                case 1:
                    return 6;
                case 2:
                    return 8;
                case 3:
                    return 10;
            }
            return -1;

        }
    }
}
