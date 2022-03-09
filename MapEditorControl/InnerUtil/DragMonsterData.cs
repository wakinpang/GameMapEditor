using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace MapEditorControl.InnerUtil
{
    public class MonsterData
    {
        public TextBlock Source { get; set; }
        public MouseEventArgs Args { get; set; }
    }

    public class MonsterGiveFeedBackData
    {
        public MonsterSection Data { get; set; }
        public GiveFeedbackEventArgs Args { get; set; }
    }
}
