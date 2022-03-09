/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GameMapEditor"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace GameMapEditor.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic) 
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MapEditorViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
            SimpleIoc.Default.Register<ProjectConfigViewModel>();
            SimpleIoc.Default.Register<LibraryViewModel>();
            SimpleIoc.Default.Register<CostumeMessageDialogViewModel>();
            SimpleIoc.Default.Register<CostumeWaitingDialogViewModel>();
            SimpleIoc.Default.Register<MonsterConfigViewModel>();
            SimpleIoc.Default.Register<ToolBarViewModel>();

            ServiceLocator.Current.GetInstance<ProjectConfigViewModel>();
            ServiceLocator.Current.GetInstance<MainViewModel>();
            ServiceLocator.Current.GetInstance<MonsterConfigViewModel>();
        }

        public MainViewModel Main { get { return ServiceLocator.Current.GetInstance<MainViewModel>(); } }

        public MapEditorViewModel MapEditorStatus { get { return ServiceLocator.Current.GetInstance<MapEditorViewModel>(); } }

        public MenuViewModel Menu { get { return ServiceLocator.Current.GetInstance<MenuViewModel>(); } }

        public ProjectConfigViewModel ProjectConfig { get { return ServiceLocator.Current.GetInstance<ProjectConfigViewModel>(); } }

        public LibraryViewModel LibrarySource { get { return ServiceLocator.Current.GetInstance<LibraryViewModel>(); } }

        public CostumeMessageDialogViewModel CostumeMessageDialog { get { return ServiceLocator.Current.GetInstance<CostumeMessageDialogViewModel>(); } }

        public CostumeWaitingDialogViewModel CostumeWaitingDialog { get { return ServiceLocator.Current.GetInstance<CostumeWaitingDialogViewModel>(); } }

        public MonsterConfigViewModel MonsterConfig { get { return ServiceLocator.Current.GetInstance<MonsterConfigViewModel>(); } }

        public ToolBarViewModel ToolBar { get { return ServiceLocator.Current.GetInstance<ToolBarViewModel>(); } }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
} 