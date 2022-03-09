using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEditorControl.InnerUtil;

namespace LibraryTest.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        public RelayCommand NewProjectHandle { get; set; }

        public MenuViewModel()
        {
            NewProjectHandle = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, MenuMessageTokens.ShowNewProjectWindowFromViewModel);
            });
        }
    }
}
