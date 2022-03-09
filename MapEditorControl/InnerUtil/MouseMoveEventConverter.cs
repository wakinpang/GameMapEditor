using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapEditorControl.InnerUtil
{
    class MouseMoveEventConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var control = parameter as Canvas;
            var args = (MouseEventArgs)value;

            var point = args.GetPosition(control);

            return point;
        }

    }
}