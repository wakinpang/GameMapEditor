using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using GameMapEditor.InnerUtil;

namespace GameMapEditor.ViewModel
{
    public class CostumeMessageDialogViewModel : ViewModelBase
    {

        private string _title = "";

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

        Action _okFunc = null;
        Action _cancleFunc = null;

        public RelayCommand MessageBoxOK { get; set; }
        public RelayCommand MessageBoxCancle { get; set; }

        public CostumeMessageDialogViewModel()
        {
            Messenger.Default.Register<MessageParameterCostumeMessageDialogParams>(this, MainWindowTokens.UpdateCostumeDialogCallback, (param) =>
            {
                Title = param.Title;
                Message = param.Message;
                ButtonType = param.ButtonType;
                _okFunc = param.OKFunc;
                _cancleFunc = param.CancleFunc;
            });

            MessageBoxOK = new RelayCommand(()=>
            {
                if (!CostumeMessageDialogHelper.MsgDialogShowing)
                {
                    return;
                }
                Messenger.Default.Send<NotificationMessageAction>(
                    new NotificationMessageAction(
                        this, 
                        "message", 
                        ()=> 
                        {
                            _okFunc.Invoke();
                        }), 
                    CostumeMessageDialogMessageTokens.HideCostumeMessageDialog);
            });

            MessageBoxCancle = new RelayCommand(() =>
            {
                //Messenger.Default.Send<object>(null, CostumeMessageDialogMessageTokens.HideCostumeMessageDialog);
                //_cancleFunc.Invoke();

                if (!CostumeMessageDialogHelper.MsgDialogShowing)
                {
                    return;
                }

                Messenger.Default.Send<NotificationMessageAction>(
                    new NotificationMessageAction(
                        this, 
                        "message", 
                        ()=> 
                        {
                            _cancleFunc.Invoke();
                        }), 
                    CostumeMessageDialogMessageTokens.HideCostumeMessageDialog);

            });

        }
    }
}
