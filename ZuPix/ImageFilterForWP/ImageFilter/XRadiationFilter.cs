
using System.Collections.Generic;
using System.Windows.Media;

namespace ZuPix.ImageFilter
{
    public class XRadiationFilter : IImageFilter
    {

        private GradientMapFilter gradientMapFx = new GradientMapFilter();
        private ImageBlender blender = new ImageBlender();

        public XRadiationFilter()
        {
            List<Color> colors = new List<Color>();
            colors.Add(TintColors.LightCyan());
            colors.Add(Colors.Black);
            gradientMapFx.Map = new Gradient(colors);
            blender.Mode = BlendMode.ColorBurn;
            blender.Mixture = 0.8f;
        }

        public string Name { get { return "Radiation"; } }
        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            imageIn = this.gradientMapFx.process(imageIn);
            imageIn = this.blender.Blend(imageIn, imageIn);
            return imageIn;
        }
    }

}
