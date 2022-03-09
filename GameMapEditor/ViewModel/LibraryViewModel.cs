#define SYNC_TO_DATABASE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using GameMapEditor.InnerUtil;
using DatabaseOperate;
using LinqToDB;
using GalaSoft.MvvmLight.Threading;

namespace GameMapEditor.ViewModel
{
    public class LibraryViewModel : ViewModelBase {
        private LibraryControlModel _librarySource;

        public RelayCommand NewMonsterHandle { get; set; }
        public RelayCommand ChangeMonsterHandle { get; set; }
        public RelayCommand MusicErrorHandle { get; set; }
        public RelayCommand<MapSection> CurrentMapChangedHandle { get; set; }
        public RelayCommand<MonsterSection> CurrentMonsterChangedHandle { get; set; }
        public RelayCommand<MusicSection> CurrentMusicChangedHandle { get; set; }
        public RelayCommand ShowDropInfo { get; set; }
        public RelayCommand EditConfirm { get; set; }
        public RelayCommand EditCancel { get; set; }
        public RelayCommand ModifySceneMonster { get; set; }
        public RelayCommand NewSceneMonster { get; set; }
        public RelayCommand<SceneMonsterConfigArgs> SceneMonsterEditOK { get; set; }
        public RelayCommand<SceneMonsterConfigArgs> SceneMonsterEditCancel { get; set; }

        public RelayCommand BaseRewardEdit { get; set; }
        public RelayCommand MissionRewardEdit { get; set; }

        public RelayCommand BaseRewardInfoEditOK { get; set; }
        public RelayCommand BaseRewardInfoEditCancel { get; set; }

        public RelayCommand TaskRewardInfoEditOK { get; set; }
        public RelayCommand TaskRewardInfoEditCancel { get; set; }

        private bool _sceneMonsterModifying;

        public bool SceneMonsterModifying
        {
            get { return _sceneMonsterModifying; }
            set
            {
                if (value == _sceneMonsterModifying)
                {
                    return;
                }
                _sceneMonsterModifying = value;
                RaisePropertyChanged(() => SceneMonsterModifying);
            }
        }


        public LibraryControlModel LibrarySource {
            get { return _librarySource; }
            set {
                _librarySource = value;
                RaisePropertyChanged(() => LibrarySource);
            }
        }

        private bool NpcDataValid(NpcSection npcData)
        {
            var r = from npc in LibrarySource.CurentNpcs
                    where npc != npcData && npc.POJOStatus != POJOStatus.Deleted && npc.NPCId == npcData.NPCId
                    select npc;

            return r.Count() == 0;
        }

        public static string get_uft8(string unicodeString)
        {
            //UTF8Encoding utf8 = new UTF8Encoding();
            //Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            //String decodedString = utf8.GetString(encodedBytes);
            //return decodedString;
            return unicodeString;
        }

        public LibraryViewModel() {
            LibrarySource = LibraryControlModel.Instance();

            Messenger.Default.Register<ObservableCollection<MapSection>>(this, MainWindowTokens.UpdateMapSections, (list) =>
            {
                LibrarySource.MapCollection = list;

                foreach(var i in list)
                {
                    i.POJOStatus = POJOStatus.Normal;
                }
            });

            Messenger.Default.Register<ObservableCollection<MonsterSection>>(this, MainWindowTokens.UpdateMonsterSections, (list) =>
            {
                LibrarySource.MonsterCollection = list;

                foreach (var i in list)
                {
                    i.POJOStatus = POJOStatus.Normal;
                }
            });

            Messenger.Default.Register<ObservableCollection<MusicSection>>(this, MainWindowTokens.UpdateMusicSections, (list) =>
            {
                LibrarySource.MusicCollection = list;
            });

            Messenger.Default.Register<ObservableCollection<ItemPOJO>>(this, MainWindowTokens.UpdateCurrentItemsInfo, (list) =>
            {
                LibrarySource.CurrentItemsInfo = list;
            });

            Messenger.Default.Register<ObservableCollection<SceneMonsterPOJO>>(this, MainWindowTokens.UpdateCurrentMapSceneMonsterPOJOs, (list) =>
            {
                LibrarySource.CurrentMapSceneMonsterPOJOs = list;
                foreach (var i in list)
                {
                    i.POJOStatus = POJOStatus.Normal;
                }

            });

            Messenger.Default.Register<ObservableCollection<NpcSection>>(this, MainWindowTokens.UpdateCurrentMapNpcs, (npcs) =>
            {
                LibrarySource.CurentNpcs = npcs;
                foreach (var i in npcs)
                {
                    i.POJOStatus = POJOStatus.Normal;
                }

                return;
            });

            Messenger.Default.Register<object>(this, ToolBarControlMessageTokens.SyncMapData, (dummy) =>
            {
                // 处理数据
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    foreach(var map in LibrarySource.MapCollection)
                    {
                        switch (map.POJOStatus)
                        {
                            case POJOStatus.Normal:
                                break;
                            case POJOStatus.Inserted:
                                //db.Insert(map);
                                map.Name = get_uft8(map.Name);
                                System.Diagnostics.Debug.WriteLine("Map Inserted:", map.MapID);
                                break;
                            case POJOStatus.Updated:
                                map.Name = get_uft8(map.Name);
                                db.Update(map);
                                System.Diagnostics.Debug.WriteLine("Map Updated:", map.MapID);
                                break;
                            case POJOStatus.Deleted:
                                break;
                        }
                        map.POJOStatus = POJOStatus.Normal;
                    }
                }
            });

            Messenger.Default.Register<object>(this, ToolBarControlMessageTokens.SyncMonsterData, (dummy) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    foreach(var monster in LibrarySource.MonsterCollection)
                    {
                        switch (monster.POJOStatus)
                        {
                            case POJOStatus.Normal:
                                break;
                            case POJOStatus.Inserted:
#if SYNC_TO_DATABASE
                                monster.Name = get_uft8(monster.Name);
                                db.Monster.Value(m => m.MonsterID, monster.MonsterID).Insert();
                                db.Update(monster);
#endif
                                System.Diagnostics.Debug.WriteLine("Monster Inserted:", monster.Name);
                                break;
                            case POJOStatus.Updated:
#if SYNC_TO_DATABASE
                                monster.Name = get_uft8(monster.Name);
                                db.Update(monster);
#endif
                                System.Diagnostics.Debug.WriteLine("Monster Updated:", monster.Name);

                                break;
                            case POJOStatus.Deleted:
                                break;
                        }
                        monster.POJOStatus = POJOStatus.Normal;
                    }
                }
            });

            Messenger.Default.Register<object>(this, ToolBarControlMessageTokens.SyncSceneMonsterData, (dummy) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var result = from obj in MapEditorAndNavigationModel.Instance().MapObjectCollection
                                 where obj.SceneObjectBase != null
                                 group obj by (obj.SceneObjectBase as SceneMonsterPOJO).SceneMonsterID into g
                                 select g;
                    foreach(var group in result)
                    {
                        var builder = new StringBuilder();
                        var flag = false;
                        foreach(var obj in group)
                        {
                            if (!flag)
                            {
                                builder.Append(obj.X).Append(",").Append(obj.Y);
                                flag = true;
                            }
                            else
                            {
                                builder.Append(";").Append(obj.X).Append(",").Append(obj.Y);
                            }
                        }
                        (group.ElementAt(0).SceneObjectBase as SceneMonsterPOJO).PosArr = builder.ToString();
                    }

                    foreach(var sceneMonster in LibraryControlModel.Instance().CurrentMapSceneMonsterPOJOs)
                    {
                        if (sceneMonster.Temp)
                        {
                            continue;
                        }
                        switch (sceneMonster.POJOStatus)
                        {
                            case POJOStatus.Normal:
                                break;
                            case POJOStatus.Inserted:
#if SYNC_TO_DATABASE
                                sceneMonster.Name = get_uft8(sceneMonster.Name);
                                db.SceneMonster.Value(m => m.MonsterID, sceneMonster.SceneMonsterID).Insert();
                                db.Update(sceneMonster);
#endif
                                System.Diagnostics.Debug.WriteLine("Scene Monster Inserted:", sceneMonster.Name);
                                break;
                            case POJOStatus.Updated:
#if SYNC_TO_DATABASE
                                sceneMonster.Name = get_uft8(sceneMonster.Name);
                                db.Update(sceneMonster);
#endif
                                System.Diagnostics.Debug.WriteLine("Scene Monster Updated:", sceneMonster.Name);
                                break;
                            case POJOStatus.Deleted:
                                break;
                        }
                        sceneMonster.POJOStatus = POJOStatus.Normal;
                    }
                }
            });

            Messenger.Default.Register<object>(this, ToolBarControlMessageTokens.SyncNpcData, (dummy) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                            ProjectConfigModel.CurrentInstance().DatabaseName,
                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var result = from obj in MapEditorAndNavigationModel.Instance().MapObjectCollection
                                 where obj.SceneObjectBase == null
                                 select obj.ObjectBase as NpcSection;

                    var colle = result.ToList().FindAll((elem) =>
                    {
                        var npcData = elem;
                        if (npcData.Temp)
                        {
                            if (npcData.POJOStatus != POJOStatus.Deleted && NpcDataValid(npcData))
                            {
                                npcData.Temp = false;
                                return true;
                            }
                            return false;
                        }
                        else
                        {
                            if(npcData.POJOStatus != POJOStatus.Normal)
                            {
                                return true;
                            }
                            return false;
                        }
                    });

                    //DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    //{
                    //    var deleted = from npc in MapEditorAndNavigationModel.Instance().MapObjectCollection
                    //                  where npc.SceneObjectBase == null && (npc.ObjectBase as NpcSection).POJOStatus == POJOStatus.Deleted && (npc.ObjectBase as NpcSection).Temp
                    //                  select npc;

                    //    foreach (var npc in deleted.ToList())
                    //    {
                    //        //if(!npc.)
                    //        System.Diagnostics.Debug.WriteLine("Npc Deleted:", npc.Name);
                    //        LibrarySource.CurentNpcs.Remove(npc.ObjectBase as NpcSection);
                    //        MapEditorAndNavigationModel.Instance().MapObjectCollection.Remove(npc);
                    //    }
                    //});

                    foreach (var npc in colle)
                    {
                        var npcData = npc as NpcSection;

                        switch (npcData.POJOStatus)
                        {
                            case POJOStatus.Normal:
                                break;
                            case POJOStatus.Inserted:
#if SYNC_TO_DATABASE
                                npcData.Name = get_uft8(npcData.Name);
                                db.Npc.Value(n => n.NPCId, npcData.NPCId).Insert();
                                db.Update(npcData);
#endif
                                System.Diagnostics.Debug.WriteLine("Npc Inserted:", npcData.Name);
                                break;
                            case POJOStatus.Updated:
#if SYNC_TO_DATABASE
                                npcData.Name = get_uft8(npcData.Name);
                                db.Update(npcData);
#endif
                                System.Diagnostics.Debug.WriteLine("Npc Updated:", npcData.Name);
                                break;
                            case POJOStatus.Deleted:
#if SYNC_TO_DATABASE
                                db.Delete(npcData);
#endif
                                System.Diagnostics.Debug.WriteLine("Npc Deleted:", npcData.Name);
                                break;
                        }
                        npcData.POJOStatus = POJOStatus.Normal;
                    }
                }

            });

            Messenger.Default.Register<ObservableCollection<MissionPOJO>>(this, MainWindowTokens.UpdateCurrentMissionsInfo, (missions) => 
            {
                LibrarySource.CurrentMissionPOJOs = missions;
            });


            NewMonsterHandle = new RelayCommand(() =>
            {
                LibrarySource.Types = OkTypes.Create;
                MapEditorAndNavigationModel.Instance().BackCurrentMonster = MapEditorAndNavigationModel.Instance().CurrentMonster;
                MapEditorAndNavigationModel.Instance().CurrentMonster = new MonsterSection();

                Messenger.Default.Send<string>("新建怪物", LibraryControlMessageTokens.ShowDialogFromViewModel);
            });

            ChangeMonsterHandle = new RelayCommand(() =>
            {
                LibrarySource.Types = OkTypes.Change;
                Messenger.Default.Send<string>("修改怪物配置", LibraryControlMessageTokens.ShowDialogFromViewModel);
            });

            CurrentMapChangedHandle = new RelayCommand<MapSection>((section) =>
            {
                //Messenger.Default.Send<object>(null, MainWindowTokens.ReInitializeMapEditor);

                //LibrarySource.CurrentMapSection = section;

                MapEditorAndNavigationModel.Instance().MapObjectCollection.Clear();
                MapEditorAndNavigationModel.Instance().BackgroundSource = ProjectConfigModel.CurrentInstance().MapSourcePath + "\\" + section.Name + ".jpg";
                if(MapEditorAndNavigationModel.Instance().Valid)
                {
                    ToolBarModel.INSTANCE.CanEdit = true;
                    LibrarySource.CurrentMapSection = section;
                    section.Width = MapEditorAndNavigationModel.Instance().MapPixelWidth;
                    section.Height = MapEditorAndNavigationModel.Instance().MapPixelHeight;

                    MapEditorAndNavigationModel.Instance().BornPointX = section.PosX;
                    MapEditorAndNavigationModel.Instance().BornPointY = section.PosY;

                    // Fetch Map Data
                    Messenger.Default.Send<int>(section.MapID, LibraryControlMessageTokens.FetchMapMonsterData);
                    Messenger.Default.Send<int>(section.MapID, LibraryControlMessageTokens.FetchMapNpcData);
                    //Messenger.Default.Send<int>(section.MapID, LibraryControlMessageTokens.FetchCurrentMapSceneMonsterPOJOsData);
                }
                else
                {
                    ToolBarModel.INSTANCE.CanEdit = false;
                    LibrarySource.CurrentMapSection = null;
                    Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                    {
                        Title = "发生错误",
                        Message = String.Format(
                            "读取地图文件 {0} 时发生错误，可能的原因：\n1、文件不存在；\n2、图像文件损坏。",
                            ProjectConfigModel.CurrentInstance().MapSourcePath + "\\" + section.Name + ".jpg",
                            MapEditorAndNavigationModel.Instance().TileWidth,
                            MapEditorAndNavigationModel.Instance().TileHeight),
                    }, MainWindowTokens.ShowMessageDialog);
                    MapEditorAndNavigationModel.Instance().BackgroundSource = "";
                }
            });

            CurrentMonsterChangedHandle = new RelayCommand<MonsterSection>((section) =>
            {
                MapEditorAndNavigationModel.Instance().CurrentMonster = section;
            });

            MusicErrorHandle = new RelayCommand(() =>
            {
                if(LibrarySource.CurrentMapSection == null)
                {
                    return;
                }

                Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                {
                    Title = "发生错误",
                    Message = String.Format("音乐{0}.mp3不存在于当前音乐目录下！", LibrarySource.CurrentMapSection.SoundId),
                }, MainWindowTokens.ShowMessageDialog);
            });

            ShowDropInfo = new RelayCommand(()=>
            {

                bool result = false;
                CostumeWaitingDialogHelper.CallWaitingDialog(
                    this,
                    (ref bool canceled) =>
                    {
                        result = DatabaseUtil.TryConn(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                    ProjectConfigModel.CurrentInstance().DatabaseName,
                                                    ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                    ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                    ProjectConfigModel.CurrentInstance().DatabasePort);
                    },
                    () =>
                    {
                        if (!result)
                        {
                            Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                            {
                                Title = "Failed",
                                Message = "数据库连接测试失败，请检查设置！",
                            }, MainWindowTokens.ShowMessageDialog);
                            return;
                        }

                        // 读取数据库信息
                        Messenger.Default.Send<object>(null, PropertyControlMessageTokens.FetchItemData);
                        Messenger.Default.Send<object>(null, PropertyControlMessageTokens.ShowDropInfoDialog);
                    },
                    "请等待",
                    "数据库连接测试中，请稍等……",
                    false
                );
            });

            EditConfirm = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.HideDropInfoDialog);
            });

            EditCancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.HideDropInfoDialog);
            });

            ModifySceneMonster = new RelayCommand(() =>
            {
                SceneMonsterModifying = true;
                Messenger.Default.Send<string>("修改当前场景怪模板", SceneMonsterConfigControlMessageTokens.ShowSceneMonsterPOJOConfigDialog);
            });

            NewSceneMonster = new RelayCommand(() =>
            {
                SceneMonsterModifying = false;
                Messenger.Default.Send<string>("新建场景怪模板", SceneMonsterConfigControlMessageTokens.ShowSceneMonsterPOJOConfigDialog);
                MapEditorAndNavigationModel.Instance().PreSceneMonsterPOJO = MapEditorAndNavigationModel.Instance().CurrentSceneMonster;
                MapEditorAndNavigationModel.Instance().CurrentSceneMonster = new SceneMonsterPOJO()
                {
                    SceneID = LibraryControlModel.Instance().CurrentMapSection.MapID,
                    MonsterID = MapEditorAndNavigationModel.Instance().CurrentMonster.MonsterID,
                    SceneType = MapEditorAndNavigationModel.Instance().CurrentSceneMonster.SceneType,
                    MonsterType = 5,
                };
            });

            SceneMonsterEditOK = new RelayCommand<SceneMonsterConfigArgs>((args) =>
            {
                if (SceneMonsterModifying)
                {
                    var current = MapEditorAndNavigationModel.Instance().CurrentSceneMonster;

                    var target = args.CurrentSceneMonsterPOJO;
                    CopyValue(target, current);

                    if (current.Temp)
                    {
                        current.Temp = false;
                        current.POJOStatus = POJOStatus.Inserted;

                        current.SceneID = LibrarySource.CurrentMapSection.MapID;

                        //MapEditorAndNavigationModel.Instance().CurrentSceneMonster = null;
                        MapEditorAndNavigationModel.Instance().CurrentSceneMonster = current;
                        LibraryControlModel.Instance().CurrentMapSceneMonsterPOJOs.Add(current);
                    }

                    var result = from mon in MapEditorAndNavigationModel.Instance().MapObjectCollection
                                 where mon.SceneObjectBase != null && (mon.SceneObjectBase as SceneMonsterPOJO).SceneMonsterID == current.SceneMonsterID
                                 select mon;

                    foreach(var mon in result)
                    {
                        mon.SpriteImage = ProjectConfigModel.CurrentInstance().MonsterPicturePath + "\\" + current.Style + ".png";
                        mon.Name = current.Name;
                        mon.ID = current.SceneMonsterID;
                    }
                }
                else
                {
                    MapEditorAndNavigationModel.Instance().CurrentSceneMonster = args.CurrentSceneMonsterPOJO;
                    LibraryControlModel.Instance().CurrentMapSceneMonsterPOJOs.Add(args.CurrentSceneMonsterPOJO);
                }

                Messenger.Default.Send<object>(null, SceneMonsterConfigControlMessageTokens.HideSceneMonsterPOJOConfigDialog);
            });

            SceneMonsterEditCancel = new RelayCommand<SceneMonsterConfigArgs>((args) =>
            {
                if (SceneMonsterModifying)
                {
                    MapEditorAndNavigationModel.Instance().CurrentSceneMonster = args.PreSceneMonsterPOJO;
                }
                else
                {
                    MapEditorAndNavigationModel.Instance().CurrentSceneMonster = MapEditorAndNavigationModel.Instance().PreSceneMonsterPOJO;
                }
                Messenger.Default.Send<object>(null, SceneMonsterConfigControlMessageTokens.HideSceneMonsterPOJOConfigDialog);
            });

            BaseRewardEdit = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.ShowBaseRewardInfoDialog);
            });

            MissionRewardEdit = new RelayCommand(() =>
            {
                //Messenger.Default.Send<object>(null, PropertyControlMessageTokens.ShowMissionRewardInfoDialog);
                bool result = false;
                CostumeWaitingDialogHelper.CallWaitingDialog(
                    this,
                    (ref bool canceled) =>
                    {
                        result = DatabaseUtil.TryConn(ProjectConfigModel.Instance().DatabaseIP,
                                                    ProjectConfigModel.Instance().DatabaseName,
                                                    ProjectConfigModel.Instance().DatabaseUserName,
                                                    ProjectConfigModel.Instance().DatabasePassword,
                                                    ProjectConfigModel.Instance().DatabasePort);
                    },
                    () =>
                    {
                        if (!result)
                        {
                            Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                            {
                                Title = "Failed",
                                Message = "数据库连接测试失败，请检查设置！",
                            }, MainWindowTokens.ShowMessageDialog);
                            return;
                        }

                        // 读取数据库信息
                        Messenger.Default.Send<object>(null, PropertyControlMessageTokens.FetchItemData);
                        Messenger.Default.Send<object>(null, PropertyControlMessageTokens.FetchMissionData);
                        Messenger.Default.Send<object>(null, PropertyControlMessageTokens.ShowMissionRewardInfoDialog);
                    },
                    "请等待",
                    "数据库连接测试中，请稍等……",
                    false
                );
            });

            BaseRewardInfoEditOK = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.HideBaseRewardInfoDialog);
            });

            BaseRewardInfoEditCancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.HideBaseRewardInfoDialog);
            });

            TaskRewardInfoEditOK = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.HideMissionRewardInfoDialog);
            });

            TaskRewardInfoEditCancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.HideMissionRewardInfoDialog);
            });

        }

        public static void CopyValue(SceneMonsterPOJO origin, SceneMonsterPOJO target)
        {
            target.Temp = origin.Temp;
            target.POJOStatus = origin.POJOStatus;

            target.SceneMonsterID = origin.SceneMonsterID;
            target.MonsterID = origin.MonsterID;
            target.SceneType = origin.SceneType;
            target.SceneID = origin.SceneID;
            target.WorldLevel = origin.WorldLevel;
            target.Camp = origin.Camp;
            target.Type = origin.Type;
            target.Floor = origin.Floor;
            target.MonsSeq = origin.MonsSeq;
            target.Point = origin.Point;
            target.Name = origin.Name;
            target.Style = origin.Style;
            target.ShowExt = origin.ShowExt;
            target.ReliveTime = origin.ReliveTime;
            target.MonsterType = origin.MonsterType;
            target.BaseRewardInfo = origin.BaseRewardInfo;
            target.TaskRewardInfo = origin.TaskRewardInfo;
            target.ItemDropID = origin.ItemDropID;
            target.PosArr = origin.PosArr;
            target.Pos = origin.Pos;
            target.Num = origin.Num;
            target.SkillData = origin.SkillData;
        }

    }
}
