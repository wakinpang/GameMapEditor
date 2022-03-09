using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Collections.ObjectModel;



namespace MapEditorControl.ViewModel
{
    public class MonsterConfigControlViewModel : ViewModelBase
    {
        private MonsterSection _backCurrentMonster;
        public MonsterSection BackCurrentMonster {
            get { return _backCurrentMonster; }
            set {
                if (value == _backCurrentMonster) {
                    return;
                }
                _backCurrentMonster = value;
                RaisePropertyChanged(() => BackCurrentMonster);
            }
        }

        private MonsterSection _currentMonster = new MonsterSection();
        public MonsterSection CurrentMonster {
            get { return _currentMonster; }
            set {
                if (value == _currentMonster) {
                    return;
                }
                _currentMonster = value;
                RaisePropertyChanged(() => CurrentMonster);
            }
        }

        private ObservableCollection<MonsterSection> _monsterCollection;
        public ObservableCollection<MonsterSection> MonsterCollection {
            get { return _monsterCollection; }
            set {
                if (value == _monsterCollection) {
                    return;
                }

                _monsterCollection = value;
                RaisePropertyChanged(() => MonsterCollection);
            }
        }

        public MonsterConfigControlViewModel() {
            Messenger.Default.Register<MonsterSection>(this, MonsterConfigControlMessageTokens.CurrentMonsterChangedFriomModel, (section) =>
            {
                if (section != null)
                {
                    CurrentMonster = section.UserMemberwiseClone();
                }
                else {
                    CurrentMonster = null;
                }

                BackCurrentMonster = section;
            });

            Messenger.Default.Register<ObservableCollection<MonsterSection>>(this, MonsterConfigControlMessageTokens.MonsterCollectionChangedFromModel, (collection) =>
            {
                MonsterCollection = collection;
            });

            OK = new RelayCommand(() =>
            {
                Messenger.Default.Send<MonsterSection>(CurrentMonster, MonsterConfigControlMessageTokens.CurrentMonsterChangedFromView);
                Messenger.Default.Send<MonsterConfigArgs>(new MonsterConfigArgs() {
                    BackCurrentMonster = BackCurrentMonster,
                    CurrentMonster = CurrentMonster,
                }, MonsterConfigControlMessageTokens.MonsterOkEventFromView);
            },
            ()=>
            {
                ValidateErrorHappendChecker checker = new ValidateErrorHappendChecker()
                {
                    Flag = false,
                };

                Messenger.Default.Send<ValidateErrorHappendChecker>(checker, MonsterConfigControlMessageTokens.NotifyUpdateErrorHapendFromViewModel);

                return checker.Flag;

            });

            Cancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<MonsterConfigArgs>(new MonsterConfigArgs() {
                    BackCurrentMonster = BackCurrentMonster,
                    CurrentMonster = CurrentMonster,
                }, MonsterConfigControlMessageTokens.HideDailogFromView);
            });
        }

        public RelayCommand OK { get; set; }
        public RelayCommand Cancel { get; set; }
    }
}
