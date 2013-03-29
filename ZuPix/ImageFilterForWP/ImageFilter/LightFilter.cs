

using System;

namespace ZuPix.ImageFilter
{
    public class LightFilter : RadialDistortionFilter
    {

       
        
        public float Light = 150.0f;
        public string Name { get { return "Light"; } }

        //@Override
        public override CustomImage process(CustomImage imageIn)
        {
            int width = imageIn.getWidth();
            int halfw = width / 2;
            int height = imageIn.getHeight();
            int halfh = height / 2;
            int R = Math.Min(halfw, halfh);
           
            Point point = new Point(halfw, halfh);
            int r = 0, g = 0, b = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float length = (float)Math.Sqrt(Math.Pow((x - point.X), 2) + Math.Pow((y - point.Y), 2));
                    r = imageIn.getRComponent(x, y);
                    g = imageIn.getGComponent(x, y);
                    b = imageIn.getBComponent(x, y);
                   
                    if (length < R)
                    {
                        float pixel = Light * (1.0f - length / R);
                        r = r + (int)pixel;
                        r = Math.Max(0, Math.Min(r, 255));
                        g = g + (int)pixel;
                        g = Math.Max(0, Math.Min(g, 255));
                        b = b + (int)pixel;
                        b = Math.Max(0, Math.Min(b, 255));
                    }
                    imageIn.setPixelColor(x, y, r, g, b);
                }
            }
            return imageIn;
        }
    }

}