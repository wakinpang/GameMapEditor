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
    /// DropItemInfoListControl.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class DropItemInfoListControl : UserControl
    {

        public string DropItemInfoString
        {
            get { return (string)GetValue(DropItemInfoStringProperty); }
            set
            {
                if (value == (string)GetValue(DropItemInfoStringProperty))
                {
                    return;
                }
                SetValue(DropItemInfoStringProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DropItemInfoString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropItemInfoStringProperty =
            DependencyProperty.Register("DropItemInfoString", typeof(string), typeof(DropItemInfoListControl), new PropertyMetadata(OnDropItemInfoStringChanged));

        private static void OnDropItemInfoStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, DropItemInfoListControlMessageTokens.UpdateDropItemInfoStringFromView);
        }


        public ObservableCollection<ItemPOJO> CurrentItemsInfo
        {
            get { return (ObservableCollection<ItemPOJO>)GetValue(CurrentItemsInfoProperty); }
            set
            {
                if (value == (ObservableCollection<ItemPOJO>)GetValue(CurrentItemsInfoProperty))
                {
                    return;
                }
                SetValue(CurrentItemsInfoProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentItemsInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentItemsInfoProperty =
            DependencyProperty.Register("CurrentItemsInfo", typeof(ObservableCollection<ItemPOJO>), typeof(DropItemInfoListControl), new PropertyMetadata(OnCurrentItemsInfoChanged));

        private static void OnCurrentItemsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<ObservableCollection<ItemPOJO>>(e.NewValue as ObservableCollection<ItemPOJO>, DropItemInfoListControlMessageTokens.UpdateCurrentItemsInfoFromView);
        }

        public event EventHandler EditCancel
        {
            add { AddHandler(EditCancelEvent, value); }
            remove { RemoveHandler(EditCancelEvent, value); }
        }

        public static readonly RoutedEvent EditCancelEvent = EventManager.RegisterRoutedEvent(
        "EditCancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(DropItemInfoListControl));


        public event EventHandler EditConfirm
        {
            add { AddHandler(EditConfirmEvent, value); }
            remove { RemoveHandler(EditConfirmEvent, value); }
        }

        public static readonly RoutedEvent EditConfirmEvent = EventManager.RegisterRoutedEvent(
        "EditConfirm", RoutingStrategy.Bubble, typeof(EventHandler), typeof(DropItemInfoListControl));


        public DropItemInfoListControl()
        {
            InitializeComponent();

            Messenger.Default.Register<ValidateErrorHappendChecker>(this, DropItemInfoListControlMessageTokens.NotifyUpdateErrorHapendFromViewModel, (checker) =>
            {
                checker.Flag = ValidateCheckHelper.IsValid(this);
            });

            Messenger.Default.Register<string>(this, DropItemInfoListControlMessageTokens.UpdateDropItemInfoStringFromViewModel, (str) =>
            {
                DropItemInfoString = str;
                RoutedEventArgs newEventArgs = new RoutedEventArgs(EditConfirmEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, DropItemInfoListControlMessageTokens.CancelEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(EditCancelEvent);
                RaiseEvent(newEventArgs);
            });

        }



    }
}
