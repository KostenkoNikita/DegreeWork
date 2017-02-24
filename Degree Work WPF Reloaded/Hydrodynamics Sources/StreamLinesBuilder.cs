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
    }
}
