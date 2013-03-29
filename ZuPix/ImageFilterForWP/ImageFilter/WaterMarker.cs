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
using System.Windows.Media.Imaging;

namespace ZuPix.ImageFilter
{
    /// <summary>
    /// Applys a watermark logo a bitmap.
    /// </summary>
    public class Watermarker
    {
        /// <summary>
        /// A watermark
        /// </summary>
        public WriteableBitmap Watermark { get; private set; }

        /// <summary>
        /// The relative size of the watermark in proportion to the input image.
        /// </summary>
        public float RelativeSize { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watermarker"/> class.
        /// </summary>
        /// <param name="relativeResourcePath">The relative resource path.</param>
        public Watermarker(string relativeResourcePath)
        {
            Watermark = new WriteableBitmap(0, 0).FromResource(relativeResourcePath);
            RelativeSize = 0.5f;
        }

        /// <summary>
        /// Applies a watermark bitmap and returns a new processed WriteabelBitmap.
        /// </summary>
        /// <param name="input">The input bitmap.</param>
        /// <returns>The result WriteableBitmap with the watermark.</returns>
        public WriteableBitmap Apply(WriteableBitmap input)
        {
            // Resize watermark
            var w = Watermark.PixelWidth;
            var h = Watermark.PixelHeight;
            var ratio = (float)w / h;
            if (ratio > 1)
            {
                w = (int)(input.PixelWidth * RelativeSize);
                h = (int)(w / ratio);
            }
            else
            {
                h = (int)(input.PixelHeight * RelativeSize);
                w = (int)(h * ratio);
            }
            var watermark = Watermark.Resize(w, h, WriteableBitmapExtensions.Interpolation.Bilinear);

            // Blit watermark into copy of the input 
            // Bottom right corner
            var result = input.Clone();
            var position = new Rect(input.PixelWidth - w, input.PixelHeight - h, w, h);
            result.Blit(position, watermark, new Rect(0, 0, w, h));

            return result;
        }
    }
}
