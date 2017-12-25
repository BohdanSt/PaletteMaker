using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteMaker.ImageProcessing
{
    public static class ImageFiltering
    {
        public enum FilterType
        {
            None,
            Bilatral,
            Blur,
            Gaussian,
            Median
        }
    }
}
