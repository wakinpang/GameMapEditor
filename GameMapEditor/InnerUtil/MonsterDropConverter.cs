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

namespace GameMapEditor.InnerUtil
{
    public class MonsterDropConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = value as MonsterDropArgs;
            return args.Args;
        }
    }
}
