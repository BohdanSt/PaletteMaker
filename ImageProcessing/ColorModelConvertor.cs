using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteMaker.ImageProcessing
{
    public enum ColorModel
    {
        BGR,
        Grayscale,
        HLS,
        HSV,
        LAB
    }

    public static class ColorModelConvertor
    {
        public static Image ConvertTo(Image image, ColorModel colorModel)
        {
            return image;
        }
    }
}
