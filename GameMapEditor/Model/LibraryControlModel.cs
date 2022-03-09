using DatabaseOperate;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEditorControl.InnerUtil;
using System.Collections.ObjectModel;
using System.Data;

namespace GameMapEditor.Model
{
    public class LibraryControlModel : ObservableObject
    {
        private static LibraryControlModel _instance = new LibraryControlModel();
        public static LibraryControlModel Instance()
        {
            return _instance;
        }

        private OkTypes _types;
        public OkTypes Types {
            get { return _types; }
            set {
                _types = value;
                RaisePropertyChanged(() => Types);
            }
        }

        private ObservableCollection<MapSection> _mapCollection;
        public ObservableCollection<MapSection> MapCollection
        {
            get { return _mapCollection; }
            set
            {
                _mapCollection = value;
                RaisePropertyChanged(() => MapCollection);
            }
        }

        private ObservableCollection<MusicSection> _musicCollection;
        public ObservableCollection<MusicSection> MusicCollection
        {
            get { return _musicCollection; }
            set
            {
                _musicCollection = value;
                RaisePropertyChanged(() => MusicCollection);
            }
        }

        private ObservableCollection<MonsterSection> _monsterCollection;
        public ObservableCollection<MonsterSection> MonsterCollection
        {
            get { return _monsterCollection; }
            set
            {
                _monsterCollection = value;
                RaisePropertyChanged(() => MonsterCollection);
            }
        }

        private MapSection _currentMapSection;

        public MapSection CurrentMapSection
        {
            get { return _currentMapSection; }
            set
            {
                if (value == _currentMapSection)
                {
                    return;
                }
                _currentMapSection = value;
                RaisePropertyChanged(() => CurrentMapSection);
            }
        }

        private ObservableCollection<ItemPOJO> _currentItemsInfo;

        public ObservableCollection<ItemPOJO> CurrentItemsInfo
        {
            get { return _currentItemsInfo; }
            set
            {
                if (value == _currentItemsInfo)
                {
                    return;
                }
                _currentItemsInfo = value;
                RaisePropertyChanged(() => CurrentItemsInfo);
            }
        }

        private ObservableCollection<SceneMonsterPOJO> _currentMapSceneMonsterPOJOs;

        public ObservableCollection<SceneMonsterPOJO> CurrentMapSceneMonsterPOJOs
        {
            get { return _currentMapSceneMonsterPOJOs; }
            set
            {
                if (value == _currentMapSceneMonsterPOJOs)
                {
                    return;
                }
                _currentMapSceneMonsterPOJOs = value;
                RaisePropertyChanged(() => CurrentMapSceneMonsterPOJOs);
            }
        }

        private ObservableCollection<MissionPOJO> _currentMissionPOJOs;

        public ObservableCollection<MissionPOJO> CurrentMissionPOJOs
        {
            get { return _currentMissionPOJOs; }
            set
            {
                if (value == _currentMissionPOJOs)
                {
                    return;
                }
                _currentMissionPOJOs = value;
                RaisePropertyChanged(() => CurrentMissionPOJOs);
            }
        }

        private ObservableCollection<NpcSection> _currentNpcs;

        public ObservableCollection<NpcSection> CurentNpcs
        {
            get { return _currentNpcs; }
            set
            {
                if (value == _currentNpcs)
                {
                    return;
                }
                _currentNpcs = value;
                RaisePropertyChanged(() => CurentNpcs);
            }
        }



        public LibraryControlModel() {
        }
    }
}
