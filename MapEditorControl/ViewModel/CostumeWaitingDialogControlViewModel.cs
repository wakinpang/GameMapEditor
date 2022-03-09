using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.ViewModel
{
    public class CostumeWaitingDialogControlViewModel : ViewModelBase
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

        private bool _cancelVaild;

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

        public CostumeWaitingDialogControlViewModel()
        {
            Messenger.Default.Register<string>(this, CostumeWaitingDialogMesssageTokens.UpdateTitleFromView, (title) =>
            {
                Title = title;
            });

            Messenger.Default.Register<string>(this, CostumeWaitingDialogMesssageTokens.UpdateMessageFromView, (msg) =>
            {
                Message = msg;
            });

            Messenger.Default.Register<bool>(this, CostumeWaitingDialogMesssageTokens.UpdateCancelVaildFromView, (vaild) =>
            {
                CancelVaild = vaild;
            });

            CancleHandle = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, CostumeWaitingDialogMesssageTokens.CancelEventFromViewModel);
            },
            () =>
            {
                return CancelVaild;
            });
        }

        public RelayCommand CancleHandle { get; set; }
    }
}
