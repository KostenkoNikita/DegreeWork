using System;
using MathCore_2_0;
using OxyPlot;
using static MathCore_2_0.complex;
using static MathCore_2_0.math;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Plate : IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] => z(dzeta);

        public override bool Equals(object obj)
        {
            return obj is Plate ? base.Equals(obj) : false;
        }
        public override string ToString()
        {
            return "Plate";
        }
        public override int GetHashCode()
        {
            return (typeof(Plate)).GetHashCode();
        }
        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
        }
        public complex dz_ddzeta(complex dzeta)
        {
            return 0.5 - 0.5 / (dzeta * dzeta);
        }
        public complex z(complex dzeta)
        {
            return 0.5 * (dzeta + 1 / dzeta);
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
