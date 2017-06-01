#pragma warning disable

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Text;

namespace Degree_Work.Mathematical_Sources.Complex
{
    /// <summary>
    /// Структура, що являє собою комплексне число
    /// </summary>
    [Serializable]
    public struct Complex : IEquatable<Complex>, IFormattable
    {
        /// <summary>
        /// Дійсна частина комплексного числа
        /// </summary>
        public readonly double Re;

        /// <summary>
        /// Уявна частина комплексного числа
        /// </summary>
        public readonly double Im;

        /// <summary>
        /// Модуль комплексного числа
        /// </summary>
        public double Abs => Math.Sqrt(Re * Re + Im * Im);

        /// <summary>
        /// Аргумент комплексного числа (радіани)
        /// </summary>
        public double ArgRadians => getArg();

        /// <summary>
        /// Аргумент комплексного числа (градуси)
        /// </summary>
        public double ArgDegrees => getArg() * 180.0 / Math.PI;

        /// <summary>
        /// Спряжене комплексне число
        /// </summary>
        public Complex Conjugate => new Complex(Re, -Im);

        /// <summary>
        /// Операція спряження
        /// </summary>
        /// <param name="z">Комплексное число, сопряженное к которому возвращает метод</param>
        /// <returns></returns>
        public static Complex ComplexConjugate(Complex z) => z.Conjugate;

        /// <summary>
        /// Мнима одиниця
        /// </summary>
        public static Complex I => new Complex(0, 1);

        /// <summary>
        /// Нуль
        /// </summary>
        public static Complex Zero => new Complex();

        /// <summary>
        /// Нескінченно віддалена
        /// </summary>
        public static Complex Infinity => new Complex(double.PositiveInfinity, double.PositiveInfinity);

        /// <summary>
        /// НЕ-число
        /// </summary>
        public static Complex NaN => new Complex(double.NaN, double.NaN);

        /// <summary>
        /// Комплексне число, дійсна та уявна частини яких є дуже малими величинами
        /// </summary>
        public static Complex Epsilon => new Complex(double.Epsilon, double.Epsilon);

        /// <summary>
        /// Аргумент комплексного числа
        /// </summary>
        /// <param name="m">Спосіб вимірювання кута (градуси чи радіани)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetArgument(AngleMeasurement m)
        {
            switch (m)
            {
                case AngleMeasurement.Degrees:
                    return getArg() * 180.0 / Math.PI;
                case AngleMeasurement.Radians:
                    return getArg();
                default:
                    throw new ArgumentOutOfRangeException("Invalid angle measurement");
            }
        }

        /// <summary>
        /// Является ли комплексное число не-числом (иными словами, определено ли оно)
        /// </summary>
        /// <param name="c">Комплексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(Complex c)
        {
            return double.IsNaN(c.Re) || double.IsNaN(c.Im);
        }

        /// <summary>
        /// Являестя ли комплексное число бесконечно отдаленной точкой
        /// </summary>
        /// <param name="c">Комплексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Complex c)
        {
            return double.IsInfinity(c.Re) || double.IsInfinity(c.Im);
        }

        /// <summary>
        /// Являестя ли комплексное число нулём
        /// </summary>
        /// <param name="c">Комплексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Complex c)
        {
            return c.Re == 0 && c.Im == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double getArg()
        {
            if ((Re > 0 && Im > 0) || (Re > 0 && Im < 0))
            {
                return Math.Atan(Im / Re);
            }
            else if ((Re < 0 && Im > 0) || (Re < 0 && Im < 0))
            {
                return Math.PI + Math.Atan(Im / Re);
            }
            else if (Im == 0 && Re > 0)
            {
                return 0;
            }
            else if (Im == 0 && Re < 0)
            {
                return Math.PI;
            }
            else if (Re == 0 && Im < 0)
            {
                return 3 * Math.PI / 2;
            }
            else if (Re == 0 && Im > 0)
            {
                return Math.PI / 2;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Конструктор комплексного числа
        /// </summary>
        /// <param name="Re">Дійсна частина</param>
        /// <param name="Im">Уявна частина</param>
        public Complex(double Re, double Im)
        {
            this.Re = Re;
            this.Im = Im;
        }

        /// <summary>
        /// Конструктор экземпляра типа
        /// </summary>
        /// <param name="d">Действительная часть</param>
        public Complex(double d)
        {
            Re = d;
            Im = 0;
        }

        /// <summary>
        /// Перевірка на рівність
        /// </summary>
        /// <param name="obj">Объект произвольного типа для сравнения</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Complex ? Equals((Complex)obj) : false;
        }

        /// <summary>
        /// Перевірка на рівність
        /// </summary>
        /// <param name="other">Другое комплексное число</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Complex other)
        {
            return Re == other.Re && Im == other.Im;
        }

        /// <summary>
        /// Отримання хеш-коду комплексного числа
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Re.GetHashCode() ^ Im.GetHashCode();
        }

        /// <summary>
        /// Строкове представлення комплексного числа
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return ToString("G", ComplexNumberFormatInfo.AlgebraicFormBigImagiaryOne);
        }

        /// <summary>
        /// Строкове представлення комплексного числа
        /// </summary>
        /// <param name="formatProvider">Поставщик формата</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString("G", formatProvider);
        }

        /// <summary>
        /// Строкове представлення комплексного числа
        /// </summary>
        /// <param name="format">Строка форматирования</param>
        /// <param name="formatProvider">Поставщик формата</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (double.IsInfinity(Re) || double.IsInfinity(Im))
            {
                return "ComplexInfinity";
            }
            else if (double.IsNaN(Re) || double.IsNaN(Im))
            {
                return "NaN";
            }
            else if (Re == 0 && Im == 0)
            {
                return "0";
            }
            if (formatProvider is ComplexNumberFormatInfo)
            {
                ComplexNumberFormatInfo tmp = formatProvider as ComplexNumberFormatInfo;
                if (tmp.Equals(ComplexNumberFormatEnum.AlgebraicFormBigImagiaryOne))
                {
                    string reStr = string.Empty;
                    string opStr = string.Empty;
                    string imStr = string.Empty;
                    if (Im > 0)
                    {
                        imStr = string.Concat(Im.ToString(format, CultureInfo.InvariantCulture), "I");
                        opStr = "+";
                    }
                    else if (Im < 0)
                    {
                        imStr = string.Concat(Im.ToString(format, CultureInfo.InvariantCulture), "I");
                    }

                    if (Re != 0)
                    {
                        reStr = Re.ToString(format, CultureInfo.InvariantCulture);
                    }
                    return string.Concat(reStr, opStr, imStr);
                }
                else if (tmp.Equals(ComplexNumberFormatEnum.AlgebraicFormLittleImagiaryOne))
                {
                    string reStr = string.Empty;
                    string opStr = string.Empty;
                    string imStr = string.Empty;
                    if (Im > 0)
                    {
                        imStr = string.Concat(Im.ToString(format, CultureInfo.InvariantCulture), "i");
                        opStr = "+";
                    }
                    else if (Im < 0)
                    {
                        imStr = string.Concat(Im.ToString(format, CultureInfo.InvariantCulture), "i");
                    }

                    if (Re != 0)
                    {
                        reStr = Re.ToString(format, CultureInfo.InvariantCulture);
                    }
                    return string.Concat(reStr, opStr, imStr);
                }
                else if (tmp.Equals(ComplexNumberFormatEnum.ExponentialFormBigImagiaryOne))
                {
                    string AbsStr = string.Empty;
                    string ExpStr = string.Empty;
                    string powStr = string.Empty;
                    if (Im != 0)
                    {
                        powStr = string.Concat(ArgRadians.ToString(format, CultureInfo.InvariantCulture), "I]");
                        ExpStr = "Exp[";
                    }
                    if (Re != 0)
                    {
                        AbsStr = Abs.ToString(format, CultureInfo.InvariantCulture);
                    }
                    return string.Concat(AbsStr, ExpStr, powStr);
                }
                else if (tmp.Equals(ComplexNumberFormatEnum.ExponentialFormLittleImagiaryOne))
                {
                    string AbsStr = string.Empty;
                    string ExpStr = string.Empty;
                    string powStr = string.Empty;
                    if (Im != 0)
                    {
                        powStr = string.Concat(ArgRadians.ToString(format, formatProvider), "i]");
                        ExpStr = "Exp[";
                    }
                    if (Re != 0)
                    {
                        AbsStr = Abs.ToString(format, CultureInfo.InvariantCulture);
                    }
                    return string.Concat(AbsStr, ExpStr, powStr);
                }
                else if (tmp.Equals(ComplexNumberFormatEnum.Parentheses))
                {
                    return $"({Re.ToString(format, CultureInfo.InvariantCulture)},{Im.ToString(format, CultureInfo.InvariantCulture)})";
                }
                else
                {
                    return $"[{Re.ToString(format, CultureInfo.InvariantCulture)},{Im.ToString(format, CultureInfo.InvariantCulture)}]";
                }
            }
            else
            {
                string reStr = string.Empty;
                string opStr = string.Empty;
                string imStr = string.Empty;
                if (Im > 0)
                {
                    imStr = string.Concat(Im.ToString(format, CultureInfo.InvariantCulture), "I");
                    opStr = "+";
                }
                else if (Im < 0)
                {
                    imStr = string.Concat(Im.ToString(format, CultureInfo.InvariantCulture), "I");
                }

                if (Re != 0)
                {
                    reStr = Re.ToString(format, CultureInfo.InvariantCulture);
                }
                return string.Concat(reStr, opStr, imStr);
            }
        }

        /// <summary>
        /// Отримання комплексного числа з строки
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Complex Parse(string s)
        {
            return Parse(s, ComplexNumberFormatInfo.AlgebraicFormBigImagiaryOne);
        }

        /// <summary>
        /// Отримання комплексного числа з строки
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static Complex Parse(string s, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException();
            }
            s = s.Replace(" ", string.Empty).Replace("*", string.Empty);
            if (formatProvider is ComplexNumberFormatInfo)
            {
                var tmp = formatProvider as ComplexNumberFormatInfo;
                switch (tmp.ComplexNumberFormat)
                {
                    case ComplexNumberFormatEnum.AlgebraicFormBigImagiaryOne:
                        {
                            StringBuilder firstPart = new StringBuilder();
                            StringBuilder secondPart = new StringBuilder();
                            int i = s.Length - 1;
                            while (!(s[i] == '+' || s[i] == '-'))
                            {
                                secondPart.Append(s[i--]);
                            }
                            secondPart.Append(s[i]);
                            firstPart.Append(s.Substring(0, i));
                            string firstPartStr = firstPart.ToString();
                            string secondPartStr = secondPart.ToString().Reverse();
                            double re = 0, im = 0;
                            if (firstPartStr.Contains('I'))
                            {
                                re = double.Parse(secondPartStr, CultureInfo.InvariantCulture);
                                im = double.Parse(firstPartStr.Replace("I", string.Empty), CultureInfo.InvariantCulture);
                            }
                            else if (secondPartStr.Contains('I'))
                            {
                                im = double.Parse(secondPartStr.Replace("I", string.Empty), CultureInfo.InvariantCulture);
                                re = double.Parse(firstPartStr, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                throw new FormatException("Didn't find imaginary one accordind to current format provider");
                            }
                            return new Complex(re, im);
                        }
                    case ComplexNumberFormatEnum.AlgebraicFormLittleImagiaryOne:
                        {
                            StringBuilder firstPart = new StringBuilder();
                            StringBuilder secondPart = new StringBuilder();
                            int i = s.Length - 1;
                            while (!(s[i] == '+' || s[i] == '-'))
                            {
                                secondPart.Append(s[i--]);
                            }
                            secondPart.Append(s[i]);
                            firstPart.Append(s.Substring(0, i));
                            string firstPartStr = firstPart.ToString();
                            string secondPartStr = secondPart.ToString().Reverse();
                            double re = 0, im = 0;
                            if (firstPartStr.Contains('i'))
                            {
                                re = double.Parse(secondPartStr, CultureInfo.InvariantCulture);
                                im = double.Parse(firstPartStr.Replace("i", string.Empty), CultureInfo.InvariantCulture);
                            }
                            else if (secondPartStr.Contains('i'))
                            {
                                im = double.Parse(secondPartStr.Replace("i", string.Empty), CultureInfo.InvariantCulture);
                                re = double.Parse(firstPartStr, CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                throw new FormatException("Didn't find imaginary one accordind to current format provider");
                            }
                            return new Complex(re, im);
                        }
                    case ComplexNumberFormatEnum.ExponentialFormBigImagiaryOne:
                        {
                            string[] strArray = s.Split('[', '(');
                            string rStr = strArray[0].Replace("Exp", string.Empty).Replace("Exp", string.Empty);
                            string phiStr = strArray[1].Replace("I", string.Empty).Replace(")", string.Empty).Replace("]", string.Empty);
                            return
                                ComplexNumberFromMagnitudeAndPhase
                                (
                                    double.Parse(rStr, CultureInfo.InvariantCulture),
                                    double.Parse(phiStr, CultureInfo.InvariantCulture)
                                );
                        }
                    case ComplexNumberFormatEnum.ExponentialFormLittleImagiaryOne:
                        {
                            string[] strArray = s.Split('[', '(');
                            string rStr = strArray[0].Replace("Exp", string.Empty).Replace("Exp", string.Empty);
                            string phiStr = strArray[1].Replace("i", string.Empty).Replace(")", string.Empty).Replace("]", string.Empty);
                            return
                                ComplexNumberFromMagnitudeAndPhase
                                (
                                    double.Parse(rStr, CultureInfo.InvariantCulture),
                                    double.Parse(phiStr, CultureInfo.InvariantCulture)
                                );
                        }
                    case ComplexNumberFormatEnum.Parentheses:
                        {
                            string[] strArray = s.Split(',');
                            string reStr = strArray[0].Replace("(", string.Empty);
                            string imStr = strArray[1].Replace(")", string.Empty);
                            return
                                new Complex
                                (
                                    double.Parse(reStr, CultureInfo.InvariantCulture),
                                    double.Parse(imStr, CultureInfo.InvariantCulture)
                                );
                        }
                    case ComplexNumberFormatEnum.SquareBrackets:
                        {
                            string[] strArray = s.Split(',');
                            string reStr = strArray[0].Replace("[", string.Empty);
                            string imStr = strArray[1].Replace("]", string.Empty);
                            return
                                new Complex
                                (
                                    double.Parse(reStr, CultureInfo.InvariantCulture),
                                    double.Parse(imStr, CultureInfo.InvariantCulture)
                                );
                        }
                    default:
                        throw new FormatException();
                }
            }
            else
            {
                return Parse(s, ComplexNumberFormatInfo.AlgebraicFormBigImagiaryOne);
            }
        }

        /// <summary>
        /// Отримання комплексного числа з строки
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Complex c)
        {
            c = new Complex();
            try
            {
                c = Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Отримання комплексного числа з строки
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Complex c, IFormatProvider formatProvider)
        {
            c = new Complex();
            try
            {
                c = Parse(s, formatProvider);
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        /// <summary>
        /// Комплексне число, отримане по значенню модуля та аргумента у радіанах
        /// </summary>
        /// <param name="r"></param>
        /// <param name="phiRad"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex ComplexNumberFromMagnitudeAndPhaseInRadians(double r, double phiRad)
        {
            return new Complex(r * Math.Cos(phiRad), r * Math.Sin(phiRad));
        }

        /// <summary>
        /// Комплексне число, отримане по значенню модуля та аргумента у градусах
        /// </summary>
        /// <param name="r"></param>
        /// <param name="phiDeg"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex ComplexNumberFromMagnitudeAndPhaseInDegrees(double r, double phiDeg)
        {
            return ComplexNumberFromMagnitudeAndPhaseInRadians(r, phiDeg * Math.PI / 180.0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex ComplexNumberFromMagnitudeAndPhase(double r, double phi)
        {
            return ComplexNumberFromMagnitudeAndPhaseInRadians(r, phi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex ComplexNumberFromMagnitudeAndPhase(double r, double phi, AngleMeasurement m)
        {
            switch (m)
            {
                case AngleMeasurement.Degrees:
                    return ComplexNumberFromMagnitudeAndPhaseInDegrees(r, phi);
                case AngleMeasurement.Radians:
                    return ComplexNumberFromMagnitudeAndPhaseInRadians(r, phi);
                default:
                    throw new ArgumentOutOfRangeException("Invalid angle measurement");
            }
        }

        #region TypeConversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(sbyte sb)
        {
            return new Complex(sb);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(byte b)
        {
            return new Complex(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(short int16)
        {
            return new Complex(int16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(ushort uint16)
        {
            return new Complex(uint16);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(int int32)
        {
            return new Complex(int32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(uint uint32)
        {
            return new Complex(uint32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(long int64)
        {
            return new Complex(int64);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(ulong uint64)
        {
            return new Complex(uint64);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(float f)
        {
            return new Complex(f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(double d)
        {
            return new Complex(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(decimal d)
        {
            return new Complex((double)d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(string s)
        {
                return Parse(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(double[] dArray)
        {
            return new Complex(dArray[0], dArray[1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(Tuple<double, double> tuple)
        {
            return new Complex(tuple.Item1, tuple.Item2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator OxyPlot.DataPoint(Complex z)
        {
            return new OxyPlot.DataPoint(z.Re, z.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SByte(Complex c)
        {
            return (sbyte)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Byte(Complex c)
        {
            return (byte)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Int16(Complex c)
        {
            return (short)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UInt16(Complex c)
        {
            return (ushort)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Int32(Complex c)
        {
            return (int)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UInt32(Complex c)
        {
            return (uint)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Int64(Complex c)
        {
            return (long)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator UInt64(Complex c)
        {
            return (ulong)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(Complex c)
        {
            return (float)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(Complex c)
        {
            return c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator decimal(Complex c)
        {
            return (decimal)c.Re;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator string(Complex c)
        {
            return c.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double[] (Complex c)
        {
            return new double[2] { c.Re, c.Im };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Tuple<double, double>(Complex c)
        {
            return new Tuple<double, double>(c.Re, c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Complex(OxyPlot.DataPoint p)
        {
            return new Complex(p.X, p.Y);
        }

        #endregion

        #region MathOperators

        /// <summary>
        /// Сума двох комплексних чисел
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Re + c2.Re, c1.Im + c2.Im);
        }

        /// <summary>
        /// Сума комплексного та дійсного числа
        /// </summary>
        /// <param name="d"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(double d, Complex c)
        {
            return new Complex(d + c.Re, c.Im);
        }

        /// <summary>
        /// Сума комплексного та дійсного числа
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex c, double d)
        {
            return new Complex(c.Re + d, c.Im);
        }

        /// <summary>
        /// Унарний плюс
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex c)
        {
            return new Complex(+c.Re, +c.Im);
        }

        /// <summary>
        /// Різниця двох комплексних чисел
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Re - c2.Re, c1.Im - c2.Im);
        }

        /// <summary>
        /// Різниця дійсного та комплексного числа
        /// </summary>
        /// <param name="d"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(double d, Complex c)
        {
            return new Complex(d - c.Re, -c.Im);
        }

        /// <summary>
        /// Різниця комплексного та дійсного числа
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex c, double d)
        {
            return new Complex(c.Re - d, c.Im);
        }

        /// <summary>
        /// Унарний мінус
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex c)
        {
            return new Complex(-c.Re, -c.Im);
        }

        /// <summary>
        /// Добуток двох комплексних чисел
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Re * c2.Im + c1.Im * c2.Re);
        }

        /// <summary>
        /// Добуток війсного та комплексного числа
        /// </summary>
        /// <param name="d"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(double d, Complex c)
        {
            return new Complex(c.Re * d, c.Im * d);
        }

        /// <summary>
        /// Добуток комплексного та дійсного числа
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(Complex c, double d)
        {
            return new Complex(c.Re * d, c.Im * d);
        }

        /// <summary>
        /// Ділення одного комплексного числа на інше
        /// </summary>
        /// <param name="z1"></param>
        /// <param name="z2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(Complex z1, Complex z2)
        {
            if ((IsZero(z1) && IsZero(z2)) || (IsInfinity(z1) && IsInfinity(z2)) || IsNaN(z1) || IsNaN(z2))
            {
                return NaN;
            }
            else if (IsZero(z1) || IsInfinity(z2))
            {
                return Zero;
            }
            else if (IsInfinity(z1) || IsZero(z2))
            {
                return Infinity;
            }
            else
            {
                return new Complex((z1.Re * z2.Re + z1.Im * z2.Im) / (z2.Re * z2.Re + z2.Im * z2.Im), (z1.Im * z2.Re - z1.Re * z2.Im) / (z2.Re * z2.Re + z2.Im * z2.Im));
            }
        }

        /// <summary>
        /// Ділення дійсного числа на комплексне
        /// </summary>
        /// <param name="d"></param>
        /// <param name="z2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(double d, Complex z2)
        {
            if ((d == 0.0 && IsZero(z2)) || (double.IsInfinity(d) && IsInfinity(z2)) || double.IsNaN(d) || IsNaN(z2))
            {
                return NaN;
            }
            else if (d == 0.0 || IsInfinity(z2))
            {
                return Zero;
            }
            else if (double.IsInfinity(d) || IsZero(z2))
            {
                return Infinity;
            }
            else
            {
                return new Complex((d * z2.Re) / (z2.Re * z2.Re + z2.Im * z2.Im), (-d * z2.Im) / (z2.Re * z2.Re + z2.Im * z2.Im));
            }
        }

        /// <summary>
        /// Ділення комплексного числа на дійсне
        /// </summary>
        /// <param name="z1"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(Complex z1, double d)
        {
            if ((IsZero(z1) && d == 0.0) || (IsInfinity(z1) && double.IsInfinity(d)) || IsNaN(z1) || double.IsNaN(d))
            {
                return NaN;
            }
            else if (IsZero(z1) || double.IsInfinity(d))
            {
                return Zero;
            }
            else if (IsInfinity(z1) || d == 0.0)
            {
                return Infinity;
            }
            else
            {
                return new Complex((z1.Re * d) / (d * d), (z1.Im * d) / (d * d));
            }
        }

        /// <summary>
        /// Перевірка на рівність двох комплексних чисел
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Complex c1, Complex c2)
        {
            return c1.Equals(c2);
        }

        /// <summary>
        /// Перевірка на нерівність двох комплексних чисел
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Complex c1, Complex c2)
        {
            return !c1.Equals(c2);
        }

        /// <summary>
        /// Декремент (діє тільки на дійну частину)
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator --(Complex z)
        {
            return new Complex(z.Re - 1, z.Im);
        }

        /// <summary>
        /// Інкремент (діє тільки на дійсну частину)
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator ++(Complex z)
        {
            return new Complex(z.Re + 1, z.Im);
        }

        #endregion
    }
}
