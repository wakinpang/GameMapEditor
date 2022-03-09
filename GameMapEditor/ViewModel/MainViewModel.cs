using DatabaseOperate;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameMapEditor.ViewModel
{
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
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Messenger.Default.Register<object>(this, MenuMessageTokens.FetchProjectDataFromDatabase, (dummy) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var mapData = from data in db.Map select data;
                    Messenger.Default.Send<ObservableCollection<MapSection>>(new ObservableCollection<MapSection>(mapData.ToList()), MainWindowTokens.UpdateMapSections);
                }
            });

            Messenger.Default.Register<int>(this, LibraryControlMessageTokens.FetchMapMonsterData, (mapID) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var monsterData = from data in db.Monster select data;
                    Messenger.Default.Send<ObservableCollection<MonsterSection>>(new ObservableCollection<MonsterSection>(monsterData.ToList()), MainWindowTokens.UpdateMonsterSections);

                    var tmp = LibraryControlModel.Instance().MonsterCollection;
                    var tmp2 = db.SceneMonster.ToList();
                    var joinMonsterData = from sceneMonster in tmp2
                                          from baseMonster in tmp
                                           where /*sceneMonster.SceneID == mapID &&*/ sceneMonster.MonsterID == baseMonster.MonsterID
                                           select new MapObjectAndSceneObjectInfo
                                           {
                                               BaseObject = baseMonster,
                                               SceneBaseObject = sceneMonster,
                                           };

                    var list = joinMonsterData.ToList();
                    Messenger.Default.Send<MonsterDictoryAndMonsterObject>(new MonsterDictoryAndMonsterObject
                    {
                        MonsterDictory = ProjectConfigModel.CurrentInstance().MonsterPicturePath,
                        Info = new ObservableCollection<MapObjectAndSceneObjectInfo>(list),
                    }, MainWindowTokens.UpdateCurrentSceneMonsters);

                    var sceneMonsters = from data in list
                                        select data.SceneBaseObject as SceneMonsterPOJO;
                    Messenger.Default.Send<ObservableCollection<SceneMonsterPOJO>>(new ObservableCollection<SceneMonsterPOJO>(sceneMonsters.Distinct().ToList()), MainWindowTokens.UpdateCurrentMapSceneMonsterPOJOs);
                }
            });

            Messenger.Default.Register<int>(this, LibraryControlMessageTokens.FetchMapNpcData, (mapID)=>{
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var npcData = from npc in db.Npc
                                  /*where npc.MapId == mapID*/
                                  select new MapObjectAndSceneObjectInfo()
                                  {
                                      BaseObject = npc,
                                      SceneBaseObject = null,
                                  };

                    var list = npcData.ToList();
                    foreach(var i in list)
                    {
                        (i.BaseObject as NpcSection).POJOStatus = POJOStatus.Normal;
                    }

                    Messenger.Default.Send<NpcDictoryAndNpcObject>(new NpcDictoryAndNpcObject()
                    {
                        NpcDictory = ProjectConfigModel.CurrentInstance().NpcPicturePath,
                        Info = new ObservableCollection<MapObjectAndSceneObjectInfo>(list),
                    }, MainWindowTokens.UpdateCurrentNpcs);

                    var npcs = from data in list
                               select data.BaseObject as NpcSection;                   
                    Messenger.Default.Send<ObservableCollection<NpcSection>>(new ObservableCollection<NpcSection>(npcs.ToList()), MainWindowTokens.UpdateCurrentMapNpcs);
                }
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.FetchItemData, (dummy) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var itemData = from item in db.Item
                                   select item;
                    Messenger.Default.Send<ObservableCollection<ItemPOJO>>(new ObservableCollection<ItemPOJO>(itemData.ToList()), MainWindowTokens.UpdateCurrentItemsInfo);
                }
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.FetchMissionData, (dummy) =>
            {
                using (var db = new DBMingtong(DatabaseUtil.MakeConnString(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseName,
                                                                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                                                                            ProjectConfigModel.CurrentInstance().DatabasePort)))
                {
                    var missionData = from mission in db.Mission
                                     select mission;
                    Messenger.Default.Send<ObservableCollection<MissionPOJO>>(new ObservableCollection<MissionPOJO>(missionData.ToList()), MainWindowTokens.UpdateCurrentMissionsInfo);
                }
            });

        }
    }
}