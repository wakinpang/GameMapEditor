using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil.MapEditorCommand
{
    public static class MapEditorCommandManager
    {
        // command max recorded items.
        private const int COMMAND_SIZE = 15;
        private static LinkedList<IMapEditorCommand> _sCommands = new LinkedList<IMapEditorCommand>();

        private static int _sCurrentIndex = -1;

        public static void AddCommand(IMapEditorCommand newCommand)
        {
            if(_sCurrentIndex < COMMAND_SIZE - 1)
            {
                // if not newest command
                if(_sCurrentIndex < _sCommands.Count - 1)
                {
                    while (_sCurrentIndex + 1 < _sCommands.Count)
                    {
                        var command = _sCommands.ElementAt(_sCurrentIndex + 1);
                        _sCommands.Remove(command);
                    }
                }

                _sCommands.AddLast(newCommand);
                ++_sCurrentIndex;
            }
            else
            {
                _sCommands.RemoveFirst();
                _sCommands.AddLast(newCommand);
            }
        }

        public static void ReDo()
        {
            if (_sCurrentIndex < _sCommands.Count - 1)
            {
                var command = _sCommands.ElementAt(++_sCurrentIndex);
                command.Do();
            }
        }

        public static void Undo()
        {
            if(_sCurrentIndex >= 0)
            {
                var command = _sCommands.ElementAt(_sCurrentIndex--);
                command.Undo();
            }
        }
    }
}
