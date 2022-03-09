using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.InnerUtil
{
    public class CostumeWaitingDialogNotificationMessageAction : NotificationMessageAction
    {
        public MessageParameterCostumeWaitingDialogParams Params { get; set; }

        public CostumeWaitingDialogNotificationMessageAction(object sender, string notification, Action callback) : base(sender, notification, callback)
        {

        }
    }
}
