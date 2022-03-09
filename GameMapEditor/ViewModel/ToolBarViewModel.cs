using DatabaseOperate;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GameMapEditor.InnerUtil;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.ViewModel
{
    public class ToolBarViewModel : ViewModelBase
    {
        private ToolBarModel _toolBarModel;

        public ToolBarModel ToolBarStatus
        {
            get { return _toolBarModel; }
            set
            {
                if (value == _toolBarModel)
                {
                    return;
                }
                _toolBarModel = value;
                RaisePropertyChanged(() => ToolBarStatus);
            }
        }

        public RelayCommand SyncHandle { get; set; }

        public ToolBarViewModel()
        {
            _toolBarModel = ToolBarModel.INSTANCE;

            SyncHandle = new RelayCommand(() => {

                bool result = false;

                CostumeWaitingDialogHelper.CallWaitingDialog(this,
                    (ref bool canceled) =>
                    {
                        result = DatabaseUtil.TryConn(ProjectConfigModel.CurrentInstance().DatabaseIP,
                                  ProjectConfigModel.CurrentInstance().DatabaseName,
                                  ProjectConfigModel.CurrentInstance().DatabaseUserName,
                                  ProjectConfigModel.CurrentInstance().DatabasePassword,
                                  ProjectConfigModel.CurrentInstance().DatabasePort);
                    },
                    () =>
                    {
                        // 测试数据库连接
                        if (!result)
                        {
                            Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                            {
                                Title = "Failed",
                                Message = "数据库连接测试失败，请检查设置！",
                            }, MainWindowTokens.ShowMessageDialog);
                        }
                        else
                        {
                            // 同步地图数据
                            CostumeWaitingDialogHelper.CallWaitingDialog(this,
                                (ref bool canceld) =>
                                {
                                    Messenger.Default.Send<object>(null, ToolBarControlMessageTokens.SyncMapData);

                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        Messenger.Default.Send<string>("正在同步基础怪物数据，请稍等……", MainWindowTokens.UpdateWaitingMessageDialogMessage);
                                    });

                                    Messenger.Default.Send<object>(null, ToolBarControlMessageTokens.SyncMonsterData);

                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        Messenger.Default.Send<string>("正在同步场景怪物数据，请稍等……", MainWindowTokens.UpdateWaitingMessageDialogMessage);
                                    });

                                    Messenger.Default.Send<object>(null, ToolBarControlMessageTokens.SyncSceneMonsterData);

                                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                    {
                                        Messenger.Default.Send<string>("正在同NPC数据，请稍等……", MainWindowTokens.UpdateWaitingMessageDialogMessage);
                                    });

                                    Messenger.Default.Send<object>(null, ToolBarControlMessageTokens.SyncNpcData);
                                },
                                () =>
                                {
                                    Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                                    {
                                        Title = "Success",
                                        Message = "同步完毕！",
                                    }, MainWindowTokens.ShowMessageDialog);
                                },
                                "请等待",
                                "正在同步地图数据，请稍等……",
                                false);
                        }
                    },
                    "请等待",
                    "数据库连接测试中，请稍等……",
                    false
                    );
            });

        }
    }
}
