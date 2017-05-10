using System;
using System.Runtime.CompilerServices;

namespace Degree_Work.Mathematical_Sources.Complex
{
    class ComplexNumberFormatInfo : IFormatProvider, IEquatable<ComplexNumberFormatInfo>, IEquatable<ComplexNumberFormatEnum>
    {
        ComplexNumberFormatEnum currentComplexFormat;

        internal ComplexNumberFormatEnum ComplexNumberFormat => currentComplexFormat;

        public static ComplexNumberFormatInfo AlgebraicFormBigImagiaryOne
        {
            get
            {
                return new ComplexNumberFormatInfo(ComplexNumberFormatEnum.AlgebraicFormBigImagiaryOne);
            }
        }

        public static ComplexNumberFormatInfo AlgebraicFormLittleImagiaryOne
        {
            get
            {
                return new ComplexNumberFormatInfo(ComplexNumberFormatEnum.AlgebraicFormLittleImagiaryOne);
            }
        }

        public static ComplexNumberFormatInfo Parentheses
        {
            get
            {
                return new ComplexNumberFormatInfo(ComplexNumberFormatEnum.Parentheses);
            }
        }

        public static ComplexNumberFormatInfo SquareBrackets
        {
            get
            {
                return new ComplexNumberFormatInfo(ComplexNumberFormatEnum.SquareBrackets);
            }
        }

        public static ComplexNumberFormatInfo ExponentialFormBigImagiaryOne
        {
            get
            {
                return new ComplexNumberFormatInfo(ComplexNumberFormatEnum.ExponentialFormBigImagiaryOne);
            }
        }

        public static ComplexNumberFormatInfo ExponentialFormLittleImagiaryOne
        {
            get
            {
                return new ComplexNumberFormatInfo(ComplexNumberFormatEnum.ExponentialFormLittleImagiaryOne);
            }
        }

        ComplexNumberFormatInfo(ComplexNumberFormatEnum format)
        {
            if (!Enum.IsDefined(typeof(ComplexNumberFormatEnum), format))
            {
                throw new ArgumentOutOfRangeException();
            }
            currentComplexFormat = format;
        }

        object IFormatProvider.GetFormat(Type formatType)
        {
            if (formatType == typeof(ComplexNumberFormatInfo)) { return this; }
            else return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is ComplexNumberFormatInfo)
            {
                return Equals((ComplexNumberFormatInfo)obj);
            }
            else if (obj is ComplexNumberFormatEnum)
            {
                return Equals((ComplexNumberFormatEnum)obj);
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ComplexNumberFormatInfo other)
        {
            return currentComplexFormat == (other as ComplexNumberFormatInfo).currentComplexFormat;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ComplexNumberFormatEnum other)
        {
            return currentComplexFormat == other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return currentComplexFormat.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return currentComplexFormat.ToString();
        }
    }
}
