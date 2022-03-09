using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class CurrentMapSectionChangedEventArgs : RoutedEventArgs
    {
        public MapSection CurrentMapSection { get; set;}

        public CurrentMapSectionChangedEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {

        }
    }
}
