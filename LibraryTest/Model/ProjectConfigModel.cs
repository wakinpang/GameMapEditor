using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.Model
{
    public class ProjectConfigModel : ObservableObject, IDataErrorInfo
    {
        static ProjectConfigModel _instance = new ProjectConfigModel();

        public static ProjectConfigModel Instance()
        {
            return _instance;
        }

        private string _projectName = "";
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                RaisePropertyChanged(() => ProjectName);
            }
        }

        private string _projectPath = "";

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            { 
                _projectPath = value;
                RaisePropertyChanged(() => ProjectPath);
            }
        }

        private string _dataBaseIP = "";

        public string DatabaseIP
        {
            get { return _dataBaseIP; }
            set
            { 
                _dataBaseIP = value;
                RaisePropertyChanged(() => DatabaseIP);
            }
        }

        private int _dataBasePort = 20;

        public int DatabasePort
        {
            get { return _dataBasePort; }
            set
            {
                _dataBasePort = value;
                RaisePropertyChanged(() => DatabasePort);
            }
        }

        private string _dataBaseName = "";

        public string DatabaseName
        {
            get { return _dataBaseName; }
            set
            {
                _dataBaseName = value;
                RaisePropertyChanged(() => DatabaseName);
            }
        }

        private string _dataBaseUserName = "";

        public string DatabaseUserName
        {
            get { return _dataBaseUserName; }
            set
            {
                _dataBaseUserName = value;
                RaisePropertyChanged(() => DatabaseUserName);
            }
        }

        private string _databasePassword = "";

        public string DatabasePassword
        {
            get { return _databasePassword; }
            set
            {
                _databasePassword = value;
                RaisePropertyChanged(() => DatabasePassword);
            }
        }

        private string _mapSourcePath = "";

        public string MapSourcePath
        {
            get { return _mapSourcePath; }
            set
            {
                _mapSourcePath = value;
                RaisePropertyChanged(() => MapSourcePath);
            }
        }

        private string _mapSourceOutputPath = "";

        public string MapSourceOutputPath
        {
            get { return _mapSourceOutputPath; }
            set
            {
                _mapSourceOutputPath = value;
                RaisePropertyChanged(() => MapSourceOutputPath);
            }
        }


        private string _npcPicturePath = "";

        public string NpcPicturePath
        {
            get { return _npcPicturePath; }
            set
            {
                _npcPicturePath = value;
                RaisePropertyChanged(() => NpcPicturePath);
            }
        }

        private string _monsterPicturePath = "";

        public string MonsterPicturePath
        {
            get { return _monsterPicturePath; }
            set
            {
                _monsterPicturePath = value;
                RaisePropertyChanged(() => MonsterPicturePath);
            }
        }

        private string _mapSoundPath = "";

        public string MapSoundPath
        {
            get { return _mapSoundPath; }
            set
            {
                _mapSoundPath = value;
                RaisePropertyChanged(() => MapSoundPath);
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                string resultString = null;
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
                        if(!IPAddress.TryParse(DatabaseIP, out ip))
                        {
                            resultString = "IP地址不合法。";
                        }
                        break;
                    //case "DatabasePort":
                    //    resultString = "请输入数字。";
                    //    break;
                    case "DatabaseName":
                        if(DatabaseName == "")
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

    }
}
