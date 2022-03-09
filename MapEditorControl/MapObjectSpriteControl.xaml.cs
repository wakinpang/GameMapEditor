using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using MapEditorControl.ViewModel;
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
    /// MapObjectSpriteControl.xaml 的交互逻辑
    /// </summary>
    public partial class MapObjectSpriteControl : UserControl
    {

        private Messenger UniqueMessenger { get; set; }

        public MapObjectSpriteControl()
        {
            InitializeComponent();

            UniqueMessenger = this.Resources["UniqueMessenger"] as Messenger;
            var viewModel = this.Resources["UniqueViewModel"] as MapObjectSpriteViewModel;
            viewModel.UniqueMessenger = UniqueMessenger;
            viewModel.InitializeMessenger();

            UniqueMessenger.Register<double>(this, MapObjectSpriteMessageTokens.UpdateXFromViewModel, (x) =>
            {
                X = x;
            });

            UniqueMessenger.Register<double>(this, MapObjectSpriteMessageTokens.UpdateYFromViewModel, (y) =>
            {
                Y = y;
            });

        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set
            {
                if (value == (double)GetValue(XProperty))
                {
                    return;
                }

                SetValue(XProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(MapObjectSpriteControl), new PropertyMetadata(OnXChanged));

        private static void OnXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var con = d as MapObjectSpriteControl;
            con.UniqueMessenger.Send<double>((double)e.NewValue, MapObjectSpriteMessageTokens.UpdateXFromView);
            con.Margin = new Thickness()
            {
                Left = con.X * con.Zoom,
                Top = con.Y * con.Zoom,
            };
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set
            {
                if (value == (double)GetValue(YProperty))
                {
                    return;
                }

                SetValue(YProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(MapObjectSpriteControl), new PropertyMetadata(OnYChanged));

        private static void OnYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var con = d as MapObjectSpriteControl;
            con.UniqueMessenger.Send<double>((double)e.NewValue, MapObjectSpriteMessageTokens.UpdateYFromView);
            con.Margin = new Thickness()
            {
                Left = con.X * con.Zoom,
                Top = con.Y * con.Zoom
            };
        }

        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set
            {
                if (value == (int)GetValue(IDProperty))
                {
                    return;
                }
                SetValue(IDProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(int), typeof(MapObjectSpriteControl), new PropertyMetadata(OnIDChanged));

        private static void OnIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MapObjectSpriteControl).UniqueMessenger.Send<int>((int)e.NewValue, MapObjectSpriteMessageTokens.UpdateIDFromView);
        }


        public string SpriteName
        {
            get { return (string)GetValue(NameProperty); }
            set
            {
                if (value == (string)GetValue(NameProperty))
                {
                    return;
                }
                SetValue(NameProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpriteNameProperty =
            DependencyProperty.Register("SpriteName", typeof(string), typeof(MapObjectSpriteControl), new PropertyMetadata(OnNameChanged));

        private static void OnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MapObjectSpriteControl).UniqueMessenger.Send<string>(e.NewValue as string, MapObjectSpriteMessageTokens.UpdateNameFromView);
        }

        public BitmapImage SpriteImageSource
        {
            get { return (BitmapImage)GetValue(SpriteImageSourceProperty); }
            set
            {
                if (value == (BitmapImage)GetValue(SpriteImageSourceProperty))
                {
                    return;
                }
                SetValue(SpriteImageSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SpriteImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpriteImageSourceProperty =
            DependencyProperty.Register("SpriteImageSource", typeof(BitmapImage), typeof(MapObjectSpriteControl), new PropertyMetadata(OnSpriteImageSourceChanged));

        private static void OnSpriteImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var con = d as MapObjectSpriteControl;

            con.UniqueMessenger.Send<BitmapImage>(e.NewValue as BitmapImage, MapObjectSpriteMessageTokens.UpdateSpriteImageSourceFromView);
            con.wrap_Canvas.Width = con.SpriteImageSource.PixelWidth * con.Zoom;
            con.wrap_Canvas.Height = con.SpriteImageSource.PixelHeight * con.Zoom;
        }

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set
            {
                if (value == (double)GetValue(ZoomProperty))
                {
                    return;
                }
                SetValue(ZoomProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Zoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(MapObjectSpriteControl), new PropertyMetadata(OnZoomChanged));

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var con = d as MapObjectSpriteControl;

            con.wrap_Canvas.Width = con.SpriteImageSource.PixelWidth * con.Zoom;
            con.wrap_Canvas.Height = con.SpriteImageSource.PixelHeight * con.Zoom;

            con.Margin = new Thickness()
            {
                Left = con.X * con.Zoom,
                Top = con.Y * con.Zoom
            };
        }


        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set
            {
                if (value == (bool)GetValue(SelectedProperty))
                {
                    return;
                }
                SetValue(SelectedProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(bool), typeof(MapObjectSpriteControl), new PropertyMetadata(OnSelectedChanged));

        private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var con = d as MapObjectSpriteControl;
            //con.border.BorderBrush = Brushes.White;
            con.UniqueMessenger.Send<bool>((bool)e.NewValue, MapObjectSpriteMessageTokens.UpdateSelectedFromView);
        }

        public MapObjectSprite SpriteObject { get; set; }

    }
}
