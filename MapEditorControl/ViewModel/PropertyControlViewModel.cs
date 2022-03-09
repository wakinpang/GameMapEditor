using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MapEditorControl.ViewModel
{
    public class PropertyControlViewModel : ViewModelBase, IDataErrorInfo
    {
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

        private MonsterSection _currentMonsterSection;

        public MonsterSection CurrentMonsterSection
        {
            get { return _currentMonsterSection; }
            set
            {
                if (value == _currentMonsterSection)
                {
                    return;
                }
                _currentMonsterSection = value;
                RaisePropertyChanged(() => CurrentMonsterSection);
            }
        }

        private SceneMonsterPOJO _currentSceneMonsterPOJO;

        public SceneMonsterPOJO CurrentSceneMonsterPOJO
        {
            get { return _currentSceneMonsterPOJO; }
            set
            {
                if (value == _currentSceneMonsterPOJO)
                {
                    return;
                }
                _currentSceneMonsterPOJO = value;

                Messenger.Default.Send<SceneMonsterPOJO>(value, PropertyControlMessageTokens.UpdateCurrentSceneMonsterPOJOFromViewModel);

                RaisePropertyChanged(() => CurrentSceneMonsterPOJO);
            }
        }

        private NpcSection _currentNpcSection;
        public NpcSection CurrentNpcSection {
            get { return _currentNpcSection; }
            set {
                if (value == _currentNpcSection) {
                    return;
                }
                _currentNpcSection = value;

                Messenger.Default.Send<NpcSection>(value, PropertyControlMessageTokens.UpdateCurrentNpcSectionFromView);
                RaisePropertyChanged(() => CurrentNpcSection);
            }
        }

        private ObservableCollection<NpcSection> _currentNpcCollection;
        public ObservableCollection<NpcSection> CurrentNpcCollection {
            get { return _currentNpcCollection; }
            set {
                if (value == _currentNpcCollection) {
                    return;
                }
                _currentNpcCollection = value;

                RaisePropertyChanged(() => CurrentNpcCollection);
            }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;

                if(value != "" && value != null)
                {
                    CurrentMapSection.Name = value;
                }

                RaisePropertyChanged(() => Name);
            }
        }

        private string _level;

        public string Level
        {
            get { return _level; }
            set
            {
                if (value == _level)
                {
                    return;
                }

                int result = 0;
                if(int.TryParse(value, out result))
                {
                    CurrentMapSection.Level = result;
                }

                _level = value;
                RaisePropertyChanged(() => Level);
            }
        }

        private string _lineNum;

        public string LineNum
        {
            get { return _lineNum; }
            set
            {
                if (value == _lineNum)
                {
                    return;
                }

                short result = 0;
                if (short.TryParse(value, out result))
                {
                    CurrentMapSection.LineNum = result;
                }

                _lineNum = value;
                RaisePropertyChanged(() => LineNum);
            }
        }

        private int _currentMapID;

        public int CurrentMapID
        {
            get { return _currentMapID; }
            set
            {
                if (value == _currentMapID)
                {
                    return;
                }
                _currentMapID = value;
                RaisePropertyChanged(() => CurrentMapID);
            }
        }


        public string Error { get { return ""; } }

        public string this[string columnName]
        {
            get
            {
                if(CurrentMapSection == null)
                {
                    return "";
                }
                string resultString = "";
                int levelResult = 0;
                short lineNumResult = 0;
                switch (columnName)
                {
                    case "Name":
                        if(Name == "")
                        {
                            resultString = "地图名不得为空。";
                        }
                        break;
                    case "Level":
                        if(!int.TryParse(Level, out levelResult))
                        {
                            resultString = "等级必须是个int类型的数字。";
                        }
                        break;
                    case "LineNum":
                        if (!short.TryParse(LineNum, out lineNumResult))
                        {
                            resultString = "分线数量必须是个short类型的数字。";
                        }
                        break;
                }
                return resultString;
            }
        }

        string _backName = "";
        int _backLevel = 0;
        short _backLineNum = 0;

        private ICollectionView _currentSceneMonsterPOJOsView;

        public ICollectionView CurrentScenmeMonsterPOJOsView
        {
            get { return _currentSceneMonsterPOJOsView; }
            set
            {
                if (value == _currentSceneMonsterPOJOsView)
                {
                    return;
                }
                _currentSceneMonsterPOJOsView = value;
                RaisePropertyChanged(() => CurrentScenmeMonsterPOJOsView);
            }
        }

        private ObservableCollection<SceneMonsterPOJO> _pojos = null;

        public PropertyControlViewModel()
        {
            Messenger.Default.Register<MapSection>(this, PropertyControlMessageTokens.UpdateCurrentMapSectionFromView, (section) =>
            {
                if (CurrentMapSection != null)
                {
                    if (Name == "")
                    {
                        CurrentMapSection.Name = _backName;
                    }

                    int levelResult = 0;
                    if (!int.TryParse(Level, out levelResult))
                    {
                        CurrentMapSection.Level = _backLevel;
                    }

                    short lineNumResult = 0;
                    if (!short.TryParse(LineNum, out lineNumResult))
                    {
                        CurrentMapSection.LineNum = _backLineNum;
                    }
                }

                CurrentMapSection = section;

                if (CurrentMapSection != null)
                {
                    Name = _backName = CurrentMapSection.Name;
                    Level = (_backLevel = CurrentMapSection.Level).ToString();
                    LineNum = (_backLineNum = CurrentMapSection.LineNum).ToString();
                }
                else
                {
                    Name = null;
                    Level = null;
                    LineNum = null;
                }
            });

            Messenger.Default.Register<MonsterSection>(this, PropertyControlMessageTokens.UpdateCurrentMonsterSectionFromView, (section) =>
            {
                CurrentMonsterSection = section;

                if (section == null)
                {
                    Messenger.Default.Send<object>(null, PropertyControlMessageTokens.SetCurrentItemTabToMap);
                }
                else
                {
                    if (_pojos != null && CurrentMonsterSection != null && CurrentSceneMonsterPOJO != null && !CurrentSceneMonsterPOJO.Temp)
                    {
                        var cvs = new CollectionViewSource();
                        cvs.Source = _pojos;

                        CurrentScenmeMonsterPOJOsView = cvs.View;
                        CurrentScenmeMonsterPOJOsView.Filter = (o) =>
                        {
                            return CurrentMonsterSection == null
                                   ? false
                                   : (o as SceneMonsterPOJO).MonsterID == CurrentMonsterSection.MonsterID && (o as SceneMonsterPOJO).SceneID == CurrentMapID;
                            //_pojos = null;
                        };
                    }
                }
            });

            Messenger.Default.Register<SceneMonsterPOJO>(this, PropertyControlMessageTokens.UpdateCurrentSceneMonsterPOJOFromView, (pojo) =>
            {
                CurrentSceneMonsterPOJO = pojo;
            });

            Messenger.Default.Register<ObservableCollection<SceneMonsterPOJO>>(this, PropertyControlMessageTokens.UpdateCurrentMapSceneMonsterPOJOsFromView, (pojos) =>
            {
                //CurrentMapSceneMonsterPOJOs = pojos;

                _pojos = pojos;
            });

            Messenger.Default.Register<NpcSection>(this, PropertyControlMessageTokens.UpdateCurrentNpcSectionFromModel, (section) =>
            {
                CurrentNpcSection = section;
                if (CurrentNpcSection != null)
                {
                    CurrentNpcSection.MapId = CurrentMapID;
                }
            });

            Messenger.Default.Register<ObservableCollection<NpcSection>>(this, PropertyControlMessageTokens.UpdateCurrentNpcCollectionFromModel, (collection) => {
                CurrentNpcCollection = collection;
            });

            Messenger.Default.Register<int>(this, PropertyControlMessageTokens.UpdateCurrentMapIDFromView, (id) =>
            {
                CurrentMapID = id;
            });

            DropInfoButtonClicked = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.RaiseShowDropInfoEventFromViewModel);
            });

            EditSceneMonsterButtonClicked = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.RaiseEditSceneMonsterButtonEventFromViewModel);
            });

            NewSceneMonsterButtonClicked = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.RaiseNewSceneMonsterButtonEventFromViewModel);
            });

            OpenBaseRewardEditControl = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.RaiseShowBaseRewardEditControlEventFromViewModel);
            });

            OpenMissionRewardEditControl = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, PropertyControlMessageTokens.RaiseShowMissionRewardEditControlEventFromViewModel);
            });

        }

        public RelayCommand DropInfoButtonClicked { get; set; }
        public RelayCommand EditSceneMonsterButtonClicked { get; set; }
        public RelayCommand NewSceneMonsterButtonClicked { get; set; }

        public RelayCommand OpenBaseRewardEditControl { get; set; }
        public RelayCommand OpenMissionRewardEditControl { get; set; }
    }
}
