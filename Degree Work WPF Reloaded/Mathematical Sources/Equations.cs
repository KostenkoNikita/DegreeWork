#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Degree_Work.Mathematical_Sources.Complex;
using Degree_Work.Mathematical_Sources.Functions;

namespace Degree_Work.Mathematical_Sources
{
    public static class Equations
    {
        //уравнение вида f(z)=Z относительно z
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f, Complex.Complex Z)
        {
            Complex.Complex initial_approximation = Z, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c) - Z; }, dFdz = (c) => { return FirstDerivative(f, c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:
            initial_approximation += (rnd.NextDouble() + Complex.Complex.I * rnd.NextDouble());
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if ((f(z) - Z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f, Func<Complex.Complex, Complex.Complex> df, Complex.Complex Z)
        {
            Complex.Complex initial_approximation = Z, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c) - Z; }, dFdz = (c) => { return df(c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:
            initial_approximation += (rnd.NextDouble() + Complex.Complex.I * rnd.NextDouble());
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if ((f(z) - Z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f)
        {
            Complex.Complex initial_approximation = 0, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c); }, dFdz = (c) => { return FirstDerivative(f, c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:
            initial_approximation += (rnd.NextDouble() + Complex.Complex.I * rnd.NextDouble());
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if (f(z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }
        public static Complex.Complex Solve(Func<Complex.Complex, Complex.Complex> f, Func<Complex.Complex, Complex.Complex> df)
        {
            Complex.Complex initial_approximation = 0, z, tmp;
            int fails = 0, iters;
            Func<Complex.Complex, Complex.Complex> F = (c) => { return f(c); }, dFdz = (c) => { return df(c); };
            Random rnd = new Random(DateTime.Now.Millisecond);

            fail_mark:
            initial_approximation += (rnd.NextDouble() + Complex.Complex.I * rnd.NextDouble());
            z = initial_approximation;
            tmp = z;
            iters = 0;
            while (fails < 50)
            {
                iters++;
                z = z - F(z) / dFdz(z);
                if ((z - tmp).Abs <= Settings.Eps)
                {
                    if (f(z).Abs <= Settings.Eps) { return z; }
                    else { fails++; goto fail_mark; }
                }
                if (Complex.Complex.IsNaN(z) || Complex.Complex.IsInfinity(z) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = z;
            }
            return Complex.Complex.NaN;
        }
        //уравнение вида f(x)=0
        public static double Solve(Func<double, double> f)
        {
            double initial_approximation = 0, x, tmp;
            int fails = 0, iters;
            Func<double, double> dfdx = (c) => { return FirstDerivative(f, c); };
            Random rnd = new Random(DateTime.Now.Millisecond);
            fail_mark:
            initial_approximation += rnd.NextDouble();
            x = initial_approximation;
            tmp = x;
            iters = 0;
            while (fails < 100)
            {
                iters++;
                x = x - f(x) / dfdx(x);
                if (Math.Abs(x - tmp) <= Settings.Eps)
                {
                    if (Math.Abs(x) <= Settings.Eps) { return x; }
                    else { fails++; goto fail_mark; }
                }
                if (double.IsNaN(x) || double.IsInfinity(x) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = x;
            }
            return double.NaN;
        }
        public static double Solve(Func<double, double> f, Func<double, double> dfdx)
        {
            double initial_approximation = 0, x, tmp;
            int fails = 0, iters;
            Random rnd = new Random(DateTime.Now.Millisecond);
            fail_mark:
            initial_approximation += rnd.NextDouble();
            x = initial_approximation;
            tmp = x;
            iters = 0;
            while (fails < 100)
            {
                iters++;
                x = x - f(x) / dfdx(x);
                if (Math.Abs(x - tmp) <= Settings.Eps)
                {
                    if (Math.Abs(x) <= Settings.Eps) { return x; }
                    else { fails++; goto fail_mark; }
                }
                if (double.IsNaN(x) || double.IsInfinity(x) || iters > 100)
                {
                    fails++; goto fail_mark;
                }
                tmp = x;
            }
            return double.NaN;
        }
        public static double NSolve(Func<double, double> f, double initial_approximation)
        {
            double x = initial_approximation, tmp = x;
            Func<double, double> dfdx = (c) => { return FirstDerivative(f, c); };
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { if (Math.Abs(f(x)) < Settings.Eps) { return x; } else { throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation."); } }
                tmp = x;
            }
            throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation.");
        }
        public static double NSolve(Func<double, double> f, Func<double, double> dfdx, double initial_approximation)
        {
            double x = initial_approximation, tmp = x;
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { if (Math.Abs(f(x)) < Settings.Eps) { return x; } else { throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation."); } }
                tmp = x;
            }
            throw new ApplicationException("Can't solve an equation f(x)=0. Try to use another initial approximation.");
        }
        public static bool TryNSolve(Func<double, double> f, double initial_approximation, out double x)
        {
            x = initial_approximation; double tmp = x;
            Func<double, double> dfdx = (c) => { return FirstDerivative(f, c); };
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { return Math.Abs(f(x)) < Settings.Eps ? true : false; }
                tmp = x;
            }
            return false;
        }
        public static bool TryNSolve(Func<double, double> f, Func<double, double> dfdx, double initial_approximation, out double x)
        {
            x = initial_approximation; double tmp = x;
            for (int i = 0; i < 1000; i++)
            {
                x -= f(x) / dfdx(x);
                if (Math.Abs(tmp - x) < Settings.Eps) { return Math.Abs(f(x)) < Settings.Eps ? true : false; }
                tmp = x;
            }
            return false;
        }
        private static Complex.Complex FirstDerivative(Func<Complex.Complex, Complex.Complex> f, Complex.Complex z)
        {
            return (f(z + Settings.Eps) - f(z - Settings.Eps)) / (2 * Settings.Eps) + ((f(z + Settings.Eps) - f(z - Settings.Eps)) / (2 * Settings.Eps) - (f(z + Settings.Eps) - f(z - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }
        private static double FirstDerivative(Func<double, double> f, double x)
        {
            return (f(x + Settings.Eps) - f(x - Settings.Eps)) / (2 * Settings.Eps) + ((f(x + Settings.Eps) - f(x - Settings.Eps)) / (2 * Settings.Eps) - (f(x + Settings.Eps) - f(x - Settings.Eps)) / (2 * 2 * Settings.Eps)) / 3;
        }
    }
}
