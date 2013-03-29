
namespace ZuPix.ImageFilter
{
    public class SaturationModifyFilter : IImageFilter
    {

        public float SaturationFactor = 0.5f;
        public string Name { get { return "Saturation"; } }
        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            float saturation = this.SaturationFactor + 1f;
            float negosaturation = 1f - saturation;

            int r, g, b, a;
            for (int x = 0; x < imageIn.getWidth(); x++)
            {
                for (int y = 0; y < imageIn.getHeight(); y++)
                {
                    r = imageIn.getRComponent(x, y);
                    g = imageIn.getGComponent(x, y);
                    b = imageIn.getBComponent(x, y);

                    float nego1 = negosaturation * 0.2126f;
                    float ngeo2 = nego1 + saturation;
                    float ngeo3 = negosaturation * 0.7152f;
                    float nego4 = ngeo3 + saturation;
                    float nego5 = negosaturation * 0.0722f;
                    float nego6 = nego5 + saturation;
                    float nego7 = ((r * ngeo2) + (g * ngeo3)) + (b * nego5);
                    float nego8 = ((r * nego1) + (g * nego4)) + (b * nego5);
                    float nego9 = ((r * nego1) + (g * ngeo3)) + (b * nego6);
                    r = (nego7 > 255f) ? ((byte)255f) : ((nego7 < 0f) ? ((byte)0f) : ((byte)nego7));
                    g = (nego8 > 255f) ? ((byte)255f) : ((nego8 < 0f) ? ((byte)0f) : ((byte)nego8));
                    b = (nego9 > 255f) ? ((byte)255f) : ((nego9 < 0f) ? ((byte)0f) : ((byte)nego9));
                    imageIn.setPixelColor(x, y, r, g, b);
                }
            }
            return imageIn;
        }

    }

}
