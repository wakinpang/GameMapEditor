using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using MapEditorControl.InnerUtil.MapEditorCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapEditorControl.ViewModel
{
    using TileType = Byte;
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MapEditorControlViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        private int _initialzeCount = 0;        
        private string _currentBackgroundPath = null;
        private int _cols = 0;
        private int _rows = 0;

        private string _backgroundSource;

        public string BackgroundSource
        {
            get { return _backgroundSource; }
            set
            {
                if (value == _backgroundSource)
                {
                    return;
                }
                _backgroundSource = value;

                RaisePropertyChanged(() => BackgroundSource);
            }
        }

        private double _backgroudPixelWidth;

        public double BackgroundPixelWidth
        {
            get { return _backgroudPixelWidth; }
            set
            {
                if (value == _backgroudPixelWidth)
                {
                    return;
                }
                _backgroudPixelWidth = value;
                RaisePropertyChanged(() => BackgroundPixelWidth);
            }
        }

        private double _backgroundPixelHeight;

        public double BackgroundPixelHeight
        {
            get { return _backgroundPixelHeight; }
            set
            {
                if (value == _backgroundPixelHeight)
                {
                    return;
                }
                _backgroundPixelHeight = value;
                RaisePropertyChanged(() => BackgroundPixelHeight);
            }
        }

        private double _zoom = 1.0;

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                if (value == _zoom)
                {
                    return;
                }
                _zoom = value;

                RaisePropertyChanged(() => Zoom);
            }
        }

        private int _tileWidth;

        public int TileWidth
        {
            get { return _tileWidth; }
            set
            {
                if (value == _tileWidth)
                {
                    return;
                }
                _tileWidth = value;

                RaisePropertyChanged(() => TileWidth);
            }
        }

        private int _tileHeight;

        public int TileHeight
        {
            get { return _tileHeight; }
            set
            {
                if (value == _tileHeight)
                {
                    return;
                }
                _tileHeight = value;

                RaisePropertyChanged(() => TileHeight);
            }
        }

        private BitmapImage _backgroundImage;

        public BitmapImage BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                if (value == _backgroundImage)
                {
                    return;
                }
                _backgroundImage = value;

                RaisePropertyChanged(() => BackgroundImage);
            }
        }

        private bool _vaild;

        public bool Vaild
        {
            get { return _vaild; }
            set
            {
                if (value == _vaild)
                {
                    return;
                }
                _vaild = value;

                Messenger.Default.Send<bool>(value, MapEditorControlMessageTokens.UpdateVaildFromViewModel);

                RaisePropertyChanged(() => Vaild);
            }
        }

        private bool _dragToolSelected;

        public bool DragToolSelected
        {
            get { return _dragToolSelected; }
            set
            {
                if (value == _dragToolSelected)
                {
                    return;
                }
                _dragToolSelected = value;
                RaisePropertyChanged(() => DragToolSelected);
            }
        }

        private bool _areaToolSelected;

        public bool AreaToolSelected
        {
            get { return _areaToolSelected; }
            set
            {
                if (value == _areaToolSelected)
                {
                    return;
                }
                _areaToolSelected = value;
                RaisePropertyChanged(() => AreaToolSelected);
            }
        }

        private bool _penToolSelected;

        public bool PenToolSelected
        {
            get { return _penToolSelected; }
            set
            {
                if (value == _penToolSelected)
                {
                    return;
                }
                _penToolSelected = value;
                RaisePropertyChanged(() => PenToolSelected);
            }
        }

        private bool _pointToolSelected;

        public bool PointToolSelected
        {
            get { return _pointToolSelected; }
            set
            {
                if (value == _pointToolSelected)
                {
                    return;
                }
                _pointToolSelected = value;
                RaisePropertyChanged(() => PointToolSelected);
            }
        }

        private bool _transparentSelected;

        public bool TransparentSelected
        {
            get { return _transparentSelected; }
            set
            {
                if (value == _transparentSelected)
                {
                    return;
                }
                _transparentSelected = value;
                RaisePropertyChanged(() => TransparentSelected);
            }
        }

        private bool _safety;

        public bool Safety
        {
            get { return _safety; }
            set
            {
                if (value == _safety)
                {
                    return;
                }
                _safety = value;
                RaisePropertyChanged(() => Safety);
            }
        }

        private bool _Fishing;

        public bool Fishing
        {
            get { return _Fishing; }
            set
            {
                if (value == _Fishing)
                {
                    return;
                }
                _Fishing = value;
                RaisePropertyChanged(() => Fishing);
            }
        }

        private bool _toolBarCanEdit;

        public bool ToolBarCanEdit
        {
            get { return _toolBarCanEdit; }
            set
            {
                if (value == _toolBarCanEdit)
                {
                    return;
                }
                _toolBarCanEdit = value;

                Messenger.Default.Send<bool>(value, MapEditorControlMessageTokens.UpdateToolBarCanEditFromViewModel);

                RaisePropertyChanged(() => ToolBarCanEdit);
            }
        }

        //ObservableCollection<MapObjectSprite> MapObjectSprites = null;
        private ObservableCollection<MapObjectSprite> _mapObjectSelection;

        public ObservableCollection<MapObjectSprite> MapObjectSelection
        {
            get { return _mapObjectSelection; }
            set
            {
                if (value == _mapObjectSelection)
                {
                    return;
                }
                _mapObjectSelection = value;
                RaisePropertyChanged(() => MapObjectSelection);
            }
        }

        private int _currentMapDI;

        public int CurrentMapID
        {
            get { return _currentMapDI; }
            set
            {
                if (value == _currentMapDI)
                {
                    return;
                }
                _currentMapDI = value;
                RaisePropertyChanged(() => CurrentMapID);
            }
        }


        //TileType[][] Tiles
        TileType[,] _tiles = null;

        // 自动机的状态
        private enum MapEditionState
        {
            StateInitialize,
            StateMouseLeftDown,

            StateMouseRightDown,
        }

        MapEditionState _currentState = MapEditionState.StateInitialize;

        bool _penInitialized = false;

        const int MAX_PENPOINTS = 999;
        SelectedAreaPoint[] _penPoints = new SelectedAreaPoint[MAX_PENPOINTS];
        int _penPointIndex = -1;

        public MapEditorControlViewModel()
        {

            Messenger.Default.Register<int>(this, MapEditorControlMessageTokens.UpdateCurrentMapIDFromView, (id) =>
            {
                CurrentMapID = id;
            });

            Messenger.Default.Register<string>(this, MapEditorControlMessageTokens.UpdateBackgroudMapFromView, (path) =>
            {

                if (Initialized())
                {
                    --_initialzeCount;
                }

                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ReinitializeView);

                _currentBackgroundPath = path;
                if (Initialized())
                {
                    UpdateBackgroundDisplay(path);
                }
                else
                {
                    ++_initialzeCount;
                    if (Initialized())
                    {
                        UpdateBackgroundDisplay(path);
                    }
                }

                // Trick
                ToolBarCanEdit = true;
            });

            Messenger.Default.Register<double>(this, MapEditorControlMessageTokens.UpdateZoomFromView, (zoom) =>
            {
                Zoom = zoom;
                if (Initialized())
                {
                    BackgroundPixelWidth = BackgroundImage.PixelWidth;
                    BackgroundPixelHeight = BackgroundImage.PixelHeight;

                    Messenger.Default.Send<MessageParameterTileDisplay>(new MessageParameterTileDisplay
                    {
                        PixelWidth = (int)BackgroundImage.PixelWidth,
                        PixelHeight = (int)BackgroundImage.PixelHeight,
                        TileWidth = this.TileWidth,
                        TileHeight = this.TileHeight,
                        Zoom = (double)this.Zoom,
                    }, MapEditorControlMessageTokens.DisplayTilesAndSelectedTile);
                }
                else
                {
                    ++_initialzeCount;
                    if (Initialized())
                    {
                        UpdateBackgroundDisplay(_currentBackgroundPath);
                    }
                }

            });

            Messenger.Default.Register<int>(this, MapEditorControlMessageTokens.UpdateTileWidthFromView, (tileWidth) =>
            {
                TileWidth = tileWidth;
                if (Initialized())
                {
                    Messenger.Default.Send<MessageParameterTileDisplay>(new MessageParameterTileDisplay
                    {
                        PixelWidth = (int)BackgroundImage.PixelWidth,
                        PixelHeight = (int)BackgroundImage.PixelHeight,
                        TileWidth = this.TileWidth,
                        TileHeight = this.TileHeight,
                        Zoom = (double)this.Zoom,
                    }, MapEditorControlMessageTokens.DisplayTilesAndSelectedTile);
                }
                else
                {
                    ++_initialzeCount;
                    if (Initialized())
                    {
                        UpdateBackgroundDisplay(_currentBackgroundPath);
                    }
                }

            });

            Messenger.Default.Register<int>(this, MapEditorControlMessageTokens.UpdateTileHeightFromView, (tileHeight) =>
            {
                TileHeight = tileHeight;
                if (Initialized())
                {
                    Messenger.Default.Send<MessageParameterTileDisplay>(new MessageParameterTileDisplay
                    {
                        PixelWidth = (int)BackgroundImage.PixelWidth,
                        PixelHeight = (int)BackgroundImage.PixelHeight,
                        TileWidth = this.TileWidth,
                        TileHeight = this.TileHeight,
                        Zoom = (double)this.Zoom,
                    }, MapEditorControlMessageTokens.DisplayTilesAndSelectedTile);
                }
                else
                {
                    ++_initialzeCount;
                    if (Initialized())
                    {
                        UpdateBackgroundDisplay(_currentBackgroundPath);
                    }
                }
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateDragToolSelectedFromView, (v) =>
            {
                DragToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateAreaToolSelectedFromView, (v) =>
            {
                AreaToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdatePenToolSelectedFromView, (v) =>
            {
                PenToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdatePointToolSelectedFromView, (v) =>
            {
                PointToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateTransparentSelectedFromView, (v) =>
            {
                TransparentSelected = v;
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateSafetyFromView, (v) =>
            {
                Safety = v;
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateFishingFromView, (v) =>
            {
                Fishing = v;
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.ArrangedFromView, (param) =>
            {
                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.DoArrangeFromViewModel);
            });

            Messenger.Default.Register<SelectedAreaPoint[]>(this, MapEditorControlMessageTokens.UpdateSelectedArea, (area) =>
            {
                var command = new AreaOperateMapEditorCommand(area, this._tiles, GenerateTileType());
                MapEditorCommandManager.AddCommand(command);
                command.Do();
            });

            Messenger.Default.Register<SelectedAreaPoint[]>(this, MapEditorControlMessageTokens.UpdateUnselectedArea, (area) =>
            {
                var command = new AreaOperateMapEditorCommand(area, this._tiles, Convert.ToByte("00000000", 2));
                MapEditorCommandManager.AddCommand(command);
                command.Do();
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.UpdateDefaultBornPointImageFromView, (dummy) => {
                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.UpdateBornPointFromViewModel);
            });

            Messenger.Default.Register<Point>(this, MapEditorControlMessageTokens.UpdateBornPointXFromView, (p) =>
            {
                UpdateWithClickedTileLogicPosition(p.X, p.Y);
            });

            Messenger.Default.Register<Point>(this, MapEditorControlMessageTokens.UpdateBornPointXFromView, (p) =>
            {
                UpdateWithClickedTileLogicPosition(p.X, p.Y);
            });

            Messenger.Default.Register<ObservableCollection<MapObjectSprite>>(this, MapEditorControlMessageTokens.SetNewObjectCollection, (list) =>
            {
                MapObjectSelection = list;

                Messenger.Default.Send<ObservableCollection<MapObjectSprite>>(list, MapEditorControlMessageTokens.DoMapObjectBind);
                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ShowNewObjects);
            });

            Messenger.Default.Register<IList>(this, MapEditorControlMessageTokens.ObjectCollectionAdded, (list) =>
            {
                var newList = new ObservableCollection<MapObjectSprite>();
                foreach (var obj in list)
                {
                    newList.Add(obj as MapObjectSprite);
                }

                Messenger.Default.Send<ObservableCollection<MapObjectSprite>>(newList, MapEditorControlMessageTokens.DoMapObjectBind);
                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ShowNewObjects);
            });

            Messenger.Default.Register<IList>(this, MapEditorControlMessageTokens.ObjectCollectionRemoved, (list) =>
            {
                Messenger.Default.Send<IList>(list, MapEditorControlMessageTokens.RemoveMapObject);
            });

            Messenger.Default.Register<IList>(this, MapEditorControlMessageTokens.ObjectCollectionReplaced, (list) =>
            {

            });

            Messenger.Default.Register<IList>(this, MapEditorControlMessageTokens.ObjectCollectionMoved, (list) =>
            {

            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.ObjectCollectionReseted, (dummy) =>
            {
                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ResetMapObject);
            });

            Messenger.Default.Register<MonsterDictoryAndMonsterObject>(this, MapEditorControlMessageTokens.UpdateCurrentSceneMonsters, (sceneMonsters) =>
            {
                char[] splitChars = new char[] { ';' };
                char[] posSplitChars = new char[] { ',' };
                var newList = new ObservableCollection<MapObjectSprite>();

                foreach (var obj in sceneMonsters.Info)
                {
                    var monsterScene = obj.SceneBaseObject as SceneMonsterPOJO;
                    if(monsterScene.SceneID != CurrentMapID)
                    {
                        continue;
                    }
                    string posString = monsterScene.PosArr;
                    var posArr = posString.Split(splitChars);
                    foreach (var pos in posArr)
                    {

                        if(pos == "")
                        {
                            continue;
                        }

                        var position = pos.Split(posSplitChars);
                        int x = int.Parse(position[0]);
                        int y = int.Parse(position[1]);

                        newList.Add(new MapObjectSprite()
                        {
                            ID = monsterScene.SceneMonsterID,
                            Name = monsterScene.Name,
                            X = x,
                            Y = y,
                            SpriteImage = sceneMonsters.MonsterDictory + "\\" + monsterScene.Style + ".png",

                            ObjectBase = obj.BaseObject,
                            SceneObjectBase = obj.SceneBaseObject,
                        });
                    }

                }

                foreach(var e in newList)
                {
                    MapObjectSelection.Add(e);
                }

                //Messenger.Default.Send<ObservableCollection<MapObjectSprite>>(newList, MapEditorControlMessageTokens.DoMapObjectBind);
                //Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ShowNewObjects);
            });

            Messenger.Default.Register<NpcDictoryAndNpcObject>(this, MapEditorControlMessageTokens.UpdateCurrentNpcs, (npcs) =>
            {
                var newList = new ObservableCollection<MapObjectSprite>();
                foreach (var npc in npcs.Info)
                {
                    var baseNpc = npc.BaseObject as NpcSection;

                    if(baseNpc.MapId != CurrentMapID)
                    {
                        continue;
                    }

                    newList.Add(new MapObjectSprite()
                    {
                        X = baseNpc.PosX,
                        Y = baseNpc.PosY,
                        ID = baseNpc.NPCId,
                        Name = baseNpc.Name,
                        SpriteImage = npcs.NpcDictory + "\\" + baseNpc.Style + ".png",
                        ObjectBase = baseNpc,
                        SceneObjectBase = null,
                    });
                }

                foreach (var e in newList)
                {
                    MapObjectSelection.Add(e);
                }

                //Messenger.Default.Send<ObservableCollection<MapObjectSprite>>(newList, MapEditorControlMessageTokens.DoMapObjectBind);
                //Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ShowNewObjects);
            });

            //Messenger.Default.Register<SceneMonsterPOJO>(this, MapEditorControlMessageTokens.UpdateCurrentSceneMonsterFromView, (pojo) =>
            //{
            //    _selec
            //});

            UpdateContentRate = new RelayCommand<ScrollChangedEventArgs>((args) =>
            {
                Messenger.Default.Send<ScrollChangedEventArgs>(args, MapEditorControlMessageTokens.UpdateOffsetRatioFromMapEditorViewModel);
            });

            LeftMouseDownHandler = new RelayCommand<Point>((pos) =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                _currentState = MapEditionState.StateMouseLeftDown;

                if (DragToolSelected)
                {
                    Messenger.Default.Send<Point>(pos, MapEditorControlMessageTokens.SelectMapObject);
                    ToolBarCanEdit = false;
                }
                else if (AreaToolSelected)
                {
                    Messenger.Default.Send<SelectedAreaPoint>(GetLogicPoint(pos.X, pos.Y), MapEditorControlMessageTokens.SetInitialMousePointLeftButtonDown);
                    ToolBarCanEdit = false;
                }
                else if (PenToolSelected)
                {
                    ;
                }
                else if (PointToolSelected)
                {
                    ;
                }
            });

            LeftMouseUpHandler = new RelayCommand<Point>((pos) =>
            {
                if (!this.Vaild)
                {
                    return;
                }
                
                if (DragToolSelected)
                {
                    if (_currentState == MapEditionState.StateMouseLeftDown)
                    {
                        _currentState = MapEditionState.StateInitialize;
                        ToolBarCanEdit = true;
                    }
                }
                else if (AreaToolSelected)
                {
                    if (_currentState == MapEditionState.StateMouseLeftDown)
                    {
                        Messenger.Default.Send<SelectedAreaPoint>(GetLogicPoint(pos.X, pos.Y), MapEditorControlMessageTokens.DrawDragedArea);
                        _currentState = MapEditionState.StateInitialize;
                        ToolBarCanEdit = true;
                    }
                }
                else if (PenToolSelected)
                {
                    var logicPoint = GetLogicPoint(pos.X, pos.Y);
                    if (!_penInitialized)
                    {
                        ToolBarCanEdit = false;

                        Messenger.Default.Send<SelectedAreaPoint>(logicPoint, MapEditorControlMessageTokens.SetInitialPenPoint);
                        _penInitialized = true;

                        _penPointIndex = 0;
                        _penPoints[_penPointIndex].X = logicPoint.X;
                        _penPoints[_penPointIndex].Y = logicPoint.Y;
                    }
                    else
                    {
                        if(_penPointIndex >= 1)
                        {
                            //foreach (var p in Enumerable.Range(1, _penPointIndex - 1))
                            //{
                            //    if (logicPoint.X == _penPoints[p].X && logicPoint.Y == _penPoints[p].Y)
                            //    {
                            //        return;
                            //    }
                            //}

                            var result = from p in Enumerable.Range(1, _penPointIndex - 1)
                                         where logicPoint.X == _penPoints[p].X && logicPoint.Y == _penPoints[p].Y
                                         select _penPoints[p];

                            if(result.Count() != 0)
                            {
                                return;
                            }

                        }

                        // Last 1
                        if (_penPointIndex == MAX_PENPOINTS - 2)
                        {
                            if(!(logicPoint.X == _penPoints[0].X && logicPoint.Y == _penPoints[0].Y))
                            {
                                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.CancelPenPoints);
                                _penInitialized = false;
                                ToolBarCanEdit = true;
                            }
                            else
                            {
                                Messenger.Default.Send<SelectedAreaPoint>(logicPoint, MapEditorControlMessageTokens.AddLastPenPoint);

                                var area = CalcPenSelectedArea();

                                var command = new AreaOperateMapEditorCommand(area, this._tiles, GenerateTileType());
                                MapEditorCommandManager.AddCommand(command);
                                command.Do();

                                _penInitialized = false;
                                ToolBarCanEdit = true;
                            }
                        }
                        else if(_penPointIndex < MAX_PENPOINTS - 2)
                        {
                            if (!(logicPoint.X == _penPoints[0].X && logicPoint.Y == _penPoints[0].Y))
                            {
                                ++_penPointIndex;
                                _penPoints[_penPointIndex].X = logicPoint.X;
                                _penPoints[_penPointIndex].Y = logicPoint.Y;

                                Messenger.Default.Send<SelectedAreaPoint>(logicPoint, MapEditorControlMessageTokens.AddNewPenPoint);
                            }
                            else
                            {
                                if (_penPointIndex <= 1)
                                {
                                    return;
                                }

                                Messenger.Default.Send<SelectedAreaPoint>(logicPoint, MapEditorControlMessageTokens.AddLastPenPoint);

                                var area = CalcPenSelectedArea();

                                var command = new AreaOperateMapEditorCommand(area, this._tiles, GenerateTileType());
                                //var command = new AreaOperateMapEditorCommand(area, this._tiles, TransparentSelected ? TileType.SelectedTranslucent : TileType.Selected);
                                MapEditorCommandManager.AddCommand(command);
                                command.Do();

                                _penInitialized = false;
                                ToolBarCanEdit = true;
                            }
                        }
                    }
                }
                else if (PointToolSelected)
                {
                    UpdateWithClickedTileLogicPosition(pos.X, pos.Y);
                }

            });

            RightMouseDownHandler = new RelayCommand<Point>((pos) =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                if (_currentState == MapEditionState.StateMouseLeftDown)
                {
                    _currentState = MapEditionState.StateInitialize;
                    if (AreaToolSelected)
                    {
                        Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.CancelDrag);
                        ToolBarCanEdit = true;
                    }
                    else if (PenToolSelected)
                    {
                        Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.CancelPenPoints);
                        _penInitialized = false;
                        ToolBarCanEdit = true;
                    }
                }
                else if(_currentState == MapEditionState.StateInitialize)
                {
                    _currentState = MapEditionState.StateMouseRightDown;
                    Messenger.Default.Send<SelectedAreaPoint>(GetLogicPoint(pos.X, pos.Y), MapEditorControlMessageTokens.SetInitialMousePointRightButtonDown);
                    ToolBarCanEdit = false;
                }
            });

            RightMouseUpHandler = new RelayCommand<Point>((pos) =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                if (_currentState == MapEditionState.StateMouseRightDown)
                {
                    _currentState = MapEditionState.StateInitialize;
                    Messenger.Default.Send<SelectedAreaPoint>(GetLogicPoint(pos.X, pos.Y), MapEditorControlMessageTokens.UndrawSelectedArea);
                    ToolBarCanEdit = true;
                }
            });

            MouseMoveHandler = new RelayCommand<Point>((pos) =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                if (_currentState == MapEditionState.StateMouseLeftDown || _currentState == MapEditionState.StateMouseRightDown)
                {
                    if (AreaToolSelected)
                    {
                        Messenger.Default.Send<SelectedAreaPoint>(GetLogicPoint(pos.X, pos.Y), MapEditorControlMessageTokens.DrawDragingArea);
                    }
                    else if (DragToolSelected && _currentState == MapEditionState.StateMouseLeftDown)
                    {
                        Messenger.Default.Send<Point>(pos, MapEditorControlMessageTokens.MoveMapObject);
                    }
                    else if(_currentState == MapEditionState.StateMouseRightDown)
                    {
                        Messenger.Default.Send<SelectedAreaPoint>(GetLogicPoint(pos.X, pos.Y), MapEditorControlMessageTokens.DrawDragingArea);
                    }
                }
            });

            MouseLeaveHandler = new RelayCommand(() =>
            {
            });

            MouseEnterHandler = new RelayCommand(() =>
            {
            });

            DragOverHandler = new RelayCommand<DragEventArgs>((args) =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                args.Effects = DragDropEffects.None;

                if (args.Data.GetDataPresent(typeof(MonsterSection)) ||
                    args.Data.GetDataPresent(typeof(NpcSection))) {
                    args.Effects = DragDropEffects.Copy | DragDropEffects.Move;
                }
            });

            DropHandler = new RelayCommand<DragEventArgs>((args) =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                Messenger.Default.Send<DragEventArgs>(args, MapEditorControlMessageTokens.RaiseDropEventFromView);
            });

            UndoCommand = new RelayCommand(() =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                MapEditorCommandManager.Undo();
            });

            RedoCommand = new RelayCommand(() =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                MapEditorCommandManager.ReDo();
            });

            DeleteMapObjectCommand = new RelayCommand(() =>
            {
                if (!this.Vaild)
                {
                    return;
                }

                Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.DeleteMapObjectFromViewModel);
            });

        }

        private byte GenerateTileType()
        {
            TileType tmp = (TileType)TileTypeBit.Selected;
            if (TransparentSelected)
            {
                tmp |= (TileType)TileTypeBit.Translucent;
            }

            if(Safety)
            {
                tmp |= (TileType)TileTypeBit.Safety;
            }

            if(Fishing)
            {
                tmp |= (TileType)TileTypeBit.Fishing;
            }

            return tmp;
        }

        private SelectedAreaPoint[] CalcPenSelectedArea()
        {
            var query = from r in Enumerable.Range(0, _penPointIndex + 1) select _penPoints[r];

            var boxLeft = query.Min(r => r.X);
            var boxRight = query.Max(r => r.X);
            var boxTop = query.Min(r => r.Y);
            var boxBottom = query.Max(r => r.Y);

            var query2 = from r in Enumerable.Range(0, _penPointIndex + 1)
                        select new System.Drawing.Point()
                        {
                            X = _penPoints[r].X,
                            Y = _penPoints[r].Y,
                        };

            var query3 =
                    from i in Enumerable.Range(boxLeft, boxRight - boxLeft + 1)
                    from j in Enumerable.Range(boxTop, boxBottom - boxTop + 1)
                    where PointInPolygon(i, j, query2.ToArray())
                    select new SelectedAreaPoint
                    {
                        X=i,
                        Y=j,
                    };

            return query3.ToArray();
        }

        private bool PointInPolygon(int x, int y, System.Drawing.Point[] points)
        {
            System.Drawing.Drawing2D.GraphicsPath myGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Region polygon = new System.Drawing.Region();

            myGraphicsPath.Reset();
            myGraphicsPath.AddPolygon(points);

            polygon.MakeEmpty();
            polygon.Union(myGraphicsPath);

            if(!polygon.IsVisible(x, y))
            {
                SelectedAreaPoint tmpPoint = new SelectedAreaPoint()
                {
                    X = x,
                    Y = y,
                };
                foreach (var ii in Enumerable.Range(1, _penPointIndex))
                {
                    if (OnSegment(_penPoints[ii - 1], _penPoints[ii], tmpPoint))
                    {
                        return true;
                    }
                }
                if (OnSegment(_penPoints[_penPointIndex], _penPoints[0], tmpPoint))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        bool OnSegment(SelectedAreaPoint Pi, SelectedAreaPoint Pj, SelectedAreaPoint Q)
        {
            if ((Q.X - Pi.X) * (Pj.Y - Pi.Y) == (Pj.X - Pi.X) * (Q.Y - Pi.Y)
                && Math.Min(Pi.X, Pj.X) <= Q.X && Q.X <= Math.Max(Pi.X, Pj.X)
                && Math.Min(Pi.Y, Pj.Y) <= Q.Y && Q.Y <= Math.Max(Pi.Y, Pj.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Initialized()
        {
            return _initialzeCount >= 4;
        }

        public void ReInitialize()
        {
            _initialzeCount = 0;
        }
        //public RelayCommand<System.Windows.Point> RelocateSelectedTitle { get; set; }

        public RelayCommand<ScrollChangedEventArgs> UpdateContentRate { get; set; }

        public RelayCommand<DragEventArgs> DragOverHandler { get; set; }

        public RelayCommand<DragEventArgs> DropHandler { get; set; }

        public RelayCommand<Point> LeftMouseDownHandler { get; set; }

        public RelayCommand<Point> LeftMouseUpHandler { get; set; }

        public RelayCommand<Point> RightMouseDownHandler { get; set; }

        public RelayCommand<Point> RightMouseUpHandler { get; set; }

        public RelayCommand<Point> MouseMoveHandler { get; set; }

        public RelayCommand MouseLeaveHandler { get; set; }

        public RelayCommand MouseEnterHandler { get; set; }

        public RelayCommand UndoCommand { get; set; }

        public RelayCommand RedoCommand { get; set; }

        public RelayCommand DeleteMapObjectCommand { get; set; }

        private void UpdateBackgroundDisplay(string path)
        {
            Vaild = false;

            int tileWidth = TileWidth;
            int tileHeight = TileHeight;

            if (!File.Exists(path))
            {
                if(Initialized())
                {
                    --_initialzeCount;
                }
                BackgroundImage = null;
                return;
            }

            var url = new Uri(path, UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage();
            bool loaded = true;
            bitmap.BeginInit();
            bitmap.UriSource = url;

            bitmap.DecodeFailed += (sender, ec) =>
            {
                loaded = false;
            };
            bitmap.EndInit();

            if (!loaded)
            {
                if (Initialized())
                {
                    --_initialzeCount;
                }
                BackgroundImage = null;
                return;
            }

            //if (bitmap.PixelWidth % tileWidth != 0
            //    || bitmap.PixelHeight % tileHeight != 0)
            //{
            //    if (Initialized())
            //    {
            //        --_initialzeCount;
            //    }
            //    //MessageBox.Show(String.Format(@"图片长度必须为{0}的倍数，宽度必须为{1}的倍数。", tileWidth, tileHeight));
            //    BackgroundImage = null;
            //    return;
            //}

            _currentBackgroundPath = path;
            BackgroundImage = bitmap;
            BackgroundPixelWidth = bitmap.PixelWidth;
            BackgroundPixelHeight = bitmap.PixelHeight;

            Messenger.Default.Send<int>(bitmap.PixelWidth, MapEditorControlMessageTokens.UpdateMapPixelWidthFromViewModel);
            Messenger.Default.Send<int>(bitmap.PixelHeight, MapEditorControlMessageTokens.UpdateMapPixelHeightFromViewModel);

            Vaild = true;

            Messenger.Default.Send<MessageParameterTileDisplay>(new MessageParameterTileDisplay
            {
                PixelWidth = (int)BackgroundPixelWidth,
                PixelHeight = (int)BackgroundPixelHeight,
                TileWidth = this.TileWidth,
                TileHeight = this.TileHeight,
                Zoom = (double)this.Zoom,
            }, MapEditorControlMessageTokens.DisplayTilesAndSelectedTileForce);

            _cols = (int)Math.Ceiling(BackgroundPixelHeight / TileHeight);
            _rows = (int)Math.Ceiling(BackgroundPixelWidth / TileWidth);

            _tiles = new TileType[_rows, _cols];

            for(int i = 0; i < _rows; ++i)
            {
                for(int j = 0; j < _cols; ++j)
                {
                    _tiles[i, j] = Convert.ToByte("00000000", 2);
                }
            }

            Messenger.Default.Send<TileType[,]>(_tiles, MapEditorControlMessageTokens.NotifyAreaChanged);

        }

        private void UpdateWithClickedTileLogicPosition(double realX, double realY)
        {
            //var logicX = (int)(realX / (TileWidth * Zoom));
            //var logicY = (int)(realY / (TileHeight * Zoom));

            var logicPoint = GetLogicPoint(realX, realY);

            Messenger.Default.Send<MessageParameterSelectedTileStruct>(
                new MessageParameterSelectedTileStruct()
                {
                    XDelta = TileWidth * Zoom,
                    YDelta = TileHeight * Zoom,
                    X = (int)logicPoint.X,
                    Y = (int)logicPoint.Y,
                },
                MapEditorControlMessageTokens.AdjustSelectedTilePosition
            );
        }

        private SelectedAreaPoint GetLogicPoint(double realX, double realY)
        {
            return new SelectedAreaPoint()
            {
                X = (int)(realX / (TileWidth * Zoom)),
                Y = (int)(realY / (TileHeight * Zoom)),
            };
        }

    }
}