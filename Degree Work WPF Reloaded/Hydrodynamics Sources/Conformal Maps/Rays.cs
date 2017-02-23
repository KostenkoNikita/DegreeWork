using System;

using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    public class EjectedRays : Hydrodynamics_Sources.IConformalMapFunction
    {
        public complex this[complex dzeta] { get { return z(dzeta); } }
        
        public double l { get; set; }

        public double a { get; set; }

        public double Angle => a * Math.PI;

        public EjectedRays(double l, double alpha)
        {
            this.l = l; a = alpha;
        }

        public override bool Equals(object obj) => obj.GetType() == typeof(EjectedRays) ? Equals((EjectedRays)obj) : false;

        public override int GetHashCode() => l.GetHashCode() ^ a.GetHashCode();

        public override string ToString() => "EjectedRays";

        bool Equals(EjectedRays other) => l == other.l && a == other.a;

        public complex z(complex dzeta)
        {
            return (l * Math.Pow(a, a) * Math.Pow(1 - a, 1 - a)) * (math.exp(a * dzeta) - math.exp((a - 1) * dzeta));
        }

        public complex dz_ddzeta(complex dzeta)
        {
            return (l * math.pow(a, a) * math.pow(1 - a, 1 - a)) * ((a - 1) * math.exp((a - 1) * dzeta) + a * math.exp(a * dzeta));
        }

        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
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
