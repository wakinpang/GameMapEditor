using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace MapEditorControl.ViewModel
{
    public class MenuControlViewModel : ViewModelBase
    {
        public MenuControlViewModel()
        {
            Messenger.Default.Register<bool>(this, MenuControlMessageTokens.UpdateProjectExistFromView, (exist) =>
            {
                if(exist)
                {
                    ProjectExist = "Visible";
                }
                else
                {
                    ProjectExist = "Hidden";
                }
            });

            Messenger.Default.Register<ObservableCollection<HistorySection>>(this, MenuControlMessageTokens.ChangeHistorySectionFromOutside, (section) =>
            {
                HistoryPath = section;
            });

            ItemSelected = new RelayCommand<MenuItemType>((type) =>
            {
                switch (type)
                {
                    case MenuItemType.NewProject:
                        Messenger.Default.Send<object>(null, MenuControlMessageTokens.NewProjectEventFromViewModel);
                        break;
                    case MenuItemType.ProjectConfig:
                        Messenger.Default.Send<object>(null, MenuControlMessageTokens.ProjectConfigEventFromViewModel);
                        break;
                    case MenuItemType.OpenProject:
                        Messenger.Default.Send<object>(null, MenuControlMessageTokens.OpenProjectEventFromViewModel);
                        break;
                    case MenuItemType.SelectTool:
                        break;
                    case MenuItemType.AreaTool:
                        break;
                    case MenuItemType.PenTool:
                        break;
                    case MenuItemType.PointTool:
                        break;
                    case MenuItemType.Output:
                        Messenger.Default.Send<object>(null, MenuControlMessageTokens.OutputEventFromViewModel);
                        break;
                    case MenuItemType.CutMap:
                        Messenger.Default.Send<object>(null, MenuControlMessageTokens.CutMapEventFromViewModel);
                        break;
                }
            });

            Messenger.Default.Register<bool>(this, MenuControlMessageTokens.UpdateCurrentMapValidFromView, (b) =>
            {
                CurrentMapValid = b;
            });

            HistorySelected = new RelayCommand<string>((path) =>
            {
                //SelectedPath = path;
                Messenger.Default.Send<string>(path, MenuControlMessageTokens.SelectHistoryEventFromViewModel);
            });
        }

        public RelayCommand<MenuItemType> ItemSelected { get; set; }
        public RelayCommand<string> HistorySelected { get; set; }

        private string _projectExist = "Collapsed";
        private ObservableCollection<HistorySection> _historyPath;

        public string ProjectExist
        {
            get { return _projectExist; }
            set
            {
                if (value == _projectExist)
                {
                    return;
                }
                _projectExist = value;
                RaisePropertyChanged(() => ProjectExist);
            }
        }

        private bool _currentMapValid;

        public bool CurrentMapValid
        {
            get { return _currentMapValid; }
            set
            {
                if (value == _currentMapValid)
                {
                    return;
                }
                _currentMapValid = value;
                RaisePropertyChanged(() => CurrentMapValid);
            }
        }


        public ObservableCollection<HistorySection> HistoryPath {
            get { return _historyPath; }
            set
            {
                if (value == _historyPath) {
                    return;
                }
                _historyPath = value;
                RaisePropertyChanged(() => HistoryPath);
            }
        }
    }
}
