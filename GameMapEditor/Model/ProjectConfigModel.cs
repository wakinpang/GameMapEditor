using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameMapEditor.Model
{
    public class ProjectConfigModel : ObservableObject
    {
        //public string Path { get; set; }

        private string _path;
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }


        static ProjectConfigModel _currentInstance = new ProjectConfigModel();
        static ProjectConfigModel _instance = new ProjectConfigModel();

        public static ProjectConfigModel Instance()
        {
            return _instance;
        }

        public static ProjectConfigModel CurrentInstance()
        {
            return _currentInstance;
        }

        public static void SetCurrentInstance()
        {
            _currentInstance = _instance;
        }

        public static void SetNewInstance()
        {
            _instance = new ProjectConfigModel();
        }

        public static void ResumeInstance()
        {
            _instance = _currentInstance.MemberwiseClone() as ProjectConfigModel;
        }

        private string _projectName = "TestProject";
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                RaisePropertyChanged(() => ProjectName);
            }
        }

        private string _projectPath = "D:\\";

        public string ProjectPath
        {
            get { return _projectPath; }
            set
            { 
                _projectPath = value;
                RaisePropertyChanged(() => ProjectPath);
            }
        }

        private string _dataBaseIP = "127.0.0.1";

        public string DatabaseIP
        {
            get { return _dataBaseIP; }
            set
            { 
                _dataBaseIP = value;
                RaisePropertyChanged(() => DatabaseIP);
            }
        }

        private string _dataBasePort = "3306";

        public string DatabasePort
        {
            get { return _dataBasePort; }
            set
            {
                _dataBasePort = value;
                RaisePropertyChanged(() => DatabasePort);
            }
        }

        private string _dataBaseName = "mingtong";

        public string DatabaseName
        {
            get { return _dataBaseName; }
            set
            {
                _dataBaseName = value;
                RaisePropertyChanged(() => DatabaseName);
            }
        }

        private string _dataBaseUserName = "root";

        public string DatabaseUserName
        {
            get { return _dataBaseUserName; }
            set
            {
                _dataBaseUserName = value;
                RaisePropertyChanged(() => DatabaseUserName);
            }
        }

        private string _databasePassword = "1026121287qq";

        public string DatabasePassword
        {
            get { return _databasePassword; }
            set
            {
                _databasePassword = value;
                RaisePropertyChanged(() => DatabasePassword);
            }
        }

        private string _mapSourcePath = "D:\\";

        public string MapSourcePath
        {
            get { return _mapSourcePath; }
            set
            {
                _mapSourcePath = value;
                RaisePropertyChanged(() => MapSourcePath);
            }
        }

        private string _mapSourceOutputPath = "D:\\";

        public string MapSourceOutputPath
        {
            get { return _mapSourceOutputPath; }
            set
            {
                _mapSourceOutputPath = value;
                RaisePropertyChanged(() => MapSourceOutputPath);
            }
        }


        private string _npcPicturePath = "D:\\";

        public string NpcPicturePath
        {
            get { return _npcPicturePath; }
            set
            {
                _npcPicturePath = value;
                RaisePropertyChanged(() => NpcPicturePath);
            }
        }

        private string _monsterPicturePath = "D:\\";

        public string MonsterPicturePath
        {
            get { return _monsterPicturePath; }
            set
            {
                _monsterPicturePath = value;
                RaisePropertyChanged(() => MonsterPicturePath);
            }
        }

        private string _mapSoundPath = "D:\\";

        public string MapSoundPath
        {
            get { return _mapSoundPath; }
            set
            {
                _mapSoundPath = value;
                RaisePropertyChanged(() => MapSoundPath);
            }
        }
    }
}
