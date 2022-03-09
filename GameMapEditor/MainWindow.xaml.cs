using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using MahApps.Metro.Controls.Dialogs;
using GameMapEditor.InnerUtil;
using System.Threading;

namespace GameMapEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        BaseMetroDialog _currentDialog = null;
        BaseMetroDialog _currentBaseRewardDialog = null;

        public MainWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, MenuMessageTokens.ShowNewProjectWindowFromViewModel, async (title) =>
            {
                if (_currentDialog == null) {
                    _currentDialog = (BaseMetroDialog)this.Resources["newProjectWindow"];

                    _currentDialog.Title = title;

                    _currentDialog.Background = new SolidColorBrush(new Color()
                    {
                        R = 45,
                        G = 45,
                        B = 48,
                        A = 255,
                    });

                    _currentDialog.Foreground = new SolidColorBrush(new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    });
                    await DialogManager.ShowMetroDialogAsync(this, _currentDialog);
                }

            });

            Messenger.Default.Register<object>(this, MenuMessageTokens.HideNewProjectWindowFromViewModel, async (dummy) =>
            {
                await DialogManager.HideMetroDialogAsync(this, _currentDialog);
                _currentDialog = null;
            });

            Messenger.Default.Register<string>(this, LibraryControlMessageTokens.ShowDialogFromViewModel, async (title) =>
            {
                if (_currentDialog == null)
                {
                    _currentDialog = (BaseMetroDialog)this.Resources["newMonsterWindow"];
                    _currentDialog.Title = title;

                    _currentDialog.Background = new SolidColorBrush(new Color()
                    {
                        R = 45,
                        G = 45,
                        B = 48,
                        A = 255,
                    });

                    _currentDialog.Foreground = new SolidColorBrush(new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    });
                }

                await this.ShowMetroDialogAsync(_currentDialog);
            });

            Messenger.Default.Register<object>(this, MonsterConfigControlMessageTokens.HideDailogFromViewModel, async (dummy) =>
            {
                await this.HideMetroDialogAsync(_currentDialog);
                _currentDialog = null;
            });

            Messenger.Default.Register<MessageParameterDialogTitleAndMessage>(this, MainWindowTokens.ShowMessageDialog, (msg) =>
            {
                CostumeMessageDialogHelper.ShowCostumeMessageBoxAsync(this,
                    msg.Title,
                    msg.Message,
                    CostumeDialogButtonType.OK,
                    () =>
                    {
                    },
                    () =>
                    {
                        throw new NotImplementedException();
                    });
            });

            Messenger.Default.Register<CostumeWaitingDialogNotificationMessageAction>(this, CostumeWaitingDialogMesssageTokens.ShowCostumeWaitingDialog, async (action) =>
            {
                var param = action.Params;
                await CostumeWaitingDialogHelper.ShowWaitingDialogAsync(this,
                    param.Title,
                    param.Message,
                    param.CanCancel,
                    param.Task as CostumeWaitingDialogHelper.WaitingFunc);
                action.Execute();
            });

            Messenger.Default.Register<object>(this, LibraryControlMessageTokens.ShowDialogFromViewModel, (dummy) => {

            });

            Messenger.Default.Register<string>(this, MainWindowTokens.UpdateMainWindowTitle, (title) =>
            {
                this.Title = title;
            });

            Messenger.Default.Register<BaseMapInfo>(this, MainWindowTokens.GetDropCursorPosition, (info) =>
            {
                //Point p = mapEditor.GetCursorPosition();
                if (info.GetType() == typeof(DropMapMonsterInfo))
                {
                    var e = info as DropMapMonsterInfo;
                    e.Position = mapEditor.GetDropCursorPosition(e.Args);
                }

                if (info.GetType() == typeof(DropMapNpcInfo))
                {
                    var e = info as DropMapNpcInfo;
                    e.Position = mapEditor.GetDropCursorPosition(e.Args);
                }

                Messenger.Default.Send<BaseMapInfo>(info, MainWindowTokens.SendDropCursorPositionAndAddMapObject);
            });

            Messenger.Default.Register<NotificationMessageAction>(this, CostumeMessageDialogMessageTokens.HideCostumeMessageDialog, async (action) =>
            {
                //if (CostumeMessageDialogHelper.CurrentMessageDialogIsNull())
                //{
                //    return;
                //}
                await CostumeMessageDialogHelper.HideCostumeMessageBoxAsync(this);
                action.Execute();
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.ShowDropInfoDialog, async (dummy) =>
            {
                if (_currentDialog == null)
                {
                    _currentDialog = (BaseMetroDialog)this.Resources["dropItemInfoDialog"];

                    _currentDialog.Background = new SolidColorBrush(new Color()
                    {
                        R = 45,
                        G = 45,
                        B = 48,
                        A = 255,
                    });

                    _currentDialog.Foreground = new SolidColorBrush(new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    });

                    await DialogManager.ShowMetroDialogAsync(this, _currentDialog);
                }
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.HideDropInfoDialog, async (dummy) =>
            {
                await DialogManager.HideMetroDialogAsync(this, _currentDialog);
                _currentDialog = null;
            });

            Messenger.Default.Register<string>(this, SceneMonsterConfigControlMessageTokens.ShowSceneMonsterPOJOConfigDialog, async (title) =>
            {
                if (_currentDialog == null)
                {
                    _currentDialog = (BaseMetroDialog)this.Resources["sceneMonsterConfigDialog"];

                    _currentDialog.Title = title;

                    _currentDialog.Background = new SolidColorBrush(new Color()
                    {
                        R = 45,
                        G = 45,
                        B = 48,
                        A = 255,
                    });

                    _currentDialog.Foreground = new SolidColorBrush(new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    });

                    await DialogManager.ShowMetroDialogAsync(this, _currentDialog);
                }

            });

            Messenger.Default.Register<object>(this, SceneMonsterConfigControlMessageTokens.HideSceneMonsterPOJOConfigDialog, async (dummy) =>
            {
                await DialogManager.HideMetroDialogAsync(this, _currentDialog);
                _currentDialog = null;
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.ShowBaseRewardInfoDialog, async (dummy) =>
            {
                if (_currentBaseRewardDialog == null)
                {
                    _currentBaseRewardDialog = (BaseMetroDialog)this.Resources["baseRewardInfoDialog"];

                    _currentBaseRewardDialog.Background = new SolidColorBrush(new Color()
                    {
                        R = 45,
                        G = 45,
                        B = 48,
                        A = 255,
                    });

                    _currentBaseRewardDialog.Foreground = new SolidColorBrush(new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    });
                    await DialogManager.ShowMetroDialogAsync(this, _currentBaseRewardDialog);
                }

            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.HideBaseRewardInfoDialog, async (dummy) =>
            {
                await DialogManager.HideMetroDialogAsync(this, _currentBaseRewardDialog);
                _currentBaseRewardDialog = null;
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.ShowMissionRewardInfoDialog, async (dummy) =>
            {
                if (_currentBaseRewardDialog == null)
                {
                    _currentBaseRewardDialog = (BaseMetroDialog)this.Resources["missionRewardInfoDialog"];

                    _currentBaseRewardDialog.Background = new SolidColorBrush(new Color()
                    {
                        R = 45,
                        G = 45,
                        B = 48,
                        A = 255,
                    });

                    _currentBaseRewardDialog.Foreground = new SolidColorBrush(new Color()
                    {
                        R = 255,
                        G = 255,
                        B = 255,
                        A = 255,
                    });
                    await DialogManager.ShowMetroDialogAsync(this, _currentBaseRewardDialog);
                }

            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.HideMissionRewardInfoDialog, async (dummy) =>
            {
                await DialogManager.HideMetroDialogAsync(this, _currentBaseRewardDialog);
                _currentBaseRewardDialog = null;
            });
        }
    }
}
