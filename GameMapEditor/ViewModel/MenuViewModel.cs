using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MapEditorControl.InnerUtil;
using GameMapEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Xml;
using GameMapEditor.InnerUtil;
using DatabaseOperate;
using GalaSoft.MvvmLight.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Linq;
using zlib;

namespace GameMapEditor.ViewModel
{
    using TileType = Byte;
    public class MenuViewModel : ViewModelBase
    {
        private ProjectConfigModel _projectConfig;

        public RelayCommand NewProjectHandle { get; set; }
        public RelayCommand ProjectConfigHandle { get; set; }
        public RelayCommand OpenProjectHandle { get; set; }
        public RelayCommand<string> OpenHistoryHandle { get; set; }
        public RelayCommand OutputHandle { get; set; }
        public RelayCommand CutMapHandle { get; set; }

        private XmlDocument HisFile;

        public MenuViewModel()
        {
            _projectConfig = ProjectConfigModel.Instance();
            HistorySource = new ObservableCollection<HistorySection>();

            HisFile = new XmlDocument();
            HisFile.Load("history.xml");
            XmlNode Projects = HisFile.SelectSingleNode("Project");
            XmlNodeList paths = Projects.ChildNodes;

            foreach (XmlNode path in paths) {
                HistorySource.Add(new HistorySection(path.InnerText));
            }

            Messenger.Default.Register<bool>(this, MenuMessageTokens.UpdateProjectExist, (exist) =>
            {
                ProjectExist = exist;
            });

            NewProjectHandle = new RelayCommand(() =>
            {
                // 读取历史配置
                const string historyConfigXMLPath = "history_config.xml";
                if (File.Exists(historyConfigXMLPath))
                {
                    var xDoc = XDocument.Load(historyConfigXMLPath);
                    var root = xDoc.Root;
                    var config = ProjectConfigModel.Instance();

                    config.ProjectName = root.Element("ProjectName").Value;
                    config.ProjectPath = root.Element("ProjectPath").Value;
                    config.DatabaseIP = root.Element("DatabaseIP").Value;
                    config.DatabasePort = root.Element("DatabasePort").Value;
                    config.DatabaseName = root.Element("DatabaseName").Value;
                    config.DatabaseUserName = root.Element("DatabaseUserName").Value;
                    config.DatabasePassword = root.Element("DatabasePassword").Value;
                    config.MapSourcePath = root.Element("MapSourcePath").Value;
                    config.MapSourceOutputPath = root.Element("MapSourceOutputPath").Value;
                    config.NpcPicturePath = root.Element("NpcPicturePath").Value;
                    config.MonsterPicturePath = root.Element("MonsterPicturePath").Value;
                    config.MapSoundPath = root.Element("MapSoundPath").Value;
                }

                Messenger.Default.Send<string>("新建项目", MenuMessageTokens.ShowNewProjectWindowFromViewModel);
            });

            ProjectConfigHandle = new RelayCommand(() =>
            {
                Messenger.Default.Send<object>(null, MenuMessageTokens.ModifyProjectConfig);
                Messenger.Default.Send<string>("修改项目配置", MenuMessageTokens.ShowNewProjectWindowFromViewModel);
            });

            OpenProjectHandle = new RelayCommand(() =>
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Title = "请选择工程文件",
                    Filter = "工程文件(*.project)|*.project"
                };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _projectConfig.Path = dialog.FileName;
                    //_projectConfig.ProjectName = dialog.SafeFileName;
                }
                else
                {
                    return;
                }

                XmlDocument ProjFile = new XmlDocument();
                ProjFile.Load(_projectConfig.Path);

                XmlNode root = ProjFile.SelectSingleNode("Project");
                XmlNode database = root.SelectSingleNode("DataBase");
                XmlNode resource = root.SelectSingleNode("Resource");

                _projectConfig.ProjectName = getValue(root, "Name");
                _projectConfig.ProjectPath = getValue(root, "ProjectPath");
                _projectConfig.DatabaseIP = getValue(database, "DataBaseIP");
                _projectConfig.DatabasePort = getValue(database, "DataBasePort");
                _projectConfig.DatabaseName = getValue(database, "DataBaseName");
                _projectConfig.DatabaseUserName = getValue(database, "DataBaseUserName");
                _projectConfig.DatabasePassword = getValue(database, "DataBasePassword");
                _projectConfig.MapSourcePath = getValue(resource, "MapSourcePath");
                _projectConfig.MapSourceOutputPath = getValue(resource, "MapSourceOutputPath");
                _projectConfig.NpcPicturePath = getValue(resource, "NpcPicturePath");
                _projectConfig.MonsterPicturePath = getValue(resource, "MonsterPicturePath");
                _projectConfig.MapSoundPath = getValue(resource, "MapSoundPath");

                //XmlDocument InFile = new XmlDocument();
                var InFile = HisFile;
                InFile.Load("history.xml");
                List<HistorySection> list = getList(InFile);

                bool existed = false;
                foreach (HistorySection his in list)
                {
                    if (his.ProjectPath == _projectConfig.Path)
                    {
                        existed = true;
                    }
                }

                if (!existed)
                {
                    list.Insert(0, new HistorySection(_projectConfig.Path));
                }

                if (list.Count() > 20) {
                    list.RemoveAt(20);
                } 

                writeList(list);

                ProjectExist = true;
                ProjectConfigModel.SetCurrentInstance();
                ProjectConfigModel.SetNewInstance();

                _projectConfig = ProjectConfigModel.Instance();
                Messenger.Default.Send<object>(null, MenuMessageTokens.UpdateCurrentProjectConfig);

                // 从文件目录获取音乐
                DirectoryInfo dir = new DirectoryInfo(ProjectConfigModel.CurrentInstance().MapSoundPath);
                FileInfo[] inf = dir.GetFiles();
                ObservableCollection<MusicSection> newMusicSection = new ObservableCollection<MusicSection>();
                foreach (FileInfo e in inf)
                {
                    if (e.Extension.Equals(".mp3"))
                    {
                        newMusicSection.Add(new MusicSection()
                        {
                            Name = e.FullName.Replace(ProjectConfigModel.CurrentInstance().MapSoundPath, ""),
                        });
                    }
                }

                Messenger.Default.Send<ObservableCollection<MusicSection>>(newMusicSection, MainWindowTokens.UpdateMusicSections);

                SetTitleAndTryConnect();
            });

            OpenHistoryHandle = new RelayCommand<string>((path) =>
            {
                OnOpenHistory(path);
            });

            OutputHandle = new RelayCommand(() =>
            {

                CostumeWaitingDialogHelper.CallWaitingDialog(
                    this,
                    (ref bool canceled) =>
                    {
                        var tiles = MapEditorAndNavigationModel.Instance().MapTiles;
                        var currentMap = LibraryControlModel.Instance().CurrentMapSection;
                        var filePath = ProjectConfigModel.CurrentInstance().MapSourceOutputPath + "\\" + currentMap.MapID.ToString() + ".data";

                        var fileLength = tiles.Length * sizeof(byte) + 8;
                        byte[] tempByteArray = null;

                        // 第二部分
                        byte[] compressData = new byte[fileLength + 2];
                        compressData[fileLength] = 0;
                        compressData[fileLength + 1] = 0;

                        int index = 0;
                        // 5-8 字节
                        tempByteArray = BitConverter.GetBytes((Int32)currentMap.Width);
                        Array.Reverse(tempByteArray);
                        tempByteArray.CopyTo(compressData, index);
                        index += 4;

                        // 9-12 字节
                        tempByteArray = BitConverter.GetBytes((Int32)currentMap.Height);
                        Array.Reverse(tempByteArray);
                        tempByteArray.CopyTo(compressData, index);
                        index += 4;

                        /*
                         第0位：可以通行 1 不能通行 0
                         第1位：不透明   1 半透明   0
                         第3位：不钓鱼   1 钓鱼     0
                         第4位：安全区   1 非安全区 0
                         */

                        var row = tiles.GetLength(0);
                        var col = tiles.GetLength(1);

                        for (var y = 0; y < col; ++y)
                        {
                            for (var x = 0; x < row; ++x)       
                            {
                                var tile = tiles[x, y];
                                byte value = Convert.ToByte("00001010", 2);

                                // 可以通行
                                if ((tile & (TileType)TileTypeBit.Selected) > 0)
                                {
                                    value |= Convert.ToByte("00000001", 2);
                                }
                                // 半透明
                                if ((tile & (TileType)TileTypeBit.Translucent) > 0)
                                {
                                    value &= Convert.ToByte("11111101", 2);
                                }
                                // 钓鱼
                                if ((tile & (TileType)TileTypeBit.Fishing) > 0)
                                {
                                    value &= Convert.ToByte("11110111", 2);
                                }
                                // 安全区
                                if ((tile & (TileType)TileTypeBit.Safety) > 0)
                                {
                                    value |= Convert.ToByte("00010000", 2);
                                }

                                byte[] output = new byte[] { value };
                                output.CopyTo(compressData, index);
                                index += 1;

                            }
                        }

                        var reslutBytes = compressBytes(compressData);
                        var compressedSize = reslutBytes.Length;

                        // 缩放
                        string sourcePath = MapEditorAndNavigationModel.Instance().BackgroundSource;
                        Bitmap bitmap = new Bitmap(sourcePath);

                        var newBitmap = ScaleBitmap(bitmap, bitmap.Width / 10, bitmap.Height / 10);
                        Graphics g = Graphics.FromImage(newBitmap);
                        //g.ScaleTransform(0.1f, 0.1f);

                        string newFilePath = Path.GetDirectoryName(sourcePath) + "\\" + Path.GetFileNameWithoutExtension(sourcePath) + "_scal.jpg";
                        if(File.Exists(newFilePath))
                        {
                            File.Delete(newFilePath);
                        }

                        // 保存
                        newBitmap.Save(newFilePath, ImageFormat.Jpeg);

                        // 读取
                        FileStream fs = new FileStream(newFilePath, FileMode.Open);
                        byte[] tmpArray = new byte[fs.Length];
                        fs.Read(tmpArray, 0, (int)fs.Length);

                        Int32 wholeLength = compressedSize;
                        fs.Close();

                        var wholeSizeBits = BitConverter.GetBytes((Int32)wholeLength);
                        Array.Reverse(wholeSizeBits);

                        if (!File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        var fileStream = File.Open(filePath, FileMode.Create);

                        fileStream.Write(wholeSizeBits, 0, wholeSizeBits.Length);
                        fileStream.Write(reslutBytes, 0, reslutBytes.Length);
                        fileStream.Write(tmpArray, 0, tmpArray.Length);

                        fileStream.Close();

                    },
                    () =>
                    {
                        Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                        {
                            Title = "Success",
                            Message = "地图数据导出成功！",
                        }, MainWindowTokens.ShowMessageDialog);
                    },
                    "请等待",
                    "导出地图数据中，请稍等……",
                    false
                );
            });

            CutMapHandle = new RelayCommand(() =>
            {
                var source = MapEditorAndNavigationModel.Instance().BackgroundSource;
                var mapOutPutDic = ProjectConfigModel.CurrentInstance().MapSourceOutputPath + "\\" + LibraryControlModel.Instance().CurrentMapSection.MapID.ToString() + "\\";
                if (!Directory.Exists(mapOutPutDic))
                {
                    Directory.CreateDirectory(mapOutPutDic);
                }

                Image image = null;

                try
                {
                    image = Image.FromFile(source);
                    //graph = Graphics.FromImage(image);
                }
                catch(Exception e)
                {
                    Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                    {
                        Title = "Failed",
                        Message = "加载图片失败，请检查！",
                    }, MainWindowTokens.ShowMessageDialog);
                    image.Dispose();
                    return;
                }

                var width = image.Width;
                var height = image.Height;

                const int widthStep = 256;
                const int heightStep = 256;

                int xTime = width / widthStep;
                int yTime = height / heightStep;

                int remaindWidth = width - xTime * widthStep;
                int remaindHeight = height - yTime * heightStep;

                CostumeWaitingDialogHelper.CallWaitingDialog(
                    this,
                    (ref bool canceled) =>
                    {
                        int index = 0;
                        for (int j = 0; j < yTime; ++j)
                        {
                            int startY = j * heightStep;
                            for (int i = 0; i < xTime; ++i)
                            {
                                int startX = i * widthStep;

                                OutputTileBitmap(mapOutPutDic, ref index, remaindWidth > 0 ? xTime + 1 : xTime , startX, startY, 256, 256, image);
                            }

                            if (remaindWidth > 0)
                            {
                                var startX = widthStep * xTime;

                                OutputTileBitmap(mapOutPutDic, ref index, remaindWidth > 0 ? xTime + 1 : xTime, startX, startY, 256, 256, image);
                            }
                        }
                        if(remaindHeight > 0)
                        {
                            for (int i = 0; i < (remaindWidth > 0 ? xTime + 1 : xTime); ++i)
                            {
                                int startX = i * widthStep;
                                int startY = heightStep * yTime ;

                                OutputTileBitmap(mapOutPutDic, ref index, remaindWidth > 0 ? xTime + 1 : xTime, startX, startY, 256, 256, image);
                            }

                            //if(remaindWidth > 0)
                            //{
                            //    int startX = widthStep * xTime;
                            //    int startY = heightStep * yTime;

                            //    OutputTileBitmap(mapOutPutDic, ref index, xTime, startX, startY, 256, 256, image);
                            //}
                        }

                    },
                    () =>
                    {
                        Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                        {
                            Title = "Success",
                            Message = "图片切割成功！",
                        }, MainWindowTokens.ShowMessageDialog);
                    },
                    "请等待",
                    "正在切割图片，请稍等……",
                    false
                );
            });

        }

        Bitmap ScaleBitmap(Bitmap srcBitmap, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, srcBitmap.PixelFormat);
            Graphics g = Graphics.FromImage(bitmap);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(srcBitmap, 0, 0, width, height);

            return bitmap;
        }

        private void OutputTileBitmap(string mapOutPutDic, ref int index, int xTime, int startX, int startY, int width, int height, Image source)
        {
            Bitmap tile = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var graph = Graphics.FromImage(tile);
            graph.DrawImage(source, new RectangleF(0, 0, width, height), new RectangleF(startX, startY, width, height), GraphicsUnit.Pixel);

            var tX = index / xTime;
            var tY = index % xTime;

            var outputFileName = mapOutPutDic + tX.ToString() + "_" + tY.ToString() + ".jpg";
            ++index;

            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            tile.Save(outputFileName, ImageFormat.Jpeg);

            tile.Dispose();
        }

        private void SetTitleAndTryConnect()
        {
            Messenger.Default.Send<string>(String.Format("2D地图编辑器 - {0}", ProjectConfigModel.CurrentInstance().ProjectName), MainWindowTokens.UpdateMainWindowTitle);

            var connecttResult = false;

            CostumeWaitingDialogHelper.CallWaitingDialog(
                this,
                (ref bool canceled) =>
                {
                    connecttResult = DatabaseUtil.TryConn(ProjectConfigModel.CurrentInstance().DatabaseIP,
                            ProjectConfigModel.CurrentInstance().DatabaseName,
                            ProjectConfigModel.CurrentInstance().DatabaseUserName,
                            ProjectConfigModel.CurrentInstance().DatabasePassword,
                            ProjectConfigModel.CurrentInstance().DatabasePort);
                },
                () =>
                {
                    // 测试数据库连接
                    if (!connecttResult)
                    {
                        Messenger.Default.Send<MessageParameterDialogTitleAndMessage>(new MessageParameterDialogTitleAndMessage()
                        {
                            Title = "Failed",
                            Message = "数据库连接测试失败，请检查设置！",
                        }, MainWindowTokens.ShowMessageDialog);
                        return;
                    }
                    Messenger.Default.Send<object>(null, MenuMessageTokens.FetchProjectDataFromDatabase);
                },
                "请等待",
                "数据库连接测试中，请稍等……",
                false
            );
        }

        private bool _projectExist = false;
        private ObservableCollection<HistorySection> _historySource;
        //private string _openHistoryPath;

        public bool ProjectExist
        {
            get { return _projectExist; }
            set
            {
                if (value == _projectExist)
                {
                    return;
                }
                _projectExist = value;
                RaisePropertyChanged(() => ProjectExist);
            }
        }

        public ObservableCollection<HistorySection> HistorySource {
            get { return _historySource; }
            set {
                if (value == _historySource) {
                    return;
                }
                _historySource = value;
                RaisePropertyChanged(() => HistorySource);
            }
        } 

        private void OnOpenHistory(string path) {
            XmlDocument ProjFile = new XmlDocument();
            ProjFile.Load(path);

            _projectConfig.Path = path;

            XmlNode root = ProjFile.SelectSingleNode("Project");
            XmlNode database = root.SelectSingleNode("DataBase");
            XmlNode resource = root.SelectSingleNode("Resource");

            _projectConfig.ProjectName = getValue(root, "Name");
            _projectConfig.ProjectPath = getValue(root, "ProjectPath");
            _projectConfig.DatabaseIP = getValue(database, "DataBaseIP");
            _projectConfig.DatabasePort = getValue(database, "DataBasePort");
            _projectConfig.DatabaseName = getValue(database, "DataBaseName");
            _projectConfig.DatabaseUserName = getValue(database, "DataBaseUserName");
            _projectConfig.DatabasePassword = getValue(database, "DataBasePassword");
            _projectConfig.MapSourcePath = getValue(resource, "MapSourcePath");
            _projectConfig.MapSourceOutputPath = getValue(resource, "MapSourceOutputPath");
            _projectConfig.NpcPicturePath = getValue(resource, "NpcPicturePath");
            _projectConfig.MonsterPicturePath = getValue(resource, "MonsterPicturePath");
            _projectConfig.MapSoundPath = getValue(resource, "MapSoundPath");

            ProjectExist = true;
            ProjectConfigModel.SetCurrentInstance();
            ProjectConfigModel.SetNewInstance();

            _projectConfig = ProjectConfigModel.Instance();
            Messenger.Default.Send<object>(null, MenuMessageTokens.UpdateCurrentProjectConfig);

            XmlDocument InHisFile = new XmlDocument();
            InHisFile.Load("history.xml");
            List<HistorySection> list = getList(InHisFile);
            int nowIndex = list.FindIndex(new Predicate<HistorySection>((section) =>
            {
                return section.ProjectPath == ProjectConfigModel.CurrentInstance().Path;
            }));
            if(nowIndex != -1)
            {
                list.RemoveAt(nowIndex);
            }
            list.Insert(0, new HistorySection(ProjectConfigModel.CurrentInstance().Path));
            writeList(list);

            // 从文件目录获取音乐
            DirectoryInfo dir = new DirectoryInfo(ProjectConfigModel.CurrentInstance().MapSoundPath);
            FileInfo[] inf = dir.GetFiles();
            //ObservableCollection<MusicSection> newMusicSection = new ObservableCollection<MusicSection>();
            var newSection = from mus in inf
                                  where mus.Extension.Equals(".mp3")
                                  select new MusicSection()
                                  {
                                      Name = mus.Name,
                                  };
            ObservableCollection<MusicSection> newMusicSection = new ObservableCollection<MusicSection>(newSection.ToList<MusicSection>());
            //foreach (FileInfo e in inf) {
            //    if (e.Extension.Equals(".mp3")) {
            //        newMusicSection.Add(new MusicSection()
            //        {
            //            Name = e.FullName.Replace(_projectConfig.MapSoundPath, ""),
            //        });
            //    }
            //}

            Messenger.Default.Send<ObservableCollection<MusicSection>>(newMusicSection, MainWindowTokens.UpdateMusicSections);

            SetTitleAndTryConnect();
        }

        private static byte[] compressBytes(byte[] sourceByte)
        {
            MemoryStream inputStream = new MemoryStream(sourceByte);
            Stream outStream = compressStream(inputStream);
            byte[] outPutByteArray = new byte[outStream.Length];
            outStream.Position = 0;
            outStream.Read(outPutByteArray, 0, outPutByteArray.Length);
            outStream.Close();
            inputStream.Close();
            return outPutByteArray;
        }

        private static Stream compressStream(Stream sourceStream)
        {
            MemoryStream streamOut = new MemoryStream();
            ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
            CopyStream(sourceStream, streamZOut);
            streamZOut.finish();
            return streamOut;
        }

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        private string getValue(XmlNode father, string str)
        {
            return father.SelectSingleNode(str).InnerText;
        }

        private List<HistorySection> getList(XmlDocument doc) {
            List<HistorySection> result = new List<HistorySection>();

            XmlNode root = doc.SelectSingleNode("Project");
            XmlNodeList paths = root.ChildNodes;

            foreach (XmlNode path in paths) {
                result.Add(new HistorySection(path.InnerText));
            }

            return result;
        }

        private void writeList(List<HistorySection> list) {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = doc.CreateXmlDeclaration("1.0", "gb2312", null);
            doc.AppendChild(xmldecl);

            XmlNode root = doc.CreateElement("", "Project", "");

            foreach (HistorySection his in list) {
                XmlNode newPath = doc.CreateElement("", "Path", "");
                newPath.InnerText = his.ProjectPath;
                root.AppendChild(newPath);
            }

            doc.AppendChild(root);

            doc.Save("history.xml");
        }
    }
}
