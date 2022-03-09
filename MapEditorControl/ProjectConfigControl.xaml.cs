using DatabaseOperate;
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
    /// ProjectConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectConfigControl : UserControl
    {

        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameProperty); }
            set
            {
                if (value == (string)GetValue(ProjectNameProperty))
                {
                    return;
                }
                SetValue(ProjectNameProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ProjectName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectNameProperty =
            DependencyProperty.Register("ProjectName", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnProjectNameChanged));

        private static void OnProjectNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateProjectNameFromView);
        }

        public string ProjectPath
        {
            get { return (string)GetValue(ProjectPathProperty); }
            set
            {
                if (value == (string)GetValue(ProjectPathProperty))
                {
                    return;
                }
                SetValue(ProjectPathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ProjectPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectPathProperty =
            DependencyProperty.Register("ProjectPath", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnProjectPathChanged));

        private static void OnProjectPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateProjectPathFromView);
        }

        public string DatabaseIP
        {
            get { return (string)GetValue(DatabaseIPProperty); }
            set
            {
                if (value == (string)GetValue(DatabaseIPProperty))
                {
                    return;
                }
                SetValue(DatabaseIPProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DatabaseIP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabaseIPProperty =
            DependencyProperty.Register("DatabaseIP", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnDatabaseIPChanged));

        private static void OnDatabaseIPChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateDatabaseIPFromView);
        }

        public string DatabasePort
        {
            get { return (string)GetValue(DatabasePortProperty); }
            set
            {
                if (value == (string)GetValue(DatabasePortProperty))
                {
                    return;
                }
                SetValue(DatabasePortProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DatabasePort.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabasePortProperty =
            DependencyProperty.Register("DatabasePort", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnDatabasePortChanged));

        private static void OnDatabasePortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>((string)e.NewValue, ProjectConfigControlMessageTokens.UpdateDatabasePortFromView);
        }

        public string DatabaseName
        {
            get { return (string)GetValue(DatabaseNameProperty); }
            set
            {
                if (value == (string)GetValue(DatabaseNameProperty))
                {
                    return;
                }
                SetValue(DatabaseNameProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DatabaseName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabaseNameProperty =
            DependencyProperty.Register("DatabaseName", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnDatabaseNameChanged));

        private static void OnDatabaseNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateDatabaseNameFromView);
        }

        public string DatabaseUserName
        {
            get { return (string)GetValue(DatabaseUserNameProperty); }
            set
            {
                if (value == (string)GetValue(DatabaseUserNameProperty))
                {
                    return;
                }
                SetValue(DatabaseUserNameProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DatabaseUserName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabaseUserNameProperty =
            DependencyProperty.Register("DatabaseUserName", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnDatabaseUserNameChanged));

        private static void OnDatabaseUserNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateDatabaseUserNameFromView);
        }

        public string DatabasePassword
        {
            get { return (string)GetValue(DatabasePasswordProperty); }
            set
            {
                if (value == (string)GetValue(DatabasePasswordProperty))
                {
                    return;
                }
                SetValue(DatabasePasswordProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DatabasePassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabasePasswordProperty =
            DependencyProperty.Register("DatabasePassword", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnDatabasePasswordChanged));

        private static void OnDatabasePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateDatabasePasswordFromView);
        }

        public string MapSourcePath
        {
            get { return (string)GetValue(MapSourcePathProperty); }
            set
            {
                if (value == (string)GetValue(MapSourcePathProperty))
                {
                    return;
                }
                SetValue(MapSourcePathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MapSourcePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapSourcePathProperty =
            DependencyProperty.Register("MapSourcePath", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnMapSourcePathChanged));

        private static void OnMapSourcePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateMapSourcePathFromView);
        }

        public string MapSourceOutputPath
        {
            get { return (string)GetValue(MapSourceOutputPathProperty); }
            set
            {
                if (value == (string)GetValue(MapSourceOutputPathProperty))
                {
                    return;
                }
                SetValue(MapSourceOutputPathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MapSourceOutputPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapSourceOutputPathProperty =
            DependencyProperty.Register("MapSourceOutputPath", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnMapSourceOutputPathChanged));

        private static void OnMapSourceOutputPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateMapOutputSourcePathFromView);
        }

        public string NpcPicturePath
        {
            get { return (string)GetValue(NpcPicturePathProperty); }
            set
            {
                if (value == (string)GetValue(NpcPicturePathProperty))
                {
                    return;
                }
                SetValue(NpcPicturePathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for NpcPicturePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NpcPicturePathProperty =
            DependencyProperty.Register("NpcPicturePath", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnNpcPicturePathChanged));

        private static void OnNpcPicturePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateNpcPicturePathFromView);
        }

        public string MonsterPicturePath
        {
            get { return (string)GetValue(MonsterPicturePathProperty); }
            set
            {
                if (value == (string)GetValue(MonsterPicturePathProperty))
                {
                    return;
                }
                SetValue(MonsterPicturePathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MonsterPicturePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MonsterPicturePathProperty =
            DependencyProperty.Register("MonsterPicturePath", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnMonsterPicturePathChanged));

        private static void OnMonsterPicturePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateMonsterPathFromView);
        }

        public string MapSoundPath
        {
            get { return (string)GetValue(MapSoundPathProperty); }
            set
            {
                if (value == (string)GetValue(MapSoundPathProperty))
                {
                    return;
                }
                SetValue(MapSoundPathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MapSoundPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapSoundPathProperty =
            DependencyProperty.Register("MapSoundPath", typeof(string), typeof(ProjectConfigControl), new PropertyMetadata(OnMapSoundPathChanged));

        private static void OnMapSoundPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<string>(e.NewValue as string, ProjectConfigControlMessageTokens.UpdateMapSoundPathFromView);
        }

        public event EventHandler OK
        {
            add { AddHandler(OKEvent, value); }
            remove { RemoveHandler(OKEvent, value); }
        }

        public static readonly RoutedEvent OKEvent = EventManager.RegisterRoutedEvent(
        "OK", RoutingStrategy.Bubble, typeof(EventHandler), typeof(ProjectConfigControl));


        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
        "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(ProjectConfigControl));

        //public event EventHandler TestSuccess
        //{
        //    add { AddHandler(TestSuccessEvent, value); }
        //    remove { RemoveHandler(TestSuccessEvent, value); }
        //}

        //public static readonly RoutedEvent TestSuccessEvent = EventManager.RegisterRoutedEvent(
        //"TestSuccess", RoutingStrategy.Bubble, typeof(EventHandler), typeof(ProjectConfigControl));


        //public event EventHandler TestFailed
        //{
        //    add { AddHandler(TestFailedEvent, value); }
        //    remove { RemoveHandler(TestFailedEvent, value); }
        //}

        //public static readonly RoutedEvent TestFailedEvent = EventManager.RegisterRoutedEvent(
        //"TestFailed", RoutingStrategy.Bubble, typeof(EventHandler), typeof(ProjectConfigControl));


        public event EventHandler ConnectTest
        {
            add { AddHandler(ConnectTestEvent, value); }
            remove { RemoveHandler(ConnectTestEvent, value); }
        }

        public static readonly RoutedEvent ConnectTestEvent = EventManager.RegisterRoutedEvent(
        "ConnectTest", RoutingStrategy.Bubble, typeof(EventHandler), typeof(ProjectConfigControl));


        public ProjectConfigControl()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateProjectNameFromViewModel, (msg) =>
            {
                ProjectName = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateProjectPathFromViewModel, (msg) =>
            {
                ProjectPath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabaseIPFromViewModel, (msg) =>
            {
                DatabaseIP = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabasePortFromViewModel, (msg) =>
            {
                DatabasePort = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabaseNameFromViewModel, (msg) =>
            {
                DatabaseName = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabaseUserNameFromViewModel, (msg) =>
            {
                DatabaseUserName = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabasePasswordFromViewModel, (msg) =>
            {
                DatabasePassword = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMapSourcePathFromViewModel, (msg) =>
            {
                MapSourcePath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMapOutputSourcePathFromViewModel, (msg) =>
            {
                MapSourceOutputPath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateNpcPicturePathFromViewModel, (msg) =>
            {
                NpcPicturePath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMonsterPathFromViewModel, (msg) =>
            {
                MonsterPicturePath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMapSoundPathFromViewModel, (msg) =>
            {
                MapSoundPath = msg;
            });

            Messenger.Default.Register<object>(this, ProjectConfigControlMessageTokens.OKEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(OKEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, ProjectConfigControlMessageTokens.CancelEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(CancelEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, ProjectConfigControlMessageTokens.TestConnectEventFromViewModel, (dummy) =>
            {
                //if(DatabaseUtil.TryConn(DatabaseIP, DatabaseName, DatabaseUserName, DatabasePassword, DatabasePort))
                //{
                //    RoutedEventArgs newEventArgs = new RoutedEventArgs(TestSuccessEvent);
                //    RaiseEvent(newEventArgs);
                //}
                //else
                //{
                //    RoutedEventArgs newEventArgs = new RoutedEventArgs(TestFailedEvent);
                //    RaiseEvent(newEventArgs);
                //}

                RoutedEventArgs newEventArgs = new RoutedEventArgs(ConnectTestEvent);
                RaiseEvent(newEventArgs);
            });

        }
    }
}
