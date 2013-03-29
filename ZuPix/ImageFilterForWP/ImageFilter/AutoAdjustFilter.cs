


namespace ZuPix.ImageFilter
{
    public class AutoAdjustFilter : IImageFilter{

     public string Name { get { return "Auto Adjust"; } }
	 //@Override
    public CustomImage process(CustomImage imageIn) {
     	HistogramEqualFilter hee = new HistogramEqualFilter();
    	hee.ContrastIntensity = 0.5f;
    	imageIn = hee.process(imageIn);
    	
    	AutoLevelFilter ale = new AutoLevelFilter();
    	ale.Intensity = 0.5f;
    	return ale.process(imageIn);	
    }
}
}
