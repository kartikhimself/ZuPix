


using System.Collections.Generic;
using System.Windows.Media;

namespace ZuPix.ImageFilter
{
    public class NightVisionFilter : IImageFilter
    {

        private NoiseFilter noisefx = new NoiseFilter();
        private ImageBlender blender = new ImageBlender();
        private VignetteFilter vignetteFx = new VignetteFilter();
        private GradientMapFilter gradientFx = new GradientMapFilter();

        public NightVisionFilter()
        {
            noisefx.Intensity = 0.15f;

            vignetteFx.Size = 1f;

            List<Color> colors = new List<Color>();
            colors.Add(Colors.Black);
            colors.Add(Colors.Green);
            gradientFx.Map = new Gradient(colors);
            gradientFx.BrightnessFactor = 0.2f;
        }

        public string Name { get { return "Nighth Vision"; } }

        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            imageIn = noisefx.process(imageIn);
            imageIn = gradientFx.process(imageIn);
            imageIn = vignetteFx.process(imageIn);
            return imageIn;
        }
    }
}
