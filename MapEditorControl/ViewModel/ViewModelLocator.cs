/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MVVM_TEST"
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

namespace MapEditorControl.ViewModel
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

            //SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MapEditorControlViewModel>();
            SimpleIoc.Default.Register<NavigationControlViewModel>();
            SimpleIoc.Default.Register<ProjectConfigControlViewModel>();
            SimpleIoc.Default.Register<LibraryControlViewModel>();
            SimpleIoc.Default.Register<MenuControlViewModel>();
            SimpleIoc.Default.Register<MonsterConfigControlViewModel>();
            SimpleIoc.Default.Register<CostumeMessageDialogControlViewModel>();
            SimpleIoc.Default.Register<CostumeWaitingDialogControlViewModel>();
            SimpleIoc.Default.Register<ToolBarControlViewModel>();
            SimpleIoc.Default.Register<MapObjectSpriteViewModel>();
            SimpleIoc.Default.Register<PropertyControlViewModel>();
            SimpleIoc.Default.Register<DropItemInfoListControlViewModel>();
            SimpleIoc.Default.Register<SceneMonsterConfigControlViewModel>();
            SimpleIoc.Default.Register<BaseRewardInfoViewModel>();
            SimpleIoc.Default.Register<MissionRewardInfoControlViewModel>();
        }

        //public MainViewModel Main
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<MainViewModel>();
        //    }
        //}

        public MapEditorControlViewModel MapEditorControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MapEditorControlViewModel>();
            }
        }

        public NavigationControlViewModel NavigationControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NavigationControlViewModel>();
            }
        }

        public ProjectConfigControlViewModel ProjectConfigControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProjectConfigControlViewModel>();
            }
        }
        public LibraryControlViewModel LibraryControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LibraryControlViewModel>();
            }
        }
        public MenuControlViewModel MenuControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MenuControlViewModel>();
            }
        }

        public MonsterConfigControlViewModel MonsterConfigControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonsterConfigControlViewModel>();
            }
        }
        public CostumeMessageDialogControlViewModel CostumeMessageDialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CostumeMessageDialogControlViewModel>();
            }
        }

        public CostumeWaitingDialogControlViewModel CostumeWaitingDialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CostumeWaitingDialogControlViewModel>();
            }
        }

        public ToolBarControlViewModel ToolBarControl
        {
            get {
                return ServiceLocator.Current.GetInstance<ToolBarControlViewModel>();
            }
        }

        public MapObjectSpriteViewModel MapObjectSprite
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MapObjectSpriteViewModel>();
            }
        }

        public PropertyControlViewModel PropertyData
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PropertyControlViewModel>();
            }
        }

        public DropItemInfoListControlViewModel DropItemInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DropItemInfoListControlViewModel>();
            }
        }

        public SceneMonsterConfigControlViewModel SceneMonsterConfig
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SceneMonsterConfigControlViewModel>();
            }
        }

        public BaseRewardInfoViewModel BaseRewardInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BaseRewardInfoViewModel>();
            }
        }

        public MissionRewardInfoControlViewModel MissionRewardInfo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MissionRewardInfoControlViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}