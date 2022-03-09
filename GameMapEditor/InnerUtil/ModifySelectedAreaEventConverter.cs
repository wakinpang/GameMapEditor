using GalaSoft.MvvmLight.Command;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GameMapEditor.InnerUtil
{
    class ModifySelectedAreaEventConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var arg = value as ModifySelectedAreaEventArgs;
            return arg.Area;
        }
    }
}
