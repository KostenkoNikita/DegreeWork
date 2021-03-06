﻿//#define HELP_FOR_GROUP_LEADER
using System;
using System.Collections.Generic;
using Degree_Work.Mathematical_Sources.Complex;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    abstract class StreamLinesBuilderCircle : StreamLinesBuilder
    {
        protected List<DataPoint> LeftSpecialStreamLine;
        protected List<DataPoint> RightSpecialStreamLine;
        protected Complex LeftStagnationPoint;
        protected Complex RightStagnationPoint;
        protected Complex LeftStagnationPointBase;
        protected Complex RightStagnationPointBase;
        protected List<DataPoint> LeftSpecialStreamLineBase;
        protected List<DataPoint> RightSpecialStreamLineBase;
        protected DataPoint LeftStagnationPointDP => LeftStagnationPoint;
        protected DataPoint RightStagnationPointDP => LeftStagnationPoint;
        protected DataPoint LeftStagnationPointBaseDP => LeftStagnationPointBase;
        protected DataPoint RightStagnationPointBaseDP => LeftStagnationPointBase;
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
