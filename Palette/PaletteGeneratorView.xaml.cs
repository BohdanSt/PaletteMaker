using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using MahApps.Metro.Controls;
using System.Windows.Forms;
using System.Drawing;
using PaletteMaker.ImageProcessing;

namespace PaletteMaker.Palette
{
    /// <summary>
    /// Interaction logic for PaletteGeneratorView.xaml
    /// </summary>
    public partial class PaletteGeneratorView : System.Windows.Controls.UserControl
    {
        PaletteGenerator PaletteGenerator = new PaletteGenerator();

        IImage currentImage;
        IImage currentPalleteImage;

        public PaletteGeneratorView()
        {
            InitializeComponent();

            PaletteGenerator.GeneratePaletteFinished += PaletteGenerator_GeneratePaletteFinished;
        }

        private async void PaletteGenerator_GeneratePaletteFinished()
        {
            currentPalleteImage = await PaletteGenerator.CreatePaletteImageAsync((int)imagePalette.Width, (int)imagePalette.Height);
            imagePalette.Source = CustomImageConverter.EmguCVToBitmapSource(currentPalleteImage);

            buttonGeneratePalette.IsEnabled = true;
        }

        private void buttonGeneratePalette_Click(object sender, RoutedEventArgs e)
        {
            buttonGeneratePalette.IsEnabled = false;
            PaletteGenerator.GeneratePaletteAsync((int)numericClustersCount.Value, currentImage.Bitmap);
            imagePalette.Source = new BitmapImage(new Uri("pack://application:,,,/PaletteMaker;component/Images/Processing.png"));
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.png,*.jpeg,*.jpg,*.bmp)|*.png;*.jpeg;*.jpg;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentImage = new Image<Bgr, byte>(openFileDialog.FileName);
                    imageControl.Source = CustomImageConverter.EmguCVToBitmapSource(currentImage);
                }
            }
        }

        private void buttonSavePalette_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpeg,*.jpg)|*.jpeg;*.jpg|BMP (*.bmp)|*.bmp";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentPalleteImage.Save(saveFileDialog.FileName);
                }
            }
        }
    }
}
