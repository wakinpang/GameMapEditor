using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class CurrentMonsterChangedEventArgs : RoutedEventArgs
    {
        public MonsterSection CurrentMonsterSection { get; set; }

        public CurrentMonsterChangedEventArgs(RoutedEvent routedEvent) : base(routedEvent) {

        }
    }
}
