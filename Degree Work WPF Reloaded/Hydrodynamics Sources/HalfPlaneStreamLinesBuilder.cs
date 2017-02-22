using System;
using System.Collections.Generic;
using MathCore_2_0;
using OxyPlot;
using OxyPlot.Series;
using static MathCore_2_0.math;
using static MathCore_2_0.complex;

namespace Degree_Work.Hydrodynamics_Sources
{
    class StreamLinesBuilderHalfPlane
    {

        double x_min, x_max, y_max;
        double h;//расстояние между линиями тока в начале отсчета по вертикальной оси
        double h_mrk;
        PlotWindowModel g;
        Potential w;
        IConformalMapFunction function;
        List<List<DataPoint>> StreamLines;
        List<List<DataPoint>> StreamLinesBase;
        delegate void IniFillAsync(List<DataPoint> Base, double y);
        delegate void TransformationAsync(List<DataPoint> Base, List<DataPoint> Lines);
        delegate void FullBuildAsync(List<DataPoint> Base, List<DataPoint> Lines, double y);
        IniFillAsync async_base;
        TransformationAsync async_transform;
        FullBuildAsync async_full;
        List<IAsyncResult> res_list;
        public StreamLinesBuilderHalfPlane(Potential w, PlotWindowModel g, double x_min, double x_max, double y_max, double h_horizontal, double h_vertical)
        {
            this.w = w;
            function = this.w.f;
            this.g = g;
            this.x_min = x_min;
            this.x_max = x_max;
            this.y_max = y_max;
            h_mrk = h_horizontal;
            h = h_vertical;
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
            g.DrawBorder(w);
            Transform();
        }
        void FullRebuild()
        {
            g.Clear();
            StreamLines = new List<List<DataPoint>>();
            StreamLinesBase = new List<List<DataPoint>>();
            g.DrawBorder(w);
            FindAllStreamLines();
        }
        void FindBaseStreamLines()
        {
            async_base = new IniFillAsync(AsyncIniFill);
            res_list = new List<IAsyncResult>();
            for (double y = h; y <= y_max; y += h)
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
            for (double y = h; y <= y_max; y += h)
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
            foreach (DataPoint bp in b)
            {
                l.Add(w.f.z(bp));
            }
            g.DrawCurve(l);
        }
        void AsyncFullBuild(List<DataPoint> b, List<DataPoint> l, double y)
        {
            for (double x = x_min; x <= x_max; x += h_mrk)
            {
                b.Add(new DataPoint(x, y));
                l.Add(w.f.z(b[b.Count - 1]));
            }
            g.DrawCurve(l);
        }
    }
}
