#pragma warning disable

using System;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Series;
using System.Runtime.CompilerServices;

namespace Degree_Work.Mathematical_Sources.Functions
{
    using Complex = Complex.Complex;
    /// <summary>
    /// Специальные математические функции
    /// </summary>
    public static class SpecialFunctions
    {
        /// <summary>
        /// Логарифм гамма-функции
        /// </summary>
        /// <param name="x">Число с плавающей точкой</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double LnG(double x)
        {
            return RB(x);
        }

        /// <summary>
        /// Логарифм гамма-функции
        /// </summary>
        /// <param name="z">Комплексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex LnG(Complex z)
        {
            return SB(z);
        }

        /// <summary>
        /// Гамма-функция
        /// </summary>
        /// <param name="x">Число с плавающей точкой</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Gamma(double x)
        {
            if ((!double.IsInfinity(x) && !double.IsNaN(x)))
            {
                return QB(x);
            }
            if (double.IsNaN(x))
            {
                return x;
            }
            if (x > 0.0)
            {
                return double.PositiveInfinity;
            }
            return double.NaN;
        }

        /// <summary>
        /// Гамма-функция
        /// </summary>
        /// <param name="z">Комплексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Gamma(Complex z)
        {
            if (Complex.IsNaN(z))
            {
                return z;
            }
            if (z.Im == 0)
            {
                return new Complex(Gamma(z.Re), 0);
            }
            return Exp(LnG(z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Digamma(int n)
        {
            if (n == 1)
            {
                return -MathematicalConstants.EulerMascheroniConstant;
            }
            else if (n < 1) { throw new ArgumentException(); }
            else
            {
                Complex s = 0, tmp = 10;
                for (int i = 1; i <= n + 1; i++)
                {
                    s += 1.0 / i;
                }
                return s - MathematicalConstants.EulerMascheroniConstant;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Digamma(Complex z)
        {
            bool biggerPI = false;
            if (Abs(z.ArgRadians) >= Math.PI) { z = z.Conjugate; biggerPI = true; }
            Complex s = 0, tmp = 10;
            for (int n = 1; ; n++)
            {
                s += BernulliNumber(2 * n) / (2 * n * Pow(z, 2 * n));
                if (Abs(s.Re - tmp.Re) < Settings.Eps && Abs(s.Im - tmp.Im) < Settings.Eps)
                {
                    s = Ln(z) - 1.0 / (2 * z) - s;
                    if (biggerPI) { return s.Conjugate; }
                    else { return s; }
                }
                if (Complex.IsNaN(s)) { return Complex.Infinity; }
                if (n > 1000) { throw new ApplicationException("Can't calculate the digamma-function."); }
                tmp = s;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Pochhammer(double x, double n)
        {
            return Gamma(x + n) / Gamma(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Beta(Complex z, Complex w)
        {
            if (z.Im == 0 && w.Im == 0) { return Gamma(z.Re) * Gamma(w.Re) / (Gamma(z.Re + w.Im)); }
            else if (z.Im != 0 && w.Im != 0) { return Gamma(z) * Gamma(w) / (Gamma(z + w)); }
            else if (z.Im == 0 && w.Im != 0) { return Gamma(z.Re) * Gamma(w) / (Gamma(z + w)); }
            else { return Gamma(z) * Gamma(w.Re) / (Gamma(z + w)); }
        }

        /// <summary>
        /// Гипергеометрическая функция Гаусса
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Complex Hypergeometric2F1(double a, double b, double c, Complex z)
        {
            //с!=0,-1,-2,...
            if (z == 0) { return 1; }
            if (z.Abs > 1)
            {
                if (Abs(((-z).ArgRadians)) < Math.PI)
                {
                    return (Gamma(c) * Gamma(b - a) / (Gamma(b) * Gamma(c - a))) * Hypergeometric2F1(a, a + 1 - c, a + 1 - b, 1.0 / z) * Pow(-z, -a) + (Gamma(c) * Gamma(a - b) / (Gamma(a) * Gamma(c - b))) * Hypergeometric2F1(b, b + 1 - c, b + 1 - a, 1.0 / z) * Pow(-z, -b);
                }
                else
                {
                    return ((Gamma(c) * Gamma(b - a) / (Gamma(b) * Gamma(c - a))) * Hypergeometric2F1(a, a + 1 - c, a + 1 - b, 1.0 / z.Conjugate) * Pow((-z).Conjugate, -a) + (Gamma(c) * Gamma(a - b) / (Gamma(a) * Gamma(c - b))) * Hypergeometric2F1(b, b + 1 - c, b + 1 - a, 1.0 / z.Conjugate) * Pow((-z).Conjugate, -b)).Conjugate;
                }
            }
            else
            {
                Complex sum = 1, tmp = sum, comp = 1;
                for (int k = 1; ; k++)
                {
                    for (int l = 0; l <= k - 1; l++)
                    {
                        comp *= (a + l) * (b + l) / ((1 + l) * (c + l));
                    }
                    comp *= Pow(z, k);
                    sum += comp;
                    if ((sum - tmp).Abs <= Settings.Eps) { return sum; }
                    tmp = sum;
                    comp = 1;
                }
            }
        }

        #region Additions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double SinPi(double x)
        {
            x -= 2.0 * Math.Round(x / 2.0);
            if (Abs(x) == 1.0)
            {
                return 0.0;
            }
            if (Abs(x) != 0.5)
            {
                return Sin(x * 3.1415926535897931);
            }
            if (x <= 0.0)
            {
                return -1.0;
            }
            return 1.0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Complex SB(Complex z)
        {
            bool flag = false;
            if (z.Re < 0.0)
            {
                z = 1.0 - z;
                flag = true;
            }
            z = --z;
            double num;
            double[] array;
            if (z.Re > 50.0)
            {
                num = 5.5407;
                array = TD;
            }
            else
            {
                num = 3.6998328;
                array = SD;
            }
            Complex _Complex = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                _Complex += array[i] / (z + i);
            }
            Complex Complex2 = -(num + 0.5 + z) + (0.5 + z) * Ln(num + 0.5 + z) + Ln(_Complex);
            if (flag)
            {
                Complex2 = Ln(3.1415926535897931 / Sin(3.1415926535897931 * (1.0 - z))) - Complex2 - (3.1415926535897931 * Complex.I);
            }
            return Complex2 - (Complex.I * Math.Floor(Complex2.Im / 6.2831853071795862 + 0.5) * 6.2831853071795862);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double RB(double num)
        {
            int num2 = (int)num;
            if (num == num2)
            {
                if (num2 <= 0)
                {

                    return double.PositiveInfinity;
                }
                if (num2 < Z.Length)
                {

                    return Z[num2 - 1];
                }
            }
            if (Abs(num) <= 9.9)
            {

                return Math.Log(Abs(Gamma(num)));
            }
            bool flag = false;
            if (num < 0.0)
            {

                num = -num + 1.0;
                flag = true;
            }
            num -= 1.0;
            double num3;
            double[] array;
            if (num > 50.0)
            {

                num3 = 5.5407;
                array = TD;
            }
            else
            {
                num3 = 3.6998328;
                array = SD;
            }
            double num4 = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                num4 += array[i] / (num + (double)i);
            }

            double num5 = -(num3 + 0.5 + num) + (0.5 + num) * Math.Log(num3 + 0.5 + num) + Math.Log(num4);
            if (flag)
            {

                num5 = Math.Log(Abs(3.1415926535897931 / SinPi(num))) - num5;
            }
            return num5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double QB(double num)
        {
            int i = (int)num;
            if (num == (double)i)
            {
                if (i <= 0)
                {
                    return double.NaN;
                }
                return Factorial(i - 1);
            }
            else
            {
                if (Abs(num) > 20.0)
                {
                    double num2 = Exp(LnG(num));
                    if (num < 0.0)
                    {
                        if ((int)Math.Floor(num) % 2 != 0)
                        {
                            return -num2;
                        }
                    }
                    return num2;
                }
                if (Abs(num) >= 2.2474362225598545E-308)
                {
                    double num3 = num - Math.Round(num);
                    double num4;
                    if (Abs(num3) <= 0.1)
                    {
                        if (num3 == 0.0)
                        {
                            if (num <= 0.0)
                            {
                                return double.NaN;
                            }
                            return Factorial((int)num - 1);
                        }
                        else
                        {
                            i = (int)Math.Round(num) - 1;
                            num4 = (1.0 + num3 * (0.43034126526705851 + num3 * (0.22266730777422281 + num3 * (0.040851751767329046 + num3 * 0.011565315597477564)))) / (1.0 + num3 * (1.007556930168588 - num3 * (0.18481104418026204 + num3 * (0.15487522431232129 - num3 * 0.0375661264693936))));
                            num3 += 1.0;
                        }
                    }
                    else
                    {
                        num3 = num - Math.Floor(num);
                        i = (int)Math.Floor(num) - 1;
                        num4 = 1.0 / (1.0 + num3) + (-1.3782221508780498E-14 + num3 * (0.42278433509901897 + num3 * (0.041374380109200512 - num3 * (0.0061763349932353441 - num3 * (0.0052668405087916989 - num3 * 7.82874302978088E-05))))) / (1.0 + num3 * (0.12374721683674332 - num3 * (0.23024261024083342 - num3 * (0.022635112892944842 + num3 * (0.01354793760588476 - num3 * (0.0036190944658071257 - num3 * 0.00027330395814104293))))));
                        num3 += 1.0;
                    }
                    while (i > 0)
                    {
                        num4 *= num3;
                        num3 += 1.0;
                        i--;
                    }
                    while (i < 0)
                    {
                        num4 /= num;
                        num += 1.0;
                        i++;
                    }
                    return num4;
                }
                if (num < 0.0)
                {
                    return double.NegativeInfinity;
                }
                return double.PositiveInfinity;
            }
        }

        #endregion

        #region Approximations

        private static readonly double[] TD = new double[]
{
    2.5066282746313515,
    347.05125593395849,
    -461.89006878944838,
    168.64100065122091,
    -15.57186206508017,
    0.14115928520728782,
    1.2869686141588626E-05,
    -3.2349738354906731E-06
};

        private static readonly double[] SD = new double[]
{
    2.5066282746363426,
    43.876640844001564,
    -29.441488119961249,
    2.617298932416928,
    -0.000840299612359108,
    0.00042715899834181971,
    -0.00023054552812795247,
    8.4040594453044693E-05,
    -1.4981925182237168E-05
};

        private static readonly double[] Z = new double[]
{
    0.0,
    0.0,
    0.69314718055994529,
    1.791759469228055,
    3.1780538303479458,
    4.7874917427820458,
    6.5792512120101012,
    8.5251613610654147,
    10.604602902745251,
    12.801827480081469,
    15.104412573075516,
    17.502307845873887,
    19.987214495661885,
    22.552163853123425,
    25.19122118273868,
    27.89927138384089,
    30.671860106080672,
    33.505073450136891,
    36.395445208033053,
    39.339884187199495,
    42.335616460753485,
    45.380138898476908,
    48.471181351835227,
    51.606675567764377,
    54.784729398112319,
    58.003605222980518,
    61.261701761002,
    64.557538627006338,
    67.88974313718154,
    71.257038967168015,
    74.658236348830158,
    78.0922235533153,
    81.557959456115043,
    85.054467017581516,
    88.580827542197682,
    92.1361756036871,
    95.7196945421432,
    99.330612454787428,
    102.96819861451381,
    106.63176026064346,
    110.32063971475739,
    114.03421178146171,
    117.77188139974507,
    121.53308151543864,
    125.3172711493569,
    129.12393363912722,
    132.95257503561632,
    136.80272263732635,
    140.67392364823425,
    144.5657439463449,
    148.47776695177302,
    152.40959258449735,
    156.3608363030788,
    160.3311282166309,
    164.32011226319517,
    168.32744544842765,
    172.35279713916279,
    176.39584840699735,
    180.45629141754378,
    184.53382886144948,
    188.6281734236716,
    192.7390472878449,
    196.86618167289,
    201.00931639928152,
    205.1681994826412,
    209.34258675253685,
    213.53224149456327,
    217.73693411395422,
    221.95644181913033,
    226.1905483237276,
    230.43904356577696,
    234.70172344281826,
    238.97838956183432,
    243.26884900298271,
    247.57291409618688,
    251.89040220972319,
    256.22113555000954,
    260.56494097186322,
    264.92164979855278,
    269.29109765101981,
    273.67312428569369,
    278.06757344036612,
    282.4742926876304,
    286.893133295427,
    291.32395009427029,
    295.76660135076065,
    300.22094864701415,
    304.68685676566872,
    309.1641935801469,
    313.65282994987905,
    318.1526396202093,
    322.66349912672615,
    327.1852877037752,
    331.71788719692847,
    336.26118197919845,
    340.815058870799,
    345.37940706226686,
    349.95411804077025,
    354.53908551944079,
    359.1342053695754,
    363.73937555556347
};

        #endregion

    }
}
