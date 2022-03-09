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
    public class MissionRewardInfoControlViewModel : ViewModelBase
    {
        private string _missionID;

        public string MissionID
        {
            get { return _missionID; }
            set
            {
                if (value == _missionID)
                {
                    return;
                }
                _missionID = value;
                RaisePropertyChanged(() => MissionID);
            }
        }

        private string _itemID;

        public string ItemID
        {
            get { return _itemID; }
            set
            {
                if (value == _itemID)
                {
                    return;
                }
                _itemID = value;
                RaisePropertyChanged(() => ItemID);
            }
        }

        private string _num;

        public string Num
        {
            get { return _num; }
            set
            {
                if (value == _num)
                {
                    return;
                }
                _num = value;
                RaisePropertyChanged(() => Num);
            }
        }

        private int _bound = 0;

        public int Bound
        {
            get { return _bound; }
            set
            {
                if (value == _bound)
                {
                    return;
                }
                _bound = value;
                RaisePropertyChanged(() => Bound);
            }
        }

        private string _dropProbability;

        public string DropProbability
        {
            get { return _dropProbability; }
            set
            {
                if (value == _dropProbability)
                {
                    return;
                }
                _dropProbability = value;
                RaisePropertyChanged(() => DropProbability);
            }
        }

        private ObservableCollection<ItemPOJO> _currentItemPOJOs;

        public ObservableCollection<ItemPOJO> CurrentItemPOJOs
        {
            get { return _currentItemPOJOs; }
            set
            {
                if (value == _currentItemPOJOs)
                {
                    return;
                }
                _currentItemPOJOs = value;
                RaisePropertyChanged(() => CurrentItemPOJOs);
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

        private string _backMissionID = "";
        private string _backItemID = "";
        private string _backNum = "";
        private int _backBound = 0;
        private string _backDropProbability = "";

        public MissionRewardInfoControlViewModel()
        {
            Messenger.Default.Register<string>(this, MissionRewardInfoControlMessageTokens.UpdateMissionStringFromView, (str) =>
            {

                if(str == null || str == "")
                {
                    MissionID = "";
                    ItemID = "";
                    Num = "";
                    Bound = 0;
                    DropProbability = "";
                }
                else
                {
                    char[] spliter = new char[] { ',' };
                    var arr = str.Trim().Split(spliter, StringSplitOptions.RemoveEmptyEntries);

                    MissionID = arr[0];
                    ItemID = arr[1];
                    Num = arr[2];
                    Bound = int.Parse(arr[3]);
                    DropProbability = arr[4];
                }

                _backMissionID = MissionID;
                _backItemID = ItemID;
                _backNum = Num;
                _backBound = Bound;
                _backDropProbability = DropProbability;

            });

            Messenger.Default.Register<ObservableCollection<ItemPOJO>>(this, MissionRewardInfoControlMessageTokens.UpdateCurrentItemPOJOsFromView, (pojos) =>
            {
                CurrentItemPOJOs = pojos;
            });

            Messenger.Default.Register<ObservableCollection<MissionPOJO>>(this, MissionRewardInfoControlMessageTokens.UpdateCurrentMissionPOJOsFromView, (pojos) =>
            {
                CurrentMissionPOJOs = pojos;
            });

            Confirm = new RelayCommand(() =>
            {
                if (MissionID == "" && ItemID == "" && Num == "" && DropProbability == "")
                {
                    Messenger.Default.Send<string>("", MissionRewardInfoControlMessageTokens.UpdateMissionStringFromViewModel);
                }
                else
                {
                    var builder = new StringBuilder();
                    builder.Append(MissionID).Append(",").Append(ItemID).Append(",").Append(Num).Append(",").Append(Bound.ToString()).Append(",").Append(DropProbability);
                    Messenger.Default.Send<string>(builder.ToString(), MissionRewardInfoControlMessageTokens.UpdateMissionStringFromViewModel);
                }

                _backMissionID = MissionID;
                _backItemID = ItemID;
                _backNum = Num;
                _backBound = Bound;
                _backDropProbability = DropProbability;
            },
            ()=>
            {
                if(!(MissionID == "" && ItemID == "" && Num == "" && DropProbability == "")
                && !(MissionID != "" && ItemID != "" && Num != "" && DropProbability != "")) {
                    return false;
                }

                ValidateErrorHappendChecker checker = new ValidateErrorHappendChecker()
                {
                    Flag = false,
                };

                Messenger.Default.Send<ValidateErrorHappendChecker>(checker, MissionRewardInfoControlMessageTokens.NotifyUpdateErrorHapendFromViewModel);

                return checker.Flag;
            });

            Cancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, MissionRewardInfoControlMessageTokens.RaiseCancelEventFromViewModel);

                MissionID = _backMissionID;
                ItemID = _backItemID;
                Num = _backNum;
                Bound = _backBound;
                DropProbability = _backDropProbability;
            });
        }

        public RelayCommand Confirm { get; set; }
        public RelayCommand Cancel { get; set; }

    }
}
