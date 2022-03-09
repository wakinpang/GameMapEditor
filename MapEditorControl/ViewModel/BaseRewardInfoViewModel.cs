using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.ViewModel
{
    public class BaseRewardInfoViewModel : ViewModelBase, IDataErrorInfo
    {
        private string _exp;

        public string Exp
        {
            get { return _exp; }
            set
            {
                if (value == _exp)
                {
                    return;
                }
                _exp = value;
                RaisePropertyChanged(() => Exp);
            }
        }

        private string _gold;

        public string Gold
        {
            get { return _gold; }
            set
            {
                if (value == _gold)
                {
                    return;
                }
                _gold = value;
                RaisePropertyChanged(() => Gold);
            }
        }

        private string _power;

        public string Power
        {
            get { return _power; }
            set
            {
                if (value == _power)
                {
                    return;
                }
                _power = value;
                RaisePropertyChanged(() => Power);
            }
        }

        public string Error { get { return ""; } }

        public string this[string columnName]
        {
            get
            {
                int value = 0;
                switch (columnName)
                {
                    case "Exp":
                        if (Exp != "" && !int.TryParse(Exp, out value))
                        {
                            return "经验值必须是一个整数值。";
                        }
                        break;
                    case "Gold":
                        if (Gold != "" && !int.TryParse(Gold, out value))
                        {
                            return "金钱必须是一个整数值。";
                        }
                        break;
                    case "Power":
                        if (Power != "" && !int.TryParse(Power, out value))
                        {
                            return "能量值必须是一个整数值。";
                        }
                        break;
                }

                return "";
            }
        }

        string _backGold = "";
        string _backPower = "";
        string _backExp = "";

        public BaseRewardInfoViewModel()
        {
            Messenger.Default.Register<string>(this, BaseRewardInfoControlMessageTokens.UpdateRewardStringFromView, (str) =>
            {
                //RewardString = str;
                if (str == "" || str == null)
                {
                    Gold = "";
                    Power = "";
                    Exp = "";
                }
                else
                {
                    char[] spliter = new char[] { ',' };
                    var resultArr = str.Trim().Split(spliter, StringSplitOptions.RemoveEmptyEntries);

                    Exp = resultArr[0];
                    Gold = resultArr[1];
                    Power = resultArr[2];
                }
                _backExp = Exp;
                _backGold = Gold;
                _backPower = Power;
            });

            Confirm = new RelayCommand(() =>
            {
                if(Gold == "" && Power == "" && Exp == "")
                {
                    var resultString = new StringBuilder();
                    resultString.Append(Exp).Append(",").Append(Gold).Append(",").Append(Power);

                    Messenger.Default.Send<string>("", BaseRewardInfoControlMessageTokens.UpdateRewardStringFromViewModel);
                }
                else
                {
                    var resultString = new StringBuilder();
                    resultString.Append(Exp).Append(",").Append(Gold).Append(",").Append(Power);

                    Messenger.Default.Send<string>(resultString.ToString(), BaseRewardInfoControlMessageTokens.UpdateRewardStringFromViewModel);
                }

                _backExp = Exp;
                _backGold = Gold;
                _backPower = Power;
            },
            ()=>
            {
                int result = 0;
                if(!(Power != "" && Exp != "" && Gold != "")
                    && !(Power == "" && Exp != "" && Gold != ""))
                {
                    return false;
                }

                if(!int.TryParse(Exp, out result))
                {
                    return false;
                }

                if (!int.TryParse(Gold, out result))
                {
                    return false;
                }

                if (!int.TryParse(Power, out result))
                {
                    return false;
                }

                return true;
            });

            Cancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, BaseRewardInfoControlMessageTokens.CancelEventFromViewModel);

                Gold = _backGold;
                Power = _backPower;
                Exp = _backExp;
            });
        }

        public RelayCommand Confirm { get; set; }
        public RelayCommand Cancel { get; set; }
    }
}
