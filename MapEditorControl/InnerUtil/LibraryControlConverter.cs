using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MapEditorControl.InnerUtil
{
    class LibraryControlConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter) {
            MonsterData section = new MonsterData();
            section.Args = value as MouseEventArgs;
            section.Source = parameter as TextBlock;
            
            return section;
        }
    }
}
