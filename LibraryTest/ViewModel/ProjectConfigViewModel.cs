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
    public class ProjectConfigViewModel : ViewModelBase
    {
        private ProjectConfigModel _projectConfig;

        public ProjectConfigModel ProjectConfig
        {
            get { return _projectConfig; }
            set
            {
                _projectConfig = value;
                RaisePropertyChanged(() => ProjectConfig);
            }
        }

        public ProjectConfigViewModel()
        {
            _projectConfig = ProjectConfigModel.Instance();
        }
    }
}
