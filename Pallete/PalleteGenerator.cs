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

namespace PalleteMaker.Pallete
{
    public static class PalleteGenerator
    {
        static int clustersCount;
        static Image<Bgr, byte> currentImage;
        static Image<Bgr, byte> palleteImage;

        static Range range = new Range(0, 255);
        static Random random = new Random();

        public static Image<Bgr, byte> Generate(int count, Bitmap image, int imagePalleteWidth, int imagePalleteHeight)
        {
            ColorCluster[] clusters;
            InitializeData(count, image, out clusters);

            DivisionToClusters(clusters);

            CreatePallete(clusters, imagePalleteWidth, imagePalleteHeight);

            return palleteImage;
        }

        private static void InitializeData(int count, Bitmap image, out ColorCluster[] clusters)
        {
            clustersCount = count;

            palleteImage = null;

            currentImage = new Image<Bgr, byte>(image);
            currentImage = ResizeImage(currentImage);

            clusters = new ColorCluster[clustersCount];
            for (int i = 0; i < clustersCount; i++)
            {
                clusters[i].newColor = new MCvScalar(random.Next(range.Start, range.End), random.Next(range.Start, range.End), random.Next(range.Start, range.End));
            }
        }

        private static void DivisionToClusters(ColorCluster[] clusters)
        {
            double minRGBEuclidean = 0;
            double oldRGBEuclidean = 0;

            Image<Bgr, byte> clusterImage = new Image<Bgr, byte>(currentImage.Size);
            clusterImage.SetZero(); ;

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

        private static double RgbEuclidean(MCvScalar p1, MCvScalar p2)
        {
            return Math.Sqrt((p1.V0 - p2.V0) * (p1.V0 - p2.V0) +
                (p1.V1 - p2.V1) * (p1.V1 - p2.V1) +
                (p1.V2 - p2.V2) * (p1.V2 - p2.V2) +
                (p1.V3 - p2.V3) * (p1.V3 - p2.V3));
        }

        private static void CreatePallete(ColorCluster[] clusters, int imagePalleteWidth, int imagePalleteHeight)
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

            palleteImage = new Image<Bgr, byte>(imagePalleteWidth, imagePalleteHeight);
            palleteImage.SetZero();
            int h = palleteImage.Height / colorsCount;
            int w = palleteImage.Width;
            for (int i = 0; i < colorsCount; i++)
            {
                CvInvoke.Rectangle(palleteImage, new System.Drawing.Rectangle(0, i * h, w, i * h + h), clusters[colors[i].Item1].color, -1);
            }
        }

        // sorting colors by count
        private static int colorSortExpression(Tuple<int, int> a, Tuple<int, int> b)
        {
            return (a.Item2 > b.Item2)? 1 : 0;
        }

        private static Image<Bgr, byte> ResizeImage(Image<Bgr, byte> originalImage)
        {
            int newWidth = currentImage.Width / 10;
            int newHeight = currentImage.Height / 10;

            Image<Bgr, byte> smallImage = new Image<Bgr, byte>(newWidth, newHeight);
            CvInvoke.Resize(currentImage, smallImage, new System.Drawing.Size(newWidth, newHeight), 0, 0, Inter.Linear);

            return smallImage;
        }
    }
}
