
using System.Collections.Generic;
using System.Windows.Media;

namespace ZuPix.ImageFilter
{
    public class RainBowFilter : IImageFilter
    {

        public ImageBlender blender = new ImageBlender();
        public bool IsDoubleRainbow = false;
        private GradientFilter gradientFx;
        public float gradAngleDegree = 40f;

        public RainBowFilter()
        {
            blender.Mixture = 0.25f;
            blender.Mode = BlendMode.Additive;

            IsDoubleRainbow = true;

            List<Color> rainbowColors = Gradient.RainBow().MapColors;
            if (this.IsDoubleRainbow)
            {
                rainbowColors.RemoveAt(rainbowColors.Count - 1);//remove red
                rainbowColors.AddRange(Gradient.RainBow().MapColors);
            }
            gradientFx = new GradientFilter();
            gradientFx.OriginAngleDegree = gradAngleDegree;
            gradientFx.Gradientf = new Gradient(rainbowColors);
        }

        public string Name { get { return "Rainbow"; } }

        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            CustomImage clone = gradientFx.process(imageIn.clone());
            return blender.Blend(imageIn, clone);
        }
    }

}
