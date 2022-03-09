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
    /// MissionRewardInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class MissionRewardInfoControl : UserControl
    {

        public string MissionString
        {
            get { return (string)GetValue(MissionStringProperty); }
            set
            {
                if (value == (string)GetValue(MissionStringProperty))
                {
                    return;
                }
                SetValue(MissionStringProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MissionString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MissionStringProperty =
            DependencyProperty.Register("MissionString", typeof(string), typeof(MissionRewardInfoControl), new PropertyMetadata(OnMissionStringChanged));

        private static void OnMissionStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, MissionRewardInfoControlMessageTokens.UpdateMissionStringFromView);
        }


        public ObservableCollection<MissionPOJO> CurrentMissionPOJOs
        {
            get { return (ObservableCollection<MissionPOJO>)GetValue(CurrentMissionPOJOsProperty); }
            set
            {
                if (value == (ObservableCollection<MissionPOJO>)GetValue(CurrentMissionPOJOsProperty))
                {
                    return;
                }
                SetValue(CurrentMissionPOJOsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMissionPOJOs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMissionPOJOsProperty =
            DependencyProperty.Register("CurrentMissionPOJOs", typeof(ObservableCollection<MissionPOJO>), typeof(MissionRewardInfoControl), new PropertyMetadata(OnCurrentMissionPOJOsChanged));

        private static void OnCurrentMissionPOJOsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<ObservableCollection<MissionPOJO>>(e.NewValue as ObservableCollection<MissionPOJO>, MissionRewardInfoControlMessageTokens.UpdateCurrentMissionPOJOsFromView);
        }


        public ObservableCollection<ItemPOJO> CurrentItemPOJOs
        {
            get { return (ObservableCollection<ItemPOJO>)GetValue(CurrentItemPOJOsProperty); }
            set
            {
                if (value == (ObservableCollection<ItemPOJO>)GetValue(CurrentItemPOJOsProperty))
                {
                    return;
                }
                SetValue(CurrentItemPOJOsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentItemPOJOs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemPOJOsProperty =
            DependencyProperty.Register("CurrentItemPOJOs", typeof(ObservableCollection<ItemPOJO>), typeof(MissionRewardInfoControl), new PropertyMetadata(OnCurrentItemPOJOsChanged));

        private static void OnCurrentItemPOJOsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<ObservableCollection<ItemPOJO>>(e.NewValue as ObservableCollection<ItemPOJO>, MissionRewardInfoControlMessageTokens.UpdateCurrentItemPOJOsFromView);
        }


        public event EventHandler Confirm
        {
            add { AddHandler(ConfirmEvent, value); }
            remove { RemoveHandler(ConfirmEvent, value); }
        }

        public static readonly RoutedEvent ConfirmEvent = EventManager.RegisterRoutedEvent(
        "Confirm", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MissionRewardInfoControl));


        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
        "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MissionRewardInfoControl));


        public MissionRewardInfoControl()
        {
            InitializeComponent();

            Messenger.Default.Register<ValidateErrorHappendChecker>(this, MissionRewardInfoControlMessageTokens.NotifyUpdateErrorHapendFromViewModel, (checker) =>
            {
                checker.Flag = ValidateCheckHelper.IsValid(this);
            });

            Messenger.Default.Register<string>(this, MissionRewardInfoControlMessageTokens.UpdateMissionStringFromViewModel, (str) =>
            {
                MissionString = str;

                RoutedEventArgs newEventArgs = new RoutedEventArgs(ConfirmEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, MissionRewardInfoControlMessageTokens.RaiseCancelEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(CancelEvent);
                RaiseEvent(newEventArgs);
            });
        }
    }
}
