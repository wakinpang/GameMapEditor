using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MapEditorControl.InnerUtil
{
    static class BitmapExtend
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }


    class PathToSpriteBitmapConverter : System.Windows.Data.IValueConverter
    {
        public string RootPath { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path;

            if (RootPath != "")
            {
                path = RootPath + "\\" + ((int)value).ToString() + ".png";
            }
            else
            {
                path = value as string;
            }

            bool success = true;
            BitmapImage bitmap = null;

            if (!File.Exists(path))
            {
                success = false;
            }
            else
            {
                var url = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap = new BitmapImage();
                bool loaded = true;
                bitmap.BeginInit();
                bitmap.UriSource = url;

                bitmap.DecodeFailed += (sender, ec) =>
                {
                    loaded = false;
                };
                bitmap.EndInit();

                success = loaded;
            }

            if(success)
            {
                return bitmap;
            }
            else
            {
                var coloredBmp = new Bitmap(64, 64);
                var g = Graphics.FromImage(coloredBmp);
                g.FillRectangle(Brushes.Gray, 0, 0, coloredBmp.Width, coloredBmp.Height);

                return coloredBmp.ToBitmapImage();
            }
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
