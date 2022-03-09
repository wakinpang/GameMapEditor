using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MapEditorControl
{
    using TileType = Byte;
    /// <summary>
    /// MapEditorControl.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditorControl : UserControl
    {

        private bool _initialized = false;
        //private bool _vaild = false;
        
        private LinkedList<Line> _rowLines = new LinkedList<Line>();
        private LinkedList<Line> _colLines = new LinkedList<Line>();
        private LinkedList<MapObjectSpriteControl> _objectSprites = new LinkedList<MapObjectSpriteControl>();
        private LinkedList<MapObjectSpriteControl> _newObjectSpritesTempList = new LinkedList<MapObjectSpriteControl>();

        private MapObjectSpriteControl _currentSprite = null;
        private Point _startSelectMousePoint = new Point();
        private Point _startSpritePoint = new Point();

        private Point _selectedTilePosition = new Point();

        private SelectedAreaPoint _areaStartPoint = new SelectedAreaPoint();
        private Point _leavingMovingPoint = new Point();

        private Rectangle _selectedTile = new Rectangle()
        {
            Stroke = Brushes.White,
            StrokeThickness = 3,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
        };

        private Rectangle _selectingArea = new Rectangle()
        {
            Stroke = Brushes.White,
            Opacity = 0.6,
            StrokeThickness = 3,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            
            Visibility = Visibility.Hidden,
        };

        Rectangle[,] _areaTiles = null;

        SelectedAreaPoint _startPenPoint = new SelectedAreaPoint();
        SelectedAreaPoint _currentPenPoint = new SelectedAreaPoint();

        const int MAX_PENPOINTS = 999;

        struct _PenPoint
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Ellipse DrawTarget { get; set; }
        }

        struct _PenLine
        {
            public int StartX { get; set; }
            public int StartY { get; set; }
            public int EndX { get; set; }
            public int EndY { get; set; }
            public Line DrawTarget { get; set; }
        }

        _PenPoint[] _penPoints = new _PenPoint[MAX_PENPOINTS];
        _PenLine[] _penLines = new _PenLine[MAX_PENPOINTS];
        int _currentPenPointCount = 0;
        int _currentPenLineCount = 0;

        const double POINT_WIDTH = 10.0;
        const double POINT_HEIGHT = 10.0;

        private DispatcherTimer _scrollTimer = new DispatcherTimer();

        public event RoutedEventHandler ModifySelectedArea
        {
            add { AddHandler(ModifySelectedAreaEvent, value); }
            remove { RemoveHandler(ModifySelectedAreaEvent, value); }
        }

        public static readonly RoutedEvent ModifySelectedAreaEvent = EventManager.RegisterRoutedEvent(
        "ModifySelectedArea", RoutingStrategy.Bubble, typeof(EventHandler<ModifySelectedAreaEventArgs>), typeof(MapEditorControl));

        public new event RoutedEventHandler Drop
        {
            add { AddHandler(DropEvent, value); }
            remove { RemoveHandler(DropEvent, value); }
        }

        public static readonly new RoutedEvent DropEvent = EventManager.RegisterRoutedEvent(
            "Drop", RoutingStrategy.Bubble, typeof(EventHandler<DragEventArgs>), typeof(MapEditorControl));

        static ImageBrush _selSafetyImage = new ImageBrush(new BitmapImage(new Uri("image\\sel_safety.png", UriKind.RelativeOrAbsolute)));
        static ImageBrush _selFishingImage = new ImageBrush(new BitmapImage(new Uri("image\\sel_fishing.png", UriKind.RelativeOrAbsolute)));
        static ImageBrush _transSafetyImage = new ImageBrush(new BitmapImage(new Uri("image\\trans_safety.png", UriKind.RelativeOrAbsolute)));
        static ImageBrush _transFishingImage = new ImageBrush(new BitmapImage(new Uri("image\\trans_fishing.png", UriKind.RelativeOrAbsolute)));

        public MapEditorControl()
        {
            InitializeComponent();

            _scrollTimer.Tick += _scrollTimer_Tick;
            _scrollTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.ReinitializeView, (dummy) =>
            {
                this._initialized = false;
            });

            Messenger.Default.Register<DragEventArgs>(this, MapEditorControlMessageTokens.RaiseDropEventFromView, (args) =>
            {
                MonsterDropArgs newEventArgs = new MonsterDropArgs(DropEvent)
                {
                    Args = args,
                };
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateToolBarCanEditFromViewModel, (b) =>
            {
                ToolBarCanEdit = b;
            });

            Messenger.Default.Register<MessageParameterTileDisplay>(this, MapEditorControlMessageTokens.DisplayTilesAndSelectedTile, (param) =>
            {
                if (Valid)
                {
                    DrawTiles(param.PixelWidth, param.PixelHeight, param.TileWidth, param.TileHeight, param.Zoom, false);
                    AdjustRatio(this);
                }
            });

            Messenger.Default.Register<MessageParameterTileDisplay>(this, MapEditorControlMessageTokens.DisplayTilesAndSelectedTileForce, (param) =>
            {
                if (Valid)
                {
                    DrawTiles(param.PixelWidth, param.PixelHeight, param.TileWidth, param.TileHeight, param.Zoom, true);
                    AdjustRatio(this);
                }
            });

            Messenger.Default.Register<MessageParameterSelectedTileStruct>(this, MapEditorControlMessageTokens.AdjustSelectedTilePosition, (pos) =>
            {
                if (Valid)
                {
                    _selectedTilePosition.X = pos.X;
                    _selectedTilePosition.Y = pos.Y;

                    BornPointX = pos.X * TileWidth;
                    BornPointY = pos.Y * TileHeight;

                    RerenderSelectedTile(pos.XDelta, pos.YDelta);
                }
            });

            Messenger.Default.Register<ScrollChangedEventArgs>(this, MapEditorControlMessageTokens.UpdateOffsetRatioFromMapEditorViewModel, (args) => 
            {
                if (Valid)
                {
                    AdjustOffsetRatio(args);
                }
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.DoArrangeFromViewModel, (param) =>
            {
                if (Valid)
                {
                    AdjustRatio(this);
                }
            });

            Messenger.Default.Register<bool>(this, MapEditorControlMessageTokens.UpdateVaildFromViewModel, (vaild) =>
            {
                Valid = vaild;
                if (!vaild)
                {
                    canvas_Wrapper.Children.Clear();
                    this.IsEnabled = false;
                }
                else
                {
                    this.IsEnabled = true;
                }
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.SetInitialMousePointLeftButtonDown, (pos) =>
            {
                _selectingArea.Fill = Brushes.Blue;
                ResetSelectingArea(pos);
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.SetInitialMousePointRightButtonDown, (pos) =>
            {
                _selectingArea.Fill = Brushes.Gray;
                ResetSelectingArea(pos);
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.DrawDragingArea, (pos) =>
            {
                _leavingMovingPoint = Mouse.GetPosition(scroll_Main);

                if (_leavingMovingPoint.X < 0
                    || _leavingMovingPoint.X * TileWidth * Zoom > scroll_Main.ActualWidth
                    || _leavingMovingPoint.Y < 0
                    || _leavingMovingPoint.Y * TileHeight * Zoom > scroll_Main.ActualHeight)
                {
                    _scrollTimer.Start();
                }

                _selectingArea.Width = (Math.Abs(pos.X - _areaStartPoint.X) + 1) * TileWidth * Zoom;
                _selectingArea.Height = (Math.Abs(pos.Y - _areaStartPoint.Y) + 1) * TileWidth * Zoom;

                _selectingArea.Margin = new Thickness()
                {
                    Left = Math.Min(_areaStartPoint.X, pos.X) * TileWidth * Zoom,
                    Top = Math.Min(_areaStartPoint.Y, pos.Y) * TileHeight * Zoom,
                };
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.DrawDragedArea, (pos) =>
            {
                var updatedArea = CalcSelectedArea(pos);
                Messenger.Default.Send<SelectedAreaPoint[]>(updatedArea, MapEditorControlMessageTokens.UpdateSelectedArea);

            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.UndrawSelectedArea, (pos) =>
            {
                var updatedArea = CalcSelectedArea(pos);
                Messenger.Default.Send<SelectedAreaPoint[]>(updatedArea, MapEditorControlMessageTokens.UpdateUnselectedArea);
            });

            

            Messenger.Default.Register<AreaPointState[]>(this, MapEditorControlMessageTokens.DrawAppointedArea, (area) =>
            {
                foreach (var p in area)
                {
                    //switch (p.Type)
                    //{
                    //    case TileType.Normal:
                    //        _areaTiles[p.X, p.Y].Visibility = Visibility.Hidden;
                    //        break;
                    //    case TileType.Selected:
                    //        _areaTiles[p.X, p.Y].Visibility = Visibility.Visible;
                    //        _areaTiles[p.X, p.Y].Fill = Brushes.Pink;
                    //        break;
                    //    case TileType.SelectedTranslucent:
                    //        _areaTiles[p.X, p.Y].Visibility = Visibility.Visible;
                    //        _areaTiles[p.X, p.Y].Fill = Brushes.Yellow;
                    //        break;
                    //}
                    var type = p.Type;

                    if(type == 0)
                    {
                        _areaTiles[p.X, p.Y].Visibility = Visibility.Hidden;
                        continue;
                    }

                    if((type & (TileType)TileTypeBit.Selected) > 0)
                    {

                        if ((type & (TileType)TileTypeBit.Translucent) > 0)
                        {
                            _areaTiles[p.X, p.Y].Visibility = Visibility.Visible;
                            if ((type & (TileType)TileTypeBit.Safety) > 0)
                            {
                                _areaTiles[p.X, p.Y].Fill = _transSafetyImage;
                            }
                            else if ((type & (TileType)TileTypeBit.Fishing) > 0)
                            {
                                _areaTiles[p.X, p.Y].Fill = _transFishingImage;
                            }
                            else
                            {
                                _areaTiles[p.X, p.Y].Fill = Brushes.Yellow;
                            }
                        }
                        else
                        {
                            _areaTiles[p.X, p.Y].Visibility = Visibility.Visible;
                            if ((type & (TileType)TileTypeBit.Safety) > 0)
                            {
                                _areaTiles[p.X, p.Y].Fill = _selSafetyImage;
                            }
                            else if ((type & (TileType)TileTypeBit.Fishing) > 0)
                            {
                                _areaTiles[p.X, p.Y].Fill = _selFishingImage;
                            }
                            else
                            {
                                _areaTiles[p.X, p.Y].Fill = Brushes.Green;
                            }
                        }
                    }
                }
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.CancelDrag, (dummy) => {
                _selectingArea.Visibility = Visibility.Hidden;
                canvas_Wrapper.ReleaseMouseCapture();
                _scrollTimer.Stop();
            });

            Messenger.Default.Register<TileType[,]>(this, MapEditorControlMessageTokens.NotifyAreaChanged, (area) =>
            {
                ModifySelectedAreaEventArgs newEventArgs = new ModifySelectedAreaEventArgs(ModifySelectedAreaEvent)
                {
                    Area = area,
                };
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.SetInitialPenPoint, (pos) =>
            {
                _startPenPoint = pos;
                _currentPenPoint = pos;
                _currentPenPointCount = 1;
                _currentPenLineCount = 0;

                DrawNewPenPoint(pos);
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.AddNewPenPoint, (pos) =>
            {
                ++_currentPenPointCount;
                ++_currentPenLineCount;
                DrawNewPenPoint(pos);
                DrawPenPointLine(_currentPenPoint, pos);

                _currentPenPoint = pos;
            });

            Messenger.Default.Register<SelectedAreaPoint>(this, MapEditorControlMessageTokens.AddLastPenPoint, (pos) =>
            {
                ReinitializePenPointsAndLines();
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.CancelPenPoints, (dummy) =>
            {
                ReinitializePenPointsAndLines();
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.UpdateBornPointFromViewModel, (dummy) => {
                var path = DefaultBornPointImage;

                if (!File.Exists(path)) {
                    _selectedTile.Fill = Brushes.Red;
                }
                else 
                {
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

                    if(loaded) {
                        _selectedTile.Fill = new ImageBrush(bitmap);
                    }
                    else 
                    {
                        _selectedTile.Fill = Brushes.Red;
                    }
                }
            });

            Messenger.Default.Register<ObservableCollection<MapObjectSprite>>(this, MapEditorControlMessageTokens.DoMapObjectBind, (list) =>
            {
                var converter = new PathToSpriteBitmapConverter() { RootPath = "" };
                var converter2 = new PathToSpriteBitmapConverter();


                // 手动绑定
                foreach (var obj in list)
                {
                    var mapObjectControl = new MapObjectSpriteControl();

                    mapObjectControl.X = obj.X;
                    mapObjectControl.Y = obj.Y;

                    if(obj.SceneObjectBase == null)
                    {
                        var npc = obj.ObjectBase as NpcSection;

                        converter2.RootPath = obj.FolderPath;

                        var binding = new Binding()
                        {
                            Source = npc,
                            Path = new PropertyPath("NPCId"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        };

                        BindingOperations.SetBinding(
                            mapObjectControl,
                            MapObjectSpriteControl.IDProperty,
                            binding
                        );

                        binding = new Binding()
                        {
                            Source = npc,
                            Path = new PropertyPath("Name"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        };

                        BindingOperations.SetBinding(
                            mapObjectControl,
                            MapObjectSpriteControl.SpriteNameProperty,
                            binding
                        );

                        binding = new Binding()
                        {
                            Source = npc,
                            Path = new PropertyPath("PosX"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        };

                        BindingOperations.SetBinding(
                            mapObjectControl,
                            MapObjectSpriteControl.XProperty,
                            binding
                        );

                        binding = new Binding()
                        {
                            Source = npc,
                            Path = new PropertyPath("PosY"),
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        };

                        BindingOperations.SetBinding(
                            mapObjectControl,
                            MapObjectSpriteControl.YProperty,
                            binding
                        );

                        binding = new Binding()
                        {
                            Source = npc,
                            Path = new PropertyPath("Style"),
                            Mode = BindingMode.OneWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Converter = converter2,
                        };

                        BindingOperations.SetBinding(
                            mapObjectControl,
                            MapObjectSpriteControl.SpriteImageSourceProperty,
                            binding
                        );

                        //BindSpriteControlToMapObject(obj, mapObjectControl, "SpriteImage", MapObjectSpriteControl.SpriteImageSourceProperty, BindingMode.OneWay, converter);
                        BindSpriteControlToMapObject(this, mapObjectControl, "Zoom", MapObjectSpriteControl.ZoomProperty, BindingMode.OneWay);
                    }
                    else
                    {
                        BindSpriteControlToMapObject(obj, mapObjectControl, "ID", MapObjectSpriteControl.IDProperty, BindingMode.OneWay);
                        BindSpriteControlToMapObject(obj, mapObjectControl, "X", MapObjectSpriteControl.XProperty);
                        BindSpriteControlToMapObject(obj, mapObjectControl, "Y", MapObjectSpriteControl.YProperty);
                        BindSpriteControlToMapObject(obj, mapObjectControl, "Name", MapObjectSpriteControl.SpriteNameProperty, BindingMode.OneWay);
                        BindSpriteControlToMapObject(obj, mapObjectControl, "SpriteImage", MapObjectSpriteControl.SpriteImageSourceProperty, BindingMode.OneWay, converter);
                        BindSpriteControlToMapObject(this, mapObjectControl, "Zoom", MapObjectSpriteControl.ZoomProperty, BindingMode.OneWay);
                    }

                    mapObjectControl.SpriteObject = obj;
                    obj.SpriteControl = mapObjectControl;

                    _newObjectSpritesTempList.AddLast(mapObjectControl);
                }
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.ShowNewObjects, (dummy) =>
            {
                _objectSprites = new LinkedList<MapObjectSpriteControl>(_objectSprites.Concat(_newObjectSpritesTempList).ToList());

                foreach(var control in _newObjectSpritesTempList)
                {
                    this.canvas_Wrapper.Children.Add(control);
                }

                if(_newObjectSpritesTempList.Count > 0)
                {
                    if(_currentSprite != null)
                    {
                        _currentSprite.Selected = false;
                        _currentSprite = null;
                    }

                    _currentSprite = _newObjectSpritesTempList.Last();
                    _currentSprite.Selected = true;

                    if ((_currentSprite.SpriteObject.SceneObjectBase as SceneMonsterPOJO) != null)
                    {
                        CurrentSceneMonster = _currentSprite.SpriteObject.SceneObjectBase as SceneMonsterPOJO;
                        CurrentMonster = _currentSprite.SpriteObject.ObjectBase as MonsterSection;

                        CurrentNPC = null;
                    }
                    else
                    {
                        CurrentSceneMonster = null;
                        CurrentMonster = null;

                        CurrentNPC = _currentSprite.SpriteObject.ObjectBase as NpcSection;
                    }
                }

                _newObjectSpritesTempList.Clear();
            });

            Messenger.Default.Register<Point>(this, MapEditorControlMessageTokens.SelectMapObject, (mousPos) =>
            {
                var result = from obj in this._objectSprites
                             where obj.X * Zoom <= mousPos.X && (obj.X + obj.ActualWidth) * Zoom >= mousPos.X
                                    && obj.Y * Zoom <= mousPos.Y && (obj.Y + obj.ActualHeight) * Zoom >= mousPos.Y
                             select obj;

                if (_currentSprite != null)
                {
                    _currentSprite.Selected = false;
                }

                if (result.Count() == 0)
                {
                    if(_currentSprite == null)
                    {
                        return;
                    }
                    _currentSprite = null;

                    CurrentMonster = null;
                    CurrentSceneMonster = null;
                    CurrentNPC = null;
                }
                else
                {
                    _currentSprite = result.Last();

                    if ((_currentSprite.SpriteObject.SceneObjectBase as SceneMonsterPOJO) != null)
                    {
                        CurrentSceneMonster = _currentSprite.SpriteObject.SceneObjectBase as SceneMonsterPOJO;
                        CurrentMonster = _currentSprite.SpriteObject.ObjectBase as MonsterSection;

                        CurrentNPC = null;
                    }
                    else
                    {
                        CurrentSceneMonster = null;
                        CurrentMonster = null;

                        CurrentNPC = _currentSprite.SpriteObject.ObjectBase as NpcSection;
                    }

                    _startSelectMousePoint.X = mousPos.X;
                    _startSelectMousePoint.Y = mousPos.Y;

                    _startSpritePoint.X = _currentSprite.X * Zoom;
                    _startSpritePoint.Y = _currentSprite.Y * Zoom;

                    _currentSprite.Selected = true;
                }
            });

            Messenger.Default.Register<IList>(this, MapEditorControlMessageTokens.RemoveMapObject, (list) =>
            {
                foreach(var obj in list)
                {
                    var tmpObj = obj as MapObjectSprite;
                    this.canvas_Wrapper.Children.Remove(tmpObj.SpriteControl);
                    _objectSprites.Remove(tmpObj.SpriteControl);
                }
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.ResetMapObject, (dummy) =>
            {
                foreach(var obj in _objectSprites)
                {
                    this.canvas_Wrapper.Children.Remove(obj);
                }
                _objectSprites.Clear();
            });

            Messenger.Default.Register<Point>(this, MapEditorControlMessageTokens.MoveMapObject, (mousePos) =>
            {
                if (_currentSprite != null)
                {
                    var xDelta = mousePos.X - _startSelectMousePoint.X;
                    var yDelta = mousePos.Y - _startSelectMousePoint.Y;

                    _currentSprite.X = (_startSpritePoint.X + xDelta) / Zoom;
                    _currentSprite.Y = (_startSpritePoint.Y + yDelta) / Zoom;

                }
            });

            Messenger.Default.Register<object>(this, MapEditorControlMessageTokens.DeleteMapObjectFromViewModel, (dummy) =>
            {
                if(_currentSprite != null)
                {
                    //ItemSource.Remove(_currentSprite.SpriteObject);

                    if(_currentSprite.SpriteObject.SceneObjectBase == null && !(_currentSprite.SpriteObject.ObjectBase as NpcSection).Temp)
                    {
                        _objectSprites.Remove(_currentSprite);
                        this.canvas_Wrapper.Children.Remove(_currentSprite);
                        var npcBase = _currentSprite.SpriteObject.ObjectBase as NpcSection;
                        npcBase.POJOStatus = POJOStatus.Deleted;
                    }
                    else
                    {
                        ItemSource.Remove(_currentSprite.SpriteObject);
                    }
                    _currentSprite.Selected = false;
                    _currentSprite = null;
                }
            });

            Messenger.Default.Register<int>(this, MapEditorControlMessageTokens.UpdateMapPixelWidthFromViewModel, (width) =>
            {
                MapPixelWidth = width;
            });

            Messenger.Default.Register<int>(this, MapEditorControlMessageTokens.UpdateMapPixelHeightFromViewModel, (height) =>
            {
                MapPixelHeight = height;
            });

        }

        private static void BindSpriteControlToMapObject(object obj, 
            MapObjectSpriteControl mapObjectControl,
            string propertyName,
            DependencyProperty dependencyProperty,
            BindingMode mode = BindingMode.TwoWay,
            System.Windows.Data.IValueConverter converter = null)
        {
            Binding binding = null;
            if (converter != null)
            {
                binding = new Binding()
                {
                    Source = obj,
                    Path = new PropertyPath(propertyName),
                    Mode = mode,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Converter = converter,
                };
            }
            else
            {
                binding = new Binding()
                {
                    Source = obj,
                    Path = new PropertyPath(propertyName),
                    Mode = mode,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };
            }

            BindingOperations.SetBinding(
                mapObjectControl,
                dependencyProperty,
                binding
            );
        }

        private void ReinitializePenPointsAndLines()
        {
            foreach (var i in Enumerable.Range(0, _currentPenPointCount))
            {
                _penPoints[i].DrawTarget.Visibility = Visibility.Hidden;
            }

            foreach (var i in Enumerable.Range(0, _currentPenLineCount))
            {
                _penLines[i].DrawTarget.Visibility = Visibility.Hidden;
            }

            _currentPenPointCount = 0;
            _currentPenLineCount = 0;
        }

        // 描线
        private void DrawPenPointLine(SelectedAreaPoint from, SelectedAreaPoint to)
        {
            var target = _penLines[_currentPenLineCount - 1];
            target.StartX = from.X;
            target.StartY = from.Y;
            target.EndX = to.X;
            target.EndY = to.Y;
            target.DrawTarget.Visibility = Visibility.Visible;

            target.DrawTarget.X1 = from.X * TileWidth * Zoom + TileWidth * Zoom / 2;
            target.DrawTarget.Y1 = from.Y * TileHeight * Zoom + TileHeight * Zoom / 2;
            target.DrawTarget.X2 = to.X * TileWidth * Zoom + TileWidth * Zoom / 2 ;
            target.DrawTarget.Y2 = to.Y * TileHeight * Zoom + TileHeight * Zoom / 2 ;
        }

        // 描点
        private void DrawNewPenPoint(SelectedAreaPoint pos)
        {
            var target = _penPoints[_currentPenPointCount - 1];
            target.X = pos.X;
            target.Y = pos.Y;
            target.DrawTarget.Visibility = Visibility.Visible;
            target.DrawTarget.Margin = new Thickness()
            {
                Left = pos.X * TileWidth * Zoom + TileWidth * Zoom / 2 - POINT_WIDTH / 2,
                Top = pos.Y * TileHeight * Zoom + TileHeight * Zoom / 2 - POINT_HEIGHT / 2,
            };
        }

        private SelectedAreaPoint[] CalcSelectedArea(SelectedAreaPoint pos)
        {
            _selectingArea.Visibility = Visibility.Hidden;
            canvas_Wrapper.ReleaseMouseCapture();
            _scrollTimer.Stop();

            // Draw
            var startX = Math.Max(0, Math.Min(pos.X, _areaStartPoint.X));
            var startY = Math.Max(0, Math.Min(pos.Y, _areaStartPoint.Y));

            var endX = Math.Min(Math.Max(pos.X, _areaStartPoint.X), _colLines.Count - 1);
            var endY = Math.Min(Math.Max(pos.Y, _areaStartPoint.Y), _rowLines.Count - 1);

            SelectedAreaPoint[] updatedArea = new SelectedAreaPoint[(endX - startX + 1) * (endY - startY + 1)];

            int count = 0;
            for (int i = startX; i <= endX; ++i)
            {
                for (int j = startY; j <= endY; ++j)
                {
                    updatedArea[count].X = i;
                    updatedArea[count].Y = j;
                    ++count;
                }
            }

            return updatedArea;
        }

        private void ResetSelectingArea(SelectedAreaPoint pos)
        {
            _selectingArea.Visibility = Visibility.Visible;
            _selectingArea.Width = 0;
            _selectingArea.Height = 0;

            _areaStartPoint = pos;
            canvas_Wrapper.CaptureMouse();
        }

        private void _scrollTimer_Tick(object sender, EventArgs e)
        {

            _leavingMovingPoint = Mouse.GetPosition(scroll_Main);

            if (!(_leavingMovingPoint.X < 0
                || _leavingMovingPoint.X > scroll_Main.ActualWidth
                || _leavingMovingPoint.Y < 0
                || _leavingMovingPoint.Y > scroll_Main.ActualHeight))
            {
                _scrollTimer.Stop();
            }

            if (_leavingMovingPoint.X < 0)
            {
                scroll_Main.ScrollToHorizontalOffset(Math.Max(0, scroll_Main.HorizontalOffset - scroll_Main.ViewportWidth / 20.0));
            }
            else if (_leavingMovingPoint.X > scroll_Main.ActualWidth)
            {
                scroll_Main.ScrollToHorizontalOffset(Math.Min(canvas_Wrapper.Width, scroll_Main.HorizontalOffset + scroll_Main.ViewportWidth / 20.0));
            }

            if (_leavingMovingPoint.Y < 0)
            {
                scroll_Main.ScrollToVerticalOffset(Math.Max(0, scroll_Main.VerticalOffset - scroll_Main.ViewportHeight / 20.0));
            }
            else if (_leavingMovingPoint.Y > scroll_Main.ActualHeight)
            {
                scroll_Main.ScrollToVerticalOffset(Math.Min(canvas_Wrapper.Height, scroll_Main.VerticalOffset + scroll_Main.ViewportHeight / 20.0));
            }
        }

        private void DrawTiles(int width, int height, int tileWidth, int tileHeight, double zoom, bool forceUpdate)
        {
            var colLines = (int)Math.Ceiling((double)width / tileWidth);
            var rowLines = (int)Math.Ceiling((double)height / tileHeight);

            var xDelta = tileWidth * zoom;
            var yDelta = tileHeight * zoom;

            if(forceUpdate)
            {
                this._initialized = false;
            }

            if (!this._initialized)
            {
                RerenderTiles(xDelta, yDelta, colLines, rowLines, width * zoom, height * zoom);
                this._initialized = true;
            }
            else
            {
                int counter = 0;
                foreach (var line in this._colLines)
                {
                    line.X1 = line.X2 = xDelta * counter;
                    line.Y2 = height;
                    ++counter;
                }

                counter = 0;
                foreach (var line in this._rowLines)
                {
                    line.Y1 = line.Y2 = yDelta * counter;
                    line.X2 = width;
                    ++counter;
                }

                RerenderSelectedTile(xDelta, yDelta);

                for (int i = 0; i < colLines; ++i)
                {
                    for (int j = 0; j < rowLines; ++j)
                    {
                        _areaTiles[i, j].Margin = new Thickness()
                        {
                            Left = i * xDelta,
                            Top = j * yDelta,
                        };
                        _areaTiles[i, j].Width = xDelta;
                        _areaTiles[i, j].Height = yDelta;
                    }
                }

                foreach (var p in _penPoints)
                {
                    if(p.DrawTarget.Visibility == Visibility.Visible)
                    {
                        p.DrawTarget.Margin = new Thickness()
                        {
                            Left = p.X * xDelta + xDelta / 2 - POINT_WIDTH / 2,
                            Top = p.Y * yDelta + yDelta / 2 - POINT_HEIGHT / 2,
                        };
                    }

                }

                foreach(var l in _penLines)
                {
                    if(l.DrawTarget.Visibility == Visibility.Visible)
                    {
                        l.DrawTarget.X1 = l.StartX * xDelta + xDelta / 2 - POINT_WIDTH / 2;
                        l.DrawTarget.Y1 = l.StartY * yDelta + yDelta / 2 - POINT_HEIGHT / 2;
                        l.DrawTarget.X2 = l.EndX * xDelta + xDelta / 2 - POINT_WIDTH / 2;
                        l.DrawTarget.Y2 = l.EndY * yDelta + yDelta / 2 - POINT_HEIGHT / 2;
                    }
                }
            }
        }

        private void RerenderTiles(double xDelta, double yDelta, int colLines, int rowLines, double width, double height)
        {
            InitializeSelectedTiles(xDelta, yDelta, colLines, rowLines);
            InitializePenPoints();
            InitializePenLines();

            this.canvas_Wrapper.Children.Clear();

            RerenderSelectedTile(xDelta, yDelta);

            this.canvas_Wrapper.Children.Add(this._selectedTile);
            this.canvas_Wrapper.Children.Add(this._selectingArea);
            
            foreach(var tile in _areaTiles)
            {
                this.canvas_Wrapper.Children.Add(tile);
            }

            foreach(var p in _penPoints)
            {
                this.canvas_Wrapper.Children.Add(p.DrawTarget);
            }

            foreach(var l in _penLines)
            {
                this.canvas_Wrapper.Children.Add(l.DrawTarget);
            }

            // 横向
            this._colLines.Clear();
            for (int i = 0; i < colLines; ++i)
            {
                Line tileLine = new Line()
                {
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,

                    Y1 = 0,
                    Y2 = height
                };

                tileLine.Opacity = 0.4;
                tileLine.X1 = tileLine.X2 = xDelta * i;
                this.canvas_Wrapper.Children.Add(tileLine);
                this._colLines.AddLast(tileLine);
            }

            // 纵向
            this._rowLines.Clear();
            for (int i = 0; i < rowLines; ++i)
            {
                Line tileLine = new Line()
                {
                    Stroke = Brushes.White,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,

                    X1 = 0,
                    X2 = width
                };

                tileLine.Opacity = 0.4;
                tileLine.Y1 = tileLine.Y2 = yDelta * i;
                this.canvas_Wrapper.Children.Add(tileLine);
                this._rowLines.AddLast(tileLine);
            }
        }

        private void InitializeSelectedTiles(double xDelta, double yDelta, int colLines, int rowLines)
        {
            _areaTiles = new Rectangle[colLines, rowLines];

            for (int i = 0; i < colLines; ++i)
            {
                for (int j = 0; j < rowLines; ++j)
                {
                    _areaTiles[i, j] = new Rectangle()
                    {
                        Opacity = 0.6,
                        StrokeThickness = 3,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,

                        Visibility = Visibility.Hidden,

                        Width = xDelta,
                        Height = yDelta,
                        Margin = new Thickness()
                        {
                            Left = i * xDelta,
                            Top = j * yDelta,
                        }
                    };
                }
            }
        }

        private void InitializePenPoints()
        {
            for (int i = 0; i < _penPoints.Count(); ++i)
            {
                _penPoints[i] = new _PenPoint()
                {
                    DrawTarget = new Ellipse()
                    {
                        Visibility = Visibility.Hidden,
                        Fill = Brushes.Red,
                        Height = POINT_WIDTH,
                        Width = POINT_HEIGHT,
                    }
                };
            };
        }

        private void InitializePenLines()
        {
            for(int i = 0; i < _penLines.Count(); ++i)
            {
                _penLines[i] = new _PenLine()
                {
                    DrawTarget = new Line()
                    {
                        Visibility = Visibility.Hidden,

                        Stroke = Brushes.Red,
                        StrokeThickness = 2,

                        StrokeDashArray = new DoubleCollection() { 2, 3 },

                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                };
            }
        }

        private void RerenderSelectedTile(double xDelta, double yDelta)
        {
            this._selectedTile.Width = xDelta;
            this._selectedTile.Height = yDelta;

            this._selectedTile.Margin = new Thickness()
            {
                Left = xDelta * this._selectedTilePosition.X,
                Top = yDelta * this._selectedTilePosition.Y,
                Right = 0,
                Bottom = 0,
            };
        }


        public bool Valid
        {
            get { return (bool)GetValue(ValidProperty); }
            set
            {
                if (value == (bool)GetValue(ValidProperty))
                {
                    return;
                }
                SetValue(ValidProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Valid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidProperty =
            DependencyProperty.Register("Valid", typeof(bool), typeof(MapEditorControl), new PropertyMetadata());

        public int TileWidth
        {
            get { return (int)GetValue(TileWidthProperty); }
            set
            {
                if(value == (int)GetValue(TileWidthProperty))
                {
                    return;
                }
                SetValue(TileWidthProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for TileWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileWidthProperty =
            DependencyProperty.Register("TileWidth", typeof(int), typeof(MapEditorControl), new PropertyMetadata(OnTileWidthChanged));

        private static void OnTileWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as MapEditorControl;

            Messenger.Default.Send<int>(source.TileWidth, MapEditorControlMessageTokens.UpdateTileWidthFromView);
        }

        public int TileHeight
        {
            get { return (int)GetValue(TileHeightProperty); }
            set
            {
                if(value == (int)GetValue(TileHeightProperty))
                {
                    return;
                }
                SetValue(TileHeightProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for TileHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileHeightProperty =
            DependencyProperty.Register("TileHeight", typeof(int), typeof(MapEditorControl), new PropertyMetadata(OnTileHeightChanged));

        private static void OnTileHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as MapEditorControl;

            Messenger.Default.Send<int>(source.TileHeight, MapEditorControlMessageTokens.UpdateTileHeightFromView);
        }

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set
            {
                if (value == (double)GetValue(ZoomProperty))
                {
                    return;
                }
                SetValue(ZoomProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Zoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(MapEditorControl), new PropertyMetadata(OnZoomChanged));

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as MapEditorControl;

            Messenger.Default.Send<double>(source.Zoom, MapEditorControlMessageTokens.UpdateZoomFromView);

        }

        public string BackgroundSource
        {
            get { return (string)GetValue(BackgroundSourceProperty); }
            set
            {
                if (value == (string)GetValue(BackgroundSourceProperty))
                {
                    return;
                }
                SetValue(BackgroundSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for BackgroundSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundSourceProperty =
            DependencyProperty.Register("BackgroundSource", typeof(string), typeof(MapEditorControl), new PropertyMetadata(OnBackgroundImageChanged));

        private static void OnBackgroundImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as MapEditorControl;

            Messenger.Default.Send<string>(source.BackgroundSource, MapEditorControlMessageTokens.UpdateBackgroudMapFromView);
        }

        public double ScrollVerticalOffsetRatio
        {
            get { return (double)GetValue(ScrollVerticalOffsetRatioProperty); }
            set
            {
                if (Math.Abs(value - (double)GetValue(ScrollVerticalOffsetRatioProperty)) < 0.00001)
                {
                    return;
                }
                SetValue(ScrollVerticalOffsetRatioProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ScrollVerticalOffsetRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollVerticalOffsetRatioProperty =
            DependencyProperty.Register("ScrollVerticalOffsetRatio", typeof(double), typeof(MapEditorControl), new PropertyMetadata(OnScrollVerticalOffsetRatioChanged));

        private static void OnScrollVerticalOffsetRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MapEditorControl;
            var offset = (control.canvas_Wrapper.Height - control.scroll_Main.ViewportHeight) * control.ScrollVerticalOffsetRatio;

            control.scroll_Main.ScrollToVerticalOffset(offset);
        }

        public double ScrollHorizentalOffsetRatio
        {
            get { return (double)GetValue(ScrollHorizentalOffsetRatioProperty); }
            set
            {
                if(Math.Abs(value - (double)GetValue(ScrollHorizentalOffsetRatioProperty)) < 0.00001)
                {
                    return;
                }
                SetValue(ScrollHorizentalOffsetRatioProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ScrollHorizentalOffsetRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollHorizentalOffsetRatioProperty =
            DependencyProperty.Register("ScrollHorizentalOffsetRatio", typeof(double), typeof(MapEditorControl), new PropertyMetadata(OnScrollHorizentalOffsetRatioChanged));

        private static void OnScrollHorizentalOffsetRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MapEditorControl;
            var offset = (control.canvas_Wrapper.Width - control.scroll_Main.ViewportWidth) * control.ScrollHorizentalOffsetRatio;

            control.scroll_Main.ScrollToHorizontalOffset(offset);
        }

        public double ContentWidthRatio
        {
            get { return (double)GetValue(ContentWidthRatioProperty); }
            set
            {
                if (value == (double)GetValue(ContentWidthRatioProperty))
                {
                    return;
                }
                SetValue(ContentWidthRatioProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ContentWidthRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentWidthRatioProperty =
            DependencyProperty.Register("ContentWidthRatio", typeof(double), typeof(MapEditorControl), new PropertyMetadata());

        public double ContentHeightRatio
        {
            get { return (double)GetValue(ContentHeightRatioProperty); }
            set
            {
                if (value == (double)GetValue(ContentHeightRatioProperty))
                {
                    return;
                }
                SetValue(ContentHeightRatioProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for _contentHeightRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHeightRatioProperty =
            DependencyProperty.Register("ContentHeightRatio", typeof(double), typeof(MapEditorControl), new PropertyMetadata());

        public bool DragToolSelected
        {
            get { return (bool)GetValue(DragToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(DragToolSelectedProperty))
                {
                    return;
                }
                SetValue(DragToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DragToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragToolSelectedProperty =
            DependencyProperty.Register("DragToolSelected", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnDragToolSelectedChanged));

        private static void OnDragToolSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdateDragToolSelectedFromView);
        }

        public bool AreaToolSelected
        {
            get { return (bool)GetValue(AreaToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(AreaToolSelectedProperty))
                {
                    return;
                }
                SetValue(AreaToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for AreaToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaToolSelectedProperty =
            DependencyProperty.Register("AreaToolSelected", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnAreaToolSelectedChanged));

        private static void OnAreaToolSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdateAreaToolSelectedFromView);
        }

        public bool PenToolSelected
        {
            get { return (bool)GetValue(PenToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(PenToolSelectedProperty))
                {
                    return;
                }
                SetValue(PenToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PenToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PenToolSelectedProperty =
            DependencyProperty.Register("PenToolSelected", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnPenToolSelectedChanged));

        private static void OnPenToolSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdatePenToolSelectedFromView);
        }

        public bool PointToolSelected
        {
            get { return (bool)GetValue(PointToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(PointToolSelectedProperty))
                {
                    return;
                }
                SetValue(PointToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PointToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointToolSelectedProperty =
            DependencyProperty.Register("PointToolSelected", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnPointToolSelectedChanged));

        private static void OnPointToolSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdatePointToolSelectedFromView);
        }

        public bool TransparentSelected
        {
            get { return (bool)GetValue(TransparentSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(TransparentSelectedProperty))
                {
                    return;
                }
                SetValue(TransparentSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for TransparentSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransparentSelectedProperty =
            DependencyProperty.Register("TransparentSelected", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnTransparentSelectedChanged));

        private static void OnTransparentSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdateTransparentSelectedFromView);
        }


        public bool Safety
        {
            get { return (bool)GetValue(SafetyProperty); }
            set
            {
                if (value == (bool)GetValue(SafetyProperty))
                {
                    return;
                }
                SetValue(SafetyProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Safety.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SafetyProperty =
            DependencyProperty.Register("Safety", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnSafetyChanged));

        private static void OnSafetyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdateSafetyFromView);
        }


        public bool Fishing
        {
            get { return (bool)GetValue(FishingProperty); }
            set
            {
                if (value == (bool)GetValue(FishingProperty))
                {
                    return;
                }
                SetValue(FishingProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Fishing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FishingProperty =
            DependencyProperty.Register("Fishing", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(OnFishingChanged));

        private static void OnFishingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MapEditorControlMessageTokens.UpdateFishingFromView);
        }


        public bool ToolBarCanEdit
        {
            get { return (bool)GetValue(ToolBarCanEditProperty); }
            set
            {
                if (value == (bool)GetValue(ToolBarCanEditProperty))
                {
                    return;
                }
                SetValue(ToolBarCanEditProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ToolBarCanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToolBarCanEditProperty =
            DependencyProperty.Register("ToolBarCanEdit", typeof(bool), typeof(MapEditorControl), new PropertyMetadata(true));


        public string DefaultBornPointImage
        {
            get { return (string)GetValue(DefaultBornPointImageProperty); }
            set
            {
                if (value == (string)GetValue(DefaultBornPointImageProperty)) {
                    return;
                }
                SetValue(DefaultBornPointImageProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DefaultBornPointImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultBornPointImageProperty =
            DependencyProperty.Register("DefaultBornPointImage", typeof(string), typeof(MapEditorControl), new PropertyMetadata(OnDefaultBornPointImageChanged));

        private static void OnDefaultBornPointImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.UpdateDefaultBornPointImageFromView);
        }

        public int BornPointX
        {
            get { return (int)GetValue(BornPointXProperty); }
            set
            {
                if (value == (int)GetValue(BornPointXProperty)) {
                    return;
                }
                SetValue(BornPointXProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for BornPointX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BornPointXProperty =
            DependencyProperty.Register("BornPointX", typeof(int), typeof(MapEditorControl), new PropertyMetadata(OnBornPointXChanged));

        private static void OnBornPointXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<Point>(new Point
            {
                X = (int)e.NewValue,
                Y = (d as MapEditorControl).BornPointY,
            }, MapEditorControlMessageTokens.UpdateBornPointXFromView);
        }

        public int BornPointY
        {
            get { return (int)GetValue(BornPointYProperty); }
            set
            {
                if (value == (int)GetValue(BornPointYProperty)) {
                    return;
                }
                SetValue(BornPointYProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for BornPointY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BornPointYProperty =
            DependencyProperty.Register("BornPointY", typeof(int), typeof(MapEditorControl), new PropertyMetadata(OnBornPointYChanged));

        private static void OnBornPointYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<Point>(new Point
            {
                Y = (int)e.NewValue,
                X = (d as MapEditorControl).BornPointX,
            }, MapEditorControlMessageTokens.UpdateBornPointXFromView);
        }

        public int MapPixelWidth
        {
            get { return (int)GetValue(MapPixelWidthProperty); }
            set
            {
                if (value == (int)GetValue(MapPixelWidthProperty))
                {
                    return;
                }
                SetValue(MapPixelWidthProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MapPixelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapPixelWidthProperty =
            DependencyProperty.Register("MapPixelWidth", typeof(int), typeof(MapEditorControl), new PropertyMetadata());

        public int MapPixelHeight
        {
            get { return (int)GetValue(MapPixelHeightProperty); }
            set
            {
                if (value == (int)GetValue(MapPixelHeightProperty))
                {
                    return;
                }
                SetValue(MapPixelHeightProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MapPixelHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapPixelHeightProperty =
            DependencyProperty.Register("MapPixelHeight", typeof(int), typeof(MapEditorControl), new PropertyMetadata());



        public MonsterDictoryAndMonsterObject CurrentSceneMonsters
        {
            get { return (MonsterDictoryAndMonsterObject)GetValue(CurrentSceneMonstersProperty); }
            set
            {
                if (value == (MonsterDictoryAndMonsterObject)GetValue(CurrentSceneMonstersProperty))
                {
                    return;
                }
                SetValue(CurrentSceneMonstersProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentSceneMonsters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSceneMonstersProperty =
            DependencyProperty.Register("CurrentSceneMonsters", typeof(MonsterDictoryAndMonsterObject), typeof(MapEditorControl), new PropertyMetadata(OnCurrentSceneMonstersChanged));

        private static void OnCurrentSceneMonstersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<MonsterDictoryAndMonsterObject>(e.NewValue as MonsterDictoryAndMonsterObject, MapEditorControlMessageTokens.UpdateCurrentSceneMonsters);
        }


        public int CurrentMapID
        {
            get { return (int)GetValue(CurrentMapIDProperty); }
            set
            {
                if (value == (int)GetValue(CurrentMapIDProperty))
                {
                    return;
                }
                SetValue(CurrentMapIDProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMapID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapIDProperty =
            DependencyProperty.Register("CurrentMapID", typeof(int), typeof(MapEditorControl), new PropertyMetadata(OnCurrentMapIDChanged));

        private static void OnCurrentMapIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<int>((int)e.NewValue, MapEditorControlMessageTokens.UpdateCurrentMapIDFromView);
        }


        public NpcDictoryAndNpcObject CurrentNpcs
        {
            get { return (NpcDictoryAndNpcObject)GetValue(CurrentNpcsProperty); }
            set
            {
                if (value == (NpcDictoryAndNpcObject)GetValue(CurrentNpcsProperty))
                {
                    return;
                }
                SetValue(CurrentNpcsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentNpcs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentNpcsProperty =
            DependencyProperty.Register("CurrentNpcs", typeof(NpcDictoryAndNpcObject), typeof(MapEditorControl), new PropertyMetadata(OnCurrentNpcsChanged));

        private static void OnCurrentNpcsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<NpcDictoryAndNpcObject>(e.NewValue as NpcDictoryAndNpcObject, MapEditorControlMessageTokens.UpdateCurrentNpcs);
        }


        public MonsterSection CurrentMonster
        {
            get { return (MonsterSection)GetValue(CurrentMonsterProperty); }
            set
            {
                if (value == (MonsterSection)GetValue(CurrentMonsterProperty))
                {
                    return;
                }
                SetValue(CurrentMonsterProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMonster.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMonsterProperty =
            DependencyProperty.Register("CurrentMonster", typeof(MonsterSection), typeof(MapEditorControl), new PropertyMetadata());

        public SceneMonsterPOJO CurrentSceneMonster
        {
            get { return (SceneMonsterPOJO)GetValue(CurrentSceneMonsterProperty); }
            set
            {
                if (value == (SceneMonsterPOJO)GetValue(CurrentSceneMonsterProperty))
                {
                    return;
                }
                SetValue(CurrentSceneMonsterProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentSceneMonster.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSceneMonsterProperty =
            DependencyProperty.Register("CurrentSceneMonster", typeof(SceneMonsterPOJO), typeof(MapEditorControl), new PropertyMetadata(OnCurrentSceneMonsterChanged));

        private static void OnCurrentSceneMonsterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Messenger.Default.Send<SceneMonsterPOJO>(e.NewValue as SceneMonsterPOJO, MapEditorControlMessageTokens.UpdateCurrentSceneMonsterFromView);

            var control = d as MapEditorControl;

            if (e.NewValue == null || control._currentSprite == null)
            {
                return;
            }

            var newPOJO = e.NewValue as SceneMonsterPOJO;
            var sprite = control._currentSprite.SpriteObject;

            sprite.ID = newPOJO.SceneMonsterID;
            sprite.Name = newPOJO.Name;

            var oldPath = sprite.SpriteImage;
            var pos = oldPath.LastIndexOf('\\');
            var newPath = oldPath.Substring(0, pos);
            newPath += ("\\" + newPOJO.Name + ".png");

            sprite.SceneObjectBase = e.NewValue as SceneMonsterPOJO;
        }

        public ObservableCollection<MapObjectSprite> ItemSource
        {
            get { return (ObservableCollection<MapObjectSprite>)GetValue(ItemSourceProperty); }
            set
            {
                if (value == (ObservableCollection<MapObjectSprite>)GetValue(ItemSourceProperty))
                {
                    return;
                }
                SetValue(ItemSourceProperty, value);
            }
        }


        public NpcSection CurrentNPC
        {
            get { return (NpcSection)GetValue(CurrentNPCProperty); }
            set
            {
                if (value == (NpcSection)GetValue(CurrentNPCProperty))
                {
                    return;
                }
                SetValue(CurrentNPCProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentNPC.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentNPCProperty =
            DependencyProperty.Register("CurrentNPC", typeof(NpcSection), typeof(MapEditorControl), new PropertyMetadata());

        // Using a DependencyProperty as the backing store for ItemSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(ObservableCollection<MapObjectSprite>), typeof(MapEditorControl), new PropertyMetadata(OnItemSourceChanged));

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.OldValue != null)
            {
                (e.OldValue as ObservableCollection<MapObjectSprite>).CollectionChanged -= new NotifyCollectionChangedEventHandler(OnItemSourceChanged);
            }

            if(e.NewValue != null)
            {
                (e.NewValue as ObservableCollection<MapObjectSprite>).CollectionChanged += new NotifyCollectionChangedEventHandler(OnItemSourceChanged);
            }

            Messenger.Default.Send<ObservableCollection<MapObjectSprite>>(e.NewValue as ObservableCollection<MapObjectSprite>, MapEditorControlMessageTokens.SetNewObjectCollection);
        }

        static void OnItemSourceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var list = sender as ObservableCollection<MapObjectSprite>;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Messenger.Default.Send<IList>(e.NewItems, MapEditorControlMessageTokens.ObjectCollectionAdded);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Messenger.Default.Send<IList>(e.OldItems, MapEditorControlMessageTokens.ObjectCollectionRemoved);
                    break;
                    // won't work
                case NotifyCollectionChangedAction.Replace:
                    Messenger.Default.Send<IList>(e.OldItems, MapEditorControlMessageTokens.ObjectCollectionReplaced);
                    break;
                    // won't work
                case NotifyCollectionChangedAction.Move:
                    Messenger.Default.Send<IList>(e.OldItems as IList, MapEditorControlMessageTokens.ObjectCollectionMoved);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ObjectCollectionReseted);
                    break;
            }
        }

        private void AdjustOffsetRatio(ScrollChangedEventArgs e)
        {
            double widthRatio = 0;
            double heightRatio = 0;

            if (this.canvas_Wrapper.Width - this.scroll_Main.ViewportWidth == 0)
            {
                widthRatio = 0;
            }
            else
            {
                widthRatio = e.HorizontalOffset / (this.canvas_Wrapper.Width - this.scroll_Main.ViewportWidth);
            }

            if (this.canvas_Wrapper.Height - this.scroll_Main.ViewportHeight == 0)
            {
                heightRatio = 0;
            }
            else
            {
                heightRatio = e.VerticalOffset / (this.canvas_Wrapper.Height - this.scroll_Main.ViewportHeight);
            }

            if (widthRatio > 1)
            {
                widthRatio = 1;
            }

            if (heightRatio > 1)
            {
                heightRatio = 1;
            }

            this.ScrollHorizentalOffsetRatio = widthRatio;
            this.ScrollVerticalOffsetRatio = heightRatio;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var size = base.ArrangeOverride(arrangeBounds);

            Messenger.Default.Send<object>(null, MapEditorControlMessageTokens.ArrangedFromView);

            return size;
        }

        public static void AdjustRatio(MapEditorControl source)
        {
            source.ContentWidthRatio = source.scroll_Main.ActualWidth / source.canvas_Wrapper.Width;
            source.ContentHeightRatio = source.scroll_Main.ActualHeight / source.canvas_Wrapper.Height;
        }

        public Point GetDropCursorPosition(DragEventArgs args) {
            return args.GetPosition(canvas_Wrapper);
        }
    }
}
