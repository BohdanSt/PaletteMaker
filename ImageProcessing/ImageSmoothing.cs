using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteMaker.ImageProcessing
{
    public static class ImageSmoothing
    {
        public enum SmoothingType
        {
            Bilatral,
            Blur,
            Gaussian,
            Median
        }
    }
}
