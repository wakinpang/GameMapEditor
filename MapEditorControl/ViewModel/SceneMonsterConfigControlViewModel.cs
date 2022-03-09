using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.ViewModel
{
    public class SceneMonsterConfigControlViewModel : ViewModelBase
    {
        //private SceneMonsterPOJO _backCurrentSceneMonsterPOJO = null;

        private SceneMonsterPOJO _backCurrentSceneMonsterPOJO;

        public SceneMonsterPOJO BackCurrentSceneMonsterPOJO
        {
            get { return _backCurrentSceneMonsterPOJO; }
            set
            {
                if (value == _backCurrentSceneMonsterPOJO)
                {
                    return;
                }
                _backCurrentSceneMonsterPOJO = value;
                RaisePropertyChanged(() => BackCurrentSceneMonsterPOJO);
            }
        }

        private SceneMonsterPOJO _currentSceneMonsterPOJOClone;

        public SceneMonsterPOJO CurrentMonsterPOJOClone
        {
            get { return _currentSceneMonsterPOJOClone; }
            set
            {
                if (value == _currentSceneMonsterPOJOClone)
                {
                    return;
                }
                _currentSceneMonsterPOJOClone = value;
                RaisePropertyChanged(() => CurrentMonsterPOJOClone);
            }
        }

        private bool _modifying;

        public bool Modifying
        {
            get { return _modifying; }
            set
            {
                if (value == _modifying)
                {
                    return;
                }
                _modifying = value;
                RaisePropertyChanged(() => Modifying);
            }
        }

        private ObservableCollection<SceneMonsterPOJO> _currentSceneMonsterPOJOs;

        public ObservableCollection<SceneMonsterPOJO> CurrentSceneMonsterPOJOs
        {
            get { return _currentSceneMonsterPOJOs; }
            set
            {
                if (value == _currentSceneMonsterPOJOs)
                {
                    return;
                }
                _currentSceneMonsterPOJOs = value;
                RaisePropertyChanged(() => CurrentSceneMonsterPOJOs);
            }
        }

        private string _currentMonsterPicturePath;

        public string CurrentMonsterPictruePath
        {
            get { return _currentMonsterPicturePath; }
            set
            {
                if (value == _currentMonsterPicturePath)
                {
                    return;
                }
                _currentMonsterPicturePath = value;
                RaisePropertyChanged(() => CurrentMonsterPictruePath);
            }
        }

        private bool _temp;

        public bool Temp
        {
            get { return _temp; }
            set
            {
                if (value == _temp)
                {
                    return;
                }
                _temp = value;
                RaisePropertyChanged(() => Temp);
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


        public SceneMonsterConfigControlViewModel()
        {
            Messenger.Default.Register<SceneMonsterPOJO>(this, SceneMonsterConfigControlMessageTokens.UpdateCurrentSceneMonsterPOJOFromView, (pojo) =>
            {
                if(pojo != null)
                {
                    CurrentMonsterPOJOClone = pojo.UserMemberwiseClone();
                    Temp = pojo.Temp;
                }
                else
                {
                    CurrentMonsterPOJOClone = null;
                    Temp = false;
                }
                BackCurrentSceneMonsterPOJO = pojo;
            });

            Messenger.Default.Register<bool>(this, SceneMonsterConfigControlMessageTokens.UpdateModifyingFromView, (b) =>
            {
                Modifying = b;
            });

            Messenger.Default.Register<ObservableCollection<SceneMonsterPOJO>>(this, SceneMonsterConfigControlMessageTokens.UpdateCurrentSceneMonsterPOJOsFromView, (pojos) =>
            {
                CurrentSceneMonsterPOJOs = pojos;
            });

            Messenger.Default.Register<string>(this, SceneMonsterConfigControlMessageTokens.UpdateCurrentMonsterPicturePathFromView, (path) =>
            {
                CurrentMonsterPictruePath = path;
            });

            Messenger.Default.Register<int>(this, SceneMonsterConfigControlMessageTokens.UpdateCurrentMapIDFromView, (id) =>
            {
                CurrentMapID = id;
            });

            OKHandler = new RelayCommand(() =>
            {
                Messenger.Default.Send<SceneMonsterConfigArgs>(new SceneMonsterConfigArgs()
                {
                    PreSceneMonsterPOJO = BackCurrentSceneMonsterPOJO,
                    CurrentSceneMonsterPOJO = CurrentMonsterPOJOClone,
                }, SceneMonsterConfigControlMessageTokens.RaiseOKEvent);
            },
            () =>
            {
                ValidateErrorHappendChecker checker = new ValidateErrorHappendChecker()
                {
                    Flag = false,
                };

                Messenger.Default.Send<ValidateErrorHappendChecker>(checker, SceneMonsterConfigControlMessageTokens.NotifyUpdateErrorHapendFromViewModel);

                return checker.Flag;
            });

            CancelHandler = new RelayCommand(() =>
            {
                Messenger.Default.Send<SceneMonsterConfigArgs>(new SceneMonsterConfigArgs()
                {
                    PreSceneMonsterPOJO = BackCurrentSceneMonsterPOJO,
                    CurrentSceneMonsterPOJO = CurrentMonsterPOJOClone,
                }, SceneMonsterConfigControlMessageTokens.RaiseCancelEvent);
            });
        }

        public RelayCommand OKHandler { get; set; }
        public RelayCommand CancelHandler { get; set; }
    }
}
