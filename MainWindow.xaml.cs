using Microsoft.Win32;
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
using PalleteMaker.Pallete;
using Emgu.CV;
using Emgu.CV.Structure;
using MahApps.Metro.Controls;

namespace PalleteMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        PalleteGenerator palleteGenerator = new PalleteGenerator();

        public MainWindow()
        {
            InitializeComponent();

            palleteGenerator.GeneratePalleteFinished += PalleteGenerator_GeneratePalleteFinished;
        }

        private void PalleteGenerator_GeneratePalleteFinished()
        {
            imagePallete.Image = palleteGenerator.CreatePalleteImage(imagePallete.Width, imagePallete.Height);

            imagePalleteBased.Image = palleteGenerator.CreatePalleteBasedImage();
        }

        private void buttonGeneratePallete_Click(object sender, RoutedEventArgs e)
        {
            palleteGenerator.GeneratePallete((int)numericClustersCount.Value, imageControl.Image.Bitmap);
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                imageControl.Image = new Image<Bgr, byte>(openFileDialog.FileName);
            }
        }
    }
}
