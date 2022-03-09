using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class SelectedHistoryPathArgs : RoutedEventArgs
    {
        public string HistoryPath { get; set; }

        public SelectedHistoryPathArgs(RoutedEvent routedEvent) : base(routedEvent)
        {

        }
    }
}
