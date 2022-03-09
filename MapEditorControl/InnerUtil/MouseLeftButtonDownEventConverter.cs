using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditorControl.InnerUtil
{
    class MouseLeftButtonDownEventConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var source = parameter as Canvas;
            var args = (MouseButtonEventArgs)value;

            var point = args.GetPosition(source);

            return point;
        }
    }
}
