using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_for_UserControl_Test
{

    enum MessageTokens
    {
        MyTextChangedFromView,
        MyTextChangedFromViewModel,
    }

    public class TestViewModel : ViewModelBase
    {
        private String _myText = "123312321";

        public String ThisText
        {
            get { return _myText; }
            set
            {
                if(_myText == value)
                {
                    return;
                }
                _myText = value;
                Messenger.Default.Send<String>(value, MessageTokens.MyTextChangedFromViewModel);
                RaisePropertyChanged(() => ThisText);
            }
        }

        public TestViewModel()
        {
            Messenger.Default.Register<String>(this, MessageTokens.MyTextChangedFromView, (msg) =>
            {
                ThisText = msg;
            });

        }
    }
}
