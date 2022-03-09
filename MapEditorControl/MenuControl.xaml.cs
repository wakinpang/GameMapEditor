using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapEditorControl.InnerUtil;

namespace MapEditorControl
{
    /// <summary>
    /// MenuControl.xaml 的交互逻辑
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public event EventHandler NewProject
        {
            add { AddHandler(NewProjectEvent, value); }
            remove { RemoveHandler(NewProjectEvent, value); }
        }

        public static readonly RoutedEvent NewProjectEvent = EventManager.RegisterRoutedEvent(
        "NewProject", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MapEditorControl));

        public event EventHandler ProjectConfig
        {
            add { AddHandler(ProjectConfigEvent, value); }
            remove { RemoveHandler(ProjectConfigEvent, value); }
        }

        public static readonly RoutedEvent ProjectConfigEvent = EventManager.RegisterRoutedEvent(
        "ProjectConfig", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MapEditorControl));

        public event EventHandler OpenProject {
            add { AddHandler(OpenProjectEvent, value); }
            remove { RemoveHandler(OpenProjectEvent, value); }
        }

        public static readonly RoutedEvent OpenProjectEvent = EventManager.RegisterRoutedEvent(
            "OpenProject", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MapEditorControl));

        public event EventHandler OpenHistory
        {
            add { AddHandler(OpenHistoryEvent, value); }
            remove { RemoveHandler(OpenHistoryEvent, value); }
        }

        public static readonly RoutedEvent OpenHistoryEvent = EventManager.RegisterRoutedEvent(
        "OpenHistory", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MapEditorControl));


        public event EventHandler Output
        {
            add { AddHandler(OutputEvent, value); }
            remove { RemoveHandler(OutputEvent, value); }
        }

        public static readonly RoutedEvent OutputEvent = EventManager.RegisterRoutedEvent(
        "Output", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MapEditorControl));


        public event EventHandler CutMap
        {
            add { AddHandler(CutMapEvent, value); }
            remove { RemoveHandler(CutMapEvent, value); }
        }

        public static readonly RoutedEvent CutMapEvent = EventManager.RegisterRoutedEvent(
        "CutMap", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MenuControl));


        public bool ProjectExist
        {
            get { return (bool)GetValue(ProjectExistProperty); }
            set
            {
                if (value == (bool)GetValue(ProjectExistProperty))
                {
                    return;
                }
                SetValue(ProjectExistProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ProjectExist.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectExistProperty =
            DependencyProperty.Register("ProjectExist", typeof(bool), typeof(MenuControl), new PropertyMetadata(OnProjectExistChanged));


        public bool CurrentMapValid
        {
            get { return (bool)GetValue(CurrentMapValidProperty); }
            set
            {
                if (value == (bool)GetValue(CurrentMapValidProperty))
                {
                    return;
                }
                SetValue(CurrentMapValidProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CurrentMapValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMapValidProperty =
            DependencyProperty.Register("CurrentMapValid", typeof(bool), typeof(MenuControl), new PropertyMetadata(OnCurrentMapValidChanged));

        private static void OnCurrentMapValidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MenuControlMessageTokens.UpdateCurrentMapValidFromView);
        }


        private static void OnProjectExistChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, MenuControlMessageTokens.UpdateProjectExistFromView);
        }

        public ObservableCollection<HistorySection> HistoryPath
        {
            get { return (ObservableCollection<HistorySection>)GetValue(HistoryPathProperty); }
            set
            {
                if (value == (ObservableCollection<HistorySection>)GetValue(HistoryPathProperty))
                {
                    return;
                }
                SetValue(HistoryPathProperty, value);
            }
        }

        public static readonly DependencyProperty HistoryPathProperty =
            DependencyProperty.Register("HistoryPath", typeof(ObservableCollection<HistorySection>), typeof(MenuControl), new PropertyMetadata(OnHistoryPathChanged));

        private static void OnHistoryPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<ObservableCollection<HistorySection>>((ObservableCollection<HistorySection>)e.NewValue, MenuControlMessageTokens.ChangeHistorySectionFromOutside);
        }


        public MenuControl()
        {
            InitializeComponent();

            Messenger.Default.Register<object>(this, MenuControlMessageTokens.NewProjectEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(NewProjectEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, MenuControlMessageTokens.ProjectConfigEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(ProjectConfigEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, MenuControlMessageTokens.OpenProjectEventFromViewModel, (dummy) => {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(OpenProjectEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<string>(this, MenuControlMessageTokens.SelectHistoryEventFromViewModel, (path) =>
            {
                SelectedHistoryPathArgs newEventArgs = new SelectedHistoryPathArgs(OpenHistoryEvent)
                {
                    HistoryPath = path,
                };
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, MenuControlMessageTokens.OutputEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(OutputEvent);
                RaiseEvent(newEventArgs);
            });

            Messenger.Default.Register<object>(this, MenuControlMessageTokens.CutMapEventFromViewModel, (dummy) =>
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(CutMapEvent);
                RaiseEvent(newEventArgs);
            });
        }
    }
}