

using System;

namespace ZuPix.ImageFilter
{
    public class EdgeFilter : IImageFilter
    {
        public string Name { get { return "Edge"; } }

        //@Override
        public CustomImage process(CustomImage imageIn) {
        // Image size
        int width = imageIn.getWidth();
        int height = imageIn.getHeight();
        bool[][] mask = null;
        int[] grayMatrix = new int[256];

        // Init gray matrix
        for (int i = 0; i <= 255; i++) {
            grayMatrix[i] = (255 << 24) + (i << 16) + (i<< 8) + i;
        }

        int [,] luminance = new int[width,height];
        for (int y = 0; y < height ; y++) {
            for (int x = 0; x < width ; x++) {
                if(mask != null && !mask[x][y]){
                    continue;
                }
                luminance[x,y] = (int) Luminance(imageIn.getRComponent(x, y), imageIn.getGComponent(x, y), imageIn.getBComponent(x, y));
            }
        }


        int grayX, grayY;
        int magnitude;
        for (int y = 1; y < height-1; y++) {
            for (int x = 1; x < width-1; x++) {

                if(mask != null && !mask[x][y]){
                    continue;
                }

                grayX = - luminance[x-1,y-1] + luminance[x-1,y-1+2] - 2* luminance[x-1+1,y-1] + 2* luminance[x-1+1,y-1+2] - luminance[x-1+2,y-1]+ luminance[x-1+2,y-1+2];
                grayY = luminance[x-1,y-1] + 2* luminance[x-1,y-1+1] + luminance[x-1,y-1+2] - luminance[x-1+2,y-1] - 2* luminance[x-1+2,y-1+1] - luminance[x-1+2,y-1+2];

                // Magnitudes sum
                magnitude = 255 - truncate(Math.Abs(grayX) + Math.Abs(grayY));
                int grayscaleColor = grayMatrix[magnitude];

                // Apply the color into a new image
                imageIn.setPixelColor(x, y, grayscaleColor);
            }
        }

        return imageIn;
    }


        /**
         * Sets the RGB between 0 and 255
         *
         * @param a
         * @return
         */
        private int truncate(int a)
        {
            if (a < 0)
                return 0;
            else if (a > 255)
                return 255;
            else
                return a;
        }

        /**
         * Apply the luminance
         *
         * @param r
         * @param g
         * @param b
         * @return
         */
        private int Luminance(int r, int g, int b)
        {
            return (int)((0.299 * r) + (0.58 * g) + (0.11 * b));
        }

    }


}
