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
using System.Windows.Controls;

namespace MapEditorControl.ViewModel
{
    public class DropItemInfoListControlViewModel : ViewModelBase
    {
        private ObservableCollection<DropItemInfo> _infoSource = new ObservableCollection<DropItemInfo>();

        public ObservableCollection<DropItemInfo> InfoSource
        {
            get { return _infoSource; }
            set
            {
                if (value == _infoSource)
                {
                    return;
                }
                _infoSource = value;
                RaisePropertyChanged(() => InfoSource);
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

        private bool _errorHappend;

        public bool ErrorHappend
        {
            get { return _errorHappend; }
            set
            {
                if (value == _errorHappend)
                {
                    return;
                }
                _errorHappend = value;
                RaisePropertyChanged(() => ErrorHappend);
            }
        }


        public DropItemInfoListControlViewModel()
        {
            Messenger.Default.Register<string>(this, DropItemInfoListControlMessageTokens.UpdateDropItemInfoStringFromView, (str) =>
            {

                InfoSource.Clear();

                char[] spliter = new char[1] { ';' };
                char[] spliter2 = new char[1] { ',' };
                var itemInfo = str.Trim().Split(spliter, StringSplitOptions.RemoveEmptyEntries);

                foreach (var info in itemInfo)
                {
                    var infoArr = info.Split(spliter2, StringSplitOptions.RemoveEmptyEntries);
                    InfoSource.Add(new DropItemInfo()
                    {
                        ID = int.Parse(infoArr[1]),
                        Type = int.Parse(infoArr[0]),
                        Number = int.Parse(infoArr[2]),
                        Bound = int.Parse(infoArr[3]),
                    });
                }

            });

            Messenger.Default.Register<ObservableCollection<ItemPOJO>>(this, DropItemInfoListControlMessageTokens.UpdateCurrentItemsInfoFromView, (info) =>
            {
                CurrentItemsInfo = info;
            });

            SelectAllHandler = new RelayCommand(() =>
            {
                foreach (var item in InfoSource)
                {
                    item.IsChecked = !item.IsChecked;
                }
            });

            DeleteHandler = new RelayCommand(() =>
            {
                var deleted = InfoSource.ToList().FindAll((info) => info.IsChecked);
                foreach (var item in deleted)
                {
                    InfoSource.Remove(item);
                }
            },
            () =>
            {
                var deleted = InfoSource.ToList().FindAll((info) => info.IsChecked);
                return deleted.Count != 0;
            });

            InsertHandler = new RelayCommand(() =>
            {
                InfoSource.Add(new DropItemInfo()
                {
                    ID = CurrentItemsInfo[0].ItemID,
                    Type = 1,
                    Number = 1,
                    Bound = 0,
                    IsChecked = false,
                });
            },
            () => CurrentItemsInfo != null && CurrentItemsInfo.Count != 0);

            ConfirmHandler = new RelayCommand(() =>
            {
                StringBuilder resultString = new StringBuilder("");
                bool flag = false;

                foreach(var dropItem in InfoSource)
                {
                    // first
                    if(!flag)
                    {
                        resultString.Append(dropItem.Type.ToString()).Append(",").Append(dropItem.ID).Append(",").Append(dropItem.Number).Append(",").Append(dropItem.Bound);
                        flag = true;
                    }
                    else
                    {
                        resultString.Append(";").Append(dropItem.Type.ToString()).Append(",").Append(dropItem.ID).Append(",").Append(dropItem.Number).Append(",").Append(dropItem.Bound);
                    }
                }

                Messenger.Default.Send<string>(resultString.ToString(), DropItemInfoListControlMessageTokens.UpdateDropItemInfoStringFromViewModel);

            },
            ()=>
            {
                var checker = new ValidateErrorHappendChecker()
                {
                    Flag = true,
                };
                Messenger.Default.Send<ValidateErrorHappendChecker>(checker, DropItemInfoListControlMessageTokens.NotifyUpdateErrorHapendFromViewModel);
                
                return checker.Flag;
            });

            CancelHandler = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, DropItemInfoListControlMessageTokens.CancelEventFromViewModel);
            });
        }
        
        public RelayCommand SelectAllHandler { get; set; }
        public RelayCommand InsertHandler { get; set; }
        public RelayCommand DeleteHandler { get; set; }
        public RelayCommand ConfirmHandler { get; set; }
        public RelayCommand CancelHandler { get; set; }

    }
}
