using System;
using System.Runtime.CompilerServices;

namespace Degree_Work.Mathematical_Sources.Functions
{
    using Complex = Complex.Complex;
    /// <summary>
    /// Элементарные математические функции
    /// </summary>
    public static class ElementaryFunctions
    {
        /// <summary>
        /// Модуль числа
        /// </summary>
        /// <param name="z">Комлексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(Complex z)
        {
            return z.Abs;
        }

        /// <summary>
        /// Возведение в степень
        /// </summary>
        /// <param name="a">Основание</param>
        /// <param name="b">Показатель</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        /// <summary>
        /// Возведение в степень
        /// </summary>
        /// <param name="a">Основание</param>
        /// <param name="b">Показатель</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Pow(double a, int b)
        {
            return Math.Pow(a, b);
        }

        /// <summary>
        /// Возведение в степень
        /// </summary>
        /// <param name="z">Основание</param>
        /// <param name="n">Показатель</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Pow(Complex z, double n)
        {
            return new Complex(Math.Pow(z.Abs, n) * Math.Cos(n * z.ArgRadians), Math.Pow(z.Abs, n) * Math.Sin(n * z.ArgRadians));
        }

        /// <summary>
        /// Возведение в степень
        /// </summary>
        /// <param name="z">Основание</param>
        /// <param name="n">Показатель</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Pow(Complex z, decimal n)
        {
            return new Complex(Math.Pow(z.Abs, (double)n) * Math.Cos((double)n * z.ArgRadians), Math.Pow(z.Abs, (double)n) * Math.Sin((double)n * z.ArgRadians));
        }

        /// <summary>
        /// Возведение в степень
        /// </summary>
        /// <param name="z">Основание</param>
        /// <param name="n">Показатель</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Pow(Complex z, int n)
        {
            return new Complex(Math.Pow(z.Abs, n) * Math.Cos(n * z.ArgRadians), Math.Pow(z.Abs, n) * Math.Sin(n * z.ArgRadians));
        }

        /// <summary>
        /// Возведение в степень
        /// </summary>
        /// <param name="z1">Основание</param>
        /// <param name="z2">Показатель</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Pow(Complex z1, Complex z2)
        {
            return Exp(z2 * Ln(z1));
        }

        /// <summary>
        /// Корень комплексного числа
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Sqrt(Complex z)
        {
            return Pow(z, 0.5);
        }

        /// <summary>
        /// Экспонента
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Exp(Complex z)
        {
            return new Complex(Math.Exp(z.Re) * Cos(z.Im), Exp(z.Re) * Sin(z.Im));
        }

        /// <summary>
        /// Экспонента
        /// </summary>
        /// <param name="z">Aргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Exp(double z)
        {
            return Math.Exp(z);
        }

        /// <summary>
        /// Натуральный логарифм
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Ln(Complex z)
        {
            return new Complex(Math.Log(z.Abs), z.ArgRadians);
        }

        /// <summary>
        /// Синус
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Sin(Complex z)
        {
            return (Exp(Complex.I * z) - Exp(-Complex.I * z)) / (2 * Complex.I);
        }

        /// <summary>
        /// Косинус
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Cos(Complex z)
        {
            return (Exp(Complex.I * z) + Exp(-Complex.I * z)) / 2.0;
        }

        /// <summary>
        /// Синус
        /// </summary>
        /// <param name="z">Aргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sin(double z)
        {
            return Math.Sin(z);
        }

        /// <summary>
        /// Косинус
        /// </summary>
        /// <param name="z">Aргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cos(double z)
        {
            return Math.Cos(z);
        }

        /// <summary>
        /// Секанс (косинус в степени -1)
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Sec(Complex z)
        {
            return 1.0 / Cos(z);
        }

        /// <summary>
        /// Косеканс (синус в степени -1)
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Csc(Complex z)
        {
            return 1.0 / Sin(z);
        }

        /// <summary>
        /// Тангенс
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Tg(Complex z)
        {
            return Sin(z) / Cos(z);
        }

        /// <summary>
        /// Котангенс
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Ctg(Complex z)
        {
            return Cos(z) / Sin(z);
        }

        /// <summary>
        /// Синус гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Sh(Complex z)
        {
            return (Exp(z) - Exp(-z)) / 2.0;
        }

        /// <summary>
        /// Косинус гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Ch(Complex z)
        {
            return (Exp(z) + Exp(-z)) / 2.0;
        }

        /// <summary>
        /// Тангенс гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Th(Complex z)
        {
            return Sh(z) / Ch(z);
        }

        /// <summary>
        /// Котангенс гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Cth(Complex z)
        {
            return Ch(z) / Sh(z);
        }

        /// <summary>
        /// Арксинус
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arcsin(Complex z)
        {
            return -Complex.I * Ln(Complex.I * z + Sqrt(1 - z * z));
        }

        /// <summary>
        /// Арккосинус
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arccos(Complex z)
        {
            return -Complex.I * Ln(z + Sqrt(z * z - 1));
        }

        /// <summary>
        /// Арктангенс
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arctg(Complex z)
        {
            return (-Complex.I / 2.0) * Ln((1 + Complex.I * z) / (1 - Complex.I * z));
        }

        /// <summary>
        /// Арккотангенс
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arcctg(Complex z)
        {
            return (Complex.I / 2.0) * Ln((Complex.I * z + 1) / (Complex.I * z - 1));
        }

        /// <summary>
        /// Арксинус гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arsh(Complex z)
        {
            return Ln(z + Sqrt(z * z + 1));
        }

        /// <summary>
        /// Арккосинус гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arch(Complex z)
        {
            return Ln(z + Sqrt(z * z - 1));
        }

        /// <summary>
        /// Арктангенс гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arth(Complex z)
        {
            return 0.5 * Ln((1 + z) / (1 - z));
        }

        /// <summary>
        /// Арккотангенс гиперболический
        /// </summary>
        /// <param name="z">Комплексный аргумент</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex Arcth(Complex z)
        {
            return 0.5 * Ln((z + 1) / (z - 1));
        }
    }
}
