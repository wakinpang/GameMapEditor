using DatabaseOperate;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GameMapEditor.InnerUtil;
using GameMapEditor.Model;
using MapEditorControl.InnerUtil;
using System.Xml;
using System.IO;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameMapEditor.ViewModel
{
    public class ProjectConfigViewModel : ViewModelBase
    {
        private ProjectConfigModel _projectConfig;

        public ProjectConfigModel ProjectConfig
        {
            get { return _projectConfig; }
            set
            {
                _projectConfig = value;
                RaisePropertyChanged(() => ProjectConfig);
            }
        }

        private ProjectConfigModel _currentProjectConfig;

        public ProjectConfigModel CurrentProjectConfig
        {
            get { return _currentProjectConfig; }
            set
            {
                if (value == _currentProjectConfig)
                {
                    return;
                }
                _currentProjectConfig = value;
                RaisePropertyChanged(() => CurrentProjectConfig);
            }
        }


        void SetValue(XmlDocument projectFile, XmlNode father, string child, string val)
        {
            XmlElement xmle = projectFile.CreateElement("", child, "");
            xmle.InnerText = val;
            father.AppendChild(xmle);
        }

        void UpdateValue(XmlNode father, string child, string val)
        {
            XmlNode xmle = father.SelectSingleNode(child);
            xmle.InnerText = val;
        }

        private List<HistorySection> getList(XmlDocument doc)
        {
            List<HistorySection> result = new List<HistorySection>();

            XmlNode root = doc.SelectSingleNode("Project");
            XmlNodeList paths = root.ChildNodes;

            foreach (XmlNode path in paths)
            {
                result.Add(new HistorySection(path.InnerText));
            }

            return result;
        }

        private void writeList(List<HistorySection> list)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", "gb2312", null);
            doc.AppendChild(xmldecl);

            XmlNode root = doc.CreateElement("", "Project", "");

            foreach (HistorySection his in list)
            {
                XmlNode newPath = doc.CreateElement("", "Path", "");
                newPath.InnerText = his.ProjectPath;
                root.AppendChild(newPath);
            }

            doc.AppendChild(root);

            doc.Save("history.xml");
        }

        private bool _modifying = false;

        public ProjectConfigViewModel()
        {
            ProjectConfig = ProjectConfigModel.Instance();
            CurrentProjectConfig = ProjectConfig;

            Messenger.Default.Register<object>(this, MenuMessageTokens.ModifyProjectConfig, (dummy) =>
            {
                _modifying = true;
                ProjectConfigModel.ResumeInstance();
                ProjectConfig = ProjectConfigModel.Instance();

                CurrentProjectConfig = ProjectConfigModel.CurrentInstance();
            });

            Messenger.Default.Register<object>(this, MenuMessageTokens.UpdateCurrentProjectConfig, (dummy) =>
            {
                CurrentProjectConfig = ProjectConfigModel.CurrentInstance();
            });

            NewProjectOKHandle = new RelayCommand(() =>
            {
                bool result = false;

                CostumeWaitingDialogHelper.CallWaitingDialog(
                    this,
                    (ref bool canceled) =>
                    {
                        result = DatabaseUtil.TryConn(ProjectConfig.DatabaseIP, ProjectConfig.DatabaseName, ProjectConfig.DatabaseUserName, ProjectConfig.DatabasePassword, ProjectConfig.DatabasePort);
                    },
                    () =>
                    {
                        if (!result)
                        {
                            Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                            {
                                Title = "Failed",
                                Message = "数据库连接测试失败，请检查设置！",
                            }, MainWindowTokens.ShowMessageDialog);
                            return;
                        }

                        if (!_modifying)
                        {
                            ProjectConfigModel.SetCurrentInstance();

                            XmlDocument ProjFile = new XmlDocument();
                            XmlDeclaration declarartion = ProjFile.CreateXmlDeclaration("1.0", "gb2312", null);
                            ProjFile.AppendChild(declarartion);
                            ProjFile.AppendChild(ProjFile.CreateElement("", "Project", ""));
                            XmlNode root = ProjFile.SelectSingleNode("Project");
                            XmlNode data = ProjFile.CreateElement("", "DataBase", "");
                            XmlNode resource = ProjFile.CreateElement("", "Resource", "");
                            root.AppendChild(data);
                            root.AppendChild(resource);

                            SetValue(ProjFile, root, "Name", _projectConfig.ProjectName);
                            SetValue(ProjFile, root, "ProjectPath", _projectConfig.ProjectPath);
                            SetValue(ProjFile, data, "DataBaseIP", _projectConfig.DatabaseIP);
                            SetValue(ProjFile, data, "DataBasePort", _projectConfig.DatabasePort);
                            SetValue(ProjFile, data, "DataBaseName", _projectConfig.DatabaseName);
                            SetValue(ProjFile, data, "DataBaseUserName", _projectConfig.DatabaseUserName);
                            SetValue(ProjFile, data, "DataBasePassword", _projectConfig.DatabasePassword);
                            SetValue(ProjFile, resource, "MapSourcePath", _projectConfig.MapSourcePath);
                            SetValue(ProjFile, resource, "MapSourceOutputPath", _projectConfig.MapSourceOutputPath);
                            SetValue(ProjFile, resource, "NpcPicturePath", _projectConfig.NpcPicturePath);
                            SetValue(ProjFile, resource, "MonsterPicturePath", _projectConfig.MonsterPicturePath);
                            SetValue(ProjFile, resource, "MapSoundPath", _projectConfig.MapSoundPath);

                            _projectConfig.Path = _projectConfig.ProjectPath + "\\" + _projectConfig.ProjectName + ".project";

                            if (File.Exists(_projectConfig.Path))
                            {
                                Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                                {
                                    Title = "发生错误",
                                    Message = "当前目录下此工程已存在!",
                                }, MainWindowTokens.ShowMessageDialog);
                                ProjectConfigModel.SetNewInstance();
                                Messenger.Default.Send<object>(null, MenuMessageTokens.HideNewProjectWindowFromViewModel);
                                return;
                            }
                            else
                            {
                                ProjFile.Save(_projectConfig.Path);
                                XmlDocument HisFile = new XmlDocument();
                                HisFile.Load("history.xml");
                                List<HistorySection> list = new List<HistorySection>();

                                list = getList(HisFile);
                                list.Insert(0, new HistorySection(_projectConfig.Path));

                                if (list.Count() > 20)
                                {
                                    list.RemoveAt(20);
                                }

                                writeList(list);
                            }
                        }
                        else
                        {
                            ProjectConfigModel.SetCurrentInstance();

                            XmlDocument ProjFile = new XmlDocument();
                            ProjFile.Load(ProjectConfigModel.CurrentInstance().Path);
                            XmlNode root = ProjFile.SelectSingleNode("Project");
                            XmlNode database = root.SelectSingleNode("DataBase");
                            XmlNode resource = root.SelectSingleNode("Resource");

                            UpdateValue(root, "Name", _projectConfig.ProjectName);
                            UpdateValue(root, "ProjectPath", _projectConfig.ProjectPath);
                            UpdateValue(database, "DataBaseIP", _projectConfig.DatabaseIP);
                            UpdateValue(database, "DataBasePort", _projectConfig.DatabasePort);
                            UpdateValue(database, "DataBaseName", _projectConfig.DatabaseName);
                            UpdateValue(database, "DataBaseUserName", _projectConfig.DatabaseUserName);
                            UpdateValue(database, "DataBasePassword", _projectConfig.DatabasePassword);
                            UpdateValue(resource, "MapSourcePath", _projectConfig.MapSourcePath);
                            UpdateValue(resource, "MapSourceOutputPath", _projectConfig.MapSourceOutputPath);
                            UpdateValue(resource, "NpcPicturePath", _projectConfig.NpcPicturePath);
                            UpdateValue(resource, "MonsterPicturePath", _projectConfig.MonsterPicturePath);
                            UpdateValue(resource, "MapSoundPath", _projectConfig.MapSoundPath);

                            ProjFile.Save(ProjectConfigModel.CurrentInstance().Path);
                        }

                        ProjectConfigModel.SetNewInstance();
                        ProjectConfig = ProjectConfigModel.Instance();
                        CurrentProjectConfig = ProjectConfigModel.CurrentInstance();

                        SaveHistoryConfigToXML();

                        _modifying = false;

                        Messenger.Default.Send<bool>(true, MenuMessageTokens.UpdateProjectExist);
                        Messenger.Default.Send<object>(null, MenuMessageTokens.HideNewProjectWindowFromViewModel);

                        // 从文件目录获取音乐
                        DirectoryInfo dir = new DirectoryInfo(_projectConfig.MapSoundPath);
                        FileInfo[] inf = dir.GetFiles();
                        var search = from e in inf
                                     where e.Extension.Equals(".mp3")
                                     select new MusicSection()
                                     {
                                         Name = e.FullName.Replace(CurrentProjectConfig.MapSoundPath, ""),
                                     };
                        var newMusicSection = new ObservableCollection<MusicSection>(search.ToList());

                        Messenger.Default.Send<ObservableCollection<MusicSection>>(newMusicSection, MainWindowTokens.UpdateMusicSections);
                        // 读取数据库信息
                        Messenger.Default.Send<object>(null, MenuMessageTokens.FetchProjectDataFromDatabase);
                        Messenger.Default.Send<string>(String.Format("2D地图编辑器 - {0}", ProjectConfigModel.CurrentInstance().ProjectName), MainWindowTokens.UpdateMainWindowTitle);
                    },
                    "请等待",
                    "数据库连接测试中，请稍等……",
                    false
                    );
            });

            NewProjectCancelHandle = new RelayCommand(() =>
            {
                //ProjectConfigModel.SetNewInstance();
                //ProjectConfig = ProjectConfigModel.Instance();

                _modifying = false;
                Messenger.Default.Send<object>(null, MenuMessageTokens.HideNewProjectWindowFromViewModel);
            });

            TestConnect = new RelayCommand(() =>
            {

                bool result = false;

                CostumeWaitingDialogHelper.CallWaitingDialog(
                    this,
                    (ref bool canceled) =>
                    {
                        result = DatabaseUtil.TryConn(ProjectConfig.DatabaseIP, ProjectConfig.DatabaseName, ProjectConfig.DatabaseUserName, ProjectConfig.DatabasePassword, ProjectConfig.DatabasePort);
                    },
                    () =>
                    {
                        // 测试数据库连接
                        if (!result)
                        {
                            Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                            {
                                Title = "Failed",
                                Message = "数据库连接测试失败！",
                            }, MainWindowTokens.ShowMessageDialog);
                        }
                        else
                        {
                            Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                            {
                                Title = "Success",
                                Message = "数据库连接测试成功！",
                            }, MainWindowTokens.ShowMessageDialog);

                        }
                    },
                    "请等待",
                    "数据库连接测试中，请稍等……",
                    false);
            });
        }

        private void SaveHistoryConfigToXML()
        {
            // 保存当前配置到XML
            const string historyConfigXMLPath = "history_config.xml";
            if (!File.Exists(historyConfigXMLPath))
            {
                var xDoc = new XDocument(
                    new XElement("HistoryConfig",
                        new XElement("ProjectName", CurrentProjectConfig.ProjectName),
                        new XElement("ProjectPath", CurrentProjectConfig.ProjectPath),
                        new XElement("DatabaseIP", CurrentProjectConfig.DatabaseIP),
                        new XElement("DatabasePort", CurrentProjectConfig.DatabasePort),
                        new XElement("DatabaseName", CurrentProjectConfig.DatabaseName),
                        new XElement("DatabaseUserName", CurrentProjectConfig.DatabaseUserName),
                        new XElement("DatabasePassword", CurrentProjectConfig.DatabasePassword),
                        new XElement("MapSourcePath", CurrentProjectConfig.MapSourcePath),
                        new XElement("MapSourceOutputPath", CurrentProjectConfig.MapSourceOutputPath),
                        new XElement("NpcPicturePath", CurrentProjectConfig.NpcPicturePath),
                        new XElement("MonsterPicturePath", CurrentProjectConfig.MonsterPicturePath),
                        new XElement("MapSoundPath", CurrentProjectConfig.MapSoundPath)
                    )
                );
                xDoc.Save(historyConfigXMLPath);
            }
            else
            {
                var xDoc = XDocument.Load(historyConfigXMLPath);
                var root = xDoc.Root;
                root.Element("ProjectName").SetValue(CurrentProjectConfig.ProjectName);
                root.Element("ProjectPath").SetValue(CurrentProjectConfig.ProjectPath);
                root.Element("DatabaseIP").SetValue(CurrentProjectConfig.DatabaseIP);
                root.Element("DatabasePort").SetValue(CurrentProjectConfig.DatabasePort);
                root.Element("DatabaseName").SetValue(CurrentProjectConfig.DatabaseName);
                root.Element("DatabaseUserName").SetValue(CurrentProjectConfig.DatabaseUserName);
                root.Element("DatabasePassword").SetValue(CurrentProjectConfig.DatabasePassword);
                root.Element("MapSourcePath").SetValue(CurrentProjectConfig.MapSourcePath);
                root.Element("MapSourceOutputPath").SetValue(CurrentProjectConfig.MapSourceOutputPath);
                root.Element("MapSourceOutputPath").SetValue(CurrentProjectConfig.MapSourceOutputPath);
                root.Element("MonsterPicturePath").SetValue(CurrentProjectConfig.MonsterPicturePath);
                root.Element("MapSoundPath").SetValue(CurrentProjectConfig.MapSoundPath);
            }
        }

        public RelayCommand NewProjectOKHandle { get; set; }
        public RelayCommand NewProjectCancelHandle { get; set; }

        public RelayCommand TestConnect { get; set; }
    }
}
