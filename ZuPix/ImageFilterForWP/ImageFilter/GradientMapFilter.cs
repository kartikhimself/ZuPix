

namespace ZuPix.ImageFilter
{
    public class GradientMapFilter : IImageFilter{
	
	public GradientMapFilter()
	{
		this.Map = new Gradient();
	}


	public GradientMapFilter(Gradient gradient)
	{
	    this.Map = gradient;
	    this.BrightnessFactor = this.ContrastFactor = 0f;
	}

	public Gradient Map;
	public float BrightnessFactor;
	public float ContrastFactor;

    public string Name { get { return "Gradient"; } }
	//@Override
    public CustomImage process(CustomImage imageIn)
    {
    	Palette palette = this.Map.CreatePalette(0x100);
	    byte[] red = palette.Red;
	    byte[] green = palette.Green;
	    byte[] blue = palette.Blue;
	    CustomImage bitmap = imageIn.clone();
	    bitmap.clearImage((255 << 24) + (255 << 16) + (255 << 8) + 255);
	    int bfactor = (int) (this.BrightnessFactor * 255f);
	    float cfactor = 1f + this.ContrastFactor;
	    cfactor *= cfactor;
	    int limit = ((int) (cfactor * 32768f)) + 1;
	    for (int i = 0; i < imageIn.colorArray.Length; i++)
	    {
	        int r = (imageIn.colorArray[i]& 0x00FF0000) >> 16;
	        int g = (imageIn.colorArray[i]& 0x0000FF00) >> 8;
	        int b = imageIn.colorArray[i]& 0x000000FF;
	        int index = (((r * 0x1b36) + (g * 0x5b8c)) + (b * 0x93e)) >> 15;
	        if (bfactor != 0)
	        {
	            index += bfactor;
	            index = (index > 0xff) ? 0xff : ((index < 0) ? 0 : index);
	        }
	        if (limit != 0x8001)
	        {
	            index -= 0x80;
	            index = (index * limit) >> 15;
	            index += 0x80;
	            index = (index > 0xff) ? 0xff : ((index < 0) ? 0 : index);
	        }
	        bitmap.colorArray[i] = (0xff << 24) + (red[index] << 16) + (green[index] << 8) + blue[index];
	    }
	    return bitmap;   	
    }
}

}
