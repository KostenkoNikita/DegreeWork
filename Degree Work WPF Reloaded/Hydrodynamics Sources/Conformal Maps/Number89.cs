using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore_2_0;
using static MathCore_2_0.complex;
using static MathCore_2_0.math;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class Number89 : Hydrodynamics_Sources.IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] { get { return z(dzeta); } }

        public double h1, h2;

        public double lam => equations.nsolve(lamRootFunc, lamRootFuncD, 0.1);

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

        public override bool Equals(object obj) => obj.GetType() == typeof(Number89) ? Equals((Number89)obj) : false;

        public override int GetHashCode() => h1.GetHashCode()^h2.GetHashCode();

        public override string ToString() => "Number89";

        bool Equals(Number89 other) => h1==other.h1 && h2==other.h2;

        public complex z(complex dzeta)
        {
            complex tmp = i + ((h1 - h2) / (pi * lam)) * (sqrt(dzeta * dzeta - 1) - lam * arch(dzeta));
            return tmp.Im<0? -tmp:tmp;
        }

        public complex dz_ddzeta(complex dzeta)
        {
            complex tmp1 = i + ((h1 - h2) / (pi * lam)) * (sqrt(dzeta * dzeta - 1) - lam * arch(dzeta));
            complex tmp = ((h1 - h2) / (lam * Math.PI)) * (-1.0 / (1 - dzeta * dzeta) + dzeta / (sqrt(dzeta * dzeta - 1)));
            if (tmp.Re < 0) { tmp = -(tmp.conjugate); }
            return tmp;
        }

        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
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
