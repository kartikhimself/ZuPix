﻿
using System.Windows.Media;

namespace ZuPix.ImageFilter
{
    public class RectMatrixFilter : RadialDistortionFilter
    {

       
        public int Oriention = 0;

        public int BannerNum = 15;

        public string Name { get { return "Maltrix"; } }
        //@Override
        public override CustomImage process(CustomImage imageIn)
        {
            int width = imageIn.getWidth();
            int height = imageIn.getHeight();
            int r = 0, g = 0, b = 0;

            CustomImage clone = imageIn.clone();
            clone.clearImage((255 << 24) + (Colors.LightGray.R << 16) + (Colors.LightGray.G << 8) + Colors.LightGray.B);

            
            Point[] point = new Point[BannerNum];
            int dh = height / BannerNum;
            int dw = width;
            for (int i = 0; i < BannerNum; i++)
            {
                point[i] = new Point(0, i * dh);
            }
            for (int x = 0; x < dh; x++)
            {
                for (int y = 0; y < BannerNum; y++)
                {
                    for (int k = 0; k < dw; k++)
                    {
                        int xx = (int)point[y].X + k;
                        int yy = (int)point[y].Y + (int)(x / 1.8);
                        r = imageIn.getRComponent(xx, yy);
                        g = imageIn.getGComponent(xx, yy);
                        b = imageIn.getBComponent(xx, yy);
                        clone.setPixelColor(xx, yy, r, g, b);
                    }
                }
            }
            //对图像其余部分做填充
            for (int xx = 0; xx < width; xx++)
            {
                for (int yy = (int)point[BannerNum - 1].Y + dh; yy < height; yy++)
                {
                    r = imageIn.getRComponent(xx, yy);
                    g = imageIn.getGComponent(xx, yy);
                    b = imageIn.getBComponent(xx, yy);
                    clone.setPixelColor(xx, yy, r, g, b);
                }
            }

            //垂直方向
            point = new Point[BannerNum];
            dw = width / BannerNum;
            dh = height;
            for (int i = 0; i < BannerNum; i++)
            {
                point[i] = new Point(i * dw, 0);
            }
            for (int x = 0; x < dw; x++)
            {
                for (int y = 0; y < BannerNum; y++)
                {
                    for (int k = 0; k < dh; k++)
                    {
                        int xx = (int)point[y].X + (int)(x / 1.8);
                        int yy = (int)point[y].Y + k;
                        r = imageIn.getRComponent(xx, yy);
                        g = imageIn.getGComponent(xx, yy);
                        b = imageIn.getBComponent(xx, yy);
                        clone.setPixelColor(xx, yy, r, g, b);
                    }
                }
            }
           
            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = (int)point[BannerNum - 1].X + dw; xx < width; xx++)
                {
                    r = imageIn.getRComponent(xx, yy);
                    g = imageIn.getGComponent(xx, yy);
                    b = imageIn.getBComponent(xx, yy);
                    clone.setPixelColor(xx, yy, r, g, b);
                }
            }

            return clone;
        }
    }

}
