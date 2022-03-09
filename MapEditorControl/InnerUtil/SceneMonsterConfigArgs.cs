using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class SceneMonsterConfigArgs : RoutedEventArgs
    {
        public SceneMonsterPOJO PreSceneMonsterPOJO { get; set; }
        public SceneMonsterPOJO CurrentSceneMonsterPOJO { get; set; }

        public SceneMonsterConfigArgs() : base()
        {

        }
    }
}
