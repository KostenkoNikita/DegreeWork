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
    class IdentityTransform : IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] => z(dzeta);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj.GetType() == typeof(Porebrick);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => (typeof(Porebrick)).GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => "IdentityTransform";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Z;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            return 1;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            return dzeta;
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
