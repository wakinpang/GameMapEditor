using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MapEditorControl.InnerUtil
{
    class ScrollEventConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var source = parameter as ScrollViewer;
            var args = (ScrollChangedEventArgs)value;

            return args;
        }
    }
}
