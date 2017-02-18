using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore_2_0;
using OxyPlot;
using static MathCore_2_0.math;
using static MathCore_2_0.complex;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class IdentityTransform : IConformalMapFunction
    {
        public complex this[complex dzeta] => z(dzeta);

        public override bool Equals(object obj) => obj.GetType() == typeof(Porebrick);

        public override int GetHashCode() => (typeof(Porebrick)).GetHashCode();

        public override string ToString() => "IdentityTransform";

        public complex dzeta(complex Z)
        {
            return Z;
        }

        public complex dz_ddzeta(complex dzeta)
        {
            return 1;
        }

        public complex z(complex dzeta)
        {
            return dzeta;
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
