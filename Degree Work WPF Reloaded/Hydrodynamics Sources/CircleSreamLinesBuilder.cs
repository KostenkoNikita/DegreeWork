//#define HELP_FOR_GROUP_LEADER

using System;
using System.Collections.Generic;
using MathCore_2_0;
using static MathCore_2_0.complex;
using static MathCore_2_0.math;
using OxyPlot;
using System.Threading.Tasks;

namespace Degree_Work.Hydrodynamics_Sources
{
    #if !HELP_FOR_GROUP_LEADER
    class CircleStreamLinesBuilder : StreamLinesBuilderCircle
    {
        delegate void IniFillAsync(List<DataPoint> BasePlus, List<DataPoint> ListMinus, double y);
        delegate void TransformationAsync(List<DataPoint> Base, List<DataPoint> Lines, complex angleMult);
        delegate void FullBuildAsync(List<DataPoint> BasePlus, List<DataPoint> BaseMinus, List<DataPoint> LinesPlus, List<DataPoint> LinesMinus, complex angleMult, double y);
        delegate void JoukowskiAsync(List<DataPoint> StreamLines, double y);
        IniFillAsync async_base;
        TransformationAsync async_transform;
        FullBuildAsync async_full;
        JoukowskiAsync async_Joukowski;
        bool wasJoukowski = false;
        public CircleStreamLinesBuilder(Potential w, PlotWindowModel g) : base(w,g,CanonicalDomain.Circular)
        {
            InitialBuild();
        }

        public override void ChangeParams(double? x_min, double? x_max, double? y_max, double? h_horizontal, double? h_vertical)
        {
            this.x_min = x_min ?? this.x_min;
            this.x_max = x_max ?? this.x_max;
            this.y_max = y_max ?? this.y_max;
            this.h_mrk = h_horizontal ?? this.h_mrk;
            this.h = h_vertical ?? this.h;
            FullRebuild();
        }

        void InitialBuild()
        {
            g.Clear();
            FindBaseStreamLines();
        }

        public override void Rebuild()
        {
            if (w.f is Conformal_Maps.JoukowskiAirfoil)
            {
                var tmp = w.f as Conformal_Maps.JoukowskiAirfoil;
                w.R = tmp.R;
                w.G = -4 * Math.PI * w.V_inf * (tmp.eps + Math.Sqrt(Math.Pow(tmp.h, 2) + Math.Pow(tmp.c, 2))) * Math.Sin(w.AlphaRadians + tmp.betaDiv2);
                FullRebuildJoukowski();
                wasJoukowski = true;
                return;
            }
            else
            {
                if (wasJoukowski)
                {
                    wasJoukowski = false;
                    w.R = 1;
                    w.G = 0;
                    FullRebuild();
                    return;
                }
            }
            g.Clear();
            g.DrawBorder(this);
            Transform();
        }

        void FullRebuild()
        {
            g.Clear();
            g.DrawBorder(this);
            FindAllStreamLines();
        }

        void FullRebuildJoukowski()
        {
            g.Clear();
            g.DrawBorder(this);
            LeftSpecialStreamLineBase = new List<DataPoint>();
            RightSpecialStreamLineBase = new List<DataPoint>();
            RightSpecialStreamLine = new List<DataPoint>();
            LeftSpecialStreamLine = new List<DataPoint>();
            StreamLines = new List<List<DataPoint>>();
            StreamLinesBase = new List<List<DataPoint>>();
            LeftStagnationPointBase = (i * exp(i * w.AlphaRadians) * w.G - exp(i * w.AlphaRadians) * sqrt(-w.G * w.G + 16 * pi * pi * w.R * w.R * w.V_inf * w.V_inf)) / (4 * pi * w.V_inf);
            RightStagnationPointBase = (i * exp(i * w.AlphaRadians) * w.G + exp(i * w.AlphaRadians) * sqrt(-w.G * w.G + 16 * pi * pi * w.R * w.R * w.V_inf * w.V_inf)) / (4 * pi * w.V_inf);
            LeftStagnationPoint = w.f.z(LeftStagnationPointBase);
            RightStagnationPoint = w.f.z(RightStagnationPointBase);
            g.DrawStagnationPoints(RightStagnationPoint, LeftStagnationPoint);
            double x_new, y_new;
            h_mrk /= 10.0;
            x_new = RightStagnationPointBase.Re + (h_mrk); 
            y_new = RightStagnationPointBase.Im + (h_mrk);
            RightSpecialStreamLineBase.Add(new DataPoint(x_new, y_new));
            RightSpecialStreamLine.Add(w.f.z(RightSpecialStreamLineBase[RightSpecialStreamLineBase.Count - 1]));
            while (x_new < x_max)
            {
                MRK_loop(ref x_new, ref y_new);
                RightSpecialStreamLineBase.Add(new DataPoint(x_new, y_new));
                RightSpecialStreamLine.Add(w.f.z(RightSpecialStreamLineBase[RightSpecialStreamLineBase.Count - 1]));
            }
            h_mrk = -h_mrk;
            x_new = LeftStagnationPointBase.Re + (h_mrk);
            y_new = LeftStagnationPointBase.Im;
            LeftSpecialStreamLineBase.Add(new DataPoint(x_new, y_new));
            LeftSpecialStreamLine.Add(w.f.z(LeftSpecialStreamLineBase[LeftSpecialStreamLineBase.Count - 1]));
            while (x_new > x_min)
            {
                MRK_loop(ref x_new, ref y_new);
                LeftSpecialStreamLineBase.Add(new DataPoint(x_new, y_new));
                LeftSpecialStreamLine.Add(w.f.z(LeftSpecialStreamLineBase[LeftSpecialStreamLineBase.Count - 1]));
            }
            h_mrk = -h_mrk*10.0;
            async_Joukowski = new JoukowskiAsync(AsyncJoukowskiBuild);
            res_list = new List<IAsyncResult>();
            for (double y1 = LeftSpecialStreamLine[LeftSpecialStreamLine.Count-1].Y+h * Math.Cos(w.AlphaRadians), y2 = LeftSpecialStreamLine[LeftSpecialStreamLine.Count - 1].Y - h * Math.Cos(w.AlphaRadians); y1 <= y_max && y2 >= -y_max; y1 += h, y2-=h)
            {
                StreamLines.Add(new List<DataPoint>());
                res_list.Add(async_Joukowski.BeginInvoke(StreamLines[StreamLines.Count - 1], y1, null, null));
                StreamLines.Add(new List<DataPoint>());
                res_list.Add(async_Joukowski.BeginInvoke(StreamLines[StreamLines.Count - 1], y2, null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
            g.DrawCurve(RightSpecialStreamLine);
            g.DrawCurve(LeftSpecialStreamLine);
        }

        void FindInitSpecial() 
        {
            LeftSpecialStreamLineBase = new List<DataPoint>();
            RightSpecialStreamLineBase = new List<DataPoint>();
            LeftStagnationPointBase = -w.R;
            RightStagnationPointBase = w.R;
            double x;
            for (x = RightStagnationPointBase.Re + (h_mrk / 100.0); x <= x_max; x++)
            {
                LeftSpecialStreamLineBase.Add(new DataPoint(-x, 0));
                RightSpecialStreamLineBase.Add(new DataPoint(x, 0));
            }
        }

        void FindBaseStreamLines()
        {
            FindInitSpecial();
            StreamLinesBase = new List<List<DataPoint>>();
            async_base = new IniFillAsync(AsyncIniFill);
            res_list = new List<IAsyncResult>();
            for (double y = h; y <= y_max - h; y += h)
            {
                StreamLinesBase.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_base.BeginInvoke(StreamLinesBase[StreamLinesBase.Count - 2], StreamLinesBase[StreamLinesBase.Count-1], y, null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }

        void Transform()
        {
            StreamLines = new List<List<DataPoint>>();
            LeftSpecialStreamLine = new List<DataPoint>();
            RightSpecialStreamLine = new List<DataPoint>();
            async_transform = new TransformationAsync(AsyncTransform);
            res_list = new List<IAsyncResult>();
            foreach (List<DataPoint> sllb in StreamLinesBase)
            {
                StreamLines.Add(new List<DataPoint>());
                res_list.Add(async_transform.BeginInvoke(sllb, StreamLines[StreamLines.Count - 1], exp(i * w.AlphaRadians), null, null));
            }
            LeftStagnationPoint = w.f.z(LeftStagnationPointBase * exp(i * w.AlphaRadians));
            RightStagnationPoint = w.f.z(RightStagnationPointBase * exp(i * w.AlphaRadians));
            g.DrawStagnationPoints(RightStagnationPoint, LeftStagnationPoint);
            res_list.Add(async_transform.BeginInvoke(LeftSpecialStreamLineBase,LeftSpecialStreamLine, exp(i * w.AlphaRadians), null,null));
            res_list.Add(async_transform.BeginInvoke(RightSpecialStreamLineBase, RightSpecialStreamLine, exp(i * w.AlphaRadians), null, null));
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }

        void FindAllStreamLines()
        {
            FindInitSpecial();
            double tmp = w.AlphaRadians;
            w.AlphaDegrees = 0;
            StreamLines = new List<List<DataPoint>>();
            StreamLinesBase = new List<List<DataPoint>>();
            async_full = new FullBuildAsync(AsyncFullBuild);
            res_list = new List<IAsyncResult>();
            for (double y = h; y <= y_max - h; y += h)
            {
                StreamLines.Add(new List<DataPoint>());
                StreamLines.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_full.BeginInvoke(StreamLinesBase[StreamLinesBase.Count - 2], StreamLinesBase[StreamLinesBase.Count - 1], StreamLines[StreamLines.Count - 2], StreamLines[StreamLines.Count - 1], exp(i * tmp), y, null, null));
            }
            LeftStagnationPoint = w.f.z(LeftStagnationPointBase * exp(i * tmp));
            RightStagnationPoint = w.f.z(RightStagnationPointBase * exp(i * tmp));
            g.DrawStagnationPoints(RightStagnationPoint, LeftStagnationPoint);
            LeftSpecialStreamLine = new List<DataPoint>();
            RightSpecialStreamLine = new List<DataPoint>();
            res_list.Add(async_transform.BeginInvoke(LeftSpecialStreamLineBase, LeftSpecialStreamLine, exp(i * tmp), null, null));
            res_list.Add(async_transform.BeginInvoke(RightSpecialStreamLineBase, RightSpecialStreamLine, exp(i * tmp), null, null));
            while (!res_list.IsAllThreadsCompleted()) { }
            w.AlphaRadians = tmp;
            res_list = null;
        }

        void AsyncIniFill(List<DataPoint> bp, List<DataPoint> bm, double y)
        {
            double x_new, y_new;
            x_new = x_min;
            y_new = y;
            bp.Add(new DataPoint(x_new, y_new));
            bm.Add(new DataPoint(x_new, -y_new));
            while (x_new < x_max)
            {
                MRK_loop(ref x_new, ref y_new);
                bp.Add(new DataPoint(x_new, y_new));
                bm.Add(new DataPoint(x_new, -y_new));
            }
        }

        void AsyncTransform(List<DataPoint> b, List<DataPoint> l, complex angleMult)
        {
            DataPoint tmp;
            foreach (DataPoint bp in b)
            {
                tmp = w.f.z((bp.DataPointToComplex()* angleMult).ComplexToDataPoint());
                if (tmp.Abs() < 20) { l.Add(tmp); }
            }
            g.DrawCurve(l);
        }

        void AsyncFullBuild(List<DataPoint> bp, List<DataPoint> bm, List<DataPoint> lp, List<DataPoint> lm, complex angleMult, double y)
        {
            DataPoint tmp;
            double x_new, y_new;
            x_new = x_min;
            y_new = y;
            bp.Add(new DataPoint(x_new, y_new));
            bm.Add(new DataPoint(x_new, -y_new));
            while (x_new < x_max)
            {
                MRK_loop(ref x_new, ref y_new);
                bp.Add(new DataPoint(x_new, y_new));
                bm.Add(new DataPoint(x_new, -y_new));
                tmp = w.f.z((bp[bp.Count - 1].DataPointToComplex() * angleMult).ComplexToDataPoint());
                if (tmp.Abs() < 20) { lp.Add(tmp); }
                tmp = w.f.z((bm[bm.Count - 1].DataPointToComplex() * angleMult).ComplexToDataPoint());
                if (tmp.Abs() < 20) { lm.Add(tmp); }
            }
            g.DrawCurve(lp);
            g.DrawCurve(lm);
        }

        void AsyncJoukowskiBuild(List<DataPoint> l, double y)
        {
            double x_new, y_new;
            x_new = x_min;
            y_new = y;
            l.Add(w.f.z(new DataPoint(x_new, y_new)));
            while (x_new < x_max)
            {
                MRK_loop(ref x_new, ref y_new);
                l.Add(w.f.z(new DataPoint(x_new, y_new)));
            }
            g.DrawCurve(l);
        }
    }
#else
    class StreamLinesBuilderForGroupLeader : StreamLinesBuilder
    {
        delegate void IniFillAsync(List<DataPoint> Base, double x);
        delegate void TransformationAsync(List<DataPoint> Base, List<DataPoint> Lines);
        delegate void FullBuildAsync(List<DataPoint> Base, List<DataPoint> Lines, double x);
        IniFillAsync async_base;
        TransformationAsync async_transform;
        FullBuildAsync async_full;
        PotentialHelp w;

        public StreamLinesBuilderForGroupLeader(PotentialHelp w, PlotWindowModel g)
        {
            this.w = w;
            function = new Conformal_Maps.IdentityTransform();
            this.g = g;
            this.x_min = -w.a;
            this.x_max = w.a;
            y_max = Settings.PlotGeomParams.YMax;
            y_min = 0;
            h_mrk = Settings.PlotGeomParams.MRKh;
            h = Settings.PlotGeomParams.hVertical;
            StreamLinesBase = new List<List<DataPoint>>();
            InitialBuild();
        }
        public override void ChangeParams(double? x_min, double? x_max, double? y_max, double? h_horizontal, double? h_vertical)
        {
            this.x_min = -w.a;
            this.x_max = w.a;
            this.y_max = y_max ?? this.y_max;
            this.h_mrk = h_horizontal ?? this.h_mrk;
            this.h = h_vertical ?? this.h;
            FullRebuild();
        }
        public override void Rebuild()
        {
            g.Clear();
            StreamLines = new List<List<DataPoint>>();
            //g.DrawBorder(this);
            Transform();
        }

        void InitialBuild()
        {
            g.Clear();
            FindBaseStreamLines();
        }
        void FullRebuild()
        {
            g.Clear();
            StreamLines = new List<List<DataPoint>>();
            StreamLinesBase = new List<List<DataPoint>>();
            //g.DrawBorder(this);
            FindAllStreamLines();
        }
        void FindBaseStreamLines()
        {
            async_base = new IniFillAsync(AsyncIniFill);
            res_list = new List<IAsyncResult>();
            for (double x = x_min + h; x <x_max; x += h)
            {
                x = Math.Round(x, 4);
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_base.BeginInvoke(StreamLinesBase[StreamLinesBase.Count - 1], x, null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }
        void Transform()
        {
            async_transform = new TransformationAsync(AsyncTransform);
            res_list = new List<IAsyncResult>();
            foreach (List<DataPoint> sllb in StreamLinesBase)
            {
                StreamLines.Add(new List<DataPoint>());
                res_list.Add(async_transform.BeginInvoke(sllb, StreamLines[StreamLines.Count - 1], null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }
        void FindAllStreamLines()
        {
            async_full = new FullBuildAsync(AsyncFullBuild);
            res_list = new List<IAsyncResult>();
            for (double y = y_min + h; y <= y_max - h; y += h)
            {
                StreamLines.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_full.BeginInvoke(StreamLinesBase[StreamLinesBase.Count - 1], StreamLines[StreamLines.Count - 1], y, null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }

        double f(double X, double Y) => w.V_ksi(new complex(X, Y)) / w.V_eta(new complex(X, Y));

        void AsyncIniFill(List<DataPoint> b, double x)
        {
            double y, x_new, y_new, k1, k2, k3, k4;
            x_new = x;
            y_new = y_min;
            b.Add(new DataPoint(x_new, y_new));
            while (y_new < y_max)
            {
                x = x_new;
                y = y_new;
                k1 = f(x, y);
                k2 = f(x + (h_mrk * k1) / 2, y + h_mrk / 2);
                k3 = f(x + (h_mrk * k2) / 2, y + h_mrk / 2);
                k4 = f(x + (h_mrk * k3), y + h_mrk);
                y_new = y + h_mrk;
                x_new = x + (h_mrk / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                b.Add(new DataPoint(x_new, y_new));
            }
        }
        void AsyncTransform(List<DataPoint> b, List<DataPoint> l)
        {
            DataPoint tmp;
            foreach (DataPoint bp in b)
            {
                tmp = function.z(bp);
                if (tmp.Abs() < 20) { l.Add(tmp); }
            }
            g.DrawCurve(l);
        }
        void AsyncFullBuild(List<DataPoint> b, List<DataPoint> l, double x)
        {
            double y, x_new, y_new, k1, k2, k3, k4;
            x_new = x;
            y_new = y_min;
            b.Add(new DataPoint(x_new, y_new));
            l.Add(function.z(b[b.Count - 1]));
            while (y_new < y_max)
            {
                x = x_new;
                y = y_new;
                k1 = f(x, y);
                k2 = f(x + (h_mrk * k1) / 2, y + h_mrk / 2);
                k3 = f(x + (h_mrk * k2) / 2, y + h_mrk / 2);
                k4 = f(x + (h_mrk * k3), y + h_mrk);
                y_new = y + h_mrk;
                x_new = x + (h_mrk / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                b.Add(new DataPoint(x_new, y_new));
                l.Add(function.z(b[b.Count - 1]));
            }
        }

    }
#endif

}
