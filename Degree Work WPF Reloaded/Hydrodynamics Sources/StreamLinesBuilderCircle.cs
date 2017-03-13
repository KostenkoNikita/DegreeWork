//#define HELP_FOR_GROUP_LEADER
using System;
using System.Collections.Generic;
using MathCore_2_0;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    abstract class StreamLinesBuilderCircle : StreamLinesBuilder
    {
        protected List<DataPoint> LeftSpecialStreamLine;
        protected List<DataPoint> RightSpecialStreamLine;
        protected complex LeftStagnationPoint;
        protected complex RightStagnationPoint;
        protected complex LeftStagnationPointBase;
        protected complex RightStagnationPointBase;
        protected List<DataPoint> LeftSpecialStreamLineBase;
        protected List<DataPoint> RightSpecialStreamLineBase;
        protected DataPoint LeftStagnationPointDP => LeftStagnationPoint.ComplexToDataPoint();
        protected DataPoint RightStagnationPointDP => LeftStagnationPoint.ComplexToDataPoint();
        protected DataPoint LeftStagnationPointBaseDP => LeftStagnationPointBase.ComplexToDataPoint();
        protected DataPoint RightStagnationPointBaseDP => LeftStagnationPointBase.ComplexToDataPoint();
        protected double f(double X, double Y) => w.V_eta(new complex(X, Y)) / w.V_ksi(new complex(X, Y));
        double[,] ReflectionMatrix()
        {
            return new double[2, 2] {
                {1,  0},
                {0, -1}};
        }
        double[,] RotationMatrix(double alpha)
        {
            return new double[2, 2] {
                {Math.Cos(alpha), -1 * Math.Sin(alpha)},
                {Math.Sin(alpha), Math.Cos(alpha)}};
        }
        double[,] Multiply(double[,] n, double[,] m)
        {
            return new double[2, 2]
            {
                { n[0,0]*m[0,0]+n[0,1]*m[1,0], n[0,0]*m[0,1]+n[0,1]*m[1,1]},
                { n[1,0]*m[0,0]+n[1,1]*m[1,0], n[1,0]*m[0,1]+n[1,1]*m[1,1]}
            };
        }
        DataPoint Multiply(DataPoint z, double[,] m)
        {
            return new DataPoint(
                z.X * m[0, 0] + z.Y * m[1, 0],
                z.X * m[0, 1] + z.Y * m[1, 1]);
        }
        protected DataPoint Reflect(DataPoint z, double alpha)
        {
            double[,] resultMatrix = Multiply(RotationMatrix(alpha), ReflectionMatrix());
            resultMatrix = Multiply(resultMatrix, RotationMatrix(-1 * alpha));
            return Multiply(z, resultMatrix);
        }
#if !HELP_FOR_GROUP_LEADER
        protected StreamLinesBuilderCircle(Potential w, PlotWindowModel g, CanonicalDomain domain) : base(w,g,domain)
        {
        }
#endif
    }
}
