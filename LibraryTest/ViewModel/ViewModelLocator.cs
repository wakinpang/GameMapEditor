/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:LibraryTest"
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

namespace LibraryTest.ViewModel
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
            
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ProjectConfigViewModel>();
            SimpleIoc.Default.Register<MapEditorViewModel>();
            SimpleIoc.Default.Register<MenuViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ProjectConfigViewModel ProjectConfig => ServiceLocator.Current.GetInstance<ProjectConfigViewModel>();

        public MapEditorViewModel MapEditorStatus => ServiceLocator.Current.GetInstance<MapEditorViewModel>();

        public MenuViewModel Menu => ServiceLocator.Current.GetInstance<MenuViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}