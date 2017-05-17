using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Degree_Work.Mathematical_Sources.Complex.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using Degree_Work.Mathematical_Sources.Complex;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Number89 : Hydrodynamics_Sources.IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] { get { return z(dzeta); } }

        public double h1, h2;

        public double lam => Mathematical_Sources.Equations.NSolve(lamRootFunc, lamRootFuncD, 0.1);

        private Func<double, double> lamRootFunc;

        private Func<double, double> lamRootFuncD;

        public Number89(double h1, double h2)
        {
            this.h1 = h1;
            this.h2 = h2;
            lamRootFunc = (l) => { return Math.Sqrt(1 - l * l) / l - Math.Acos(l) - h2 * Math.PI / (h1 - h2); };
            lamRootFuncD = (l) => { return -Math.Sqrt(1 - l * l) / (l * l); };
            double test = lam;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj.GetType() == typeof(Number89) ? Equals((Number89)obj) : false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => h1.GetHashCode()^h2.GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => "Number89";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool Equals(Number89 other) => h1==other.h1 && h2==other.h2;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            Complex tmp = I + ((h1 - h2) / (Math.PI * lam)) * (Sqrt(dzeta * dzeta - 1) - lam * Arch(dzeta));
            return tmp.Im < 0 ? -tmp : tmp;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            Complex tmp = ((h1 - h2) / (lam * Math.PI)) * (-1.0 / (1 - dzeta * dzeta) + dzeta / (Sqrt(dzeta * dzeta - 1)));
            if (tmp.Re < 0) { tmp = -(tmp.Conjugate); }
            return tmp;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Mathematical_Sources.Equations.Solve(z, dz_ddzeta, Z);
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
