using System;
using MathCore_2_0;
using OxyPlot;
using static MathCore_2_0.math;
using static MathCore_2_0.complex;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    public class Diffusor : IConformalMapFunction
    {
        public complex this[complex dzeta] => z(dzeta);
        public DataPoint this[DataPoint dzeta] => z(dzeta);
        private double a;
        private int p;
        public double h;
        public float angleDegrees
        {
            get
            {
                return (float)(a * 180.0);
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
            angleDegrees =(float)(a * 180.0);
        }

        public Diffusor(double h, float angle)
        {
            this.h = h;
            angleDegrees = angle;
        }

        public complex z(complex dzeta)
        {
            switch (p)
            {
                case 2: return (h / pi) * (2 * math.sqrt(math.exp(dzeta) + 1) + math.ln(math.sqrt(math.exp(dzeta) + 1) - 1) - math.ln(math.sqrt(math.exp(dzeta) + 1) + 1));
                case 3: return h * (0.3e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.2e1 / 0.3e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1) + 0.1e1) / 0.2e1 - Math.Sqrt(0.3e1) * math.arctg((0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1) + 0.1e1) * Math.Sqrt(0.3e1) / 0.3e1) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1) - 0.1e1) + Math.Sqrt(0.3e1) * Math.PI / 0.6e1) / Math.PI;
                case 4: return h / Math.PI * (0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.4e1) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.4e1) - 0.1e1) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.4e1) + 0.1e1) - 0.2e1 * math.arctg(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.4e1)));
                case 5: return h / Math.PI * (-math.ln(math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.2e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) / 0.16e2 + math.ln(-math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.2e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) / 0.16e2 - math.arctg((-math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.1e1) * math.pow(0.10e2 + 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) / 0.4e1 - math.arctg(0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) * (-0.5e1 + math.sqrt(0.5e1)) * math.pow(0.10e2 - 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1) / (math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.4e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) / 0.4e1 + math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) / 0.4e1 - math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) * math.ln(math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.2e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) / 0.80e2 - math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) * math.ln(-math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.2e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.2e1) / 0.80e2 + math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) * math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) - 0.1e1) / 0.20e2 - math.arctg((-math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.1e1) * math.pow(0.10e2 + 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) / 0.4e1 + math.sqrt(0.5e1) * math.arctg(0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) * (-0.5e1 + math.sqrt(0.5e1)) * math.pow(0.10e2 - 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1) / (math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + 0.4e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) / 0.4e1 - math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) * math.arctg(0.3e1 / 0.20e2 * math.sqrt(0.5e1) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) / 0.4e1) / 0.2e1 + math.ln(0.2e1) / 0.2e1);
                case 6: return h * (0.6e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) - 0.1e1) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1) / 0.2e1 - 0.2e1 * Math.Sqrt(0.3e1) * math.arctg((0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1) * Math.Sqrt(0.3e1) / 0.3e1) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1) / 0.2e1 + Math.Sqrt(0.3e1) * math.arctg(Math.Sqrt(0.3e1) / (0.1e1 + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.3e1))) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + 0.1e1)) / Math.PI;
                case 8: return h / Math.PI * (0.8e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1) - 0.1e1) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1) + 0.1e1) - 0.2e1 * math.arctg(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1)) - 0.2e1 * math.sqrt(0.2e1) * math.arctg(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1) * math.sqrt(0.2e1) + 0.1e1) + math.sqrt(0.2e1) * math.arctg(math.pow(0.1e1 + math.exp(dzeta), -0.1e1 / 0.4e1)) - math.sqrt(0.2e1) * math.ln((math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.4e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1) * math.sqrt(0.2e1) + 0.1e1) / (math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.4e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.8e1) * math.sqrt(0.2e1) + 0.1e1)) / 0.2e1);
                case 10: return h / Math.PI * math.sqrt(0.5e1) * (-math.ln(math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) + math.ln(-math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - math.ln(math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) + math.ln(-math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) + 0.40e2 * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.4e1 * math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - math.ln(math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - math.ln(-math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - 0.4e1 * math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) + math.ln(math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) + math.ln(-math.sqrt(0.5e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.5e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.2e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - 0.4e1 * math.arctg((-math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * math.pow(0.10e2 + 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) - 0.4e1 * math.arctg((math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * math.pow(0.10e2 + 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.5e1) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) + 0.4e1 * math.arctg((math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * math.pow(0.10e2 - 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.5e1) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) + 0.4e1 * math.arctg((-math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * math.pow(0.10e2 - 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.5e1) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - 0.20e2 * math.arctg((-math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * math.pow(0.10e2 + 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) - 0.20e2 * math.arctg((math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * math.pow(0.10e2 + 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.10e2 - 0.2e1 * math.sqrt(0.5e1)) - 0.20e2 * math.arctg((math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) + 0.1e1) * math.pow(0.10e2 - 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1)) - 0.20e2 * math.arctg((-math.sqrt(0.5e1) + 0.4e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.10e2) - 0.1e1) * math.pow(0.10e2 - 0.2e1 * math.sqrt(0.5e1), -0.1e1 / 0.2e1)) * math.sqrt(0.10e2 + 0.2e1 * math.sqrt(0.5e1))) / 0.80e2;
                case 12: return h / Math.PI * (0.12e2 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) - 0.1e1) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 - 0.2e1 * math.sqrt(0.3e1) * math.arctg((0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) * math.sqrt(0.3e1) / 0.3e1) - math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) + math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) - math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 + math.sqrt(0.3e1) * math.arctg(math.sqrt(0.3e1) / (0.1e1 + 0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1))) - 0.2e1 * math.arctg(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2)) - math.sqrt(0.3e1) * math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) + math.sqrt(0.3e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 - math.arctg(0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + math.sqrt(0.3e1)) + math.sqrt(0.3e1) * math.ln(math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.6e1) - math.sqrt(0.3e1) * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) + 0.1e1) / 0.2e1 - math.arctg(0.2e1 * math.pow(0.1e1 + math.exp(dzeta), 0.1e1 / 0.12e2) - math.sqrt(0.3e1)));
                default: throw new ArgumentException("invalid angle!");
            }
        }
        public complex dz_ddzeta(complex dzeta)
        {
            if (dzeta == 0) { return 0; }
            return (z(dzeta + precision.eps) - z(dzeta - precision.eps)) / (2 * precision.eps) + ((z(dzeta + precision.eps) - z(dzeta - precision.eps)) / (2 * precision.eps) - (z(dzeta + precision.eps) - z(dzeta - precision.eps)) / (2 * 2 * precision.eps)) / 3;
        }
        public complex dzeta(complex Z)
        {
            return equations.solve(z, Z);
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

        public override bool Equals(object obj) => obj is Diffusor ? Equals((Diffusor)obj) : false;

        bool Equals(Diffusor other) => h == other.h && a == other.a;

        public override int GetHashCode() => h.GetHashCode() ^ a.GetHashCode();

        public override string ToString() => "Diffusor";
    }
}
