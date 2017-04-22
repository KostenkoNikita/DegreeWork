using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class JoukowskiAirfoil : IConformalMapFunction
    {
        public complex this[complex dzeta] => z(dzeta);

        double _h;

        double _eps;

        public double c => 1;

        public double R => Math.Pow(eps + Math.Sqrt(Math.Pow(h, 2) + Math.Pow(c, 2)), 2);

        public double betaDiv2 => Math.Atan(h / c);

        public double h { get => _h; set => _h = value; }

        public double eps { get => _eps; set => _eps = value; }

        public DataPoint this[DataPoint dzeta] => z(dzeta);

        public JoukowskiAirfoil(double h, double eps)
        {
            _h = h;
            _eps = eps;
        }

        public complex dzeta(complex Z)
        {
            return equations.solve(z, dz_ddzeta, Z);
        }

        public DataPoint dzeta(DataPoint Z)
        {
            return dzeta(Z.DataPointToComplex()).ComplexToDataPoint();
        }

        public complex dz_ddzeta(complex dzeta)
        {
            return (z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * Settings.Eps) + ((z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * Settings.Eps) - (z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }

        public DataPoint dz_ddzeta(DataPoint dzeta)
        {
            return dz_ddzeta(dzeta.DataPointToComplex()).ComplexToDataPoint();
        }

        public complex z(complex dzeta)
        {
            double x, y;
            double ksi = dzeta.Re, eta = dzeta.Im;
            x = (ksi - eps * Math.Cos(betaDiv2)) * (0.1e1 + c * c /(Math.Pow(ksi - eps * Math.Cos(betaDiv2), 0.2e1) +Math.Pow(eta + h + eps * Math.Sin(betaDiv2), 0.2e1)));
            y = (eta + h + eps * Math.Sin(betaDiv2)) *(0.1e1 - c * c / (Math.Pow(ksi - eps * Math.Cos(betaDiv2), 0.2e1) +Math.Pow(eta + h + eps * Math.Sin(betaDiv2), 0.2e1)));
            return new complex(x, y);
        }

        public complex RightStagnationPointToPhysicalPlane(complex dzeta)
        {

           double newX = (dzeta.Re - eps * Math.Cos(betaDiv2)) * (0.1e1 + c * c /(Math.Pow(dzeta.Re - eps * Math.Cos(betaDiv2), 0.2e1) +Math.Pow(dzeta.Im + h + eps * Math.Sin(betaDiv2), 0.2e1)));
           double newY = (dzeta.Im + h + eps * Math.Sin(betaDiv2)) *(0.1e1 - c * c / (Math.Pow(dzeta.Re - eps * Math.Cos(betaDiv2), 0.2e1) +Math.Pow(dzeta.Im + h + eps * Math.Sin(betaDiv2), 0.2e1)));
            return new complex(newX, newY);
        }

        public DataPoint z(DataPoint dzeta)
        {
            return z(dzeta.DataPointToComplex()).ComplexToDataPoint();
        }

        public override string ToString()
        {
            return "JoukowskiAirfoil";
        }
    }
}
