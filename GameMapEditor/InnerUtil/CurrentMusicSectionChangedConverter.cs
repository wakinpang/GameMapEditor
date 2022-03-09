using GalaSoft.MvvmLight.Command;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GameMapEditor.InnerUtil
{
    public class CurrentMusicSectionChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            int SoundId = (int)value;
            return SoundId.ToString() + ".mp3";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            string SoundId = (string)value;
            int result = 0;
            if (int.TryParse(SoundId.Replace(".mp3", ""), out result)) {
                return result;
            }
            return result;
        }
    }
}
