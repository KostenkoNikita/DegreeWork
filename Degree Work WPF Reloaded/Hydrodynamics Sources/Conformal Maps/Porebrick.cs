using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OxyPlot;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Complex.Complex;
using Degree_Work.Mathematical_Sources.Complex;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Porebrick : IConformalMapFunction, IEquatable<Porebrick>
    {
        public DataPoint this[DataPoint dzeta] => z(dzeta);

        double h;

        public double H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
            }
        }

        public Porebrick(double h)
        {
            this.h = h;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj.GetType() == typeof(Porebrick) ? Equals((Porebrick)obj) : false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(Porebrick other) => H == other.H;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => h.GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => "Porebrick";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Mathematical_Sources.Equations.Solve(z, dz_ddzeta, Z);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            Complex tmp = I * H + (H / Math.PI) * (Sqrt(dzeta * dzeta - 1) - Arch(dzeta));
            if (tmp.Im < 0)
            {
                return -(H / Math.PI) * ((dzeta - 1) / (Sqrt(dzeta * dzeta - 1)));
            }
            else
            {
                return (H / Math.PI) * ((dzeta - 1) / (Sqrt(dzeta * dzeta - 1)));
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            Complex tmp = I * H + (H / Math.PI) * (Sqrt(dzeta * dzeta - 1.0) - Arch(dzeta));
            if (tmp.Im < 0)
            {
                return -I * H - (H / Math.PI) * (Sqrt(dzeta * dzeta - 1.0) - Arch(dzeta));
            }
            else
            {
                return tmp;
            }
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
            return dzeta((Complex)Z);
        }
    }
}
