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

namespace ZuPix.ViewModels
{
    public class CategoryItem
    {
        /// <summary>
        /// Main name of the item
        /// </summary>
        public string MainName { get; private set; }

        /// <summary>
        /// Subname of the item
        /// </summary>
        public string SubName { get; private set; }

        public BitmapImage Thumbnail { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CategoryItem(string mainName, string subName, BitmapImage image)
        {
            MainName = mainName;
            SubName = subName;
            Thumbnail = image;
        }
    }
}
