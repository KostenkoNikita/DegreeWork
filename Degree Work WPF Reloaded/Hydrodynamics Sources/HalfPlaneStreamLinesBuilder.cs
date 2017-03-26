//#define HELP_FOR_GROUP_LEADER
using System;
using System.Collections.Generic;
using OxyPlot;

namespace Degree_Work.Hydrodynamics_Sources
{
    class HalfPlaneAndZoneStreamLinesBuilder : StreamLinesBuilder
    {
        delegate void IniFillAsync(List<DataPoint> Base, double y);
        delegate void TransformationAsync(List<DataPoint> Base, List<DataPoint> Lines);
        delegate void FullBuildAsync(List<DataPoint> Base, List<DataPoint> Lines, double y);
        IniFillAsync async_base;
        TransformationAsync async_transform;
        FullBuildAsync async_full;
#if !HELP_FOR_GROUP_LEADER
        public HalfPlaneAndZoneStreamLinesBuilder(Potential w, PlotWindowModel g, CanonicalDomain domain) : base(w,g,domain)
        {
            InitialBuild();
        }
#else
        public HalfPlaneAndZoneStreamLinesBuilder(Potential w, PlotWindowModel g, CanonicalDomain domain)
        {
            InitialBuild();
        }
#endif
        public override void ChangeParams(double? x_min, double? x_max, double? y_max, double? h_horizontal, double? h_vertical)
        {
            this.x_min = x_min ?? this.x_min;
            this.x_max = x_max ?? this.x_max;
            this.y_max = y_max ?? this.y_max;
            this.h_mrk = h_horizontal ?? this.h_mrk;
            this.h = h_vertical ?? this.h;
            FullRebuild();
        }
        public override void Rebuild()
        {
            g.Clear();
            StreamLines = new List<List<DataPoint>>();
            g.DrawBorder(this);
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
            g.DrawBorder(this);
            FindAllStreamLines();
        }
        void FindBaseStreamLines()
        {
            async_base = new IniFillAsync(AsyncIniFill);
            res_list = new List<IAsyncResult>();
            if (w.f is Conformal_Maps.Diffusor) { y_min = -h; }
            for (double y = y_min+h; y <= y_max - h; y += h)
            {
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_base.BeginInvoke(StreamLinesBase[StreamLinesBase.Count - 1], y, null, null));
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
            if (w.f is Conformal_Maps.Diffusor) { y_min = -h; }
            for (double y = y_min + h; y <= y_max - h; y += h)
            {
                StreamLines.Add(new List<DataPoint>());
                StreamLinesBase.Add(new List<DataPoint>());
                res_list.Add(async_full.BeginInvoke(StreamLinesBase[StreamLinesBase.Count - 1], StreamLines[StreamLines.Count - 1], y, null, null));
            }
            while (!res_list.IsAllThreadsCompleted()) { }
            res_list = null;
        }
        void AsyncIniFill(List<DataPoint> b, double y)
        {
            for (double x = x_min; x <= x_max; x += h_mrk)
            {
                b.Add(new DataPoint(x, y));
            }
        }
        void AsyncTransform(List<DataPoint> b, List<DataPoint> l)
        {
            switch (domain)
            {
                case CanonicalDomain.HalfPlane:
                    DataPoint tmp;
                    foreach (DataPoint bp in b)
                    {
                        tmp = w.f.z(bp);
                        if (tmp.Abs() < 20) { l.Add(tmp); }
                    }
                    g.DrawCurve(l);
                    return;
                case CanonicalDomain.Zone:
                    DataPoint tmp1, tmp2;
                    List<DataPoint> lReflect = new List<DataPoint>();
                    foreach (DataPoint bp in b)
                    {
                        tmp1 = w.f.z(bp);
                        tmp2 = tmp1.Conjugate();
                        if (tmp1.Abs() < 20) { l.Add(tmp1); if (tmp2.Y != 0) { lReflect.Add(tmp2); } }
                    }
                    g.DrawCurve(l);
                    g.DrawCurve(lReflect);
                    return;
            }
        }
        void AsyncFullBuild(List<DataPoint> b, List<DataPoint> l, double y)
        {
            switch (domain)
            {
                case CanonicalDomain.HalfPlane:
                    DataPoint tmp;
                    for (double x = x_min; x <= x_max; x += h_mrk)
                    {
                        b.Add(new DataPoint(x, y));
                        tmp = w.f.z(b[b.Count - 1]);
                        if (tmp.Abs() < 20) { l.Add(tmp); }
                    }
                    g.DrawCurve(l);
                    return;
                case CanonicalDomain.Zone:
                    DataPoint tmp1, tmp2;
                    List<DataPoint> lReflect = new List<DataPoint>();
                    for (double x = x_min; x <= x_max; x += h_mrk)
                    {
                        b.Add(new DataPoint(x, y));
                        tmp1 = w.f.z(b[b.Count - 1]);
                        tmp2 = tmp1.Conjugate();
                        if (tmp1.Abs() < 20) { l.Add(tmp1); if (tmp2.Y != 0) { lReflect.Add(tmp2); } }
                    }
                    g.DrawCurve(l);
                    g.DrawCurve(lReflect);
                    return;
            }
        }
    }
}

