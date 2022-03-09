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
    /// ToolBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBarControl : UserControl
    {
        public bool DragToolSelected
        {
            get { return (bool)GetValue(DragToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(DragToolSelectedProperty))
                {
                    return;
                }
                SetValue(DragToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for DragToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragToolSelectedProperty =
            DependencyProperty.Register("DragToolSelected", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());

        public bool AreaToolSelected
        {
            get { return (bool)GetValue(AreaToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(AreaToolSelectedProperty))
                {
                    return;
                }
                SetValue(AreaToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for AreaToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaToolSelectedProperty =
            DependencyProperty.Register("AreaToolSelected", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());

        public bool PenToolSelected
        {
            get { return (bool)GetValue(PenToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(PenToolSelectedProperty))
                {
                    return;
                }
                SetValue(PenToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PenToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PenToolSelectedProperty =
            DependencyProperty.Register("PenToolSelected", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());

        public bool PointToolSelected
        {
            get { return (bool)GetValue(PointToolSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(PointToolSelectedProperty))
                {
                    return;
                }
                SetValue(PointToolSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PointToolSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointToolSelectedProperty =
            DependencyProperty.Register("PointToolSelected", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());

        public bool TransparentSelected
        {
            get { return (bool)GetValue(TransparentSelectedProperty); }
            set
            {
                if (value == (bool)GetValue(TransparentSelectedProperty))
                {
                    return;
                }
                SetValue(TransparentSelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for TransparentSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransparentSelectedProperty =
            DependencyProperty.Register("TransparentSelected", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());


        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set
            {
                if (value == (bool)GetValue(CanEditProperty))
                {
                    return;
                }
                SetValue(CanEditProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(ToolBarControl), new PropertyMetadata(OnCanEditChanged));

        private static void OnCanEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, ToolBarControlMessageTokens.UpdateCanEditFromView);
        }


        public bool ProjectValid
        {
            get { return (bool)GetValue(ProjectValidProperty); }
            set
            {
                if (value == (bool)GetValue(ProjectValidProperty))
                {
                    return;
                }
                SetValue(ProjectValidProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ProjectValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectValidProperty =
            DependencyProperty.Register("ProjectValid", typeof(bool), typeof(ToolBarControl), new PropertyMetadata(OnProjectValidChanged));

        private static void OnProjectValidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<bool>((bool)e.NewValue, ToolBarControlMessageTokens.UpdateProjectValidFromView);
        }


        public bool Safety
        {
            get { return (bool)GetValue(SafetyProperty); }
            set
            {
                if (value == (bool)GetValue(SafetyProperty))
                {
                    return;
                }
                SetValue(SafetyProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Safety.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SafetyProperty =
            DependencyProperty.Register("Safety", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());


        public bool Fishing
        {
            get { return (bool)GetValue(FishingProperty); }
            set
            {
                if (value == (bool)GetValue(FishingProperty))
                {
                    return;
                }
                SetValue(FishingProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Fishing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FishingProperty =
            DependencyProperty.Register("Fishing", typeof(bool), typeof(ToolBarControl), new PropertyMetadata());

        public ToolBarControl()
        {
            InitializeComponent();

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateDragToolSelectedFromViewModel, (v) =>
            {
               DragToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateAreaToolSelectedFromViewModel, (v) =>
            {
                AreaToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdatePointToolSelectedFromViewModel, (v) =>
            {
                PointToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdatePenToolSelectedFromViewModel, (v) =>
            {
                PenToolSelected = v;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateTransparentSelectedFromViewModel, (v) =>
            {
                TransparentSelected = v;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateSafetyFromViewModel, (v) =>
            {
                Safety = v;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateFishingFromViewModel, (v) =>
            {
                Fishing = v;
            });

            Messenger.Default.Register<object>(this, ToolBarControlMessageTokens.SyncHandlerFromViewModel, (dummy) =>
            {
                var newEventArgs = new RoutedEventArgs(SyncEvent);
                RaiseEvent(newEventArgs);
            });

        }

        public event EventHandler Sync
        {
            add { AddHandler(SyncEvent, value); }
            remove { RemoveHandler(SyncEvent, value); }
        }

        public static readonly RoutedEvent SyncEvent = EventManager.RegisterRoutedEvent(
        "Sync", RoutingStrategy.Bubble, typeof(EventHandler), typeof(ToolBarControl));



    }
}
