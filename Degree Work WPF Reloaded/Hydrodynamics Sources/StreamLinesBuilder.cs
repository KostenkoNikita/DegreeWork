//#define HELP_FOR_GROUP_LEADER
using System.Collections.Generic;
using OxyPlot;
using System;

namespace Degree_Work.Hydrodynamics_Sources
{
    abstract class StreamLinesBuilder
    {
        protected PlotWindowModel g;
        protected Potential w;
        protected CanonicalDomain domain;
        protected double x_min, x_max, y_max, y_min;
        protected double h;//расстояние между линиями тока в начале отсчета по вертикальной оси
        protected double h_mrk;
        public CanonicalDomain Domain => domain;
        public Potential W => w;
        protected IConformalMapFunction function;
        protected List<List<DataPoint>> StreamLines;
        protected List<List<DataPoint>> StreamLinesBase;
        public virtual void Rebuild() { }
        public virtual void ChangeParams(double? x_min, double? x_max, double? y_max, double? h_horizontal, double? h_vertical) { }
        protected List<IAsyncResult> res_list;
#if !HELP_FOR_GROUP_LEADER
        protected StreamLinesBuilder(Potential w, PlotWindowModel g, CanonicalDomain domain)
        {
            this.w = w;
            function = this.w.f;
            this.g = g;
            this.domain = domain;
            switch (domain)
            {
                case CanonicalDomain.HalfPlane:
                    x_min = Settings.PlotGeomParams.XMin;
                    x_max = Settings.PlotGeomParams.XMax;
                    y_max = Settings.PlotGeomParams.YMax;
                    y_min = 0;
                    break;
                case CanonicalDomain.Zone:
                    x_min = Settings.PlotGeomParams.XMin;
                    x_max = Settings.PlotGeomParams.XMax;
                    y_max = Math.PI;
                    y_min = -Settings.PlotGeomParams.hVertical;
                    break;
                case CanonicalDomain.Circular:
                    x_min = Settings.PlotGeomParams.XMin;
                    x_max = Settings.PlotGeomParams.XMax;
                    y_max = Settings.PlotGeomParams.YMax;
                    y_min = 0;
                    break;
                default: throw new ArgumentException();
            }
            h_mrk = Settings.PlotGeomParams.MRKh;
            h = Settings.PlotGeomParams.hVertical;
            StreamLinesBase = new List<List<DataPoint>>();
        }
#endif
    }
}
