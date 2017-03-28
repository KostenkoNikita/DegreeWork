using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Triangle : IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] => throw new NotImplementedException();

        public complex this[complex dzeta] => z(dzeta);

        public double h, A;

        private double a => 1 - (1.0 / Math.PI) * Math.Atan(2 * h / A);

        private complex C => -(A * Math.Sqrt(Math.PI) / Math.Cos(a * Math.PI)) / (2 * math.gamma(1.5 - a) * math.gamma(a));

        public Triangle(double h, double A)
        {
            this.h = h;
            this.A = A;
        }

        public override bool Equals(object obj) => obj is Triangle ? Equals((Triangle)obj) : false;

        private bool Equals(Triangle other) => h == other.h && A == other.A;

        public override string ToString() => "Triangle";

        public override int GetHashCode() => h.GetHashCode() ^ A.GetHashCode();

        public complex dzeta(complex Z) => equations.solve(z, dz_ddzeta, Z);

        public DataPoint dzeta(DataPoint Z) => dzeta(Z.DataPointToComplex()).ComplexToDataPoint();

        public complex dz_ddzeta(complex dzeta) => math.pow(1 - 1.0 / (dzeta * dzeta), -1 + a);

        public DataPoint dz_ddzeta(DataPoint dzeta) => dz_ddzeta(dzeta.DataPointToComplex()).ComplexToDataPoint();

        public complex z(complex dzeta)=> C * (dzeta * math.hypergeometric2F1(-0.5, 1 - a, 0.5, 1.0 / (dzeta * dzeta)) + (complex.i * Math.Sqrt(Math.PI) * math.gamma(a) * Math.Tan(a * Math.PI)) / math.gamma(-0.5 + a)) + complex.i * h;

        public DataPoint z(DataPoint dzeta) => z(dzeta.DataPointToComplex()).ComplexToDataPoint();
    }
}
