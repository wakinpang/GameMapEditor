﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditorControl.InnerUtil
{
    public class CurrentMusicSectionChangedEventArgs : RoutedEventArgs
    {
        public MusicSection CurrentMusicSection { get; set; }

        public CurrentMusicSectionChangedEventArgs(RoutedEvent routedEvent) : base(routedEvent) {

        }
    }
}
