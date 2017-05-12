using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Degree_Work.Mathematical_Sources.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Functions.SpecialFunctions;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources.Conformal_Maps
{
    public class EjectedSegment : Hydrodynamics_Sources.IConformalMapFunction
    {
        public DataPoint this[DataPoint dzeta] { get { return z(dzeta); } }

        public Complex pos => new Complex(X, Y);

        public double X, Y;

        public EjectedSegment(Complex SegmentPosition)
        {
            X = SegmentPosition.Re;
            Y = SegmentPosition.Im;
        }

        public EjectedSegment(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj.GetType() == typeof(EjectedSegment) ? Equals((EjectedSegment)obj) : false;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        bool Equals(EjectedSegment other) => pos == other.pos;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => pos.GetHashCode() ^ (typeof(EjectedSegment)).GetHashCode();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override string ToString() => "EjectedSegment";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex z(Complex dzeta)
        {
            Complex tmp = pos.Re + Sqrt(pos.Re * pos.Re - 2 * pos.Re * dzeta - pos.Im * pos.Im + dzeta * dzeta);
            return tmp.Im < 0 ? pos.Re - Sqrt(pos.Re * pos.Re - 2 * pos.Re * dzeta - pos.Im * pos.Im + dzeta * dzeta) : tmp;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dz_ddzeta(Complex dzeta)
        {
            Complex tmp = ((dzeta - pos.Re) / (Sqrt(pos.Re * pos.Re - 2 * pos.Re * dzeta - pos.Im * pos.Im + dzeta * dzeta)));
            return tmp.Re < 0 ? -tmp : tmp;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Complex dzeta(Complex Z)
        {
            return Sqrt(Pow(Z - pos.Re, 2) + pos.Im * pos.Im) + pos.Re;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint z(DataPoint dzeta)
        {
            return z(dzeta.DataPointToComplex()).ComplexToDataPoint();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dz_ddzeta(DataPoint dzeta)
        {
            return dz_ddzeta(dzeta.DataPointToComplex()).ComplexToDataPoint();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DataPoint dzeta(DataPoint Z)
        {
            return dzeta(Z.DataPointToComplex()).ComplexToDataPoint();
        }
    }
}
