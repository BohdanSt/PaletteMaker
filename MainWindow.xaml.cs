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

namespace PalleteMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonGeneratePallete_Click(object sender, RoutedEventArgs e)
        {
            PalleteGenerator.clustersCount = Convert.ToInt32(textBoxClustersCount.Text);
            PalleteGenerator.currentImage = new Image<Bgr, byte>(imageControl.Image.Bitmap);

            PalleteGenerator.Generate();

            imagePallete.Image = PalleteGenerator.palleteImage;
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
