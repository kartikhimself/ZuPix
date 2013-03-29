using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ZuPix.ImageFilter
{
    public class PolaroidFilter :IImageFilter
    {
        private GaussianBlurFilter blurFx;
        private SepiaFilter sepiaFx;
        private TintEffect tintFx;
        
        private VignetteFilter vignetteFx;
        
        public PolaroidFilter()
        {
            blurFx = new GaussianBlurFilter();
            blurFx.Sigma = 0.3f;

            sepiaFx = new SepiaFilter();
            tintFx = new TintEffect();
            
            
            vignetteFx = new VignetteFilter();
            vignetteFx.Size = 0.6f;

            
        }

        public string Name { get { return "Polaroid"; } }
        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            imageIn = this.tintFx.process(this.blurFx.process(imageIn));
            
            return this.vignetteFx.process(imageIn);
        }

    }
}
