using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace MapEditorControl.InnerUtil
{
    public class npcTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo) {
            if ((NpcType)value == NpcType.Person) {
                return "NPC";
            }

            if ((NpcType)value == NpcType.StayPoint) {
                return "挂机点";
            }

            if ((NpcType)value == NpcType.Telereport) {
                return "传送点";
            }

            if ((NpcType)value == NpcType.NeutralTele)
            {
                return "中立传送点";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo) {
            throw new NotImplementedException();
        }
    }
}
