using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_TEST.Model
{
    public class TestModel : ObservableObject
    {
        private String _myText = "";

        public String MyText
        {
            get { return _myText; }
            set
            {
                _myText = value;
                RaisePropertyChanged(() => MyText);
            }
        }
    }
}
