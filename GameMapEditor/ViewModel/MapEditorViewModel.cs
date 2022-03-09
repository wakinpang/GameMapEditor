using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.ViewModel
{
    using TileType = Byte;

    public class MapEditorViewModel : ViewModelBase
    {
        private MapEditorAndNavigationModel _mapEditorStatus;

        public MapEditorAndNavigationModel MapEditorStatus
        {
            get { return _mapEditorStatus; }
            set
            {
                _mapEditorStatus = value;
                RaisePropertyChanged(() => MapEditorStatus);
            }
        }

        public RelayCommand<TileType[,]> ModifySelectedAreaHandler { get; set; }
        public RelayCommand<DragEventArgs> DropHandler { get; set; }

        public MapEditorViewModel()
        {
            _mapEditorStatus = MapEditorAndNavigationModel.Instance();

            Messenger.Default.Register<BaseMapInfo>(this, MainWindowTokens.SendDropCursorPositionAndAddMapObject, (info) => {
                if (info.GetType() == typeof(DropMapMonsterInfo))
                {
                    var e = info as DropMapMonsterInfo;
                    var result = from sm in LibraryControlModel.Instance().CurrentMapSceneMonsterPOJOs
                                 where sm.MonsterID == e.Monster.MonsterID && sm.SceneID == LibraryControlModel.Instance().CurrentMapSection.MapID
                                 select sm;

                    MapObjectSprite newSprite = null;

                    if(result.Count() > 0)
                    {
                        var sceneMonster = result.ElementAt(0);
                        newSprite = new MapObjectSprite()
                        {
                            ID = sceneMonster.SceneMonsterID,
                            X = e.Position.X / MapEditorStatus.Zoom,
                            Y = e.Position.Y / MapEditorStatus.Zoom,
                            Name = sceneMonster.Name,
                            SpriteImage = ProjectConfigModel.CurrentInstance().MonsterPicturePath + "\\" + sceneMonster.Style.ToString() + ".png",
                            ObjectBase = e.Monster,
                            SceneObjectBase = sceneMonster,
                        };
                    }
                    else
                    {
                        var sceneMonster = new SceneMonsterPOJO()
                        {
                            Name = e.Monster.Name,
                            MonsterID = e.Monster.MonsterID,
                            Temp = true,
                        };

                        //MapEditorAndNavigationModel.Instance().CurrentSceneMonster = sceneMonster;

                        newSprite = new MapObjectSprite()
                        {
                            ID = sceneMonster.SceneMonsterID,
                            X = e.Position.X / MapEditorStatus.Zoom,
                            Y = e.Position.Y / MapEditorStatus.Zoom,
                            Name = e.Monster.Name,
                            SpriteImage = "image\\tmp_monster.bmp",
                            ObjectBase = e.Monster,
                            SceneObjectBase = sceneMonster,
                        };
                    }
                    
                    MapEditorStatus.MapObjectCollection.Add(newSprite);
                }
                else if (info.GetType() == typeof(DropMapNpcInfo))
                {
                    var e = info as DropMapNpcInfo;

                    var npc = new NpcSection()
                    {
                        Temp = true,
                        PosX = (int)e.Position.X,
                        PosY = (int)e.Position.Y,
                        POJOStatus = POJOStatus.Inserted,
                        Type = e.Npc.Type,
                    };

                    MapEditorStatus.MapObjectCollection.Add(new MapObjectSprite()
                    {
                        ID = 0,
                        X = e.Position.X,
                        Y = e.Position.Y,
                        Name = "",
                        SpriteImage = e.Npc.CPath,
                        ObjectBase = npc,
                        SceneObjectBase = null,
                        FolderPath = ProjectConfigModel.CurrentInstance().NpcPicturePath,
                    });

                    LibraryControlModel.Instance().CurentNpcs.Add(npc);
                }
            });

            Messenger.Default.Register<MonsterDictoryAndMonsterObject>(this, MainWindowTokens.UpdateCurrentSceneMonsters, (monsters) =>
            {
                MapEditorStatus.CurrentMonsters = monsters;
            });

            Messenger.Default.Register<NpcDictoryAndNpcObject>(this, MainWindowTokens.UpdateCurrentNpcs, (npcs) =>
            {
                MapEditorStatus.CurrentNpcs = npcs;
            });

            MapEditorStatus.MapObjectCollection = new System.Collections.ObjectModel.ObservableCollection<MapObjectSprite>();

            ModifySelectedAreaHandler = new RelayCommand<TileType[,]>((tiles) =>
            {
                MapEditorStatus.MapTiles = tiles;
            });

            DropHandler = new RelayCommand<DragEventArgs>((args) =>
            {
                if (args.Data.GetDataPresent(typeof(MonsterSection))) {
                    MonsterSection section = (MonsterSection)args.Data.GetData(typeof(MonsterSection));
                    Messenger.Default.Send<BaseMapInfo>(new DropMapMonsterInfo()
                    {
                        Monster = section,
                        Args = args,
                    }, MainWindowTokens.GetDropCursorPosition);
                }
                else if (args.Data.GetDataPresent(typeof(NpcSection))) {
                    NpcSection section = (NpcSection)args.Data.GetData(typeof(NpcSection));
                    Messenger.Default.Send<BaseMapInfo>(new DropMapNpcInfo()
                    {
                        Npc = section,
                        Args = args,
                    }, MainWindowTokens.GetDropCursorPosition);
                }
            });
        }
    }
}
