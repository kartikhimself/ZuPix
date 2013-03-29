

using System.Windows.Media;

namespace ZuPix.ImageFilter
{
    public class TintFilter : IImageFilter
    {
        public string Name { get { return "Tint"; } }
        public CustomImage process(CustomImage imageIn)
        {
            int tr = (255 << 24) + (Colors.Red.R << 16) + (Colors.Red.G << 8) + Colors.Red.B;
            int tg = (255 << 24) + (Colors.Green.R << 16) + (Colors.Green.G << 8) + Colors.Green.B;
            int tb = (255 << 24) + (Colors.Blue.R << 16) + (Colors.Blue.G << 8) + Colors.Blue.B;
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
