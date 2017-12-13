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

namespace PaletteMaker.Palette
{
    /// <summary>
    /// Interaction logic for PaletteGeneratorView.xaml
    /// </summary>
    public partial class PaletteGeneratorView : System.Windows.Controls.UserControl
    {
        PaletteGenerator PaletteGenerator = new PaletteGenerator();

        public PaletteGeneratorView()
        {
            InitializeComponent();

            PaletteGenerator.GeneratePaletteFinished += PaletteGenerator_GeneratePaletteFinished;
        }

        private async void PaletteGenerator_GeneratePaletteFinished()
        {
            imagePalette.Image = await PaletteGenerator.CreatePaletteImageAsync(imagePalette.Width, imagePalette.Height);

            //imagePaletteBased.Image = await PaletteGenerator.CreatePaletteBasedImageAsync();

            buttonGeneratePalette.IsEnabled = true;
        }

        private void buttonGeneratePalette_Click(object sender, RoutedEventArgs e)
        {
            buttonGeneratePalette.IsEnabled = false;
            PaletteGenerator.GeneratePaletteAsync((int)numericClustersCount.Value, imageControl.Image.Bitmap);
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageControl.Image = new Image<Bgr, byte>(openFileDialog.FileName);
            }
        }
    }
}
