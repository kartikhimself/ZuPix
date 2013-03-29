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

namespace ZuPix.ImageFilter
{
    public class TintFilterNew:IImageFilter
    {
       
            public string Name { get { return "Tint"; } }
            Color Color = Color.FromArgb(255, 230, 179, 77);
            public CustomImage process(CustomImage imageIn)
            {
                int ta = Color.A;
                int tr = Color.R;
                int tg = Color.G;
                int tb = Color.B;
                int r, g, b;
                for (int x = 0; x < imageIn.getWidth(); x++)
                {
                    for (int y = 0; y < imageIn.getHeight(); y++)
                    {
                        r = (255 - imageIn.getRComponent(x, y));
                        g = (255 - imageIn.getGComponent(x, y));
                        b = (255 - imageIn.getBComponent(x, y));

                        // Convert to gray with constant factors 0.2126, 0.7152, 0.0722
                        int gray = (r * 6966 + g * 23436 + b * 2366) >> 15;

                        // Apply Tint color
                        r = (byte)((gray * tr) >> 8);
                        g = (byte)((gray * tg) >> 8);
                        b = (byte)((gray * tb) >> 8);

                        imageIn.setPixelColor(x, y, r, g, b);
                    }
                }
                return imageIn;
            }
        }

    
}
