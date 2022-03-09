using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Xml;

namespace GameMapEditor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherHelper.Initialize();

            if (!File.Exists("history.xml")) {
                XmlDocument HisFile = new XmlDocument();
                XmlDeclaration dec = HisFile.CreateXmlDeclaration("1.0", "gb2312", null);
                HisFile.AppendChild(dec);
                HisFile.AppendChild(HisFile.CreateElement("Project"));
                HisFile.Save("history.xml");
            }
        }
    }
}
