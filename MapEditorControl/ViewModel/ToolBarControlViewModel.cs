using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditorControl.ViewModel
{
    public class ToolBarControlViewModel : ViewModelBase
    {
        private bool _dragTool;

        public bool DragTool
        {
            get { return _dragTool; }
            set
            {
                if (value == _dragTool)
                {
                    return;
                }

                AreaTool = false;
                PenTool = false;
                PointTool = false;

                _dragTool = value;

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdateDragToolSelectedFromViewModel);

                RaisePropertyChanged(() => DragTool);
            }
        }

        private bool _areaTool;

        public bool AreaTool
        {
            get { return _areaTool; }
            set
            {
                if (value == _areaTool)
                {
                    return;
                }

                DragTool = false;
                PenTool = false;
                PointTool = false;

                _areaTool = value;

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdateAreaToolSelectedFromViewModel);

                RaisePropertyChanged(() => AreaTool);
            }
        }

        private bool _penTool;

        public bool PenTool
        {
            get { return _penTool; }
            set
            {
                if (value == _penTool)
                {
                    return;
                }

                DragTool = false;
                AreaTool = false;
                PointTool = false;

                _penTool = value;

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdatePenToolSelectedFromViewModel);

                RaisePropertyChanged(() => PenTool);
            }
        }

        private bool _pointTool;

        public bool PointTool
        {
            get { return _pointTool; }
            set
            {
                if (value == _pointTool)
                {
                    return;
                }

                DragTool = false;
                AreaTool = false;
                PenTool = false;

                _pointTool = value;

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdatePointToolSelectedFromViewModel);

                RaisePropertyChanged(() => PointTool);
            }
        }

        private bool _transparent;

        public bool Transparent
        {
            get { return _transparent; }
            set
            {
                if (value == _transparent)
                {
                    return;
                }
                _transparent = value;

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdateTransparentSelectedFromViewModel);

                RaisePropertyChanged(() => Transparent);
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

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdateSafetyFromViewModel);

                RaisePropertyChanged(() => Safety);

                if (Fishing && value)
                {
                    Fishing = false;
                }
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

                Messenger.Default.Send<bool>(value, ToolBarControlMessageTokens.UpdateFishingFromViewModel);

                RaisePropertyChanged(() => Fishing);

                if (Safety && value)
                {
                    Safety = false;
                }
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

        private bool _projectValid;

        public bool ProjectValid
        {
            get { return _projectValid; }
            set
            {
                if (value == _projectValid)
                {
                    return;
                }
                _projectValid = value;
                RaisePropertyChanged(() => ProjectValid);
            }
        }


        public RelayCommand SyncHandler { get; set; }

        public ToolBarControlViewModel()
        {
            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateCanEditFromView, (b) =>
            {
                CanEdit = b;
            });

            Messenger.Default.Register<bool>(this, ToolBarControlMessageTokens.UpdateProjectValidFromView, (b) =>
            {
                ProjectValid = b;
            });

            SyncHandler = new RelayCommand(()=>
            {
                Messenger.Default.Send<object>(null, ToolBarControlMessageTokens.SyncHandlerFromViewModel);
            });
        }
    }
}
