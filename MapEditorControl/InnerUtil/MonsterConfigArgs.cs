using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class MonsterConfigArgs : RoutedEventArgs
    {
        public MonsterSection BackCurrentMonster { get; set; }
        public MonsterSection CurrentMonster { get; set; }

        public MonsterConfigArgs() : base() {

        }
    }
}
