using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight.Command;

namespace GameMapEditor.InnerUtil
{
    public class CurrentMonsterChangedConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter) {
            return (value as CurrentMonsterChangedEventArgs).CurrentMonsterSection;
        }
    }
}
