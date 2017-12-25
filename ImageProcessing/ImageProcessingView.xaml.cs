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

        ImageFiltering.FilterType selectedFilter = ImageFiltering.FilterType.None;

        Dictionary<ImageFiltering.FilterType, string> filters = new Dictionary<ImageFiltering.FilterType, string>();

        public ImageProcessingView()
        {
            InitializeComponent();

            InitializeFiltersCombobox();

            imageFactoryWrapper.OnUpdateImageControl += ImageFactoryWrapper_OnUpdateImageControl;
        }

        private void ImageFactoryWrapper_OnUpdateImageControl(ImageSource image)
        {
            imageControl.Source = image;
        }

        private void InitializeFiltersCombobox()
        {
            filters.Add(ImageFiltering.FilterType.None, "Original image");
            filters.Add(ImageFiltering.FilterType.Bilatral, "Bilatral");
            filters.Add(ImageFiltering.FilterType.Blur, "Blur");
            filters.Add(ImageFiltering.FilterType.Gaussian, "Gaussian");
            filters.Add(ImageFiltering.FilterType.Median, "Median");

            comboboxFilterType.ItemsSource = filters;
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
            sliderFiltering.Value = 0;

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
            selectedFilter = (ImageFiltering.FilterType)comboboxFilterType.SelectedValue;
            ApplyFilter();
        }

        private void sliderFiltering_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ApplyFilter()
        {

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

        private void slider_DefaultValue(object sender, MouseButtonEventArgs e)
        {
            var slider = sender as Slider;
            if (slider != null)
            {
                slider.Value = 0;
            }
        }
    }
}
