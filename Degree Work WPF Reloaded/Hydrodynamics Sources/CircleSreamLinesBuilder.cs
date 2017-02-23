using System;
using System.Collections.Generic;
using MathCore_2_0;
using static MathCore_2_0.complex;
using static MathCore_2_0.math;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    [Obsolete]
    class CircleSreamLinesBuilder : StreamLinesBuilder
    {
        List<DataPoint> LeftSpecialStreamLine;
        List<DataPoint> RightSpecialStreamLine;
        complex LeftStagnationPoint;
        complex RightStagnationPoint;
        complex LeftStagnationPointBase;
        complex RightStagnationPointBase;
        List<DataPoint> LeftSpecialStreamLineBase;
        List<DataPoint> RightSpecialStreamLineBase;

        double YInitialCoordinate;
        bool IsVerticalMode
        {
            get { return w.AlphaRadians > 0.78539816339744830962 || w.AlphaRadians < -0.78539816339744830962 ? true : false; }
        }
        delegate void AsyncDel(double y_iters, int index);
        delegate void RotateAsync(List<DataPoint> list, List<DataPoint> Base, complex multiplicator);
        RotateAsync rotate_async_d;
        AsyncDel async_d;
        List<IAsyncResult> res_list;
        public CircleSreamLinesBuilder(Potential w, PlotWindowModel g)
        {
            this.g = g;
            this.w = w;
            h = Settings.PlotGeomParams.hVertical;
            h_mrk = Settings.PlotGeomParams.MRKh;
            x_min = Settings.PlotGeomParams.XMin;
            x_max = Settings.PlotGeomParams.XMax;
            y_min = Settings.PlotGeomParams.YMin;
            y_max = Settings.PlotGeomParams.YMax;
            domain = CanonicalDomain.Circular;
            function = this.w.f;
            LeftSpecialStreamLineBase = new List<DataPoint>();
            RightSpecialStreamLineBase = new List<DataPoint>();
            StreamLinesBase = new List<List<DataPoint>>();
            LeftSpecialStreamLine = new List<DataPoint>();
            RightSpecialStreamLine = new List<DataPoint>();
            InitialBuild();
        }
        void InitialBuild()
        {
            double tmp = w.AlphaRadians;
            w.AlphaRadians = 0;
            g.Clear();
            LeftStagnationPointBase = -w.R;
            RightStagnationPointBase = w.R;
            LeftStagnationPoint = function.z(RightStagnationPointBase);
            RightStagnationPoint = function.z(LeftStagnationPointBase);
            g.DrawStagnationPoints(RightStagnationPoint, LeftStagnationPoint);
            g.DrawBorder(this);
            FindSpecialStreamLines();
            FindStreamLinesHorizontalReflection();
            w.AlphaRadians = tmp;
        }
        public void Rebuild()
        {
            g.Clear();
            g.DrawBorder(this);
            Rotate();
        }
        void Rotate()
        {
            complex tmp = exp(i * w.AlphaRadians);
            LeftStagnationPoint = function.z(LeftStagnationPointBase * tmp);
            RightStagnationPoint = function.z(RightStagnationPointBase * tmp);
            g.DrawStagnationPoints(RightStagnationPoint, LeftStagnationPoint);
            rotate_async_d = new RotateAsync(RotateMultiplicationAsync);
            res_list = new List<IAsyncResult>();
            LeftSpecialStreamLine.Clear();
            RightSpecialStreamLine.Clear();
            StreamLines.Clear();
            for (int i = 0; i < StreamLinesBase.Count - 1; i++)
            {
                StreamLines.Add(new List<DataPoint>());
                res_list.Add(rotate_async_d.BeginInvoke(StreamLines[i], StreamLinesBase[i], tmp, null, null));
            }
            res_list.Add(rotate_async_d.BeginInvoke(LeftSpecialStreamLine, LeftSpecialStreamLineBase, tmp, null, null));
            res_list.Add(rotate_async_d.BeginInvoke(RightSpecialStreamLine, RightSpecialStreamLineBase, tmp, null, null));
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }
        void FindSpecialStreamLines()
        {
            RungeMethodSpecial();
            g.DrawCurve(LeftSpecialStreamLine);
            g.DrawCurve(RightSpecialStreamLine);
        }
        void RungeMethodSpecial()
        {
            complex tmp = LeftStagnationPointBase + (new complex((h_mrk / 100) * Math.Cos(LeftStagnationPointBase.arg_radians), (h_mrk / 100) * Math.Sin(LeftStagnationPointBase.arg_radians)));
            YInitialCoordinate = _mrk_proc(tmp.Re, tmp.Im, -h_mrk, LeftSpecialStreamLine, LeftSpecialStreamLineBase);
            tmp = RightStagnationPointBase + (new complex((h_mrk / 100) * Math.Cos(RightStagnationPointBase.arg_radians), (h_mrk / 100) * Math.Sin(RightStagnationPointBase.arg_radians)));
            _mrk_proc(tmp.Re, tmp.Im, h_mrk, RightSpecialStreamLine, RightSpecialStreamLineBase);
        }
        void FindStreamLinesHorizontalReflection()
        {
            double y_iters;
            y_min = YInitialCoordinate + 2 * h;
            async_d = new AsyncDel(ThreadMethodHorizontalReflection);
            res_list = new List<IAsyncResult>();
            for (y_iters = y_min; y_iters <= y_max; y_iters += h)
            {
                StreamLines.Add(new List<DataPoint>());
                StreamLines.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_d.BeginInvoke(
                    y_iters, StreamLines.Count - 2, null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
            y_min = Settings.PlotGeomParams.YMin;
        }
        void ThreadMethodHorizontalReflection(double y_iters, int index)
        {
            double x, y, x_new, y_new, k1, k2, k3, k4;
            x_new = x_min;
            y_new = y_iters;
            StreamLinesBase[index].Add(new DataPoint(x_min, y_iters));
            StreamLinesBase[index + 1].Add(Reflect(StreamLinesBase[index][StreamLinesBase[index].Count - 1].DataPointToComplex(), w.AlphaRadians).ComplexToDataPoint());
            StreamLines[index].Add(function[StreamLinesBase[index][StreamLinesBase[index].Count - 1]]);
            StreamLines[index + 1].Add(function[StreamLinesBase[index + 1][StreamLinesBase[index].Count - 1]]);
            while (x_new < x_max)
            {
                x = x_new;
                y = y_new;
                k1 = f(x, y);
                k2 = f(x + h_mrk / 2, y + (h_mrk * k1) / 2);
                k3 = f(x + h_mrk / 2, y + (h_mrk * k2) / 2);
                k4 = f(x + h_mrk, y + (h_mrk * k3));
                y_new = y + (h_mrk / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                x_new = x + h_mrk;
                StreamLinesBase[index].Add(new DataPoint(x_new, y_new));
                StreamLinesBase[index + 1].Add(Reflect(StreamLinesBase[index][StreamLinesBase[index].Count - 1].DataPointToComplex(), w.AlphaRadians).ComplexToDataPoint());
                StreamLines[index].Add(function[StreamLinesBase[index][StreamLinesBase[index].Count - 1]]);
                StreamLines[index + 1].Add(function[StreamLinesBase[index + 1][StreamLinesBase[index].Count - 1]]);
            }
            DrawStreamLine(index);
            DrawStreamLine(index + 1);
        }
        void RotateMultiplicationAsync(List<DataPoint> list, List<DataPoint> Base, complex multiplicator)
        {
            foreach (DataPoint c in Base)
            {
                list.Add(function.z(c.DataPointToComplex() * multiplicator).ComplexToDataPoint());
            }
            g.DrawCurve(list);
        }
        void DrawStreamLines()
        {
            foreach (List<DataPoint> l in StreamLines) { g.DrawCurve(l); }
        }
        void DrawStreamLine(int index) => g.DrawCurve(StreamLines[index]);
        double f(double X, double Y) => w.V_eta(new complex(X, Y)) / w.V_ksi(new complex(X, Y));
        double _mrk_proc(double x_init, double y_init, double _mrk_h, List<DataPoint> l, List<DataPoint> Base)
        {
            double x, y, x_new, y_new, k1, k2, k3, k4;
            x_new = x_init;
            y_new = y_init;
            Base.Add(new DataPoint(x_new, y_new));
            l.Add(function.z(Base[Base.Count - 1]));
            while (Math.Abs(x_new) < x_max)
            {
                x = x_new;
                y = y_new;
                k1 = f(x, y);
                k2 = f(x + _mrk_h / 2, y + (_mrk_h * k1) / 2);
                k3 = f(x + _mrk_h / 2, y + (_mrk_h * k2) / 2);
                k4 = f(x + _mrk_h, y + (_mrk_h * k3));
                y_new = y + (_mrk_h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                x_new = x + _mrk_h;
                Base.Add(new DataPoint(x_new, y_new));
                l.Add(function.z(Base[Base.Count - 1]));
            }
            return y_new;
        }
        static double[,] ReflectionMatrix()
        {
            return new double[2, 2] {
                {1,  0},
                {0, -1}};
        }
        static double[,] RotationMatrix(double alpha)
        {
            return new double[2, 2] {
                {Math.Cos(alpha), -1 * Math.Sin(alpha)},
                {Math.Sin(alpha), Math.Cos(alpha)}};
        }
        static double[,] Multiply(double[,] n, double[,] m)
        {
            return new double[2, 2]
            {
                { n[0,0]*m[0,0]+n[0,1]*m[1,0], n[0,0]*m[0,1]+n[0,1]*m[1,1]},
                { n[1,0]*m[0,0]+n[1,1]*m[1,0], n[1,0]*m[0,1]+n[1,1]*m[1,1]}
            };
        }
        static complex Multiply(complex z, double[,] m)
        {
            return new complex(
                z.Re * m[0, 0] + z.Im * m[1, 0],
                z.Re * m[0, 1] + z.Im * m[1, 1]);
        }
        static complex Reflect(complex z, double alpha)
        {
            double[,] resultMatrix = Multiply(RotationMatrix(alpha), ReflectionMatrix());
            resultMatrix = Multiply(resultMatrix, RotationMatrix(-1 * alpha));
            return Multiply(z, resultMatrix);
        }
    }
}
