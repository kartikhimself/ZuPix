

namespace ZuPix.ImageFilter
{
    public class ColorQuantizeFilter : IImageFilter
    {

        private float levels = 5f;

        public string Name { get { return "Colorize"; } }

        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            int r, g, b, a;
            for (int x = 0; x < imageIn.getWidth(); x++)
            {
                for (int y = 0; y < imageIn.getHeight(); y++)
                {
                    r = imageIn.getRComponent(x, y);
                    g = imageIn.getGComponent(x, y);
                    b = imageIn.getBComponent(x, y);
                    float quanR = (((float)((int)(r * 0.003921569f * levels))) / levels) * 255f;
                    float quanG = (((float)((int)(g * 0.003921569f * levels))) / levels) * 255f;
                    float quanB = (((float)((int)(b * 0.003921569f * levels))) / levels) * 255f;
                    r = (quanR > 255f) ? 255 : ((quanR < 0f) ? 0 : ((byte)quanR));
                    g = (quanG > 255f) ? 255 : ((quanG < 0f) ? 0 : ((byte)quanG));
                    b = (quanB > 255f) ? 255 : ((quanB < 0f) ? 0 : ((byte)quanB));
                    imageIn.setPixelColor(x, y, r, g, b);
                }
            }
            return imageIn;
        }

    }
}
