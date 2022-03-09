using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil
{
    public class MusicCheckedConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter) {
            return parameter as MusicSection;
        }
    }
}
