﻿
using System;

namespace ZuPix.ImageFilter
{
    public class WaterWaveFilter : RadialDistortionFilter
    {
        int width;
        int height;
        short[] buf2;
        short[] buf1;
        int[] temp;
        int[] source;

       
        //@Override
        public override CustomImage process(CustomImage imageIn)
        {
            width = imageIn.getWidth();
            height = imageIn.getHeight();
            buf2 = new short[width * height];
            buf1 = new short[width * height];
            source = imageIn.colorArray;
            temp = new int[source.Length];
            DropStone(width / 2, height / 2, Math.Max(width, height) / 4, Math.Max(width, height));
            for (int i = 0; i < 170; i++)
            {
                RippleSpread();
                render();
            }
            imageIn.colorArray = temp;
            return imageIn;
        }

        void DropStone(int x /*x坐标*/, int y /*y坐标*/, int stonesize /*波源半径*/, int stoneweight /*波源能量*/)
        {
            if ((x + stonesize) > width || (y + stonesize) > height || (x - stonesize) < 0 || (y - stonesize) < 0)
            {
                return;
            }
            for (int posx = x - stonesize; posx < x + stonesize; posx++)
            {
                for (int posy = y - stonesize; posy < y + stonesize; posy++)
                {
                    if ((posx - x) * (posx - x) + (posy - y) * (posy - y) <= stonesize * stonesize)
                    {
                        buf1[width * posy + posx] = (short)-stoneweight;
                    }
                }
            }
        }


        void RippleSpread()
        {
            for (int i = width; i < width * height - width; i++)
            {
                //波能扩散
                buf2[i] = (short)(((buf1[i - 1] + buf1[i + 1] + buf1[i - width] + buf1[i + width]) >> 1) - buf2[i]);
                //波能衰减
                buf2[i] -= (short)(buf2[i] >> 5);
            }
            //交换波能数据缓冲区
            short[] tmp = buf1;
            buf1 = buf2;
            buf2 = tmp;
        }


        /* 渲染你水纹效果 */
        void render()
        {
            int xoff, yoff;
            int k = width;
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //计算偏移量
                    xoff = buf1[k - 1] - buf1[k + 1];
                    yoff = buf1[k - width] - buf1[k + width];
                    //判断坐标是否在窗口范围内
                    if ((i + yoff) < 0 || (i + yoff) >= height || (j + xoff) < 0 || (j + xoff) >= width)
                    {
                        k++;
                        continue;
                    }
                    //Calculate the offset of the memory address of the pixel and the original pixel offset	   
                    // image.setPixelColour(j, i, clone.getPixelColour(j+xoff, i+yoff));	
                    int pos1, pos2;
                    pos1 = width * (i + yoff) + (j + xoff);
                    pos2 = width * i + j;
                    temp[pos2++] = source[pos1++];
                    k++;
                }
            }
        }
    }

}
