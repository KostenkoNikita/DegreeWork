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
    class Porebrick : IConformalMapFunction
    {
        public complex this[complex dzeta] => z(dzeta);

        public double h { get; private set; }

        public Porebrick(double h)
        {
            this.h = h;
        }

        public override bool Equals(object obj) => obj.GetType() == typeof(Porebrick) ? Equals((Porebrick)obj) : false;

        bool Equals(Porebrick other) => h == other.h;

        public override int GetHashCode() => h.GetHashCode() ^ (typeof(Porebrick)).GetHashCode();

        public override string ToString() => "Porebrick";

        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
        }

        public complex dz_ddzeta(complex dzeta)
        {
            complex tmp = i * h + (h / pi) * (sqrt(dzeta * dzeta - 1) - arch(dzeta));
            if (tmp.Im < 0)
            {
                return -(h / pi) * ((dzeta - 1) / (sqrt(dzeta * dzeta - 1)));
            }
            else
            {
                return (h / pi) * ((dzeta - 1) / (sqrt(dzeta * dzeta - 1)));
            }
        }

        public complex z(complex dzeta)
        {
            complex tmp = i * h + (h / pi) * (sqrt(dzeta * dzeta - 1) - arch(dzeta));
            if (tmp.Im < 0)
            {
                return -i * h - (h / pi) * (sqrt(dzeta * dzeta - 1) - arch(dzeta));
            }
            else
            {
                return tmp;
            }
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
