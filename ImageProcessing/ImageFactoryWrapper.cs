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
using System.Windows.Media.Imaging;

namespace PaletteMaker.ImageProcessing
{
    class ImageFactoryWrapper
    {
        private ImageSource resultImage;
        private ImageLayer processingImage;

        private Image originalImage;
        private Image currentImage;
        private int brightness;
        private int saturation;
        private int contrast;
        private int hueValue;
        private bool isUseAutoCorrection;
        private FilterType filterType;
        private EffectType effectType;

        public delegate void UpdateImageControlDelegate(ImageSource image);
        public event UpdateImageControlDelegate OnUpdateImageControl;

        public ImageFactoryWrapper()
        {
            processingImage = new ImageLayer();
            Stream memoryStream = App.GetResourceStream(new Uri("pack://application:,,,/PaletteMaker;component/Images/Processing.png")).Stream;
            processingImage.Image = Image.FromStream(memoryStream);
        }

        private async void ApplyChangesToImage()
        {
            if (originalImage != null)
            {
                ImageFactory imageFactory = new ImageFactory(preserveExifData: true);

                if (effectType != EffectType.None)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        imageFactory.Load(currentImage);
                        imageFactory.Overlay(processingImage);
                    });

                    UpdateResultImage(imageFactory.Image);
                }

                await Task.Factory.StartNew(() =>
                {
                    imageFactory.Load(originalImage);

                    imageFactory.Saturation(saturation);
                    imageFactory.Brightness(brightness);
                    imageFactory.Contrast(contrast);
                    imageFactory.Hue(hueValue);

                    ImageFiltering.ApplyFilter(filterType, ref imageFactory);
                    
                    ImageEffect.ApplyEffect(effectType, ref imageFactory);

                    if (isUseAutoCorrection)
                    {
                        isUseAutoCorrection = false;
                    }
                });

                UpdateResultImage(imageFactory.Image);

                imageFactory.Dispose();
            }
        }

        private void UpdateResultImage(Image image)
        {
            currentImage = image.Clone() as Image;

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

        public void ApplyFilter(FilterType selectedFilter)
        {
            filterType = selectedFilter;
            ApplyChangesToImage();
        }

        public void ApplyEffect(EffectType selectedEffect)
        {
            effectType = selectedEffect;
            ApplyChangesToImage();
        }
    }
}
