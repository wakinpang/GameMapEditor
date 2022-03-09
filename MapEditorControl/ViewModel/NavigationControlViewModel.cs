using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MapEditorControl.InnerUtil;
using System.IO;
using System.Windows.Media;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace MapEditorControl.ViewModel
{
    public class NavigationControlViewModel : ViewModelBase
    {
        private int _backgroundPixelHeight;

        public int BackgroundPixelHeight
        {
            get { return _backgroundPixelHeight; }
            set
            {
                if (value == _backgroundPixelHeight)
                {
                    return;
                }
                _backgroundPixelHeight = value;
                RaisePropertyChanged(() => BackgroundPixelHeight);
            }
        }

        private int _backgroundPixelWidth;

        public int BackgroundPixelWidth
        {
            get { return _backgroundPixelWidth; }
            set
            {
                if (value == _backgroundPixelWidth)
                {
                    return;
                }
                _backgroundPixelWidth = value;
                RaisePropertyChanged(() => BackgroundPixelWidth);
            }
        }

        private double _zoom = 1.0;

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                if (value == _zoom)
                {
                    return;
                }
                _zoom = value;

                Messenger.Default.Send<double>(value, NavigationControlMessageTokens.UpdateZoomFromViewModel);

                RaisePropertyChanged(() => Zoom);
            }
        }

        private BitmapImage _backgroundImage;

        public BitmapImage BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                if (value == _backgroundImage)
                {
                    return;
                }
                _backgroundImage = value;
                RaisePropertyChanged(() => BackgroundImage);
            }
        }

        private bool _vaild;

        public bool Vaild
        {
            get { return _vaild; }
            set
            {
                if (value == _vaild)
                {
                    return;
                }
                _vaild = value;

                Messenger.Default.Send<bool>(value, NavigationControlMessageTokens.UpdateVaildFromViewModel);

                RaisePropertyChanged(() => Vaild);
            }
        }

        public RelayCommand<Point> LeftMouseDownHandler { get; set; }
        public RelayCommand LeftMouseUpHandler { get; set; }
        public RelayCommand<Point> MouseMoveHandler { get; set; }
        public RelayCommand MouseLeaveHandler { get; set; }

        private bool _isDrag = false;

        public NavigationControlViewModel()
        {
            Messenger.Default.Register<string>(this, NavigationControlMessageTokens.UpdateBackgroudMapFromView, (path) =>
            {

                Vaild = false;
                if (!File.Exists(path))
                {
                    BackgroundImage = null;
                    return;
                }

                var url = new Uri(path, UriKind.RelativeOrAbsolute);
                var bitmap = new BitmapImage();
                bool loaded = true;
                bitmap.BeginInit();
                bitmap.UriSource = url;

                bitmap.DecodeFailed += (object sender, ExceptionEventArgs ec) =>
                {
                    loaded = false;
                };
                bitmap.EndInit();

                if (!loaded)
                {
                    BackgroundImage = null;
                    return;
                }

                this.BackgroundImage = bitmap;
                Vaild = true;

                Messenger.Default.Send<MessageParameterNavigationDisplay>(new MessageParameterNavigationDisplay()
                {
                    WidthRatio = 0.0,
                    HeightRatio = 0.0,
                    PixelHeight = bitmap.PixelHeight,
                    PixelWidth = bitmap.PixelWidth,
                    Zoom = Zoom,
                }, NavigationControlMessageTokens.UpdateBackgroundShowFromViewModel);

            });
            Messenger.Default.Register<double>(this, NavigationControlMessageTokens.UpdateZoomFromView, (zoom) =>
            {
                Messenger.Default.Send<object>(null, NavigationControlMessageTokens.AdjustAreaFromViewModel);
            });
            Messenger.Default.Register<Size>(this, NavigationControlMessageTokens.ArrangedFromView, (size) =>
            {
                Messenger.Default.Send<Size>(size, NavigationControlMessageTokens.DoArrangeFromViewModel);
            });
            Messenger.Default.Register<object>(this, NavigationControlMessageTokens.UpdateContentRatioFromView, (par) =>
            {
                Messenger.Default.Send<object>(null, NavigationControlMessageTokens.AdjustAreaFromViewModel);
            });

            Messenger.Default.Register<object>(this, NavigationControlMessageTokens.UpdateContentScrollRatioFromView, (obj) =>
            {
                Messenger.Default.Send<object>(this, NavigationControlMessageTokens.UpdateContentRatioFromViewModel);
            });

            LeftMouseDownHandler = new RelayCommand<Point>((pos) => {

                _isDrag = true;
                Messenger.Default.Send<Point>(pos, NavigationControlMessageTokens.AdjustAreaWithDragFromViewModel);
            });

            LeftMouseUpHandler = new RelayCommand(() =>
            {
                _isDrag = false;
            });

            MouseLeaveHandler = new RelayCommand(() =>
            {
                _isDrag = false;
            });

            MouseMoveHandler = new RelayCommand<Point>((pos) =>
            {
                if (_isDrag)
                {
                    Messenger.Default.Send<Point>(pos, NavigationControlMessageTokens.AdjustAreaWithDragFromViewModel);
                }
            });

        }

    }
}
