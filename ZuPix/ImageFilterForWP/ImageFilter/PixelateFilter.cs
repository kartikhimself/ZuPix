
namespace ZuPix.ImageFilter
{
    public class PixelateFilter : IImageFilter
    {

        /**
         * Size of the blurred pixel - the bigger this is, the more
         * coarsely the image will be pixelated. The pixelation appearence
         * will always be different for different size images
         */
        private int pixelSize = 4;
        public string Name { get { return "Pixelate"; } }
        //@Override
        public CustomImage process(CustomImage imageIn)
        {
            int l_rgb;
            for (int x = 0; x < imageIn.getWidth(); x += pixelSize)
            {
                for (int y = 0; y < imageIn.getHeight(); y += pixelSize)
                {
                    l_rgb = getPredominantRGB(imageIn, x, y, pixelSize);
                    fillRect(imageIn, x, y, pixelSize, l_rgb);
                }
            }

            return imageIn;
        }



        /**
         * @return the pixelSize
         */
        public int getPixelSize()
        {
            return pixelSize;
        }



        /**
         * @param pixelSize the pixelSize to set
         */
        public void setPixelSize(int pixelSize)
        {
            this.pixelSize = pixelSize;
        }



        /**
         * Method gets the predominant colour pixels to extrapolate
         * the pixelation from
         * 
         * @param imageIn
         * @param a_x
         * @param a_y
         * @param squareSize
         * @return
         */
        private int getPredominantRGB(CustomImage imageIn, int a_x, int a_y, int squareSize)
        {
            int red = -1;
            int green = -1;
            int blue = -1;

            for (int x = a_x; x < a_x + squareSize; x++)
            {
                for (int y = a_y; y < a_y + squareSize; y++)
                {
                    if (x < imageIn.getWidth() && y < imageIn.getHeight())
                    {
                        if (red == -1)
                        {
                            red = imageIn.getRComponent(x, y);
                        }
                        else
                        {
                            red = (red + imageIn.getRComponent(x, y)) / 2;
                        }
                        if (green == -1)
                        {
                            green = imageIn.getGComponent(x, y);
                        }
                        else
                        {
                            green = (green + imageIn.getGComponent(x, y)) / 2;
                        }
                        if (blue == -1)
                        {
                            blue = imageIn.getBComponent(x, y);
                        }
                        else
                        {
                            blue = (blue + imageIn.getBComponent(x, y)) / 2;
                        }
                    }
                }
            }
            return (255 << 24) + (red << 16) + (green << 8) + blue;
        }

        /**
         * Method to extrapolate out
         * 
         * @param imageIn
         * @param a_x
         * @param a_y
         * @param squareSize
         * @param a_rgb
         */
        private void fillRect(CustomImage imageIn, int a_x, int a_y, int squareSize, int a_rgb)
        {
            for (int x = a_x; x < a_x + squareSize; x++)
            {
                for (int y = a_y; y < a_y + squareSize; y++)
                {
                    if (x < imageIn.getWidth() && y < imageIn.getHeight())
                    {
                        imageIn.setPixelColor(x, y, a_rgb);
                    }
                }
            }
        }

    }

}
