#pragma warning disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace Degree_Work.Mathematical_Sources
{
    /// <summary>
    /// Перечисление, представляющее собой тип измерения угла -- радианы или градусы
    /// </summary>
    public enum AngleMeasurement { Radians, Degrees };

    /// <summary>
    /// Структура, представляющая собой комплексное число
    /// </summary>
    [Serializable]
    public struct Complex : IEquatable<Complex>, IFormattable
    {
        public readonly double Re;

        public readonly double Im;

        public double Abs => Math.Sqrt(Re * Re + Im * Im);

        public double ArgRadians => getArg();

        public double ArgDegrees => getArg() * 180.0 / Math.PI;

        public double Phi => getArg();

        public Complex Conjugate => new Complex(Re, -Im);

        public static Complex I => new Complex(0, 1);

        public static Complex Zero => new Complex();

        public static Complex Infinity => new Complex(double.PositiveInfinity, double.PositiveInfinity);

        public static Complex NaN => new Complex(double.NaN, double.NaN);

        public static Complex Epsilon => new Complex(double.Epsilon, double.Epsilon);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(Complex c)
        {
            return double.IsNaN(c.Re) || double.IsNaN(c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Complex c)
        {
            return double.IsInfinity(c.Re) || double.IsInfinity(c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Complex c)
        {
            return c.Re == 0 && c.Im == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double getArg()
        {
            if (Re > 0)
            {
                return Math.Atan(Im / Re);
            }
            else if (Re < 0 && Im >= 0)
            {
                double test = Math.Atan(Im / Re);
                return Math.Atan(Im / Re) + Math.PI;
            }
            else if (Re < 0 && Im < 0)
            {
                return Math.Atan(Im / Re) - Math.PI;
            }
            else if (Re == 0 && Im > 0)
            {
                return 0.5 * Math.PI;
            }
            else if (Re == 0 && Im < 0)
            {
                return -0.5 * Math.PI;
            }
            else
            {
                return double.NaN;
            }
        }

        public Complex(double Re, double Im)
        {
            this.Re = Re;
            this.Im = Im;
        }

        public Complex(double d)
        {
            Re = d;
            Im = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Complex ? Equals((Complex)obj) : false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Complex other)
        {
            return Re == other.Re && Im == other.Im;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Re.GetHashCode() ^ Im.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
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
            string reStr = string.Empty;
            string opStr = string.Empty;
            string imStr = string.Empty;
            if (Im > 0)
            {
                imStr = string.Concat(Im.ToString("G", CultureInfo.InvariantCulture), "I");
                opStr = "+";
            }
            else if (Im < 0)
            {
                imStr = string.Concat(Im.ToString("G", CultureInfo.InvariantCulture), "I");
            }
            if (Re != 0)
            {
                reStr = Re.ToString("G", CultureInfo.InvariantCulture);
            }
            return string.Concat(reStr, opStr, imStr);
        }

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
            string reStr = string.Empty;
            string opStr = string.Empty;
            string imStr = string.Empty;
            if (Im > 0)
            {
                imStr = string.Concat(Im.ToString(format, formatProvider), "I");
                opStr = "+";
            }
            else if (Im < 0)
            {
                imStr = string.Concat(Im.ToString(format, formatProvider), "I");
            }

            if (Re != 0)
            {
                reStr = Re.ToString(format, formatProvider);
            }
            return string.Concat(reStr, opStr, imStr);
        }

        /// <summary>
        /// To be added
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Complex Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            s = s.Replace(" ", string.Empty);
            if (s[0] == '+') { s.Remove(0, 1); }
            if (s.Contains('+'))
            {
            }
            else if (s.Contains('-'))
            {
            }
            return new Complex();
        }

        /// <summary>
        /// To be added
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out Complex c)
        {
            try
            {
                c = new Complex();
                return true;
            }
            catch (Exception)
            {
                c = new Complex();
                return false;
            }
        }

        /// <summary>
        /// To be added
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex ComplexNumberFromMagnitudeAndPhaseInRadians(double r, double phiRad)
        {
            return new Complex();
        }

        /// <summary>
        /// To be added
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex ComplexNumberFromMagnitudeAndPhaseInDegrees(double r, double phiDeg)
        {
            return ComplexNumberFromMagnitudeAndPhaseInRadians(r, phiDeg * Math.PI / 180.0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex ComplexNumberFromMagnitudeAndPhase(double r, double phi)
        {
            return ComplexNumberFromMagnitudeAndPhaseInRadians(r, phi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex ComplexNumberFromMagnitudeAndPhase(double r, double phi, AngleMeasurement m)
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
            try
            {
                return Parse(s);
            }
            catch
            {
                throw new FormatException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(double[] dArray)
        {
            if (dArray.Length != 2)
            {
                throw new ArgumentException();
            }
            return new Complex(dArray[0], dArray[1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Complex(Tuple<double, double> tuple)
        {
            return new Complex(tuple.Item1, tuple.Item2);
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

        #endregion

        #region MathOperators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Re + c2.Re, c1.Im + c2.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(double d, Complex c)
        {
            return new Complex(d + c.Re, c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex c, double d)
        {
            return new Complex(c.Re + d, c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex c)
        {
            return new Complex(+c.Re, +c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Re - c2.Re, c1.Im - c2.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(double d, Complex c)
        {
            return new Complex(d - c.Re, -c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex c, double d)
        {
            return new Complex(c.Re - d, c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex c)
        {
            return new Complex(-c.Re, -c.Im);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Re * c2.Im + c1.Im * c2.Re);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(double d, Complex c)
        {
            return new Complex(c.Re * d, c.Im * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(Complex c, double d)
        {
            return new Complex(c.Re * d, c.Im * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(Complex z1, Complex z2)
        {
            return new Complex((z1.Re * z2.Re + z1.Im * z2.Im) / (z2.Re * z2.Re + z2.Im * z2.Im), (z1.Im * z2.Re - z1.Re * z2.Im) / (z2.Re * z2.Re + z2.Im * z2.Im));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(double d, Complex z2)
        {
            return new Complex((d * z2.Re) / (z2.Re * z2.Re + z2.Im * z2.Im), (-d * z2.Im) / (z2.Re * z2.Re + z2.Im * z2.Im));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(Complex z1, double d)
        {
            return new Complex((z1.Re * d) / (d * d), (z1.Im * d) / (d * d));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Complex c1, Complex c2)
        {
            return c1.Equals(c2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Complex c1, Complex c2)
        {
            return !c1.Equals(c2);
        }

        #endregion
    }
}
