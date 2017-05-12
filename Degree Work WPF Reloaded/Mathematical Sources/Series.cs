#pragma warning disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Degree_Work.Mathematical_Sources
{
    public static class Series
    {
        public static double BernulliNumber(int n)
        {
            double[] B = new double[n + 1]; B[0] = 1;
            for (int j = 1; j <= n; j++)
            {
                double sum = 0;
                for (int k = 1; k <= j; k++)
                {
                    sum += BinomialCoefficient(k + 1, j + 1) * B[j - k];
                }
                B[j] = (-1.0 / (j + 1)) * sum;
                sum = 0;
            }
            return B[B.Length - 1];
        }
        public static int FibonacciNumber(int n)
        {
            bool isNegative = false;
            if (n < 0) { n = -n; isNegative = true; }
            if (n == 0) { return 0; }
            else
            {
                int[] f = new int[n + 1];
                f[0] = 0; f[1] = 1;
                for (int i = 2; i <= n; i++)
                {
                    f[i] = f[i - 1] + f[i - 2];
                }
                return isNegative ? f[n] * (int)Math.Pow(-1, n + 1) : f[n];
            }
        }
        public static int[] FibonacciNumbers(int n)
        {
            if (n == 0) { return new int[1] { 0 }; }
            else if (n < 0) { throw new ArgumentException(); }
            else
            {
                int[] f = new int[n + 1];
                f[0] = 0; f[1] = 1;
                for (int i = 2; i <= n; i++)
                {
                    f[i] = f[i - 1] + f[i - 2];
                }
                return f;
            }
        }
        public static double BinomialCoefficient(int k, int n)
        {
            if (n >= k)
            {
                return Factorial(n) / (Factorial(n - k) * Factorial(k));
            }
            else { return 0; }
        }
        public static double Factorial(int n)
        {
            if (n > 0)
            {
                double tmp = 1;
                for (int i = 1; i <= n; i++) { tmp *= i; }
                return tmp;
            }
            else if (n == 0) { return 1; }
            else { throw new ArgumentException(); }
        }
        public static double DoubleFactorial(int n)
        {
            if (n == 0) { return 1; }
            else if (n < 0)
            {
                throw new ArgumentException();
            }
            else
            {
                if (n % 2 == 0)
                {
                    return Convert.ToDouble(Math.Pow(2, (double)(n / 2)) * Factorial((int)(n / 2)));
                }
                else
                {
                    return Convert.ToDouble(Factorial(n) / (Math.Pow(2, (double)((n - 1) / 2)) * Factorial((int)((n - 1) / 2))));
                }
            }
        }
    }
}
