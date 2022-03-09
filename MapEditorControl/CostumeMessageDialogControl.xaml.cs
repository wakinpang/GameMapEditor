using GalaSoft.MvvmLight.Command;
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
    /// CostumeMessageDialogControl.xaml 的交互逻辑
    /// </summary>
    public partial class CostumeMessageDialogControl : UserControl
    {
        public CostumeMessageDialogControl()
        {
            InitializeComponent();

            Messenger.Default.Register<object>(this, CostumeMessageDialogMessageTokens.OKEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(OKEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, CostumeMessageDialogMessageTokens.CancelEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(CancelEvent);
                RaiseEvent(newEventArgs);
            });

        }

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
            DependencyProperty.Register("Title", typeof(string), typeof(CostumeMessageDialogControl), new PropertyMetadata(OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, CostumeMessageDialogMessageTokens.UpdateTitleFromView);
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
            DependencyProperty.Register("Message", typeof(string), typeof(CostumeMessageDialogControl), new PropertyMetadata(OnMessageChanged));

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, CostumeMessageDialogMessageTokens.UpdateMessageFromView);
        }

        public CostumeDialogButtonType ButtonType
        {
            get { return (CostumeDialogButtonType)GetValue(ButtonTypeProperty); }
            set
            {
                if (value == (CostumeDialogButtonType)GetValue(ButtonTypeProperty))
                {
                    return;
                }
                SetValue(ButtonTypeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ButtonType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType", typeof(CostumeDialogButtonType), typeof(CostumeMessageDialogControl), new PropertyMetadata(OnButtonTypeChanged));

        private static void OnButtonTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<CostumeDialogButtonType>((CostumeDialogButtonType)e.NewValue, CostumeMessageDialogMessageTokens.UpdateButtonTypeFromView);
        }

        public event EventHandler OK
        {
            add { AddHandler(OKEvent, value); }
            remove { RemoveHandler(OKEvent, value); }
        }

        public static readonly RoutedEvent OKEvent = EventManager.RegisterRoutedEvent(
        "OK", RoutingStrategy.Bubble, typeof(EventHandler), typeof(CostumeMessageDialogMessageTokens));


        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
        "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(CostumeMessageDialogMessageTokens));


    }
}
