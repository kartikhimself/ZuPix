

namespace ZuPix.ImageFilter
{
    public class SepiaFilter : IImageFilter
    {

        private GradientMapFilter gradientMapFx;
        private SaturationModifyFilter saturationFx;
        public SepiaFilter()
        {
            gradientMapFx = new GradientMapFilter(Gradient.BlackSepia());
            gradientMapFx.ContrastFactor = 0.2f;
            gradientMapFx.BrightnessFactor = 0.1f;

            saturationFx = new SaturationModifyFilter();
            saturationFx.SaturationFactor = -0.6f;
        }

        public string Name { get { return "Sepia"; } }
        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            imageIn = gradientMapFx.process(imageIn);
            return saturationFx.process(imageIn);
        }

    }


}
