using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MVVM_TEST.Model;
using System.Windows;

namespace MVVM_TEST.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        private TestModel _testModel;

        public TestModel MainModel
        {
            get { return _testModel; }
            set
            {
                _testModel = value;
                RaisePropertyChanged(() => MainModel);
            }
        }

        public RelayCommand ClickHandle { get; set; }

        public MainViewModel()
        {
            MainModel = new TestModel()
            {
                MyText = "Hello"
            };

            ClickHandle = new RelayCommand(() =>
            {
                MainModel.MyText = "Hello, World!";
            });
        }
    }
}