using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Emgu.CV.CvEnum;

namespace PalleteMaker.Pallete
{
    public static class PalleteGenerator
    {
        public static int clustersCount;
        public static Image<Bgr, byte> currentImage;
        public static Image<Bgr, byte> palleteImage;

        static Range range = new Range(0, 255);
        static Random random = new Random();

        public static void Generate()
        {
            Image<Bgr, byte> smallImage = new Image<Bgr, byte>(currentImage.Width / 10, currentImage.Height / 10);
            CvInvoke.Resize(currentImage, smallImage, new System.Drawing.Size(currentImage.Width / 10, currentImage.Height / 10), 0, 0, Inter.Linear);
            currentImage = smallImage;

            palleteImage = null;

            Image<Bgr, byte> clusterImage = new Image<Bgr, byte>(currentImage.Size);
            clusterImage.SetZero();

            ColorCluster[] clusters = new ColorCluster[clustersCount];

            for (int i = 0; i < clustersCount; i++)
            {
                clusters[i].newColor = new MCvScalar(random.Next(range.Start, range.End), random.Next(range.Start, range.End), random.Next(range.Start, range.End));
            }

            DivisionToClusters(clusterImage, clusters);

            List<Tuple<int, int>> colors = new List<Tuple<int, int>>(clustersCount);
            int colorsCount = 0;
            for (int i = 0; i < clustersCount; i++)
            {
                colors.Add(new Tuple<int, int>(i, clusters[i].count));
                if (clusters[i].count > 0)
                    colorsCount++;
            }

            colors.Sort(colorSortExpression);

            palleteImage = new Image<Bgr, byte>(currentImage.Width, currentImage.Height);
            palleteImage.SetZero();
            int h = palleteImage.Height / clustersCount;
            int w = palleteImage.Width;
            for (int i = 0; i < clustersCount; i++)
            {
                CvInvoke.Rectangle(palleteImage, new System.Drawing.Rectangle(0, i * h, w, i * h + h), clusters[colors[i].Item1].color, -1);
            }

            currentImage = null;
        }

        private static void DivisionToClusters(Image<Bgr, byte> clusterImage, ColorCluster[] clusters)
        {
            double minRGBEuclidean = 0;
            double oldRGBEuclidean = 0;

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
            }
        }

        private static double RgbEuclidean(MCvScalar p1, MCvScalar p2)
        {
            return Math.Sqrt((p1.V0 - p2.V0) * (p1.V0 - p2.V0) +
                (p1.V1 - p2.V1) * (p1.V1 - p2.V1) +
                (p1.V2 - p2.V2) * (p1.V2 - p2.V2) +
                (p1.V3 - p2.V3) * (p1.V3 - p2.V3));
        }

        // sorting colors by count
        private static int colorSortExpression(Tuple<int, int> a, Tuple<int, int> b)
        {
            return (a.Item2 > b.Item2)? 1 : 0;
        }
    }
}
