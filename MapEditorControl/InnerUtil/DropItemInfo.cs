using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.InnerUtil
{
    public class DropItemInfo : ObservableObject
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set
            {
                if (value == _id)
                {
                    return;
                }
                _id = value;
                RaisePropertyChanged(() => ID);
            }
        }

        private int _type;

        public int Type
        {
            get { return _type; }
            set
            {
                if (value == _type)
                {
                    return;
                }
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        private int _number;

        public int Number
        {
            get { return _number; }
            set
            {
                if (value == _number)
                {
                    return;
                }
                _number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        private int _bound;

        public int Bound
        {
            get { return _bound; }
            set
            {
                if (value == _bound)
                {
                    return;
                }
                _bound = value;
                RaisePropertyChanged(() => Bound);
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked)
                {
                    return;
                }
                _isChecked = value;
                RaisePropertyChanged(() => IsChecked);
            }
        }


    }
}
