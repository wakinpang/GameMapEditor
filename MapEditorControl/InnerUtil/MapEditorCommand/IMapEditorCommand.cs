using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil.MapEditorCommand
{
    public interface IMapEditorCommand
    {
        void Undo();
        void Do();
    }
}
