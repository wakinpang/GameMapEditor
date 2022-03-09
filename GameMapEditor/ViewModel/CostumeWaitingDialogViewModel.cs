using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameMapEditor.InnerUtil;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.ViewModel
{
    public class CostumeWaitingDialogViewModel : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                {
                    return;
                }
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message)
                {
                    return;
                }
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private bool _cancelVaild = true;

        public bool CancelVaild
        {
            get { return _cancelVaild; }
            set
            {
                if (value == _cancelVaild)
                {
                    return;
                }
                _cancelVaild = value;
                RaisePropertyChanged(() => CancelVaild);
            }
        }

        CostumeWaitingDialogHelper.WaitingFunc task = null;

        public CostumeWaitingDialogViewModel()
        {
            WaitingDialogCancle = new RelayCommand(() =>
            {
                CostumeWaitingDialogHelper.Canceled = true;
                CancelVaild = false;
            });

            Messenger.Default.Register<MessageParameterCostumeWaitingDialogParams>(this, MainWindowTokens.UpdateWaitingDialogCallback, (param) =>
            {
                Title = param.Title;
                Message = param.Message;
                CancelVaild = param.CanCancel;
                task = param.Task as CostumeWaitingDialogHelper.WaitingFunc;
            });

            Messenger.Default.Register<string>(this, MainWindowTokens.UpdateWaitingMessageDialogTitle, (title) =>
            {
                Title = title;
            });

            Messenger.Default.Register<string>(this, MainWindowTokens.UpdateWaitingMessageDialogMessage, (message) =>
            {
                Message = message;
            });

        }

        public RelayCommand WaitingDialogCancle { get; set; }

    }
}
