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
    /// CostumeWaitingDialogControl.xaml 的交互逻辑
    /// </summary>
    public partial class CostumeWaitingDialogControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                if (value == (string)GetValue(TitleProperty))
                {
                    return;
                }
                SetValue(TitleProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CostumeWaitingDialogControl), new PropertyMetadata(OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, CostumeWaitingDialogMesssageTokens.UpdateTitleFromView);
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set
            {
                if (value == (string)GetValue(MessageProperty))
                {
                    return;
                }
                SetValue(MessageProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(CostumeWaitingDialogControl), new PropertyMetadata(OnMessageChanged));

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, CostumeWaitingDialogMesssageTokens.UpdateMessageFromView);
        }

        public bool CancelVaild
        {
            get { return (bool)GetValue(CancelVaildProperty); }
            set
            {
                if (value == (bool)GetValue(CancelVaildProperty))
                {
                    return;
                }
                SetValue(CancelVaildProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CancelVaild.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelVaildProperty =
            DependencyProperty.Register("CancelVaild", typeof(bool), typeof(CostumeWaitingDialogControl), new PropertyMetadata(OnCancelVaildChanged));

        private static void OnCancelVaildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, CostumeWaitingDialogMesssageTokens.UpdateCancelVaildFromView);
        }

        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
        "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(CostumeWaitingDialogControl));

        public CostumeWaitingDialogControl()
        {
            InitializeComponent();

            Messenger.Default.Register<object>(this, CostumeWaitingDialogMesssageTokens.CancelEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(CancelEvent);
                RaiseEvent(newEventArgs);
            });
        }
    }
}
