using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GameMapEditor.InnerUtil
{
    public static class CostumeMessageDialogHelper
    {

        private static BaseMetroDialog _currentMsgDialog = null;
        private static bool _msgDialogShowing = false;
        public static bool MsgDialogShowing { get { return _msgDialogShowing; } }

        public static async void ShowCostumeMessageBoxAsync(MetroWindow window, string title, string message, CostumeDialogButtonType buttonType, Action success, Action failed)
        {

            if(_currentMsgDialog != null)
            {
                return;
            }

            _currentMsgDialog = (BaseMetroDialog)window.Resources["messageBox"];
            _currentMsgDialog.Background = new SolidColorBrush(new Color()
            {
                R = 45,
                G = 45,
                B = 48,
                A = 255,
            });

            _currentMsgDialog.Foreground = new SolidColorBrush(new Color()
            {
                R = 255,
                G = 255,
                B = 255,
                A = 255,
            });

            Messenger.Default.Send<MessageParameterCostumeMessageDialogParams>(new MessageParameterCostumeMessageDialogParams()
            {
                Title = title,
                Message = message,
                ButtonType = buttonType,
                OKFunc = success,
                CancleFunc = failed,
            }, MainWindowTokens.UpdateCostumeDialogCallback);

            await window.ShowMetroDialogAsync(_currentMsgDialog);
            _msgDialogShowing = true;
        }

        public static async Task HideCostumeMessageBoxAsync(MetroWindow window)
        {
            await window.HideMetroDialogAsync(_currentMsgDialog);
            _msgDialogShowing = false;
            _currentMsgDialog = null;
        }

        //public static bool CurrentMessageDialogIsNull()
        //{
        //    return _currentMsgDialog == null;
        //}
    }
}
