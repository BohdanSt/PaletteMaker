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

namespace PalleteMaker.Pallete
{
    /// <summary>
    /// Interaction logic for PalleteGenratorView.xaml
    /// </summary>
    public partial class PalleteGenratorView : System.Windows.Controls.UserControl
    {
        PalleteGenerator palleteGenerator = new PalleteGenerator();

        public PalleteGenratorView()
        {
            InitializeComponent();

            palleteGenerator.GeneratePalleteFinished += PalleteGenerator_GeneratePalleteFinished;
        }

        private async void PalleteGenerator_GeneratePalleteFinished()
        {
            imagePallete.Image = await palleteGenerator.CreatePalleteImageAsync(imagePallete.Width, imagePallete.Height);

            imagePalleteBased.Image = await palleteGenerator.CreatePalleteBasedImageAsync();
        }

        private void buttonGeneratePallete_Click(object sender, RoutedEventArgs e)
        {
            palleteGenerator.GeneratePalleteAsync((int)numericClustersCount.Value, imageControl.Image.Bitmap);
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
