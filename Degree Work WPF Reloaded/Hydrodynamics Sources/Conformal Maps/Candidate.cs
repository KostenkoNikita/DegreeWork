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
    class Candidate : Hydrodynamics_Sources.IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] { get { return z(dzeta); } }

        public Candidate()
        { 
        }

        public override bool Equals(object obj) => obj.GetType() == typeof(Candidate) ? Equals((Candidate)obj) : false;

        public override int GetHashCode() => 0;

        public override string ToString() => "EjectedRays";

        bool Equals(Candidate other) => true;

        public complex z(complex dzeta)
        {
            return dzeta;
            //CONFIRMED
            //double h = 1;
            //complex tmp = dzeta + sqrt(dzeta * dzeta - 1);
            //return tmp.Im<0? 2*dzeta - 2*sqrt(dzeta * dzeta - 1) :2*tmp;
            //CONFIRMED
            //double h1 = 2;
            //double h2 = 1;
            //double lam = 0.217234;
            //complex tmp = i+ ((h1 - h2) / (pi * lam)) * (sqrt(dzeta*dzeta-1)-lam*arch(dzeta));
            //if (tmp.Im < 0)
            //{
            //    return -i-((h1 - h2) / (pi * lam)) * (sqrt(dzeta * dzeta - 1) - lam * arch(dzeta));
            //}
            //else
            //{
            //    return tmp;
            //}
        }

        public complex dz_ddzeta(complex dzeta)
        {
            return 1;
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
