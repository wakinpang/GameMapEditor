using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
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

namespace MapEditorControl
{
    /// <summary>
    /// BaseRewardInfoControl.xaml 的交互逻辑
    /// </summary>
    public partial class BaseRewardInfoControl : UserControl
    {


        public string BaseRewardString
        {
            get { return (string)GetValue(BaseRewardStringProperty); }
            set
            {
                if (value == (string)GetValue(BaseRewardStringProperty))
                {
                    return;
                }
                SetValue(BaseRewardStringProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for BaseRewardString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseRewardStringProperty =
            DependencyProperty.Register("BaseRewardString", typeof(string), typeof(BaseRewardInfoControl), new PropertyMetadata(OnBaseRewardStringChanged));

        private static void OnBaseRewardStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, BaseRewardInfoControlMessageTokens.UpdateRewardStringFromView);
        }

        public event EventHandler Confirm
        {
            add { AddHandler(ConfirmEvent, value); }
            remove { RemoveHandler(ConfirmEvent, value); }
        }

        public static readonly RoutedEvent ConfirmEvent = EventManager.RegisterRoutedEvent(
        "Confirm", RoutingStrategy.Bubble, typeof(EventHandler), typeof(BaseRewardInfoControl));


        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
        "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(BaseRewardInfoControl));


        public BaseRewardInfoControl()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, BaseRewardInfoControlMessageTokens.UpdateRewardStringFromViewModel, (str) =>
            {
                BaseRewardString = str;

                RoutedEventArgs newEventArgs = new RoutedEventArgs(ConfirmEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, BaseRewardInfoControlMessageTokens.CancelEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(CancelEvent);
                RaiseEvent(newEventArgs);
            });
        }
    }
}
