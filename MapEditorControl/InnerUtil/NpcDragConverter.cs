using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Controls;

namespace MapEditorControl.InnerUtil
{
    public class NpcDragConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter) {
            DragNpcData data = new DragNpcData();
            data.Args = value as MouseEventArgs;
            data.Source = parameter as Image;
            return data;
        }
    }
}
