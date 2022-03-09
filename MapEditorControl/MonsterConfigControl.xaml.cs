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
using System.Collections.ObjectModel;
using MapEditorControl.InnerUtil;
using GalaSoft.MvvmLight.Messaging;

namespace MapEditorControl
{
    /// <summary>
    /// MonsterConfigControl.xaml 的交互逻辑
    /// </summary>
    public partial class MonsterConfigControl : UserControl
    {
        public MonsterSection CurrentMonsterData {
            get { return (MonsterSection)GetValue(CurrentMonsterDataProperty); }
            set {
                if (value == GetValue(CurrentMonsterDataProperty)) {
                    return;
                }
                SetValue(CurrentMonsterDataProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentMonsterDataProperty = DependencyProperty.Register(
            "CurrentMonsterData", typeof(MonsterSection), typeof(MonsterConfigControl), new PropertyMetadata(OnCurrentMonsterChanged));

        private static void OnCurrentMonsterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as MonsterConfigControl;

            Messenger.Default.Send<MonsterSection>(obj.CurrentMonsterData, MonsterConfigControlMessageTokens.CurrentMonsterChangedFriomModel);
        }

        public ObservableCollection<MonsterSection> MonsterCollectionData {
            get { return (ObservableCollection<MonsterSection>)GetValue(MonsterCollectionDataProperty); }
            set {
                if (value == GetValue(MonsterCollectionDataProperty)) {
                    return;
                }
                SetValue(MonsterCollectionDataProperty, value);
            }
        }

        public static readonly DependencyProperty MonsterCollectionDataProperty = DependencyProperty.Register(
            "MonsterCollectionData", typeof(ObservableCollection<MonsterSection>), typeof(MonsterConfigControl), new PropertyMetadata(OnMonsterCollectionDataChanged));

        private static void OnMonsterCollectionDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = d as MonsterConfigControl;

            Messenger.Default.Send<ObservableCollection<MonsterSection>>(obj.MonsterCollectionData, MonsterConfigControlMessageTokens.MonsterCollectionChangedFromModel);
        }

        public event EventHandler OK
        {
            add { AddHandler(OKEvent, value); }
            remove { RemoveHandler(OKEvent, value); }
        }

        public static readonly RoutedEvent OKEvent = EventManager.RegisterRoutedEvent(
            "OK", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MonsterConfigControl));

        public event EventHandler Cancel
        {
            add { AddHandler(CancelEvent, value); }
            remove { RemoveHandler(CancelEvent, value); }
        }

        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent(
            "Cancel", RoutingStrategy.Bubble, typeof(EventHandler), typeof(MonsterConfigControl));

        public MonsterConfigControl()
        {
            InitializeComponent();

            Messenger.Default.Register<MonsterConfigArgs>(this, MonsterConfigControlMessageTokens.MonsterOkEventFromView, (args) =>
            {
                args.RoutedEvent = OKEvent;
                RaiseEvent(args);
            });

            Messenger.Default.Register<MonsterConfigArgs>(this, MonsterConfigControlMessageTokens.HideDailogFromView, (args) =>
            {
                args.RoutedEvent = CancelEvent;
                RaiseEvent(args);
            });

            Messenger.Default.Register<MonsterSection>(this, MonsterConfigControlMessageTokens.CurrentMonsterChangedFromView, (section) =>
            {
                CurrentMonsterData = section;
            });

            Messenger.Default.Register<ValidateErrorHappendChecker>(this, MonsterConfigControlMessageTokens.NotifyUpdateErrorHapendFromViewModel, (checker) =>
            {
                checker.Flag = ValidateCheckHelper.IsValid(this);
            });
        }
    }
}
