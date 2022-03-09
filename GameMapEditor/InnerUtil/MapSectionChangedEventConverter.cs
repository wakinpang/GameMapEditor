using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEditorControl.InnerUtil;

namespace GameMapEditor.InnerUtil
{
    class MapSectionChangedEventConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = value as CurrentMapSectionChangedEventArgs;
            return args.CurrentMapSection;
        }
    }
}
