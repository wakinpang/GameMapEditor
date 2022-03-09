using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace MapEditorControl.ViewModel {
    public class LibraryControlViewModel : ViewModelBase {
        private bool _projectExist;
        private bool _mapValid;

        private MonsterSection _selectMonster;
        private ObservableCollection<MapSection> _mapSection;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        private ObservableCollection<MusicSection> _musicSection = new ObservableCollection<InnerUtil.MusicSection>();
        private ObservableCollection<MonsterSection> _monsterSection;
        private ObservableCollection<NpcSection> _npcSection;

        public RelayCommand NewMonsterButton { get; set; }
        public RelayCommand ChangeMonsterButton { get; set; }
        public RelayCommand MonsterLeftMouseDown { get; set; }
        public RelayCommand<MonsterData> MonsterMouseMove { get; set; }
        public RelayCommand<MonsterGiveFeedBackData> MonsterGiveFeedBack { get; set; }
        public RelayCommand<MapSection> MapChecked { get; set; }
        public RelayCommand<DragNpcData> NpcMouseMove { get; set; }
        public RelayCommand<MusicSection> MusicChecked { get; set; }

        public MonsterSection SelectMonster {
            get { return _selectMonster; }
            set {
                _selectMonster = value;
                Messenger.Default.Send<MonsterSection>(value, LibraryControlMessageTokens.ChangeCurrentMonsterFromLibrary);
                RaisePropertyChanged(() => SelectMonster);
            }
        }

        public ObservableCollection<MapSection> MapSection {
            get { return _mapSection; }
            set {
                if (value == _mapSection) {
                    return;
                }
                _mapSection = value;

                //Messenger.Default.Send<ObservableCollection<MapSection>>(value, LibraryControlMessageTokens.MapSectionChangedFromView);
                RaisePropertyChanged(() => MapSection);
            }
        }

        public ObservableCollection<MusicSection> MusicSection
        {
            get { return _musicSection; }
            set
            {
                if (value == _musicSection)
                {
                    return;
                }
                _musicSection = value;

                RaisePropertyChanged(() => MusicSection);
            }
        }


        public ObservableCollection<MonsterSection> MonsterSection {
            get {
                return _monsterSection;
            }
            set {
                if (value == _monsterSection) {
                    return;
                }
                _monsterSection = value;

                //Messenger.Default.Send<ObservableCollection<MonsterSection>>(value, LibraryControlMessageTokens.SelectMonsterFromView);
                RaisePropertyChanged(() => MonsterSection);
            }
        }

        public ObservableCollection<NpcSection> NpcSection {
            get { return _npcSection; }
            set {
                if (value == _npcSection) {
                    return;
                }
                _npcSection = value;

                RaisePropertyChanged(() => NpcSection);
            }
        }

        private MapSection _currentMapSection;

        public MapSection CurrentMapSection
        {
            get { return _currentMapSection; }
            set
            {
                if (value == _currentMapSection)
                {
                    return;
                }
                _currentMapSection = value;

                Messenger.Default.Send<MapSection>(value, LibraryControlMessageTokens.CurrentMapSectionChangedEventFromViewModel);

                RaisePropertyChanged(() => CurrentMapSection);
            }
        }

        private MusicSection _currentMusicSection;
        
        public MusicSection CurrentMusicSection {
            get { return _currentMusicSection; }
            set {
                if (value == _currentMusicSection) {
                    return;
                }
                _currentMusicSection = value;

                RaisePropertyChanged(() => CurrentMusicSection);
            }
        }

        public bool ProjectExist {
            get { return _projectExist; }
            set {
                if (value == _projectExist) {
                    return;
                }
                _projectExist = value;
                RaisePropertyChanged(() => ProjectExist);
            }
        }

        public bool MapValid {
            get { return _mapValid; }
            set {
                if (value == _mapValid) {
                    return;
                }
                _mapValid = value;
                RaisePropertyChanged(() => MapValid);
            }
        }

        //Initialize
        public LibraryControlViewModel()
        {
            Messenger.Default.Register<ObservableCollection<MapSection>>(this, LibraryControlMessageTokens.UpdateMapFromOutside, (section) =>
            {
                MapSection = section;
                if(section != null && section.Count != 0)
                {
                    CurrentMapSection = section[0];
                    CurrentMapSection.IsChecked = true;

                    if(MusicSection == null)
                    {
                        return;
                    }

                    var result = from mus in MusicSection
                                 where mus.Name == CurrentMapSection.SoundId.ToString() + ".mp3"
                                 select mus;


                    if(result.Count() > 0)
                    {
                        result.ToList()[0].IsChecked = true;
                    }
                    else
                    {
                        // Fixed
                        Messenger.Default.Send<object>(this, LibraryControlMessageTokens.CallMusicErrorDialogFromViewModel);
                        CurrentMusicSection = new MusicSection();
                    }

                    //foreach (MusicSection mus in MusicSection) {
                    //    if (mus.Name == CurrentMapSection.SoundId.ToString() + ".mp3") {
                    //        mus.IsChecked = true;
                    //        return;
                    //    }
                    //}
                    //CurrentMusicSection = new MusicSection();
                }
            });

            Messenger.Default.Register<ObservableCollection<MusicSection>>(this, LibraryControlMessageTokens.UpdateMusicFromOutside, (section) =>
            {
                MusicSection = section;
            });


            Messenger.Default.Register<ObservableCollection<MonsterSection>>(this, LibraryControlMessageTokens.UpdateMonsterFromOutside, (section) =>
            {
                MonsterSection = section;
            });

            Messenger.Default.Register<bool>(this, LibraryControlMessageTokens.UpdateProjectExistFromModel, (b) =>
            {
                ProjectExist = b;
            });

            Messenger.Default.Register<bool>(this, LibraryControlMessageTokens.UpdateMapValidFromModel, (b) =>
            {
                MapValid = b;
            });

            NewMonsterButton = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, LibraryControlMessageTokens.CallNewDialogFromView);
            }, ()=> {
                return ProjectExist;
            });

            ChangeMonsterButton = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, LibraryControlMessageTokens.CallChangeDialogFromView);
            }, () => {
                return ProjectExist;
            });

            MonsterMouseMove = new RelayCommand<MonsterData>((section) =>
            {
                var e = section.Args;
                var source = section.Source;
                
                if (source != null && e.LeftButton == MouseButtonState.Pressed) {
                    DragDrop.DoDragDrop(source, source.DataContext, DragDropEffects.Copy);
                }
            });

            MonsterGiveFeedBack = new RelayCommand<MonsterGiveFeedBackData>((section) =>
            {
                GiveFeedbackEventArgs e = section.Args;
                MonsterSection data = section.Data;
            });

            MapChecked = new RelayCommand<MapSection>((section) =>
            {
                //if (right) {
                //    right = false;
                //    return;
                //}
                //right = true;

                //foreach (MapSection map in MapSection) {
                //    map.IsChecked = false;
                //}
                //section.IsChecked = true;

                if(CurrentMapSection == null || CurrentMapSection == section)
                {
                    return;
                }

                CurrentMapSection.IsChecked = false;
                CurrentMapSection = section;
                CurrentMapSection.IsChecked = true;
                //CurrentMusicSection = from mus in MusicSection
                //                      where mus.Name.Rplace(".mp3", "") == CurrentMapSection.SoundId.ToString()
                //                      select mus;

                Messenger.Default.Send<ObservableCollection<MapSection>>(MapSection, LibraryControlMessageTokens.MapSectionChangedFromView);
                //Messenger.Default.Send<string>(section.Name, LibraryControlMessageTokens.MapCheckedFromView);
            });

            MusicChecked = new RelayCommand<MusicSection>((section) =>
            {
                if (CurrentMusicSection == section) {
                    return;
                }

                CurrentMusicSection.IsChecked = false;
                CurrentMusicSection = section;
                CurrentMusicSection.IsChecked = true;

                Messenger.Default.Send<string>(CurrentMusicSection.Name, LibraryControlMessageTokens.CheckedMusicChangedFromView);
                Messenger.Default.Send<ObservableCollection<MusicSection>>(MusicSection, LibraryControlMessageTokens.MusicSectionChangedFromView);
            });

            NpcMouseMove = new RelayCommand<DragNpcData>((data) =>
            {
                MouseEventArgs args = data.Args;
                Image source = data.Source;

                if (source != null && args.LeftButton == MouseButtonState.Pressed && MapValid) {

                    DragDrop.DoDragDrop(source, source.DataContext, DragDropEffects.Copy);
                }
            });

            //NpcView
            NpcSection = new ObservableCollection<InnerUtil.NpcSection>();
            NpcSection.Add(new NpcSection()
            {
                Type = NpcType.Person,
                Path = "pack://SiteOfOrigin:,,,/image/NPC.png",
                CPath = "image\\NPC.png"
            });

            NpcSection.Add(new NpcSection()
            {
                Type = NpcType.StayPoint,
                Path = "pack://SiteOfOrigin:,,,/image/StayPoint.png",
                CPath = "image\\StayPoint.png"
            });

            NpcSection.Add(new NpcSection()
            {
                Type = NpcType.Telereport,
                Path = "pack://SiteOfOrigin:,,,/image/Telereport.png",
                CPath = "image\\Telereport.png"
            });

            NpcSection.Add(new NpcSection()
            {
                Type = NpcType.NeutralTele,
                Path = "pack://SiteOfOrigin:,,,/image/NeutralTele.png",
                CPath = "image\\NeutralTele.png"
            });
        }

        
    }
}
