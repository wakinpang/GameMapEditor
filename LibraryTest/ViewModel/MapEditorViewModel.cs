using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LibraryTest.Model;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.ViewModel
{
    public class MapEditorViewModel : ViewModelBase
    {
        private MapEditorAndNavigationModel _mapEditorStatus;

        public MapEditorAndNavigationModel MapEditorStatus
        {
            get { return _mapEditorStatus; }
            set
            {
                _mapEditorStatus = value;
                RaisePropertyChanged(() => MapEditorStatus);
            }
        }

        public MapEditorViewModel()
        {
            _mapEditorStatus = MapEditorAndNavigationModel.Instance();
        }
    }
}
