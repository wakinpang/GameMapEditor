using GalaSoft.MvvmLight;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.Model
{
    using TileType = Byte;

    public class MapEditorAndNavigationModel : ObservableObject
    {
        private static MapEditorAndNavigationModel _instance = new MapEditorAndNavigationModel();

        public static MapEditorAndNavigationModel Instance()
        {
            return _instance;
        }

        private double _zoom = 1.0;

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                RaisePropertyChanged(() => Zoom);
            }
        }

        private String _backgroundSource = "";

        public String BackgroundSource
        {
            get { return _backgroundSource; }
            set
            {
                _backgroundSource = value;
                RaisePropertyChanged(() => BackgroundSource);
            }
        }

        private double _contentWidthRatio = 0.0;

        public double ContentWidthRatio
        {
            get { return _contentWidthRatio; }
            set
            {
                _contentWidthRatio = value;
                RaisePropertyChanged(() => ContentWidthRatio);
            }
        }

        private double _contentHeightRatio = 0.0;

        public double ContentHeightRatio
        {
            get { return _contentHeightRatio; }
            set
            {
                _contentHeightRatio = value;
                RaisePropertyChanged(() => ContentHeightRatio);
            }
        }

        private double _contentVerticalOffsetRatio = 0.0;

        public double ContentVerticalOffsetRatio
        {
            get { return _contentVerticalOffsetRatio; }
            set
            {
                _contentVerticalOffsetRatio = value;
                RaisePropertyChanged(() => ContentVerticalOffsetRatio);
            }
        }

        private double _contentHorizentalOffsetRatio = 0.0;

        public double ContentHorizentalOffsetRatio
        {
            get { return _contentHorizentalOffsetRatio; }
            set
            {
                _contentHorizentalOffsetRatio = value;
                RaisePropertyChanged(() => ContentHorizentalOffsetRatio);
            }
        }

        private int _tileWidth = 25;

        public int TileWidth
        {
            get { return _tileWidth; }
            set
            {
                _tileWidth = value;
                RaisePropertyChanged(() => TileWidth);
            }
        }

        private int _tileHeight = 25;

        public int TileHeight 
        {
            get { return _tileHeight; }
            set
            {
                _tileHeight = value;
                RaisePropertyChanged(() => TileHeight);
            }
        }

        private bool _valid = false;

        public bool Valid
        {
            get { return _valid; }
            set
            {
                if (value == _valid)
                {
                    return;
                }
                _valid = value;
                RaisePropertyChanged(() => Valid);
            }
        }

        private int _mapPixelWidth;

        public int MapPixelWidth
        {
            get { return _mapPixelWidth; }
            set
            {
                if (value == _mapPixelWidth)
                {
                    return;
                }
                _mapPixelWidth = value;
                RaisePropertyChanged(() => MapPixelWidth);
            }
        }

        private int _mapPixelHeight;

        public int MapPixelHeight
        {
            get { return _mapPixelHeight; }
            set
            {
                if (value == _mapPixelHeight)
                {
                    return;
                }
                _mapPixelHeight = value;
                RaisePropertyChanged(() => MapPixelHeight);
            }
        }

        private int _bornPointX;

        public int BornPointX
        {
            get { return _bornPointX; }
            set
            {
                if (value == _bornPointX)
                {
                    return;
                }
                _bornPointX = value;

                //****
                LibraryControlModel.Instance().CurrentMapSection.PosX = value;
                //****

                RaisePropertyChanged(() => BornPointX);
            }
        }

        private int _bornPointY;

        public int BornPointY
        {
            get { return _bornPointY; }
            set
            {
                if (value == _bornPointY)
                {
                    return;
                }
                _bornPointY = value;

                //****
                LibraryControlModel.Instance().CurrentMapSection.PosY = value;
                //****

                RaisePropertyChanged(() => BornPointY);
            }
        }

        public TileType[,] MapTiles { get; set; }

        private ObservableCollection<MapObjectSprite> _mapObjectCollection = new ObservableCollection<MapObjectSprite>();

        public ObservableCollection<MapObjectSprite> MapObjectCollection
        {
            get { return _mapObjectCollection; }
            set
            {
                if (value == _mapObjectCollection)
                {
                    return;
                }
                _mapObjectCollection = value;
                RaisePropertyChanged(() => MapObjectCollection);
            }
        }

        private MonsterDictoryAndMonsterObject _currentMonsters;

        public MonsterDictoryAndMonsterObject CurrentMonsters
        {
            get { return _currentMonsters; }
            set
            {
                if (value == _currentMonsters)
                {
                    return;
                }
                _currentMonsters = value;
                RaisePropertyChanged(() => CurrentMonsters);
            }
        }

        private NpcDictoryAndNpcObject _currentNpcs;

        public NpcDictoryAndNpcObject CurrentNpcs
        {
            get { return _currentNpcs; }
            set
            {
                if (value == _currentNpcs)
                {
                    return;
                }
                _currentNpcs = value;
                RaisePropertyChanged(() => CurrentNpcs);
            }
        }


        //public MonsterSection CurrentMonster { get; set; }
        //public SceneMonsterPOJO CurrentSceneMonster { get; set; }
        //public NpcSection CurrentNPC { get; set; }

        private MonsterSection _currentMonster;

        public MonsterSection CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if (value == _currentMonster)
                {
                    return;
                }
                _currentMonster = value;
                RaisePropertyChanged(() => CurrentMonster);
            }
        }

        private MonsterSection _backCurrentMonster;

        public MonsterSection BackCurrentMonster
        {
            get { return _backCurrentMonster; }
            set
            {
                if (value == _backCurrentMonster)
                {
                    return;
                }
                _backCurrentMonster = value;
                RaisePropertyChanged(() => BackCurrentMonster);
            }
        }

        private SceneMonsterPOJO _currentSceneMonster;

        public SceneMonsterPOJO CurrentSceneMonster
        {
            get { return _currentSceneMonster; }
            set
            {
                if (value == _currentSceneMonster)
                {
                    return;
                }
                _currentSceneMonster = value;
                RaisePropertyChanged(() => CurrentSceneMonster);
            }
        }

        private SceneMonsterPOJO _preSceneMonsterPOJO;

        public SceneMonsterPOJO PreSceneMonsterPOJO
        {
            get { return _preSceneMonsterPOJO; }
            set
            {
                if (value == _preSceneMonsterPOJO)
                {
                    return;
                }
                _preSceneMonsterPOJO = value;
                RaisePropertyChanged(() => PreSceneMonsterPOJO);
            }
        }


        private NpcSection _currentNpc;

        public NpcSection CurrentNpc
        {
            get { return _currentNpc; }
            set
            {
                if (value == _currentNpc)
                {
                    return;
                }
                _currentNpc = value;
                RaisePropertyChanged(() => CurrentNpc);
            }
        }


    }
}
