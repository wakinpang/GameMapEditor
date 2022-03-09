using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil.MapEditorCommand
{
    using TileType = Byte;
    public class AreaOperateMapEditorCommand : IMapEditorCommand
    {
        private AreaPointState[] _newArea = null;
        private TileType[,] _tiles = null;

        private AreaPointState[] _preArea = null;

        public AreaOperateMapEditorCommand(SelectedAreaPoint[] newArea, TileType[,] tiles, TileType newType)
        {
            this._tiles = tiles;
            this._newArea = new AreaPointState[newArea.Length];
            this._preArea = new AreaPointState[newArea.Length];

            int count = 0;
            foreach(var p in newArea)
            {
                this._newArea[count].Type = newType;
                this._newArea[count].X = p.X;
                this._newArea[count].Y = p.Y;
                ++count;
            }
        }

        public void Do()
        {
            int count = 0;
            foreach(var p in _newArea)
            {
                this._preArea[count].X = p.X;
                this._preArea[count].Y = p.Y;
                this._preArea[count].Type = this._tiles[p.X, p.Y];

                this._tiles[p.X, p.Y] = p.Type;
                ++count;
            }

            Messenger.Default.Send<AreaPointState[]>(this._newArea, MapEditorControlMessageTokens.DrawAppointedArea);
            Messenger.Default.Send<TileType[,]>(this._tiles, MapEditorControlMessageTokens.NotifyAreaChanged);
        }

        public void Undo()
        {
            foreach(var p in _preArea)
            {
                this._tiles[p.X, p.Y] = p.Type;
            }

            Messenger.Default.Send<AreaPointState[]>(this._preArea, MapEditorControlMessageTokens.DrawAppointedArea);
            Messenger.Default.Send<TileType[,]>(this._tiles, MapEditorControlMessageTokens.NotifyAreaChanged);
        }
    }
}
