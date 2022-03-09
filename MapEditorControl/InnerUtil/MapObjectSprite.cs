using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapEditorControl.InnerUtil
{
    public class MapObjectSprite : ObservableObject
    {
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
                RaisePropertyChanged(() => Y);
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
                RaisePropertyChanged(() => Name);
            }
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
                RaisePropertyChanged(() => ID);
            }
        }

        private string _spriteImageSource;

        public string SpriteImage
        {
            get { return _spriteImageSource; }
            set
            {
                if (value == _spriteImageSource)
                {
                    return;
                }
                _spriteImageSource = value;
                RaisePropertyChanged(() => SpriteImage);
            }
        }

        private string _folderPath;

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                if (value == _folderPath)
                {
                    return;
                }
                _folderPath = value;
                RaisePropertyChanged(() => FolderPath);
            }
        }



        public MapObjectSpriteControl SpriteControl { get; set; }

        public IBaseObjectBase ObjectBase { get; set; }
        public ISceneObjectBase SceneObjectBase { get; set; }


        public MapObjectSprite()
        {

        }

    }
}
