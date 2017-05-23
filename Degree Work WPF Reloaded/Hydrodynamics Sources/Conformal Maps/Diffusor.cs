using System;

using OxyPlot;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Functions.SpecialFunctions;
using static Degree_Work.Mathematical_Sources.MathematicalConstants;
using static Degree_Work.Mathematical_Sources.Complex.Complex;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Degree_Work.Mathematical_Sources.Complex;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    public class Diffusor : IConformalMapFunction, IEquatable<Diffusor>
    {
        public Complex this[Complex dzeta] => z(dzeta);

        public DataPoint this[DataPoint dzeta] => z(dzeta);

        private double a;

        private int p;

        private double h;

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

        public double AngleDegrees
        {
            get
            {
                return a * 180.0;
            }
            set
            {
                if (value == 15)
                {
                    p = 12;
                }
                else if (value == 18)
                {
                    p = 10;
                }
                else if (value  == 22.5)
                {
                    p = 8;
                }
                else if (value == 30)
                {
                    p = 6;
                }
                else if (value == 36)
                {
                    p = 5;
                }
                else if (value == 45)
                {
                    p = 4;
                }
                else if (value == 60)
                {
                    p = 3;
                }
                else if (value == 90)
                {
                    p = 2;
                }
                else { throw new ArgumentException("invalid angle!"); }
                a = 1.0 / p;
            }
        }

        public Diffusor(double h, int p)
        {
            this.h = h; this.p = p;
            a = 1.0 / p;
            AngleDegrees = a * 180.0;
        }

        public Diffusor(double h, float angle)
        {
            this.H = h;
            AngleDegrees = angle;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            switch (p)
            {
                case 2: return (H / Pi) * (2 * Sqrt(Exp(dzeta) + 1) + Ln(Sqrt(Exp(dzeta) + 1) - 1) - Ln(Sqrt(Exp(dzeta) + 1) + 1));
                case 3: return H * (0.3e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1) - Ln(Pow(0.1e1 + Exp(dzeta), 0.2e1 / 0.3e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1) + 0.1e1) / 0.2e1 - Sqrt(0.3e1) * Arctg((0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1) + 0.1e1) * Sqrt(0.3e1) / 0.3e1) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1) - 0.1e1) + Sqrt(0.3e1) * Pi / 0.6e1) / Pi;
                case 4: return H / Pi * (0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.4e1) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.4e1) - 0.1e1) - Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.4e1) + 0.1e1) - 0.2e1 * Arctg(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.4e1)));
                case 5: return H / Pi * (-Ln(Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.2e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) / 0.16e2 + Ln(-Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.2e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) / 0.16e2 - Arctg((-Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.1e1) * Pow(0.10e2 + 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) / 0.4e1 - Arctg(0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) * (-0.5e1 + Sqrt(0.5e1)) * Pow(0.10e2 - 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1) / (Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.4e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) / 0.4e1 + Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) / 0.4e1 - Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) * Ln(Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.2e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) / 0.80e2 - Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) * Ln(-Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.2e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) / 0.80e2 + Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) * Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) - 0.1e1) / 0.20e2 - Arctg((-Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.1e1) * Pow(0.10e2 + 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) / 0.4e1 + Sqrt(0.5e1) * Arctg(0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) * (-0.5e1 + Sqrt(0.5e1)) * Pow(0.10e2 - 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1) / (Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + 0.4e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) / 0.4e1 - Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) * Arctg(0.3e1 / 0.20e2 * Sqrt(0.5e1) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) / 0.4e1) / 0.2e1 + Ln(0.2e1) / 0.2e1);
                case 6: return H * (0.6e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) - 0.1e1) - Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1) / 0.2e1 - 0.2e1 * Sqrt(0.3e1) * Arctg((0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1) * Sqrt(0.3e1) / 0.3e1) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1) / 0.2e1 + Sqrt(0.3e1) * Arctg(Sqrt(0.3e1) / (0.1e1 + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.3e1))) - Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1)) / Pi;
                case 8: return H / Pi * (0.8e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1) - 0.1e1) - Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1) + 0.1e1) - 0.2e1 * Arctg(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1)) - 0.2e1 * Sqrt(0.2e1) * Arctg(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1) * Sqrt(0.2e1) + 0.1e1) + Sqrt(0.2e1) * Arctg(Pow(0.1e1 + Exp(dzeta), -0.1e1 / 0.4e1)) - Sqrt(0.2e1) * Ln((Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.4e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1) * Sqrt(0.2e1) + 0.1e1) / (Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.4e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.8e1) * Sqrt(0.2e1) + 0.1e1)) / 0.2e1);
                case 10: return H / Pi * Sqrt(0.5e1) * (-Ln(Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) + Ln(-Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - Ln(Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) + Ln(-Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) + 0.40e2 * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.4e1 * Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - Ln(Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - Ln(-Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - 0.4e1 * Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) + Ln(Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) + Ln(-Sqrt(0.5e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.5e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - 0.4e1 * Arctg((-Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * Pow(0.10e2 + 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) - 0.4e1 * Arctg((Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * Pow(0.10e2 + 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.5e1) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) + 0.4e1 * Arctg((Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * Pow(0.10e2 - 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.5e1) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) + 0.4e1 * Arctg((-Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * Pow(0.10e2 - 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.5e1) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - 0.20e2 * Arctg((-Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * Pow(0.10e2 + 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) - 0.20e2 * Arctg((Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * Pow(0.10e2 + 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.10e2 - 0.2e1 * Sqrt(0.5e1)) - 0.20e2 * Arctg((Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * Pow(0.10e2 - 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1)) - 0.20e2 * Arctg((-Sqrt(0.5e1) + 0.4e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * Pow(0.10e2 - 0.2e1 * Sqrt(0.5e1), -0.1e1 / 0.2e1)) * Sqrt(0.10e2 + 0.2e1 * Sqrt(0.5e1))) / 0.80e2;
                case 12: return H / Pi * (0.12e2 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) - 0.1e1) - Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 - 0.2e1 * Sqrt(0.3e1) * Arctg((0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) * Sqrt(0.3e1) / 0.3e1) - Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) + Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) - Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 + Sqrt(0.3e1) * Arctg(Sqrt(0.3e1) / (0.1e1 + 0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1))) - 0.2e1 * Arctg(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2)) - Sqrt(0.3e1) * Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) + Sqrt(0.3e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 - Arctg(0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + Sqrt(0.3e1)) + Sqrt(0.3e1) * Ln(Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.6e1) - Sqrt(0.3e1) * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 - Arctg(0.2e1 * Pow(0.1e1 + Exp(dzeta), 0.1e1 / 0.12e2) - Sqrt(0.3e1)));
                default: throw new ArgumentException("invalid angle!");
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            if (dzeta == 0) { return 0; }
            return (z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * Settings.Eps) + ((z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * Settings.Eps) - (z(dzeta + Settings.Eps) - z(dzeta - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Mathematical_Sources.Equations.Solve(z, Z);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint z(DataPoint dzeta)
        {
            return z((Complex)dzeta);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dz_ddzeta(DataPoint dzeta)
        {
            return dz_ddzeta((Complex)dzeta);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dzeta(DataPoint Z)
        {
            return dzeta((Complex)Z);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Diffusor ? Equals((Diffusor)obj) : false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(Diffusor other) => h == other.h && a == other.a && p==other.p;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => h.GetHashCode() ^ a.GetHashCode() ^ p.GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => "Diffusor";
    }
}
