using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.Model
{
    public class ToolBarModel : ObservableObject
    {
        public static readonly ToolBarModel INSTANCE = new ToolBarModel();

        private bool _dragToolSelected;

        public bool DragToolSelected
        {
            get { return _dragToolSelected; }
            set
            {
                if (value == _dragToolSelected)
                {
                    return;
                }
                _dragToolSelected = value;
                RaisePropertyChanged(() => DragToolSelected);
            }
        }

        private bool _areaToolSelected;

        public bool AreaToolSelected
        {
            get { return _areaToolSelected; }
            set
            {
                if (value == _areaToolSelected)
                {
                    return;
                }
                _areaToolSelected = value;
                RaisePropertyChanged(() => AreaToolSelected);
            }
        }

        private bool _penToolSelected;

        public bool PenToolSelected
        {
            get { return _penToolSelected; }
            set
            {
                if (value == _penToolSelected)
                {
                    return;
                }
                _penToolSelected = value;
                RaisePropertyChanged(() => PenToolSelected);
            }
        }

        private bool _pointToolSelected;

        public bool PointToolSelected
        {
            get { return _pointToolSelected; }
            set
            {
                if (value == _pointToolSelected)
                {
                    return;
                }
                _pointToolSelected = value;
                RaisePropertyChanged(() => PointToolSelected);
            }
        }

        private bool _transparentSelected;

        public bool TransparentSelected
        {
            get { return _transparentSelected; }
            set
            {
                if (value == _transparentSelected)
                {
                    return;
                }
                _transparentSelected = value;
                RaisePropertyChanged(() => TransparentSelected);
            }
        }

        private bool _safety;

        public bool Safety
        {
            get { return _safety; }
            set
            {
                if (value == _safety)
                {
                    return;
                }
                _safety = value;
                RaisePropertyChanged(() => Safety);
            }
        }

        private bool _fishing;

        public bool Fishing
        {
            get { return _fishing; }
            set
            {
                if (value == _fishing)
                {
                    return;
                }
                _fishing = value;
                RaisePropertyChanged(() => Fishing);
            }
        }
        
        private bool _canEdit;

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                if (value == _canEdit)
                {
                    return;
                }
                _canEdit = value;
                RaisePropertyChanged(() => CanEdit);
            }
        }

    }
}
