using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;

namespace GameMapEditor.ViewModel
{
    public class MonsterConfigViewModel : ViewModelBase
    {
        private LibraryControlModel _libSource = LibraryControlModel.Instance();
        public LibraryControlModel LibSource {
            get { return _libSource; }
            set {
                if (value == _libSource) {
                    return;
                }
                _libSource = value;
                RaisePropertyChanged(() => LibSource);
            }
        }

        public MonsterConfigViewModel() {
            NewMonsterOKHandle = new RelayCommand<MonsterConfigArgs>((args) => 
            {
                OkTypes type = LibSource.Types;
                if (type == OkTypes.Create) {
                    MapEditorAndNavigationModel.Instance().CurrentMonster = args.CurrentMonster;
                    args.CurrentMonster.POJOStatus = POJOStatus.Inserted;
                    LibSource.MonsterCollection.Add(args.CurrentMonster);
                }

                if (type == OkTypes.Change) {
                    updateMonster(MapEditorAndNavigationModel.Instance().CurrentMonster, args.CurrentMonster);
                }

                Messenger.Default.Send<object>(null, MonsterConfigControlMessageTokens.HideDailogFromViewModel);
            });

            NewMonsterCancelHandle = new RelayCommand<MonsterConfigArgs>((args) =>
            {
                OkTypes type = LibSource.Types;
                if (type == OkTypes.Create) {
                    MapEditorAndNavigationModel.Instance().CurrentMonster = MapEditorAndNavigationModel.Instance().BackCurrentMonster;
                }

                if (type == OkTypes.Change) {
                    MapEditorAndNavigationModel.Instance().CurrentMonster = args.BackCurrentMonster;
                }

                Messenger.Default.Send<object>(null, MonsterConfigControlMessageTokens.HideDailogFromViewModel);
            });

        }

        void updateMonster(MonsterSection now, MonsterSection source) {
            now.MonsterID = source.MonsterID;
            now.Name = source.Name;
            now.Level = source.Level;
            now.Exp = source.Exp;
            now.Hp = source.Hp;
            now.Mp = source.Mp;
            now.MinAtk = source.MinAtk;
            now.MaxAtk = source.MaxAtk;
            now.MAtk = source.MAtk;
            now.Def = source.Def;
            now.MDef = source.MDef;
            now.AtkRate = source.AtkRate;
            now.MissRate = source.MissRate;
            now.MAtkRate = source.MAtkRate;
            now.MMissRate = source.MMissRate;
            now.AtkCdtm = source.AtkCdtm;
            now.AtkRange = source.AtkRange;
            now.ExAtk = source.ExAtk;
            now.ExDmg = source.ExDmg;
            now.Move = source.Move;
            now.BeatBack = source.BeatBack;
            now.ActionInterval = source.ActionInterval;
            now.WalkRange = source.WalkRange;
            now.ChaseRange = source.ChaseRange;
            now.HpResume = source.HpResume;
        }

        public RelayCommand<MonsterConfigArgs> NewMonsterOKHandle { get; set; }
        public RelayCommand<MonsterConfigArgs> NewMonsterCancelHandle { get; set; }
    }
}
