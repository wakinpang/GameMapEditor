using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapEditorControl.ViewModel
{

    public class MapObjectSpriteViewModel : ViewModelBase
    {

        public Messenger UniqueMessenger { get; set; }

        public MapObjectSpriteViewModel()
        {
        }

        public void InitializeMessenger()
        {
            UniqueMessenger.Register<int>(this, MapObjectSpriteMessageTokens.UpdateIDFromView, (id) =>
            {
                ID = id;
            });

            UniqueMessenger.Register<int>(this, MapObjectSpriteMessageTokens.UpdateXFromView, (x) =>
            {
                X = x;
            });

            UniqueMessenger.Register<int>(this, MapObjectSpriteMessageTokens.UpdateYFromView, (y) =>
            {
                Y = y;
            });

            UniqueMessenger.Register<string>(this, MapObjectSpriteMessageTokens.UpdateNameFromView, (name) =>
            {
                Name = name;
            });

            UniqueMessenger.Register<BitmapImage>(this, MapObjectSpriteMessageTokens.UpdateSpriteImageSourceFromView, (source) =>
            {
                SpriteImage = source;
            });

            UniqueMessenger.Register<bool>(this, MapObjectSpriteMessageTokens.UpdateSelectedFromView, (selected) =>
            {
                Selected = selected;
            });
        }


        private int _id;

        public int ID
        {
            get { return _id; }
            set
            {
                if (value == _id)
                {
                    return;
                }
                _id = value;

                //UniqueMessenger.Send<int>(value, MapObjectSpriteMessageTokens.UpdateIDFromViewModel);

                SpriteText = String.Format("{0} : {1}", ID, Name);

                RaisePropertyChanged(() => ID);
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;

                //UniqueMessenger.Send<string>(value, MapObjectSpriteMessageTokens.UpdateNameFromViewModel);

                SpriteText = String.Format("{0} : {1}", ID, Name);

                RaisePropertyChanged(() => Name);
            }
        }

        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                if (value == _x)
                {
                    return;
                }
                _x = value;

                UniqueMessenger.Send<double>(value, MapObjectSpriteMessageTokens.UpdateXFromViewModel);

                RaisePropertyChanged(() => X);
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                if (value == _y)
                {
                    return;
                }
                _y = value;

                UniqueMessenger.Send<double>(value, MapObjectSpriteMessageTokens.UpdateYFromViewModel);

                RaisePropertyChanged(() => Y);
            }
        }

        private BitmapImage _spriteImageSource;

        public BitmapImage SpriteImage
        {
            get { return _spriteImageSource; }
            set
            {
                if (value == _spriteImageSource)
                {
                    return;
                }
                _spriteImageSource = value;

                //UniqueMessenger.Send<BitmapImage>(value, MapObjectSpriteMessageTokens.UpdateSpriteImageSourceFromViewModel);

                RaisePropertyChanged(() => SpriteImage);
            }
        }

        private string _spriteText;

        public string SpriteText
        {
            get { return _spriteText; }
            set
            {
                if (value == _spriteText)
                {
                    return;
                }
                _spriteText = value;
                RaisePropertyChanged(() => SpriteText);
            }
        }

        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (value == _selected)
                {
                    return;
                }
                _selected = value;
                RaisePropertyChanged(() => Selected);
            }
        }



    }
}
