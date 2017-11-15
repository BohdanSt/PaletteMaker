using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalleteMaker.PalleteGenerator
{
    public struct ColorCluster
    {
        public MCvScalar color;
        public MCvScalar newColor;
        public int count;
    }
}
