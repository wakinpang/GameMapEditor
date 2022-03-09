using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameMapEditor.InnerUtil
{
    public static class CostumeWaitingDialogHelper
    {
        public delegate void WaitingFunc(ref bool cancel);
        private static BaseMetroDialog _currentWaitingDialog = null;

        private static bool _canceled = false;
        public static bool Canceled {
            get
            {
                return _canceled;
            }
            set
            {
                _canceled = value;
            }
        }

        public static async Task ShowWaitingDialogAsync(MetroWindow window, string title, string message, bool canCancel, WaitingFunc task)
        {

            if(_currentWaitingDialog != null)
            {
                return;
            }

            // Show Dialog
            _currentWaitingDialog = (BaseMetroDialog)window.Resources["waitingDialog"];

            _currentWaitingDialog.Background = new SolidColorBrush(new Color()
            {
                R = 45,
                G = 45,
                B = 48,
                A = 255,
            });

            _currentWaitingDialog.Foreground = new SolidColorBrush(new Color()
            {
                R = 255,
                G = 255,
                B = 255,
                A = 255,
            });

            //await window.ShowMetroDialogAsync(_currentWaitingDialog);
            await DialogManager.ShowMetroDialogAsync(window, _currentWaitingDialog);

            //await _currentWaitingDialog.WaitUntilUnloadedAsync();

            //BaseMetroDialog.

            // Update Param
            Messenger.Default.Send<MessageParameterCostumeWaitingDialogParams>(new MessageParameterCostumeWaitingDialogParams()
            {
                Title = title,
                Message = message,
                CanCancel = canCancel,
                Task = task as object,
            }, MainWindowTokens.UpdateWaitingDialogCallback);

            await Task.Run(() =>
            {
                task(ref _canceled);
                //Messenger.Default.Send<object>(null, CostumeWaitingDialogMesssageTokens.HideCostumeWaitingDialog);
            });
            //await window.HideMetroDialogAsync(_currentWaitingDialog);
            await DialogManager.HideMetroDialogAsync(window, _currentWaitingDialog);
            _currentWaitingDialog = null;
            Canceled = false;
        }

        public async static void HideWaitingDialog(MetroWindow window)
        {
            await window.HideMetroDialogAsync(_currentWaitingDialog);
        }

        public static void SetWaitingDialogTitle(string title)
        {
            Messenger.Default.Send<string>(title, MainWindowTokens.UpdateWaitingMessageDialogTitle);
        }

        public static void SetWaitingDialogMessage(string message)
        {
            Messenger.Default.Send<string>(message, MainWindowTokens.UpdateWaitingMessageDialogMessage);

        }

        public static void CallWaitingDialog(object sender, CostumeWaitingDialogHelper.WaitingFunc task, Action afterTask, string title, string message, bool canCancel)
        {
            CostumeWaitingDialogNotificationMessageAction action = new CostumeWaitingDialogNotificationMessageAction(sender, "waiting", afterTask)
            {
                Params = new MessageParameterCostumeWaitingDialogParams()
                {
                    Title = title,
                    Message = message,
                    CanCancel = false,
                    Task = task,
                }
            };

            Messenger.Default.Send<CostumeWaitingDialogNotificationMessageAction>(action, CostumeWaitingDialogMesssageTokens.ShowCostumeWaitingDialog);
        }
    }
}
