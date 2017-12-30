using ImageProcessor;
using ImageProcessor.Imaging.Filters.Photo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaletteMaker.ImageProcessing
{
    public enum FilterType
    {
        None,
        BlackWhite,
        Comic,
        Gotham,
        GreyScale,
        HiSatch,
        Invert,
        Lomograph,
        LoSatch,
        Polaroid,
        Sepia
    }

    public static class ImageFiltering
    {
        private static IMatrixFilter GetFilterToApply(FilterType filterType)
        {
            IMatrixFilter selectedFilter;
            
            switch(filterType)
            {
                case FilterType.BlackWhite:
                    selectedFilter = MatrixFilters.BlackWhite;
                    break;
                case FilterType.Comic:
                    selectedFilter = MatrixFilters.Comic;
                    break;
                case FilterType.Gotham:
                    selectedFilter = MatrixFilters.Gotham;
                    break;
                case FilterType.GreyScale:
                    selectedFilter = MatrixFilters.GreyScale;
                    break;
                case FilterType.HiSatch:
                    selectedFilter = MatrixFilters.HiSatch;
                    break;
                case FilterType.Invert:
                    selectedFilter = MatrixFilters.Invert;
                    break;
                case FilterType.Lomograph:
                    selectedFilter = MatrixFilters.Lomograph;
                    break;
                case FilterType.LoSatch:
                    selectedFilter = MatrixFilters.LoSatch;
                    break;
                case FilterType.Polaroid:
                    selectedFilter = MatrixFilters.Polaroid;
                    break;
                case FilterType.Sepia:
                    selectedFilter = MatrixFilters.Sepia;
                    break;
                default:
                    selectedFilter = null;
                    break;
            }

            return selectedFilter;
        }

        public static void ApplyFilter(FilterType filterType, ref ImageFactory imageFactory)
        {
            IMatrixFilter selectedFilter = GetFilterToApply(filterType);
            if (selectedFilter != null)
            {
                imageFactory.Filter(selectedFilter);
            }
        }
    }
}
