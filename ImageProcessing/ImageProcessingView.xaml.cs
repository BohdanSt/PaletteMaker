using Emgu.CV;
using Emgu.CV.Structure;
using ImageProcessor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaletteMaker.ImageProcessing
{
    /// <summary>
    /// Interaction logic for ImageProcessingView.xaml
    /// </summary>
    public partial class ImageProcessingView : System.Windows.Controls.UserControl
    {
        ImageFactoryWrapper imageFactoryWrapper = new ImageFactoryWrapper();

        Dictionary<FilterType, string> filters = new Dictionary<FilterType, string>();

        Dictionary<EffectType, string> effects = new Dictionary<EffectType, string>();

        public ImageProcessingView()
        {
            InitializeComponent();

            InitializeFiltersCombobox();
            InitializeEffectsCombobox();

            imageFactoryWrapper.OnUpdateImageControl += ImageFactoryWrapper_OnUpdateImageControl;
        }

        private void ImageFactoryWrapper_OnUpdateImageControl(ImageSource image)
        {
            imageControl.Source = image;
        }

        private void InitializeFiltersCombobox()
        {
            filters.Add(FilterType.None, "Original image");
            filters.Add(FilterType.BlackWhite, "BlackWhite");
            filters.Add(FilterType.Comic, "Comic");
            filters.Add(FilterType.Gotham, "Gotham");
            filters.Add(FilterType.GreyScale, "GreyScale");
            filters.Add(FilterType.HiSatch, "HiSatch");
            filters.Add(FilterType.Invert, "Invert");
            filters.Add(FilterType.Lomograph, "Lomograph");
            filters.Add(FilterType.LoSatch, "LoSatch");
            filters.Add(FilterType.Polaroid, "Polaroid");
            filters.Add(FilterType.Sepia, "Sepia");

            comboboxFilterType.ItemsSource = filters;
        }

        private void InitializeEffectsCombobox()
        {
            effects.Add(EffectType.None, "Original image");
            effects.Add(EffectType.GaussianBlur, "GaussianBlur");
            effects.Add(EffectType.GaussianSharpen, "GaussianSharpen");
            effects.Add(EffectType.Pixelate, "Pixelate");
            effects.Add(EffectType.Vignette, "Vignette");
            effects.Add(EffectType.BinaryThreshold, "BinaryThreshold");
            effects.Add(EffectType.Halftone, "Halftone");
            effects.Add(EffectType.OilPainting, "OilPainting");

            comboboxEffectType.ItemsSource = effects;
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.png,*.jpeg,*.jpg,*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imageFactoryWrapper.LoadImage(openFileDialog.FileName);
                }
            }
        }

        private void buttonCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            imageFactoryWrapper.CancelChanges();

            comboboxFilterType.SelectedIndex = 0;
            comboboxEffectType.SelectedIndex = 0;

            sliderBrightness.Value = 0;
            sliderSaturation.Value = 0;
            sliderContrast.Value = 0;
            sliderHue.Value = 0;
        }

        private void buttonSaveImage_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpeg,*.jpg)|*.jpeg;*.jpg|BMP (*.bmp)|*.bmp";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    imageFactoryWrapper.SaveImage(saveFileDialog.FileName);
                }
            }
        }

        private void comboboxFilterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedFilter = (FilterType)comboboxFilterType.SelectedValue;
            imageFactoryWrapper.ApplyFilter(selectedFilter);
        }

        private void buttonAutoCorrection_Click(object sender, RoutedEventArgs e)
        {
            imageFactoryWrapper.AutoCorrection();
        }

        private void sliderBrightness_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageFactoryWrapper.BrightnessChange((int)sliderBrightness.Value);
        }

        private void sliderSaturation_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageFactoryWrapper.SaturationChange((int)sliderSaturation.Value);
        }

        private void sliderContrast_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageFactoryWrapper.ContrastChange((int)sliderContrast.Value);
        }

        private void sliderHue_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            imageFactoryWrapper.HueChange((int)sliderHue.Value);
        }

        private void sliderBrightness_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliderBrightness.Value = 0;
            imageFactoryWrapper.BrightnessChange((int)sliderBrightness.Value);
        }

        private void sliderSaturation_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliderSaturation.Value = 0;
            imageFactoryWrapper.SaturationChange((int)sliderSaturation.Value);
        }

        private void sliderContrast_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliderContrast.Value = 0;
            imageFactoryWrapper.ContrastChange((int)sliderContrast.Value);
        }

        private void sliderHue_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            sliderHue.Value = 0;
            imageFactoryWrapper.HueChange((int)sliderHue.Value);
        }

        private void comboboxEffectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEffect = (EffectType)comboboxEffectType.SelectedValue;
            imageFactoryWrapper.ApplyEffect(selectedEffect);
        }
    }
}
