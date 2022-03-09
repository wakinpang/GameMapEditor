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
    /// SceneMonsterConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class SceneMonsterConfigControl : UserControl
    {


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
            DependencyProperty.Register("CurrentSceneMonsterPOJO", typeof(SceneMonsterPOJO), typeof(SceneMonsterConfigControl), new PropertyMetadata(OnCurrentSceneMonsterPOJOChanged));

        private static void OnCurrentSceneMonsterPOJOChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<SceneMonsterPOJO>(e.NewValue as SceneMonsterPOJO, SceneMonsterConfigControlMessageTokens.UpdateCurrentSceneMonsterPOJOFromView);
        }


        public bool Modifying
        {
            get { return (bool)GetValue(ModifyingProperty); }
            set
            {
                if (value == (bool)GetValue(ModifyingProperty))
                {
                    return;
                }
                SetValue(ModifyingProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Modifying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModifyingProperty =
            DependencyProperty.Register("Modifying", typeof(bool), typeof(SceneMonsterConfigControl), new PropertyMetadata(OnModifyingChanged));

        private static void OnModifyingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, SceneMonsterConfigControlMessageTokens.UpdateModifyingFromView);
        }


        public ObservableCollection<SceneMonsterPOJO> CurrentSceneMonsterPOJOs
        {
            get { return (ObservableCollection<SceneMonsterPOJO>)GetValue(CurrentSceneMonsterPOJOsProperty); }
            set
            {
                if (value == (ObservableCollection<SceneMonsterPOJO>)GetValue(CurrentSceneMonsterPOJOsProperty))
                {
                    return;
                }
                SetValue(CurrentSceneMonsterPOJOsProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentSceneMonsterPOJOs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSceneMonsterPOJOsProperty =
            DependencyProperty.Register("CurrentSceneMonsterPOJOs", typeof(ObservableCollection<SceneMonsterPOJO>), typeof(SceneMonsterConfigControl), new PropertyMetadata(OnCurrentSceneMonsterPOJOsChanged));

        private static void OnCurrentSceneMonsterPOJOsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<ObservableCollection<SceneMonsterPOJO>>(e.NewValue as ObservableCollection<SceneMonsterPOJO>, SceneMonsterConfigControlMessageTokens.UpdateCurrentSceneMonsterPOJOsFromView);
        }


        public string CurrentMonsterPicturePath
        {
            get { return (string)GetValue(CurrentMonsterPicturePathProperty); }
            set
            {
                if (value == (string)GetValue(CurrentMonsterPicturePathProperty))
                {
                    return;
                }
                SetValue(CurrentMonsterPicturePathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMonsterPicturePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMonsterPicturePathProperty =
            DependencyProperty.Register("CurrentMonsterPicturePath", typeof(string), typeof(SceneMonsterConfigControl), new PropertyMetadata(OnCurrentMonsterPicturePathChanged));

        private static void OnCurrentMonsterPicturePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, SceneMonsterConfigControlMessageTokens.UpdateCurrentMonsterPicturePathFromView);
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
            DependencyProperty.Register("CurrentMapID", typeof(int), typeof(SceneMonsterConfigControl), new PropertyMetadata(OnCurrentMapIDChanged));

        private static void OnCurrentMapIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<int>((int)e.NewValue, SceneMonsterConfigControlMessageTokens.UpdateCurrentMapIDFromView);
        }


        public event EventHandler OK
        {
            add { AddHandler(OKEvent, value); }
            remove { RemoveHandler(OKEvent, value); }
        }

        public static readonly RoutedEvent OKEvent = EventManager.RegisterRoutedEvent(
        "OK", RoutingStrategy.Bubble, typeof(EventHandler), typeof(SceneMonsterConfigControl));


        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
        "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(SceneMonsterConfigControl));

        public SceneMonsterConfigControl()
        {
            InitializeComponent();

            Messenger.Default.Register<SceneMonsterConfigArgs>(this, SceneMonsterConfigControlMessageTokens.RaiseOKEvent, (args) =>
            {
                args.RoutedEvent = OKEvent;
                RaiseEvent(args);
            });

            Messenger.Default.Register<SceneMonsterConfigArgs>(this, SceneMonsterConfigControlMessageTokens.RaiseCancelEvent, (args) =>
            {
                args.RoutedEvent = CancelEvent;
                RaiseEvent(args);
            });

            Messenger.Default.Register<ValidateErrorHappendChecker>(this, SceneMonsterConfigControlMessageTokens.NotifyUpdateErrorHapendFromViewModel, (checker) =>
            {
                checker.Flag = ValidateCheckHelper.IsValid(this);
            });
        }
    }
}
