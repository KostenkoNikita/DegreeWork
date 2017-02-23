using System;
using System.Collections.Generic;
using MathCore_2_0;
using OxyPlot;
using OxyPlot.Series;
using static MathCore_2_0.math;
using static MathCore_2_0.complex;

namespace Degree_Work.Hydrodynamics_Sources
{
    class StreamLinesBuilderHalfPlaneAndZone : StreamLinesBuilder
    {
        delegate void IniFillAsync(List<DataPoint> Base, double y);
        delegate void TransformationAsync(List<DataPoint> Base, List<DataPoint> Lines);
        delegate void FullBuildAsync(List<DataPoint> Base, List<DataPoint> Lines, double y);
        IniFillAsync async_base;
        TransformationAsync async_transform;
        FullBuildAsync async_full;
        List<IAsyncResult> res_list;
        public StreamLinesBuilderHalfPlaneAndZone(Potential w, PlotWindowModel g, CanonicalDomain domain)
        {
            this.w = w;
            function = this.w.f;
            this.g = g;
            this.x_min = Settings.PlotGeomParams.XMin;
            this.x_max = Settings.PlotGeomParams.XMax;
            this.domain = domain;
            switch (domain)
            {
                case CanonicalDomain.HalfPlane:
                    y_max = Settings.PlotGeomParams.YMax;
                    y_min = 0;
                    break;
                case CanonicalDomain.Zone:
                    y_max = Math.PI;
                    y_min = -Math.PI;
                    break;
                default: throw new ArgumentException();
            }
            h_mrk = Settings.PlotGeomParams.MRKh;
            h = Settings.PlotGeomParams.hVertical;
            StreamLinesBase = new List<List<DataPoint>>();
            InitialBuild();
        }
        public void ChangeParams(double? x_min, double? x_max, double? y_max, double? h_horizontal, double? h_vertical)
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
        public void Rebuild()
        {
            g.Clear();
            StreamLines = new List<List<DataPoint>>();
            g.DrawBorder(this);
            Transform();
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
            for (double y = y_min + h; y <= y_max-h; y += h)
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
            for (double y = y_min + h; y <= y_max-h; y += h)
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
            //switch (Domain)
            //{
            //    case CanonicalDomain.HalfPlane:
            //        foreach (DataPoint bp in b)
            //        {
            //            l.Add(w.f.z(bp));
            //        }
            //        break;
            //    case CanonicalDomain.Zone:
            //        DataPoint tmp;
            //        foreach (DataPoint bp in b)
            //        {
            //            tmp = w.f.z(bp);
            //            if (tmp.Abs() < 20){l.Add(tmp);}
            //        }
            //        break;
            //}
            DataPoint tmp;
            foreach (DataPoint bp in b)
            {
                tmp = w.f.z(bp);
                if (tmp.Abs() < 20) { l.Add(tmp); }
            }
            g.DrawCurve(l);
        }
        void AsyncFullBuild(List<DataPoint> b, List<DataPoint> l, double y)
        {
            //switch (Domain)
            //{
            //    case CanonicalDomain.HalfPlane:
            //        for (double x = x_min; x <= x_max; x += h_mrk)
            //        {
            //            b.Add(new DataPoint(x, y));
            //            l.Add(w.f.z(b[b.Count - 1]));
            //        }
            //        break;
            //    case CanonicalDomain.Zone:
            //        DataPoint tmp;
            //        for (double x = x_min; x <= x_max; x += h_mrk)
            //        {
            //            b.Add(new DataPoint(x, y));
            //            tmp = w.f.z(b[b.Count - 1]);
            //            if (tmp.Abs() < 20) { l.Add(tmp); }
            //        }
            //        break;
            //}
            DataPoint tmp;
            for (double x = x_min; x <= x_max; x += h_mrk)
            {
                b.Add(new DataPoint(x, y));
                tmp = w.f.z(b[b.Count - 1]);
                if (tmp.Abs() < 20) { l.Add(tmp); }
            }
            g.DrawCurve(l);
        }
    }
}
