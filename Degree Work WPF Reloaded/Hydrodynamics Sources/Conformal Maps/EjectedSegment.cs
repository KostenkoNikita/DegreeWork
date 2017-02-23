using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    public class EjectedSegment : Hydrodynamics_Sources.IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] { get { return z(dzeta); } }

        public complex pos => new complex(X, Y);

        public double X, Y;

        public EjectedSegment(complex SegmentPosition)
        {
            X = SegmentPosition.Re;
            Y = SegmentPosition.Im;
        }

        public EjectedSegment(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override bool Equals(object obj) => obj.GetType() == typeof(EjectedSegment) ? Equals((EjectedSegment)obj) : false;

        bool Equals(EjectedSegment other) => pos == other.pos;

        public override int GetHashCode() => pos.GetHashCode() ^ (typeof(EjectedSegment)).GetHashCode();

        public override string ToString() => "EjectedSegment";

        public complex z(complex dzeta)
        {
            complex tmp = pos.Re + math.sqrt(pos.Re * pos.Re - 2 * pos.Re * dzeta - pos.Im * pos.Im + dzeta * dzeta);
            return tmp.Im < 0 ? pos.Re - math.sqrt(pos.Re * pos.Re - 2 * pos.Re * dzeta - pos.Im * pos.Im + dzeta * dzeta) : tmp;
        }

        public complex dz_ddzeta(complex dzeta)
        {
            complex tmp = ((dzeta - pos.Re) / (math.sqrt(pos.Re * pos.Re - 2 * pos.Re * dzeta - pos.Im * pos.Im + dzeta * dzeta)));
            return tmp.Re < 0 ? -tmp : tmp;
        }

        public complex dzeta(complex Z)
        {
            return math.sqrt(math.pow(Z - pos.Re, 2) + pos.Im * pos.Im) + pos.Re;
        }

        public DataPoint z(DataPoint dzeta)
        {
            return z(dzeta.DataPointToComplex()).ComplexToDataPoint();
        }

        public DataPoint dz_ddzeta(DataPoint dzeta)
        {
            return dz_ddzeta(dzeta.DataPointToComplex()).ComplexToDataPoint();
        }

        public DataPoint dzeta(DataPoint Z)
        {
            return dzeta(Z.DataPointToComplex()).ComplexToDataPoint();
        }
    }
}
