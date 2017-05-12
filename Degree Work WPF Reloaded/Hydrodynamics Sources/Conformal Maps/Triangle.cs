using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Degree_Work.Mathematical_Sources.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Functions.SpecialFunctions;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Triangle : IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] => throw new NotImplementedException();

        public Complex this[Complex dzeta] => z(dzeta);

        public double h, A;

        private double a => 1 - (1.0 / Math.PI) * Math.Atan(2 * h / A);

        private Complex C => -(A * Sqrt(Math.PI) / Cos(a * Math.PI)) / (2 * Gamma(1.5 - a) * Gamma(a));

        public Triangle(double h, double A)
        {
            this.h = h;
            this.A = A;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Triangle ? Equals((Triangle)obj) : false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private bool Equals(Triangle other) => h == other.h && A == other.A;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => "Triangle";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => h.GetHashCode() ^ A.GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z) => Mathematical_Sources.Equations.Solve(z, dz_ddzeta, Z);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dzeta(DataPoint Z) => dzeta(Z.DataPointToComplex()).ComplexToDataPoint();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta) => Pow(1 - 1.0 / (dzeta * dzeta), -1 + a);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dz_ddzeta(DataPoint dzeta) => dz_ddzeta(dzeta.DataPointToComplex()).ComplexToDataPoint();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)=> C * (dzeta * Hypergeometric2F1(-0.5, 1 - a, 0.5, 1.0 / (dzeta * dzeta)) + (Complex.I * Sqrt(Math.PI) * Gamma(a) * Math.Tan(a * Math.PI)) / Gamma(-0.5 + a)) + Complex.I * h;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint z(DataPoint dzeta) => z(dzeta.DataPointToComplex()).ComplexToDataPoint();
    }
}
