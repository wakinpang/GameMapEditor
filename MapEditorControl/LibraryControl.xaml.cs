using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;

namespace MapEditorControl
{
    /// <summary>
    /// LibraryControl.xaml 的交互逻辑
    /// </summary>
    public partial class LibraryControl : UserControl
    {
        //Map
        public ObservableCollection<MapSection> MapData {
            get { return (ObservableCollection<MapSection>)GetValue(MapDataProperty); }
            set {
                if (value == (ObservableCollection<MapSection>)GetValue(MapDataProperty)) {
                    return;
                }
                SetValue(MapDataProperty, value);
            }
        }
        public static readonly DependencyProperty MapDataProperty =
            DependencyProperty.Register("MapData", typeof(ObservableCollection<MapSection>), typeof(LibraryControl), new PropertyMetadata(OnMapDataChanged));

        private static void OnMapDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as LibraryControl;

            Messenger.Default.Send<ObservableCollection<MapSection>>(obj.MapData, LibraryControlMessageTokens.UpdateMapFromOutside);
        }

        public event EventHandler CurrentMapChanged
        {
            add { AddHandler(CurrentMapChangedEvent, value); }
            remove { RemoveHandler(CurrentMapChangedEvent, value); }
        }

        public static readonly RoutedEvent CurrentMapChangedEvent = EventManager.RegisterRoutedEvent(
        "CurrentMapChanged", RoutingStrategy.Bubble, typeof(EventHandler), typeof(LibraryControl));

        public event EventHandler CurrentMonsterChanged {
            add { AddHandler(CurrentMonsterChangedEvent, value); }
            remove { RemoveHandler(CurrentMonsterChangedEvent, value); }
        }

        public static readonly RoutedEvent CurrentMonsterChangedEvent = EventManager.RegisterRoutedEvent(
            "CurrentMonsterChanged", RoutingStrategy.Bubble, typeof(EventHandler), typeof(LibraryControl));


        //public event EventHandler OnMapSourceChanged {
        //    add { AddHandler(OnMapSourceChangedEvent, value); }
        //    remove { RemoveHandler(OnMapSourceChangedEvent, value); }
        //}

        //public static readonly RoutedEvent OnMapSourceChangedEvent = EventManager.RegisterRoutedEvent(
        //"OnMapSourceChanged", RoutingStrategy.Bubble, typeof(EventHandler), typeof(LibraryControl));

        //Music
        public ObservableCollection<MusicSection> MusicData
        {
            get { return (ObservableCollection<MusicSection>)GetValue(MusicDataProperty); }
            set
            {
                if (value == (ObservableCollection<MusicSection>)GetValue(MusicDataProperty))
                {
                    return;
                }
                SetValue(MusicDataProperty, value);
            }
        }
        public static readonly DependencyProperty MusicDataProperty =
            DependencyProperty.Register("MusicData", typeof(ObservableCollection<MusicSection>), typeof(LibraryControl), new PropertyMetadata(OnMusicDataChanged));

        private static void OnMusicDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as LibraryControl;

            Messenger.Default.Send<ObservableCollection<MusicSection>>(obj.MusicData, LibraryControlMessageTokens.UpdateMusicFromOutside);
        }

        public string CurrentMusicId {
            get { return (string)GetValue(CurrentMusicIdProperty); }
            set {
                if (value == (string)GetValue(CurrentMusicIdProperty)) {
                    return;
                }
                SetValue(CurrentMusicIdProperty, value);
            }
        }
        public static readonly DependencyProperty CurrentMusicIdProperty =
            DependencyProperty.Register("CurrentMusicId", typeof(string), typeof(LibraryControl), new PropertyMetadata(OnCurrentMusicIdChanged));

        private static void OnCurrentMusicIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            Messenger.Default.Send<object>(0, LibraryControlMessageTokens.ChangeCheckedMusic);
        }

        //Style

        //Monster
        public ObservableCollection<MonsterSection> MonsterData
        {
            get { return (ObservableCollection<MonsterSection>)GetValue(MonsterDataProperty); }
            set
            {
                if (value == (ObservableCollection<MonsterSection>)GetValue(MonsterDataProperty))
                {
                    return;
                }
                SetValue(MonsterDataProperty, value);
            }
        }
        public static readonly DependencyProperty MonsterDataProperty =
            DependencyProperty.Register("MonsterData", typeof(ObservableCollection<MonsterSection>), typeof(LibraryControl), new PropertyMetadata(OnMonsterDataChanged));

        private static void OnMonsterDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as LibraryControl;

            Messenger.Default.Send<ObservableCollection<MonsterSection>>(obj.MonsterData, LibraryControlMessageTokens.UpdateMonsterFromOutside);
        }

        public event EventHandler NewMonster
        {
            add { AddHandler(NewMonsterEvent, value); }
            remove { RemoveHandler(NewMonsterEvent, value); }
        }

        public static readonly RoutedEvent NewMonsterEvent = EventManager.RegisterRoutedEvent(
            "NewMonster", RoutingStrategy.Bubble, typeof(EventHandler), typeof(LibraryControl));

        public event EventHandler ChangeMonster {
            add { AddHandler(ChangeMonsterEvent, value); }
            remove { RemoveHandler(ChangeMonsterEvent, value); }
        }

        public static readonly RoutedEvent ChangeMonsterEvent = EventManager.RegisterRoutedEvent(
            "ChangeMonster", RoutingStrategy.Bubble, typeof(EventHandler), typeof(LibraryControl));

        public event EventHandler MusicError {
            add { AddHandler(MusicErrorEvent, value); }
            remove { RemoveHandler(MusicErrorEvent, value); }
        }

        public static readonly RoutedEvent MusicErrorEvent = EventManager.RegisterRoutedEvent(
            "MusicError", RoutingStrategy.Bubble, typeof(EventHandler), typeof(LibraryControl));
        // Switch

        public bool ProjectExistData {
            get { return (bool)GetValue(ProjectExistDataProperty); }
            set {
                if (value == (bool)GetValue(ProjectExistDataProperty)) {
                    return;
                }
                SetValue(ProjectExistDataProperty, value);
            }
        }

        public static readonly DependencyProperty ProjectExistDataProperty = DependencyProperty.Register(
            "ProjectExistData", typeof(bool), typeof(LibraryControl), new PropertyMetadata(OnProjectExistDataChanged));

        private static void OnProjectExistDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as LibraryControl;

            Messenger.Default.Send<bool>(obj.ProjectExistData, LibraryControlMessageTokens.UpdateProjectExistFromModel);
        }

        public bool MapValidData {
            get { return (bool)GetValue(MapValidDataProperty); }
            set {
                if (value == (bool)GetValue(MapValidDataProperty)) {
                    return;
                }
                SetValue(MapValidDataProperty, value);
            }
        }

        public static readonly DependencyProperty MapValidDataProperty = DependencyProperty.Register(
            "MapValidData", typeof(bool), typeof(LibraryControl), new PropertyMetadata(OnMapValidDataChanged));

        private static void OnMapValidDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as LibraryControl;

            Messenger.Default.Send<bool>(obj.MapValidData, LibraryControlMessageTokens.UpdateMapValidFromModel);
        }

        //Initialize
        public LibraryControl()
        {
            InitializeComponent();

            Messenger.Default.Register<object>(this, LibraryControlMessageTokens.CallNewDialogFromView, (dummy) => {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(NewMonsterEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, LibraryControlMessageTokens.CallChangeDialogFromView, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(ChangeMonsterEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<ObservableCollection<MapSection>>(this, LibraryControlMessageTokens.MapSectionChangedFromView, (section) =>
            {
                MapData = section;
            });

            Messenger.Default.Register<ObservableCollection<MusicSection>>(this, LibraryControlMessageTokens.MusicSectionChangedFromView, (section) =>
            {
                MusicData = section;
            });

            Messenger.Default.Register<MapSection>(this, LibraryControlMessageTokens.CurrentMapSectionChangedEventFromViewModel, (section) =>
            {
                var newEventArgs = new CurrentMapSectionChangedEventArgs(CurrentMapChangedEvent)
                {
                    CurrentMapSection = section,
                };
                RaiseEvent(newEventArgs);

            });

            Messenger.Default.Register<MonsterSection>(this, LibraryControlMessageTokens.ChangeCurrentMonsterFromLibrary, (section) =>
            {
                var newEventArgs = new CurrentMonsterChangedEventArgs(CurrentMonsterChangedEvent)
                {
                    CurrentMonsterSection = section,
                };
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, LibraryControlMessageTokens.ChangeCheckedMusic, (dummy) =>
            {
                foreach (MusicSection mus in music_List.Items) {
                    if (mus.Name == CurrentMusicId) {
                        mus.IsChecked = true;
                        return;
                    }
                }
            });

            Messenger.Default.Register<string>(this, LibraryControlMessageTokens.CheckedMusicChangedFromView, (Id) =>
            {
                CurrentMusicId = Id;
            });

            Messenger.Default.Register<object>(this, LibraryControlMessageTokens.CallMusicErrorDialogFromViewModel, (dummy) =>
            {
                var newEventArgs = new RoutedEventArgs(MusicErrorEvent);
                RaiseEvent(newEventArgs);
            });
            //Messenger.Default.Register<string>(this, LibraryControlMessageTokens.MapCheckedFromView, (name) =>
            //{
            //    //SelectedMap = name;
            //    RoutedEventArgs newEventArgs = new RoutedEventArgs(OnMapSourceChangedEvent);
            //    RaiseEvent(newEventArgs);
            //});
        }
    }
}
