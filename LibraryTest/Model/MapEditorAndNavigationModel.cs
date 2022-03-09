using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.Model
{
    public class MapEditorAndNavigationModel : ObservableObject
    {
        private static MapEditorAndNavigationModel _instance = new MapEditorAndNavigationModel();

        public static MapEditorAndNavigationModel Instance()
        {
            return _instance;
        }

        private double _zoom = 1.0;

        public double Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                RaisePropertyChanged(() => Zoom);
            }
        }

        private String _backgroundSource = "";

        public String BackgroundSource
        {
            get { return _backgroundSource; }
            set
            {
                _backgroundSource = value;
                RaisePropertyChanged(() => BackgroundSource);
            }
        }

        private double _contentWidthRatio = 0.0;

        public double ContentWidthRatio
        {
            get { return _contentWidthRatio; }
            set
            {
                _contentWidthRatio = value;
                RaisePropertyChanged(() => ContentWidthRatio);
            }
        }

        private double _contentHeightRatio = 0.0;

        public double ContentHeightRatio
        {
            get { return _contentHeightRatio; }
            set
            {
                _contentHeightRatio = value;
                RaisePropertyChanged(() => ContentHeightRatio);
            }
        }

        private double _contentVerticalOffsetRatio = 0.0;

        public double ContentVerticalOffsetRatio
        {
            get { return _contentVerticalOffsetRatio; }
            set
            {
                _contentVerticalOffsetRatio = value;
                RaisePropertyChanged(() => ContentVerticalOffsetRatio);
            }
        }

        private double _contentHorizentalOffsetRatio = 0.0;

        public double ContentHorizentalOffsetRatio
        {
            get { return _contentHorizentalOffsetRatio; }
            set
            {
                _contentHorizentalOffsetRatio = value;
                RaisePropertyChanged(() => ContentHorizentalOffsetRatio);
            }
        }

        private int _tileWidth = 25;

        public int TileWidth
        {
            get { return _tileWidth; }
            set
            {
                _tileWidth = value;
                RaisePropertyChanged(() => TileWidth);
            }
        }

        private int _tileHeight = 25;

        public int TileHeight 
        {
            get { return _tileHeight; }
            set
            {
                _tileHeight = value;
                RaisePropertyChanged(() => TileHeight);
            }
        }


    }
}
