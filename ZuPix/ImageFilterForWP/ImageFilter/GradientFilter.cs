


using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ZuPix.ImageFilter
{
    public class Palette
    {
        public byte[] Blue;
        public byte[] Green;
        public int Length;
        public byte[] Red;

        public Palette(int length)
        {
            this.Length = length;
            this.Red = new byte[length];
            this.Green = new byte[length];
            this.Blue = new byte[length];
        }
    }

    public class TintColors
    {
        public static Color LightCyan()
        {
            return Color.FromArgb(255, 0xeb, 0xf5, 0xe1);
        }

        public static Color Sepia()
        {
            return Color.FromArgb(255, 230, 179, 179);
        }
    }

    public class Gradient
    {
        public List<Color> MapColors;

        public Gradient()
        {
            List<Color> list = new List<Color>();
            list.Add(Colors.Black);
            list.Add(Colors.White);
            this.MapColors = list;
        }

        public Gradient(List<Color> colors)
        {
            this.MapColors = colors;
        }


        private Palette CreateGradient(List<Color> colors, int length)
        {
            if (colors == null || colors.Count < 2)
            {
                return null;
            }

            Palette palette = new Palette(length);
            byte[] red = palette.Red;
            byte[] green = palette.Green;
            byte[] blue = palette.Blue;
            int num = length / (colors.Count - 1);
            float num1 = 1f / ((float)num);
            int index = 0;
            Color rgb = colors[0];
            int colorR = rgb.R;
            int colorG = rgb.G;
            int colorB = rgb.B;
            for (int i = 1; i < colors.Count; i++)
            {
                int r = colors[i].R;
                int g = colors[i].G;
                int b = colors[i].B;
                for (int j = 0; j < num; j++)
                {
                    float num2 = j * num1;
                    int rr = colorR + ((int)((r - colorR) * num2));
                    int gg = colorG + ((int)((g - colorG) * num2));
                    int bb = colorB + ((int)((b - colorB) * num2));
                    red[index] = (byte)(rr > 0xff ? 0xff : ((rr < 0) ? 0 : rr));
                    green[index] = (byte)(gg > 0xff ? 0xff : ((gg < 0) ? 0 : gg));
                    blue[index] = (byte)(bb > 0xff ? 0xff : ((bb < 0) ? 0 : bb));
                    index++;
                }
                colorR = r;
                colorG = g;
                colorB = b;
            }
            if (index < length)
            {
                red[index] = red[index - 1];
                green[index] = green[index - 1];
                blue[index] = blue[index - 1];
            }
            return palette;
        }

        public Palette CreatePalette(int length)
        {
            return CreateGradient(this.MapColors, length);
        }

      

        public static Gradient BlackSepia()
        {
            List<Color> colors = new List<Color>();
            colors.Add(Colors.Black);
            colors.Add(TintColors.Sepia());
            return new Gradient(colors);
        }

        public static Gradient WhiteSepia()
        {
            List<Color> colors = new List<Color>();
            colors.Add(Colors.White);
            colors.Add(TintColors.Sepia());
            return new Gradient(colors);
        }

        public static Gradient RainBow()
        {
            List<Color> colors = new List<Color>();
            colors.Add(Colors.Red);
            colors.Add(Colors.Magenta);
            colors.Add(Colors.Blue);
            colors.Add(Colors.Cyan);
            colors.Add(Colors.Green);
            colors.Add(Colors.Yellow);
            colors.Add(Colors.Red);
            return new Gradient(colors);
        }

        public static Gradient Inverse()
        {
            List<Color> colors = new List<Color>();
            colors.Add(Colors.White);
            colors.Add(Colors.Black);
            return new Gradient(colors);
        }
    }


    public class GradientFilter : IImageFilter
    {
        private Palette palette = null;
        public Gradient Gradientf;
        public float OriginAngleDegree;

        // Methods
        public GradientFilter()
        {
            this.OriginAngleDegree = 0f;
            this.Gradientf = new Gradient();
        }

        public void ClearCache()
        {
            this.palette = null;
        }

        public string Name { get { return "Gradient"; } }

        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            int width = imageIn.getWidth();
            int height = imageIn.getHeight();
            double d = this.OriginAngleDegree * 0.0174532925;
            float cos = (float)Math.Cos(d);
            float sin = (float)Math.Sin(d);
            float radio = (cos * width) + (sin * height);
            float dcos = cos * radio;
            float dsin = sin * radio;
            int dist = (int)Math.Sqrt((double)((dcos * dcos) + (dsin * dsin)));
            dist = Math.Max(Math.Max(dist, width), height);

            if ((this.palette == null) || (dist != this.palette.Length))
            {
                this.palette = this.Gradientf.CreatePalette(dist);
            }
            byte[] red = this.palette.Red;
            byte[] green = this.palette.Green;
            byte[] blue = this.palette.Blue;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    radio = (cos * j) + (sin * i);
                    dcos = cos * radio;
                    dsin = sin * radio;
                    dist = (int)Math.Sqrt((double)((dcos * dcos) + (dsin * dsin)));
                    imageIn.setPixelColor(j, i, red[dist], green[dist], blue[dist]);
                }
            }
            return imageIn;
        }
    }
}
