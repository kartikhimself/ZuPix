using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;

namespace ZuPix.Domain
{
    public class ImageUnit
    {
        public byte[] ImageContents { get; set; }
        public string ImageName { get; set; }
        public BitmapImage Source
        {
            get
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(new MemoryStream(ImageContents));
                return image;
            }
            set
            {
                Source = value;
            }
        }
    }
}
