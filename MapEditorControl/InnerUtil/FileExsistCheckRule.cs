using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MapEditorControl.InnerUtil
{
    public class InnerFileInfo : DependencyObject
    {

        public string RootPath
        {
            get { return (string)GetValue(RootPathProperty); }
            set
            {
                if (value == (string)GetValue(RootPathProperty))
                {
                    return;
                }
                SetValue(RootPathProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for RootPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RootPathProperty =
            DependencyProperty.Register("RootPath", typeof(string), typeof(InnerFileInfo), new PropertyMetadata());

        public string FileExtension
        {
            get { return (string)GetValue(FileExtensionProperty); }
            set
            {
                if (value == (string)GetValue(FileExtensionProperty))
                {
                    return;
                }
                SetValue(FileExtensionProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for FileExtension.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileExtensionProperty =
            DependencyProperty.Register("FileExtension", typeof(string), typeof(InnerFileInfo), new PropertyMetadata());
        
    }

    public class FileExsistCheckRule : ValidationRule
    {
        public InnerFileInfo FileInfo { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(!File.Exists(FileInfo.RootPath + "\\" + (value as string) + FileInfo.FileExtension))
            {
                return new ValidationResult(false, String.Format("文件 {0}\\{1}{2} 不存在。", FileInfo.RootPath, value as string, FileInfo.FileExtension));
            }

            return ValidationResult.ValidResult;
        }
    }
}
