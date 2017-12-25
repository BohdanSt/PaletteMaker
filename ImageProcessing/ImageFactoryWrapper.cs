using ImageProcessor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Diagnostics;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Filters.Photo;

namespace PaletteMaker.ImageProcessing
{
    class ImageFactoryWrapper
    {
        private ImageSource resultImage;

        private Image originalImage;
        private int brightness;
        private int saturation;
        private int contrast;
        private int hueValue;
        private bool isUseAutoCorrection;
        private ImageFiltering.FilterType filterType;

        public delegate void UpdateImageControlDelegate(ImageSource image);
        public event UpdateImageControlDelegate OnUpdateImageControl;

        private async void ApplyChangesToImage()
        {
            if (originalImage != null)
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    await Task.Factory.StartNew(() =>
                    {
                        imageFactory.Load(originalImage);

                        imageFactory.Saturation(saturation);
                        imageFactory.Brightness(brightness);
                        imageFactory.Contrast(contrast);
                        imageFactory.Hue(hueValue);

                        IMatrixFilter matrixFilter = ImageFiltering.GetFilterToApply(filterType);
                        if (matrixFilter != null)
                        {
                            imageFactory.Filter(matrixFilter);
                        }

                        if (isUseAutoCorrection)
                        {
                            isUseAutoCorrection = false;
                        }
                    });

                    UpdateResultImage(imageFactory.Image);
                }
            }
        }

        private void UpdateResultImage(Image image)
        {
            resultImage = CustomImageConverter.DrawingImageToBitmapSource(image);

            OnUpdateImageControl?.Invoke(resultImage);
        }

        public void LoadImage(string fileName)
        {
            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
            {
                imageFactory.Load(fileName);

                originalImage = imageFactory.Image.Clone() as Image;

                UpdateResultImage(imageFactory.Image);
            }
        }

        public void CancelChanges()
        {
            if (originalImage != null)
            {
                brightness = 0;
                saturation = 0;
                contrast = 0;
                hueValue = 0;

                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(originalImage);

                    UpdateResultImage(imageFactory.Image);
                }
            }
        }

        public void BrightnessChange(int value)
        {
            brightness = value;
            ApplyChangesToImage();
        }

        public void SaturationChange(int value)
        {
            saturation = value;
            ApplyChangesToImage();
        }

        public void SaveImage(string fileName)
        {
            if (originalImage != null)
            {
                originalImage.Save(fileName);
            }
        }

        public void AutoCorrection()
        {
            isUseAutoCorrection = true;
            ApplyChangesToImage();
        }

        public void ContrastChange(int value)
        {
            contrast = value;
            ApplyChangesToImage();
        }

        public void HueChange(int value)
        {
            hueValue = value;
            ApplyChangesToImage();
        }

        internal void ApplyFilter(ImageFiltering.FilterType selectedFilter)
        {
            filterType = selectedFilter;
            ApplyChangesToImage();
        }
    }
}
