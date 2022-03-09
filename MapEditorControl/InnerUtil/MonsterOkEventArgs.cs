using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class MonsterOkEventArgs : RoutedEventArgs
    {
        public OkTypes Type { get; set; }

        public MonsterOkEventArgs(RoutedEvent routedEvent) : base(routedEvent) {

        }
    }
}
