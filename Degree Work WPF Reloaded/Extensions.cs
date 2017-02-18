using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using MathCore_2_0;

namespace Degree_Work
{
    static class Extensions
    {
        public static DataPoint ComplexToDataPoint(this complex c)
        {
            return new DataPoint(c.Re, c.Im);
        }
        public static complex DataPointToComplex(this DataPoint p)
        {
            return new complex(p.X, p.Y);
        }
        public static bool IsAllThreadsCompleted(this List<IAsyncResult> l)
        {
            foreach (IAsyncResult i in l)
            {
                if (!i.IsCompleted) { return false; }
            }
            return true;
        }
    }
}
