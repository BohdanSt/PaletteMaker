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
        private Image<Bgr, byte> originalImage;

        public ImageProcessingView()
        {
            InitializeComponent();
        }

        private void buttonOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                originalImage = new Image<Bgr, byte>(openFileDialog.FileName);
                imageControl.Image = originalImage;
            }
        }

        private void buttonCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            imageControl.Image = originalImage;
        }

        private void buttonSaveImage_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
            }
        }
    }
}
