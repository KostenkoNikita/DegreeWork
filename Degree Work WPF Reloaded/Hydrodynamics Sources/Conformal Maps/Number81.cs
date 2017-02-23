using System;
using MathCore_2_0;
using static MathCore_2_0.complex;
using static MathCore_2_0.math;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Number81 : Hydrodynamics_Sources.IConformalMapFunction
    {
        public double h { get; set; }

        public DataPoint this[DataPoint dzeta] => z(dzeta);

        public Number81(double h)
        {
            this.h = h;
        }
        public override bool Equals(object obj) => obj.GetType() == typeof(Number81) ? Equals((Number81)obj) : false;

        bool Equals(Number81 other) => h == other.h;

        public override int GetHashCode() => h.GetHashCode() ^ (typeof(Number81)).GetHashCode();

        public override string ToString() => $"Number81";

        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
        }
        public complex dz_ddzeta(complex dzeta)
        {
            return -h * i * (0.25 * (-3 + dzeta) / (sqrt(dzeta)) + 0.5 * sqrt(dzeta));
        }
        public complex z(complex dzeta)
        {
            return h * i - h * i * (sqrt(dzeta) * (dzeta - 3) / 2.0 + 1);
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
