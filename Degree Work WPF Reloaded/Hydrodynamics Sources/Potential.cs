#pragma warning disable 169
#pragma warning disable 168
//#define HELP_FOR_GROUP_LEADER

using System;
using MathCore_2_0;
using static MathCore_2_0.math;
using static MathCore_2_0.complex;

namespace Degree_Work.Hydrodynamics_Sources
{
    class Potential
    {
        public complex this[complex dzeta] { get { return W(dzeta); } }
        double _V_inf;
        double _alpha;
        double _R;
        double _G;
        IConformalMapFunction _f;

        public double V_inf
        {
            get
            {
                return _V_inf;
            }
            set
            {
                if (value > 0) { _V_inf = value; }
                else if (value < 0) { _V_inf = -value; }
                else { throw new ArgumentException(); }
            }
        }
        public double R
        {
            get
            {
                return _R;
            }
            set
            {
                _R = value == 0 ? throw new ArgumentException() : Math.Abs(value);
            }
        }
        public double G
        {
            get
            {
                return _G;
            }
            set
            {
                _G = Math.Abs(value);
            }
        }
        public double AlphaRadians
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }
        }
        public double AlphaDegrees
        {
            get
            {
                return _alpha * 180.0 / Math.PI; ;
            }
            set
            {
                _alpha = value * Math.PI / 180.0;
            }
        }
        public IConformalMapFunction f
        {
            get { return _f; }
            set { _f = value; }
        }
        complex tmp;
        public Potential(double V_inf, double alpha, double R, double G, IConformalMapFunction f)
        {
            this.f = f;
            this._V_inf = V_inf;
            this._R = R;
            this._alpha = R == 0 ? 0 : alpha;
            this._G = R == 0 ? 0 : G;
        }
        public double phi(complex z)
        {
            return this[z].Re;
        }
        public double psi(complex z)
        {
            return this[z].Im;
        }
        public complex V(complex dzeta)
        {
            return dW_ddzeta(dzeta).conjugate;
        }
        public complex V_physical_plane(complex z)
        {
            try
            {
                tmp = f.dzeta(z);
                return IsNaN(tmp) ? NaN : (dW_ddzeta(z) / f.dz_ddzeta(tmp)).conjugate;
            }
            catch { return NaN; }
        }

        public double V_ksi(complex dzeta)
        {
            return dW_ddzeta(dzeta).conjugate.Re;
        }
        public double V_eta(complex dzeta)
        {
            return dW_ddzeta(dzeta).conjugate.Im;
        }
        public complex W(complex dzeta)
        {
            return _V_inf * exp(-i * this._alpha) * dzeta + (_R * _R * _V_inf * exp(i * _alpha)) / dzeta + this._G * ln(dzeta) / (2 * pi * i);
        }
        public complex dW_ddzeta(complex dzeta)
        {
            return _V_inf * exp(-i * this._alpha) - (_R * _R * _V_inf * exp(i * this._alpha)) / (dzeta * dzeta) + this._G / (2 * pi * i * dzeta);
        }
        public override string ToString()
        {
            return $"V={_V_inf},alpha={_alpha},R={_R},G={_G}";
        }
    }

#if HELP_FOR_GROUP_LEADER
    class PotentialHelp
    {
        public complex this[complex dzeta] { get { return W(dzeta); } }

        public double V0, Sh, a;
        IConformalMapFunction _f;

        complex tmp;
        public PotentialHelp(double V0, double Sh, double a)
        {
            this.V0 = 1;
            this.Sh = Sh;
            this.a = a;
            _f = new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform();
        }
        public double phi(complex z)
        {
            return this[z].Re;
        }
        public double psi(complex z)
        {
            return this[z].Im;
        }
        public complex V(complex dzeta)
        {
            return dW_ddzeta(dzeta).conjugate;
        }
        public complex V_physical_plane(complex z)
        {
            return new complex();
        }
        public double V_ksi(complex dzeta)
        {
            return dW_ddzeta(dzeta).conjugate.Re;
        }
        public double V_eta(complex dzeta)
        {
            return dW_ddzeta(dzeta).conjugate.Im;
        }
        public complex W(complex dzeta)
        {
            //return -(1.0 / pi) * (V0 * ((dzeta - 1) * ln(dzeta - a) - (dzeta + a) * ln(dzeta + a) + 2 * a)
            //    + (Sh / 4.0) * (2 * (dzeta - a) * (dzeta + a) * (ln(dzeta - a) - ln(dzeta + a)) + 3 * a * a - 4 * a * dzeta)
            //    + 2 * Sh * a * dzeta);
            return (-1 / pi) * ((dzeta - 1) * ln(dzeta - 1) - (dzeta + 1) * ln(dzeta + 1) +
   2 + 0.25 *
    Sh * (4 * dzeta + 3 +
      2 * (dzeta - 1) * (dzeta + 1) * ln((dzeta - 1) / (dzeta + 1))));
        }
        public complex dW_ddzeta(complex dzeta)
        {
            //return -(V0 * (ln(dzeta - a) - ln(dzeta + a)) + Sh * (0.2e1 * (dzeta + a) * (ln(dzeta - a) - ln(dzeta + a)) + 0.2e1 * (dzeta - a) * (ln(dzeta - a) - ln(dzeta + a)) + 0.2e1 * (dzeta - a) * (dzeta + a) * (0.1e1 / (dzeta - a) - 0.1e1 / (dzeta + a))) / 0.4e1) / Math.PI;
            return 0.31831*(-2*Sh - ln(-1 + dzeta) - dzeta*Sh*ln((-1 + dzeta) / (1 + dzeta)) + ln(1 + dzeta));
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
#endif
}
