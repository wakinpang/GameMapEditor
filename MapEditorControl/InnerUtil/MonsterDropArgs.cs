using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class MonsterDropArgs : RoutedEventArgs
    {
        public DragEventArgs Args { get; set; }

        public MonsterDropArgs(RoutedEvent routedEvent) : base(routedEvent)
        {

        }
    }
}
