using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.ViewModel
{
    public class CostumeMessageDialogControlViewModel : ViewModelBase
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

        private Visibility _okHide;

        public Visibility OKHide
        {
            get { return _okHide; }
            set
            {
                if (value == _okHide)
                {
                    return;
                }
                _okHide = value;
                RaisePropertyChanged(() => OKHide);
            }
        }

        private Visibility _cancleHide;

        public Visibility CancleHide
        {
            get { return _cancleHide; }
            set
            {
                if (value == _cancleHide)
                {
                    return;
                }
                _cancleHide = value;
                RaisePropertyChanged(() => CancleHide);
            }
        }

        private CostumeDialogButtonType _buttonType = CostumeDialogButtonType.Dummy;

        public CostumeDialogButtonType ButtonType
        {
            get { return _buttonType; }
            set
            {
                if (value == _buttonType)
                {
                    return;
                }
                _buttonType = value;
                RaisePropertyChanged(() => ButtonType);
            }
        }

        public RelayCommand OKHandle { get; set; }
        public RelayCommand CancleHandle { get; set; }

        public CostumeMessageDialogControlViewModel()
        {
            Messenger.Default.Register<string>(this, CostumeMessageDialogMessageTokens.UpdateTitleFromView, (title) => {
                Title = title;
            });

            Messenger.Default.Register<string>(this, CostumeMessageDialogMessageTokens.UpdateMessageFromView, (msg) =>
            {
                Message = msg;
            });

            Messenger.Default.Register<CostumeDialogButtonType>(this, CostumeMessageDialogMessageTokens.UpdateButtonTypeFromView, (type) =>
            {
                ButtonType = type;
                switch (type)
                {
                    case CostumeDialogButtonType.OK:
                        OKHide = Visibility.Visible;
                        CancleHide = Visibility.Collapsed;
                        break;
                    case CostumeDialogButtonType.Cancle:
                        OKHide = Visibility.Collapsed;
                        CancleHide = Visibility.Visible;
                        break;
                    case CostumeDialogButtonType.OKAndCancle:
                        OKHide = Visibility.Visible;
                        CancleHide = Visibility.Visible;
                        break;
                }
            });

            OKHandle = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, CostumeMessageDialogMessageTokens.OKEventFromViewModel);
            });

            CancleHandle = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, CostumeMessageDialogMessageTokens.CancelEventFromViewModel);
            });

        }

    }
}
