using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using MapEditorControl.InnerUtil;

namespace MapEditorControl
{
    /// <summary>
    /// NavigationControl.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class NavigationControl : UserControl
    {

        private double _width = 0.0;
        private double _height = 0.0;

        private int _pixelWidth = 0;
        private int _pixelHeight = 0;

        private bool _vaild = false;

        private Rectangle _areaRect = new Rectangle()
        {
            Stroke = Brushes.Red,
            StrokeThickness = 2,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
        };

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set
            {
                if(value == (double)GetValue(ZoomProperty))
                {
                    return;
                }
                SetValue(ZoomProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Zoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(NavigationControl), new PropertyMetadata(1.0, OnZoomChanged));

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<double>((double)e.NewValue, NavigationControlMessageTokens.UpdateZoomFromView);
        }

        public String BackgroundSource
        {
            get { return (String)GetValue(BackgroundSourceProperty); }
            set
            {
                if(value == (String)GetValue(BackgroundSourceProperty))
                {
                    return;
                }
                SetValue(BackgroundSourceProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for BackgroundImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundSourceProperty =
            DependencyProperty.Register("BackgroundSource", typeof(String), typeof(NavigationControl), new PropertyMetadata(OnBitmapSourceChanged));

        private static void OnBitmapSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var control = d as NavigationControl;

            Messenger.Default.Send<string>(control.BackgroundSource, NavigationControlMessageTokens.UpdateBackgroudMapFromView);

        }

        public NavigationControl()
        {
            InitializeComponent();

            Messenger.Default.Register<MessageParameterNavigationDisplay>(this, NavigationControlMessageTokens.UpdateBackgroundShowFromViewModel, (param) =>
            {
                if (_vaild) { 
                    this._pixelWidth = param.PixelWidth;
                    this._pixelHeight = param.PixelHeight;

                    this._height = this._width * this._pixelHeight / this._pixelWidth;

                    this.canvas_Navigation.Width = this._width;
                    this.canvas_Navigation.Height = this._height;

                    RenderaAreaRectangle();
                }
            });

            Messenger.Default.Register<double>(this, NavigationControlMessageTokens.UpdateZoomFromViewModel, (para) =>
            {
                this.Zoom = para;
            });

            Messenger.Default.Register<object>(this, NavigationControlMessageTokens.AdjustAreaFromViewModel, (para) =>
            {
                if (_vaild)
                {
                    AdjustAreaReactangle(this);
                    AdjustRetanglePosition(this);
                }
            });

            Messenger.Default.Register<Size>(this, NavigationControlMessageTokens.DoArrangeFromViewModel, (size) =>
            {
                if (_vaild)
                {
                    ArrangeComponents(size);
                }
                else
                {
                    this._width = size.Width;
                    this.canvas_Navigation.Width = this._width;
                }
            });

            Messenger.Default.Register<Point>(this, NavigationControlMessageTokens.AdjustAreaWithDragFromViewModel, (pos) =>
            {
                if (_vaild)
                {
                    MoveAreaRectangle(pos.X, pos.Y);
                }
            });

            Messenger.Default.Register<object>(this, NavigationControlMessageTokens.UpdateContentRatioFromViewModel, (obj) =>
            {
                if (_vaild)
                {
                    AdjustRetanglePosition(this);
                }
            });

            Messenger.Default.Register<bool>(this, NavigationControlMessageTokens.UpdateVaildFromViewModel, (vaild) =>
            {
                _vaild = vaild;

                if(!_vaild)
                {
                    _areaRect.Visibility = Visibility.Hidden;
                    this.IsEnabled = false;
                }
                else
                {
                    _areaRect.Visibility = Visibility.Visible;
                    this.IsEnabled = true;
                }
            });

        }

        private void RenderaAreaRectangle()
        {
            this.canvas_Navigation.Children.Clear();
            this.canvas_Navigation.Children.Add(this._areaRect);

            AdjustAreaReactangle(this);
            AdjustRetanglePosition(this);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var size = base.ArrangeOverride(arrangeBounds);
            Messenger.Default.Send<Size>(size, NavigationControlMessageTokens.ArrangedFromView);

            return size;
        }

        private void ArrangeComponents(Size size)
        {
            this._width = size.Width;
            this._height = this._width * this._pixelHeight / this._pixelWidth;

            this.canvas_Navigation.Width = this._width;
            this.canvas_Navigation.Height = this._height;

            AdjustAreaReactangle(this);
            AdjustRetanglePosition(this);

        }

        public double ContentWidthRatio
        {
            get { return (double)GetValue(WidthRatioProperty); }
            set
            {
                if (value == ContentWidthRatio)
                    return;
                SetValue(WidthRatioProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for WidthRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthRatioProperty =
            DependencyProperty.Register("ContentWidthRatio", typeof(double), typeof(NavigationControl), new PropertyMetadata(OnWidthRatioChanged));

        private static void OnWidthRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<object>(null, NavigationControlMessageTokens.UpdateContentRatioFromView);
        }

        public double ContentHeightRatio
        {
            get { return (double)GetValue(HeightRatioProperty); }
            set
            {
                if (value == ContentHeightRatio)
                    return;
                SetValue(HeightRatioProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for HeightRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeightRatioProperty =
            DependencyProperty.Register("ContentHeightRatio", typeof(double), typeof(NavigationControl), new PropertyMetadata(OnHeightRatioChanged));

        private static void OnHeightRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<object>(null, NavigationControlMessageTokens.UpdateContentRatioFromView);
        }

        private static void AdjustAreaReactangle(NavigationControl control)
        {
            control._areaRect.Width = control._width * control.ContentWidthRatio;
            control._areaRect.Height = control._height * control.ContentHeightRatio;

            // Workaround, Maybe a Bug
            control.canvas_Navigation.Children.Clear();
            control.canvas_Navigation.Children.Add(control._areaRect);
        }

        public double ContentVerticalOffset
        {
            get { return (double)GetValue(ContentVerticalOffsetProperty); }
            set
            {
                if(value == (double)GetValue(ContentVerticalOffsetProperty))
                {
                    return;
                }
                SetValue(ContentVerticalOffsetProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ContentVerticalOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentVerticalOffsetProperty =
            DependencyProperty.Register("ContentVerticalOffset", typeof(double), typeof(NavigationControl), new PropertyMetadata(OnContentVerticalOffsetChanged));

        private static void OnContentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<object>(null, NavigationControlMessageTokens.UpdateContentScrollRatioFromView);
        }

        public double ContentHorizentalOffset
        {
            get { return (double)GetValue(ContentHorizentalCOffsetProperty); }
            set
            {
                if(value == (double)GetValue(ContentHorizentalCOffsetProperty))
                {
                    return;
                }
                SetValue(ContentHorizentalCOffsetProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ContentHorizentalCOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHorizentalCOffsetProperty =
            DependencyProperty.Register("ContentHorizentalOffset", typeof(double), typeof(NavigationControl), new PropertyMetadata(OnContentHorizentalOffsetChanged));

        private static void OnContentHorizentalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Messenger.Default.Send<object>(null, NavigationControlMessageTokens.UpdateContentScrollRatioFromView);
        }

        private static void AdjustRetanglePosition(NavigationControl d)
        {
            var control = d as NavigationControl;
            var left = control._width * (1 - control.ContentWidthRatio) * control.ContentHorizentalOffset;
            var top = control._height * (1 - control.ContentHeightRatio) * control.ContentVerticalOffset;

            control._areaRect.Margin = new Thickness(left, top, 0, 0);
        }

        private void MoveAreaRectangle(double x, double y)
        {
            // 以 X, Y 为 rect的中心进行调整，如果超出区域则调整

            var width = this._areaRect.Width;
            var height = this._areaRect.Height;

            Rect newRect = new Rect()
            {
                X = x - width / 2,
                Y = y - height / 2,
                Width = width,
                Height = height,
            };

            // 调整
            if (newRect.X < 0)
            {
                newRect.X = 0;
            }

            if (newRect.Y < 0)
            {
                newRect.Y = 0;
            }

            var dec = newRect.X + newRect.Width - this._width;
            if (dec > 0)
            {
                newRect.X -= dec;
            }

            dec = newRect.Y + newRect.Height - this._height;
            if (dec > 0)
            {
                newRect.Y -= dec;
            }

            this._areaRect.Margin = new Thickness(newRect.X, newRect.Y, 0, 0);

            if (this._width - newRect.Width == 0)
            {
                this.ContentHorizentalOffset = 0;
            }
            else
            {
                this.ContentHorizentalOffset = newRect.X / (this._width - newRect.Width);
            }

            if (this._height - newRect.Height == 0)
            {
                this.ContentVerticalOffset = 0;
            }
            else
            {
                this.ContentVerticalOffset = newRect.Y / (this._height - newRect.Height);
            }

        }
    }
}
