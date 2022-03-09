using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditorControl.ViewModel
{
    public class ProjectConfigControlViewModel : ViewModelBase, IDataErrorInfo
    {
        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (value == _projectName)
                {
                    return;
                }
                _projectName = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateProjectNameFromViewModel);

                RaisePropertyChanged(() => ProjectName);
            }
        }

        private string _projectPath;

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            {
                if (value == _projectPath)
                {
                    return;
                }
                _projectPath = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateProjectPathFromViewModel);

                RaisePropertyChanged(() => ProjectPath);
            }
        }

        private string _databaseIP;

        public string DatabaseIP
        {
            get { return _databaseIP; }
            set
            {
                if (value == _databaseIP)
                {
                    return;
                }
                _databaseIP = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateDatabaseIPFromViewModel);

                RaisePropertyChanged(() => DatabaseIP);
            }
        }

        private string _databasePort;

        public string DatabasePort
        {
            get { return _databasePort; }
            set
            {
                if (value == _databasePort)
                {
                    return;
                }
                _databasePort = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateDatabasePortFromViewModel);

                RaisePropertyChanged(() => DatabasePort);
            }
        }

        private string _databaseName;

        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                if (value == _databaseName)
                {
                    return;
                }
                _databaseName = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateDatabaseNameFromViewModel);

                RaisePropertyChanged(() => DatabaseName);
            }
        }

        private string _databaseUserName;

        public string DatabaseUserName
        {
            get { return _databaseUserName; }
            set
            {
                if (value == _databaseUserName)
                {
                    return;
                }
                _databaseUserName = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateDatabaseUserNameFromViewModel);

                RaisePropertyChanged(() => DatabaseUserName);
            }
        }

        private string _databasePassword;

        public string DatabasePassword
        {
            get { return _databasePassword; }
            set
            {
                if (value == _databasePassword)
                {
                    return;
                }
                _databasePassword = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateDatabasePasswordFromViewModel);

                RaisePropertyChanged(() => DatabasePassword);
            }
        }

        private string _mapSourcePath;

        public string MapSourcePath
        {
            get { return _mapSourcePath; }
            set
            {
                if (value == _mapSourcePath)
                {
                    return;
                }
                _mapSourcePath = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateMapSourcePathFromViewModel);

                RaisePropertyChanged(() => MapSourcePath);
            }
        }

        private string _mapSourceOutputPath;

        public string MapSourceOutputPath
        {
            get { return _mapSourceOutputPath; }
            set
            {
                if (value == _mapSourceOutputPath)
                {
                    return;
                }
                _mapSourceOutputPath = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateMapOutputSourcePathFromViewModel);

                RaisePropertyChanged(() => MapSourceOutputPath);
            }
        }

        private string _npcPicturePath;

        public string NpcPicturePath
        {
            get { return _npcPicturePath; }
            set
            {
                if (value == _npcPicturePath)
                {
                    return;
                }
                _npcPicturePath = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateNpcPicturePathFromViewModel);

                RaisePropertyChanged(() => NpcPicturePath);
            }
        }

        private string _monsterPicturePath;

        public string MonsterPicturePath
        {
            get { return _monsterPicturePath; }
            set
            {
                if (value == _monsterPicturePath)
                {
                    return;
                }
                _monsterPicturePath = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateMonsterPathFromViewModel);

                RaisePropertyChanged(() => MonsterPicturePath);
            }
        }

        private string _mapSoundPath;

        public string MapSoundPath
        {
            get { return _mapSoundPath; }
            set
            {
                if (value == _mapSoundPath)
                {
                    return;
                }
                _mapSoundPath = value;

                Messenger.Default.Send<string>(value, ProjectConfigControlMessageTokens.UpdateMapSoundPathFromViewModel);

                RaisePropertyChanged(() => MapSoundPath);
            }
        }

        public string Error { get { return ""; } }

        public string this[string columnName]
        {
            get
            {
                string resultString = null;
                int port = 0;
                IPAddress ip;

                switch (columnName)
                {
                    case "ProjectName":
                        if (ProjectName == "")
                        {
                            resultString = "项目名不得为空。";
                        }
                        break;
                    case "ProjectPath":
                        if (!Directory.Exists(ProjectPath))
                        {
                            resultString = "路径不存在。";
                        }
                        break;
                    case "DatabaseIP":
                        if (!IPAddress.TryParse(DatabaseIP, out ip))
                        {
                            resultString = "IP地址不合法。";
                        }
                        break;
                    case "DatabasePort":
                        if(!int.TryParse(DatabasePort, out port))
                        {
                            resultString = "请输入数字。";
                        }
                        break;
                    case "DatabaseName":
                        if (DatabaseName == "")
                        {
                            resultString = "数据库名不得为空";
                        }
                        break;
                    case "DatabaseUserName":
                        if (DatabaseUserName == "")
                        {
                            resultString = "数据库用户名不得为空";
                        }
                        break;
                    //case "DatabasePassword"
                    case "MapSourcePath":
                        if (!Directory.Exists(MapSourcePath))
                        {
                            resultString = "路径不存在。";
                        }
                        break;
                    case "MapSourceOutputPath":
                        if (!Directory.Exists(MapSourceOutputPath))
                        {
                            resultString = "路径不存在。";
                        }
                        break;
                    case "NpcPicturePath":
                        if (!Directory.Exists(NpcPicturePath))
                        {
                            resultString = "路径不存在。";
                        }
                        break;
                    case "MonsterPicturePath":
                        if (!Directory.Exists(MonsterPicturePath))
                        {
                            resultString = "路径不存在。";
                        }
                        break;
                    case "MapSoundPath":
                        if (!Directory.Exists(MapSoundPath))
                        {
                            resultString = "路径不存在。";
                        }
                        break;
                }
                return resultString;
            }
        }

        public RelayCommand<ButtonType> SelectPath { get; set; }
        public RelayCommand OK { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand TestConnect { get; set; }

        public ProjectConfigControlViewModel()
        {

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateProjectNameFromView, (msg) =>
            {
                ProjectName = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateProjectPathFromView, (msg) =>
            {
                ProjectPath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabaseIPFromView, (msg) =>
            {
                DatabaseIP = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabasePortFromView, (msg) =>
            {
                DatabasePort = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabaseNameFromView, (msg) =>
            {
                DatabaseName = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabaseUserNameFromView, (msg) =>
            {
                DatabaseUserName = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateDatabasePasswordFromView, (msg) =>
            {
                DatabasePassword = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMapSourcePathFromView, (msg) =>
            {
                MapSourcePath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMapOutputSourcePathFromView, (msg) =>
            {
                MapSourceOutputPath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateNpcPicturePathFromView, (msg) =>
            {
                NpcPicturePath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMonsterPathFromView, (msg) =>
            {
                MonsterPicturePath = msg;
            });

            Messenger.Default.Register<string>(this, ProjectConfigControlMessageTokens.UpdateMapSoundPathFromView, (msg) =>
            {
                MapSoundPath = msg;
            });

            SelectPath = new RelayCommand<ButtonType>((ButtonType type) =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog()
                {
                    Description = "请选择一个路径"
                };
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    switch (type)
                    {
                        case ButtonType.ProjectPath:
                            ProjectPath = path;
                            break;
                        case ButtonType.MapSourcePath:
                            MapSourcePath = path;
                            break;
                        case ButtonType.MapSourceOutputPath:
                            MapSourceOutputPath = path;
                            break;
                        case ButtonType.NpcPicturePath:
                            NpcPicturePath = path;
                            break;
                        case ButtonType.MonsterPicturePath:
                            MonsterPicturePath = path;
                            break;
                        case ButtonType.MapSoundPath:
                            MapSoundPath = path;
                            break;
                    }
                }
            });

            OK = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, ProjectConfigControlMessageTokens.OKEventFromViewModel);
            }, 
            () => {
                return ProjectName != "" 
                && ProjectPath != "" 
                && DatabaseInfoVailidate() 
                && MapSourcePath != "" 
                && MapSourceOutputPath != "" 
                && NpcPicturePath != "" 
                && MonsterPicturePath != "" 
                && MapSoundPath != "";
            });
            

            Cancel = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, ProjectConfigControlMessageTokens.CancelEventFromViewModel);
            });

            TestConnect = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, ProjectConfigControlMessageTokens.TestConnectEventFromViewModel);
            },
            ()=>
            {
                return DatabaseInfoVailidate();
            });
        }

        private bool DatabaseInfoVailidate()
        {
            IPAddress ip;
            int port;
            return IPAddress.TryParse(DatabaseIP, out ip) && DatabaseName != "" && DatabaseUserName != "" && int.TryParse(DatabasePort, out port);
        }

    }
}
