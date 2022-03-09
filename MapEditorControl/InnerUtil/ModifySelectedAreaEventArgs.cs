using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    using TileType = Byte;
    public class ModifySelectedAreaEventArgs : RoutedEventArgs
    {
        public TileType[,] Area { get; set; }

        public ModifySelectedAreaEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }
    }
}
