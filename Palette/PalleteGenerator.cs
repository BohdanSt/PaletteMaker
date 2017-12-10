using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace PaletteMaker.Palette
{
    public class PaletteGenerator
    {
        int clustersCount;
        Image<Bgr, byte> clusterImage;
        Image<Bgr, byte> currentImage;

        ColorCluster[] clusters;

        Range range = new Range(0, 255);
        Random random = new Random();

        public delegate void GeneratePaletteFinishedDelegate();
        public event GeneratePaletteFinishedDelegate GeneratePaletteFinished;

        public async void GeneratePaletteAsync(int count, Bitmap image)
        {
            InitializeData(count, image);

            await Task.Factory.StartNew(() =>
            {
                DivisionToClusters();
            }, TaskCreationOptions.LongRunning);

            GeneratePaletteFinished?.Invoke();
        }

        public async Task<Image<Bgr, byte> > CreatePaletteImageAsync(int imagePaletteWidth, int imagePaletteHeight)
        {
            return await Task<Image<Bgr, byte> >.Factory.StartNew(() =>
            {
                return CreatePaletteImage(imagePaletteWidth, imagePaletteHeight);
            });
        }

        public async Task<Image<Bgr, byte>> CreatePaletteBasedImageAsync()
        {
            return await Task<Image<Bgr, byte>>.Factory.StartNew(() =>
            {
                return CreatePaletteBasedImage();
            });
        }

        private void InitializeData(int count, Bitmap image)
        {
            clustersCount = count;
            clusterImage = null;

            currentImage = ResizeImage(new Image<Bgr, byte>(image));

            clusters = new ColorCluster[clustersCount];
            for (int i = 0; i < clustersCount; i++)
            {
                clusters[i].newColor = new MCvScalar(random.Next(range.Start, range.End), random.Next(range.Start, range.End), random.Next(range.Start, range.End));
            }
        }

        private void DivisionToClusters()
        {
            double minRGBEuclidean = 0;
            double oldRGBEuclidean = 0;

            clusterImage = new Image<Bgr, byte>(currentImage.Size);
            clusterImage.SetZero();

            while (true)
            {
                for (int i = 0; i < clustersCount; i++)
                {
                    clusters[i].count = 0;
                    clusters[i].color = clusters[i].newColor;
                    clusters[i].newColor = new MCvScalar(0, 0, 0, 0);
                }

                for (int y = 0; y < currentImage.Height; y++)
                {
                    for (int x = 0; x < currentImage.Width; x++)
                    {
                        // Get RGB-components of pixel
                        byte B = currentImage[0].Data[y, x, 0];    // Blue
                        byte G = currentImage[1].Data[y, x, 0];    // Green
                        byte R = currentImage[2].Data[y, x, 0];    // Red

                        minRGBEuclidean = 255 * 255 * 255;
                        int clusterIndex = -1;
                        for (int i = 0; i < clustersCount; i++)
                        {
                            double euclid = RgbEuclidean(new MCvScalar(B, G, R, 0), clusters[i].color);
                            if (euclid < minRGBEuclidean)
                            {
                                minRGBEuclidean = euclid;
                                clusterIndex = i;
                            }
                        }

                        // Set index of cluster
                        clusterImage[0].Data[y, x, 0] = (byte)clusterIndex;

                        clusters[clusterIndex].count++;
                        clusters[clusterIndex].newColor.V0 += B;
                        clusters[clusterIndex].newColor.V1 += G;
                        clusters[clusterIndex].newColor.V2 += R;
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                minRGBEuclidean = 0;
                for (int i = 0; i < clustersCount; i++)
                {
                    // new color
                    clusters[i].newColor.V0 /= clusters[i].count;
                    clusters[i].newColor.V1 /= clusters[i].count;
                    clusters[i].newColor.V2 /= clusters[i].count;
                    double eclidean = RgbEuclidean(clusters[i].newColor, clusters[i].color);
                    if (eclidean > minRGBEuclidean)
                        minRGBEuclidean = eclidean;
                }

                if (Math.Abs(minRGBEuclidean - oldRGBEuclidean) < 1)
                    break;

                oldRGBEuclidean = minRGBEuclidean;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private double RgbEuclidean(MCvScalar p1, MCvScalar p2)
        {
            return Math.Sqrt((p1.V0 - p2.V0) * (p1.V0 - p2.V0) +
                (p1.V1 - p2.V1) * (p1.V1 - p2.V1) +
                (p1.V2 - p2.V2) * (p1.V2 - p2.V2) +
                (p1.V3 - p2.V3) * (p1.V3 - p2.V3));
        }

        private Image<Bgr, byte> CreatePaletteImage(int imagePaletteWidth, int imagePaletteHeight)
        {
            List<Tuple<int, int>> colors = new List<Tuple<int, int>>(clustersCount);
            int colorsCount = 0;
            for (int i = 0; i < clustersCount; i++)
            {
                colors.Add(new Tuple<int, int>(i, clusters[i].count));
                if (clusters[i].count > 0)
                    colorsCount++;
            }

            colors.Sort(colorSortExpression);

            var PaletteImage = new Image<Bgr, byte>(imagePaletteWidth, imagePaletteHeight);
            PaletteImage.SetZero();
            int h = PaletteImage.Height / colorsCount;
            int w = PaletteImage.Width;
            for (int i = 0; i < colorsCount; i++)
            {
                CvInvoke.Rectangle(PaletteImage, new System.Drawing.Rectangle(0, i * h, w, i * h + h), clusters[colors[i].Item1].color, -1);
            }

            return PaletteImage;
        }

        private Image<Bgr, byte> CreatePaletteBasedImage()
        {
            var PaletteBasedImage = new Image<Bgr, byte>(currentImage.Bitmap);

            for (int y = 0; y < PaletteBasedImage.Height; y++)
            {
                for (int x = 0; x < PaletteBasedImage.Width; x++)
                {
                    int cluster_index = clusterImage[0].Data[y, x, 0];

                    PaletteBasedImage[0].Data[y, x, 0] = (byte)clusters[cluster_index].color.V0;
                    PaletteBasedImage[1].Data[y, x, 0] = (byte)clusters[cluster_index].color.V1;
                    PaletteBasedImage[2].Data[y, x, 0] = (byte)clusters[cluster_index].color.V2;
                }
            }

            return PaletteBasedImage;
        }

        // sorting colors by count
        private int colorSortExpression(Tuple<int, int> a, Tuple<int, int> b)
        {
            return (a.Item2 > b.Item2)? 1 : 0;
        }

        private Image<Bgr, byte> ResizeImage(Image<Bgr, byte> originalImage, int? width = null, int? height = null)
        {
            if (width == null || height == null)
            {
                width = originalImage.Width;
                height = originalImage.Height;
                do
                {
                    width >>= 1;
                    height >>= 1;
                }
                while (width > 150 && height > 150);
            }

            Image<Bgr, byte> smallImage = new Image<Bgr, byte>(width.Value, height.Value);
            CvInvoke.Resize(originalImage, smallImage, new System.Drawing.Size(width.Value, height.Value), 0, 0, Inter.Linear);

            return smallImage;
        }
    }
}
