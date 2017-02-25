using System;
using MathCore_2_0;
using static MathCore_2_0.math;
using static MathCore_2_0.complex;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Number79 : IConformalMapFunction
    {
        public double h { get; set; }

        public DataPoint this[DataPoint dzeta] => z(dzeta);

        public Number79(double h)
        {
            this.h = h;
        }
        public override bool Equals(object obj) => obj is Number79 ? Equals((Number81)obj) : false;

        bool Equals(Number81 other) => h == other.h;

        public override int GetHashCode() => h.GetHashCode() ^ (typeof(Number81)).GetHashCode();

        public override string ToString() => $"Number79";

        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
        }
        public complex dz_ddzeta(complex dzeta)
        {
            return sqrt(dzeta)*h/(pi*(1-dzeta));
        }
        public complex z(complex dzeta)
        {
            return i + (2.0 * h / pi) * (arth(sqrt(dzeta)) - sqrt(dzeta));
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
