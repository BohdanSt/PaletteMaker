using ImageProcessor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteMaker.ImageProcessing
{
    public static class ImageEffect
    {
        public enum EffectType
        {
            None,
            GaussianBlur,
            GaussianSharpen,
            Pixelate,
            Vignette,
            BinaryThreshold,
            Halftone,
            OilPainting
        }

        public static void ApplyEffect(EffectType effectType, ref ImageFactory imageFactory)
        {
            switch (effectType)
            {
                case EffectType.GaussianBlur:
                    imageFactory.GaussianBlur(10);
                    break;
                case EffectType.GaussianSharpen:
                    imageFactory.GaussianSharpen(10);
                    break;
                case EffectType.Pixelate:
                    imageFactory.Pixelate(5);
                    break;
                case EffectType.Vignette:
                    imageFactory.Vignette(System.Drawing.Color.Black);
                    break;
                case EffectType.BinaryThreshold:
                    var binTreshold = new ImageProcessor.Imaging.Filters.Binarization.BinaryThreshold(75);
                    imageFactory.Load(binTreshold.ProcessFilter((Bitmap)imageFactory.Image));
                    break;
                case EffectType.Halftone:
                    var halftone = new ImageProcessor.Imaging.Filters.Artistic.HalftoneFilter(5);
                    imageFactory.Load(halftone.ApplyFilter((Bitmap)imageFactory.Image));
                    break;
                case EffectType.OilPainting:
                    var oilPainting = new ImageProcessor.Imaging.Filters.Artistic.OilPaintingFilter(3, 7);
                    imageFactory.Load(oilPainting.ApplyFilter((Bitmap)imageFactory.Image));
                    break;
                default:
                    break;
            }
        }
    }
}
