

namespace ZuPix.ImageFilter
{
    public interface IImageFilter
    {
        string Name { get; }
        CustomImage process(CustomImage imageIn);
    }
}
