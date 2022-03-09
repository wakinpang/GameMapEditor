using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MapEditorControl
{
    /// <summary>
    /// PropertyControl.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyControl : UserControl
    {


        public MapSection CurrentMapSection
        {
            get { return (MapSection)GetValue(CurrentMapSectionProperty); }
            set
            {
                if (value == (MapSection)GetValue(CurrentMapSectionProperty))
                {
                    return;
                }
                SetValue(CurrentMapSectionProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMapSection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapSectionProperty =
            DependencyProperty.Register("CurrentMapSection", typeof(MapSection), typeof(PropertyControl), new PropertyMetadata(OnCurrentMapSectionChanged));

        private static void OnCurrentMapSectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
            Messenger.Default.Send<MapSection>(e.NewValue as MapSection, PropertyControlMessageTokens.UpdateCurrentMapSectionFromView);
        }


        public MonsterSection CurrentMonsterSection
        {
            get { return (MonsterSection)GetValue(CurrentMonsterSectionProperty); }
            set
            {
                if (value == (MonsterSection)GetValue(CurrentMonsterSectionProperty))
                {
                    return;
                }
                SetValue(CurrentMonsterSectionProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMonsterSection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMonsterSectionProperty =
            DependencyProperty.Register("CurrentMonsterSection", typeof(MonsterSection), typeof(PropertyControl), new PropertyMetadata(OnCurrentMonsterSectionChanged));

        private static void OnCurrentMonsterSectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<MonsterSection>(e.NewValue as MonsterSection, PropertyControlMessageTokens.UpdateCurrentMonsterSectionFromView);
        }

        public SceneMonsterPOJO CurrentSceneMonsterPOJO
        {
            get { return (SceneMonsterPOJO)GetValue(CurrentSceneMonsterPOJOProperty); }
            set
            {
                if (value == (SceneMonsterPOJO)GetValue(CurrentSceneMonsterPOJOProperty))
                {
                    return;
                }
                SetValue(CurrentSceneMonsterPOJOProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentSceneMonsterPOJO.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSceneMonsterPOJOProperty =
            DependencyProperty.Register("CurrentSceneMonsterPOJO", typeof(SceneMonsterPOJO), typeof(PropertyControl), new PropertyMetadata(OnCurrentSceneMonsterPOJOChanged));

        private static void OnCurrentSceneMonsterPOJOChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<SceneMonsterPOJO>(e.NewValue as SceneMonsterPOJO, PropertyControlMessageTokens.UpdateCurrentSceneMonsterPOJOFromView);
        }


        public ObservableCollection<SceneMonsterPOJO> CurrentMapSceneMonsterPOJOs
        {
            get { return (ObservableCollection<SceneMonsterPOJO> )GetValue(CurrentMapSceneMonsterPOJOsProperty); }
            set
            {
                if (value == (ObservableCollection<SceneMonsterPOJO> )GetValue(CurrentMapSceneMonsterPOJOsProperty))
                {
                    return;
                }
                SetValue(CurrentMapSceneMonsterPOJOsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMapSceneMonsterPOJOs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapSceneMonsterPOJOsProperty =
            DependencyProperty.Register("CurrentMapSceneMonsterPOJOs", typeof(ObservableCollection<SceneMonsterPOJO> ), typeof(PropertyControl), new PropertyMetadata(OnCurrentMapSceneMonsterPOJOsChanged));

        private static void OnCurrentMapSceneMonsterPOJOsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<ObservableCollection<SceneMonsterPOJO>>(e.NewValue as ObservableCollection<SceneMonsterPOJO>, PropertyControlMessageTokens.UpdateCurrentMapSceneMonsterPOJOsFromView);
        }

        public NpcSection CurrentNpcSection {
            get { return (NpcSection)GetValue(CurrentNpcSectionProperty); }
            set {
                if (value == (NpcSection)GetValue(CurrentNpcSectionProperty)) {
                    return;
                }
                SetValue(CurrentNpcSectionProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentNpcSectionProperty = DependencyProperty.Register(
            "CurrentNpcSection", typeof(NpcSection), typeof(PropertyControl), new PropertyMetadata(OnCurrentNpcSectionChanged));

        private static void OnCurrentNpcSectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as PropertyControl;

            Messenger.Default.Send<NpcSection>(obj.CurrentNpcSection, PropertyControlMessageTokens.UpdateCurrentNpcSectionFromModel);
        }

        public ObservableCollection<NpcSection> CurrentNpcCollectionData {
            get { return (ObservableCollection<NpcSection>)GetValue(CurrentNpcCollectionDataProperty); }
            set {
                if (value == (ObservableCollection<NpcSection>)GetValue(CurrentNpcCollectionDataProperty)) {
                    return;
                }
                SetValue(CurrentNpcCollectionDataProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentNpcCollectionDataProperty = DependencyProperty.Register(
            "CurrentNpcCollectionData", typeof(ObservableCollection<NpcSection>), typeof(PropertyControl), new PropertyMetadata(OnCurrentNpcCollectionChanged));

        private static void OnCurrentNpcCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as PropertyControl;

            Messenger.Default.Send<ObservableCollection<NpcSection>>(obj.CurrentNpcCollectionData, PropertyControlMessageTokens.UpdateCurrentNpcCollectionFromModel);
        }

        public int CurrentMapID
        {
            get { return (int)GetValue(CurrentMapIDProperty); }
            set
            {
                if (value == (int)GetValue(CurrentMapIDProperty))
                {
                    return;
                }
                SetValue(CurrentMapIDProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMapID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapIDProperty =
            DependencyProperty.Register("CurrentMapID", typeof(int), typeof(PropertyControl), new PropertyMetadata(OnCurrentMapIDChanged));

        private static void OnCurrentMapIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<int>((int)e.NewValue, PropertyControlMessageTokens.UpdateCurrentMapIDFromView);
        }



        public event EventHandler ShowDropInfo
        {
            add { AddHandler(ShowDropInfoEvent, value); }
            remove { RemoveHandler(ShowDropInfoEvent, value); }
        }

        public static readonly RoutedEvent ShowDropInfoEvent = EventManager.RegisterRoutedEvent(
        "ShowDropInfo", RoutingStrategy.Bubble, typeof(EventHandler), typeof(PropertyControl));


        public event EventHandler ModifySceneMonster
        {
            add { AddHandler(ModifySceneMonsterEvent, value); }
            remove { RemoveHandler(ModifySceneMonsterEvent, value); }
        }

        public static readonly RoutedEvent ModifySceneMonsterEvent = EventManager.RegisterRoutedEvent(
        "ModifySceneMonster", RoutingStrategy.Bubble, typeof(EventHandler), typeof(PropertyControl));


        public event EventHandler NewSceneMonster
        {
            add { AddHandler(NewSceneMonsterEvent, value); }
            remove { RemoveHandler(NewSceneMonsterEvent, value); }
        }

        public static readonly RoutedEvent NewSceneMonsterEvent = EventManager.RegisterRoutedEvent(
        "NewSceneMonster", RoutingStrategy.Bubble, typeof(EventHandler), typeof(PropertyControl));


        public event EventHandler BaseRewardEdit
        {
            add { AddHandler(BaseRewardEditEvent, value); }
            remove { RemoveHandler(BaseRewardEditEvent, value); }
        }

        public static readonly RoutedEvent BaseRewardEditEvent = EventManager.RegisterRoutedEvent(
        "BaseRewardEdit", RoutingStrategy.Bubble, typeof(EventHandler), typeof(PropertyControl));


        public event EventHandler MissionRewardEdit
        {
            add { AddHandler(MissionRewardEditEvent, value); }
            remove { RemoveHandler(MissionRewardEditEvent, value); }
        }

        public static readonly RoutedEvent MissionRewardEditEvent = EventManager.RegisterRoutedEvent(
        "MissionRewardEdit", RoutingStrategy.Bubble, typeof(EventHandler), typeof(PropertyControl));


        public PropertyControl()
        {
            InitializeComponent();

            Messenger.Default.Register<SceneMonsterPOJO>(this, PropertyControlMessageTokens.UpdateCurrentSceneMonsterPOJOFromViewModel, (pojo) =>
            {
                CurrentSceneMonsterPOJO = pojo;
            });

            Messenger.Default.Register<NpcSection>(this, PropertyControlMessageTokens.UpdateCurrentNpcSectionFromView, (section) =>
            {
                CurrentNpcSection = section;
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.RaiseShowDropInfoEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(ShowDropInfoEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.SetCurrentItemTabToMap, (dummy) =>
            {
                main_Tab.SelectedIndex = 0;
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.RaiseEditSceneMonsterButtonEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(ModifySceneMonsterEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.RaiseNewSceneMonsterButtonEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(NewSceneMonsterEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.RaiseShowBaseRewardEditControlEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(BaseRewardEditEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, PropertyControlMessageTokens.RaiseShowMissionRewardEditControlEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(MissionRewardEditEvent);
                RaiseEvent(newEventArgs);
            });

        }

    }
}
