using System;

using static Degree_Work.Mathematical_Sources.Complex.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using Degree_Work.Mathematical_Sources.Complex;
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

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj.GetType() == typeof(Number81) ? Equals((Number81)obj) : false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool Equals(Number81 other) => h == other.h;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => h.GetHashCode() ^ (typeof(Number81)).GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => $"Number81";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Mathematical_Sources.Equations.Solve(z, dz_ddzeta, Z);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            return -h * I * (0.25 * (-3 + dzeta) / (Sqrt(dzeta)) + 0.5 * Sqrt(dzeta));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            return h * I - h * I * (Sqrt(dzeta) * (dzeta - 3) / 2.0 + 1);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint z(DataPoint dzeta)
        {
            return z((Degree_Work.Mathematical_Sources.Complex.Complex)dzeta);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dz_ddzeta(DataPoint dzeta)
        {
            return dz_ddzeta((Degree_Work.Mathematical_Sources.Complex.Complex)dzeta);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dzeta(DataPoint Z)
        {
            return dzeta((Degree_Work.Mathematical_Sources.Complex.Complex)Z);
        }
    }
}
