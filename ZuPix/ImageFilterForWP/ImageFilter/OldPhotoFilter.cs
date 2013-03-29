

namespace ZuPix.ImageFilter
{
    public class OldPhotoFilter : IImageFilter
    {

        private GaussianBlurFilter blurFx;
        private NoiseFilter noiseFx;
        private VignetteFilter vignetteFx;
        private GradientMapFilter gradientFx;
        public OldPhotoFilter()
        {
            blurFx = new GaussianBlurFilter();
            blurFx.Sigma = 0.3f;

            noiseFx = new NoiseFilter();
            noiseFx.Intensity = 0.03f;

            vignetteFx = new VignetteFilter();
            vignetteFx.Size = 0.6f;

            gradientFx = new GradientMapFilter();
            gradientFx.ContrastFactor = 0.3f;
        }

        public string Name { get { return "Old Me"; } }
        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            imageIn = this.noiseFx.process(this.blurFx.process(imageIn));
            imageIn = this.gradientFx.process(imageIn);
            return this.vignetteFx.process(imageIn);
        }
    }
}
