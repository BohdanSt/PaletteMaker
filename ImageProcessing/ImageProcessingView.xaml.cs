using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
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
        IImage originalImage;

        Dictionary<ImageSmoothing.SmoothingType, string> smoothingTypes = new Dictionary<ImageSmoothing.SmoothingType, string>();

        public ImageProcessingView()
        {
            InitializeComponent();

            InitializeSmoothingCombobox();
        }

        private void InitializeSmoothingCombobox()
        {
            smoothingTypes.Add(ImageSmoothing.SmoothingType.Bilatral, "Bilatral");
            smoothingTypes.Add(ImageSmoothing.SmoothingType.Blur, "Blur");
            smoothingTypes.Add(ImageSmoothing.SmoothingType.Gaussian, "Gaussian");
            smoothingTypes.Add(ImageSmoothing.SmoothingType.Median, "Median");

            comboboxSmoothingType.ItemsSource = smoothingTypes;
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalImage = new Image<Bgr, byte>(openFileDialog.FileName);
                imageControl.Source = EmguCVImageConverter.ToBitmapSource(originalImage);
            }
        }

        private void buttonCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            imageControl.Source = EmguCVImageConverter.ToBitmapSource(originalImage);
        }

        private void buttonSaveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void sliderBrightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void sliderSaturation_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void comboboxSmoothingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
