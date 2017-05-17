using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Degree_Work.Mathematical_Sources.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    class JoukowskiAirfoil : IConformalMapFunction
    {
        public Complex this[Complex dzeta] => z(dzeta);

        public DataPoint this[DataPoint dzeta] => z(dzeta);

        double _h;

        double _eps;

        public double c => 1;

        public double R => Math.Pow(eps + Math.Sqrt(Math.Pow(h, 2) + Math.Pow(c, 2)), 2);

        public double betaDiv2 => Math.Atan(h / c);

        public double h { get => _h; set => _h = value; }

        public double eps { get => _eps; set => _eps = value; }

        public JoukowskiAirfoil(double h, double eps)
        {
            _h = h;
            _eps = eps;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Mathematical_Sources.Equations.Solve(z, dz_ddzeta, Z);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dzeta(DataPoint Z)
        {
            return dzeta((Degree_Work.Mathematical_Sources.Complex.Complex)Z);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            return (z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * Settings.Eps) + ((z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * Settings.Eps) - (z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dz_ddzeta(DataPoint dzeta)
        {
            return dz_ddzeta((Degree_Work.Mathematical_Sources.Complex.Complex)dzeta);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            double x, y;
            double ksi = dzeta.Re, eta = dzeta.Im;
            x = (ksi - eps * Cos(betaDiv2)) * (0.1e1 + c * c /(Pow(ksi - eps * Cos(betaDiv2), 0.2e1) +Pow(eta + h + eps * Sin(betaDiv2), 0.2e1)));
            y = (eta + h + eps * Sin(betaDiv2)) *(0.1e1 - c * c / (Pow(ksi - eps * Cos(betaDiv2), 0.2e1) +Pow(eta + h + eps * Sin(betaDiv2), 0.2e1)));
            return new Complex(x, y);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex RightStagnationPointToPhysicalPlane(Complex dzeta)
        {

           double newX = (dzeta.Re - eps * Cos(betaDiv2)) * (0.1e1 + c * c /(Pow(dzeta.Re - eps * Cos(betaDiv2), 0.2e1) +Pow(dzeta.Im + h + eps * Sin(betaDiv2), 0.2e1)));
           double newY = (dzeta.Im + h + eps * Sin(betaDiv2)) *(0.1e1 - c * c / (Pow(dzeta.Re - eps * Cos(betaDiv2), 0.2e1) +Pow(dzeta.Im + h + eps * Sin(betaDiv2), 0.2e1)));
            return new Complex(newX, newY);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint z(DataPoint dzeta)
        {
            return z((Degree_Work.Mathematical_Sources.Complex.Complex)dzeta);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return "JoukowskiAirfoil";
        }
    }
}
