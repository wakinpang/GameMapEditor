using GalaSoft.MvvmLight.Command;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.InnerUtil 
{
    class SelectedHistoryPathConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var arg = value as SelectedHistoryPathArgs;
            return arg.HistoryPath;
        }
    }
}
