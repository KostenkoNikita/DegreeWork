#pragma warning disable 612

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace Degree_Work
{
    enum CanonicalDomain { HalfPlane, Circular }

    class PlotWindowModel : INotifyPropertyChanged
    {
        private PlotModel p;

        object locker = new object();

        public PlotModel PlotModel
        {
            get { return p; }
            set { p = value; OnPropertyChanged("PlotModel"); }
        }

        internal PlotWindowModel(CanonicalDomain domain)
        {
            PlotModel = new PlotModel();
            SetUpModel(domain);
        }

        void SetUpModel(CanonicalDomain domain)
        {
            PlotModel.LegendTitle = null;
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;
            LinearAxis XAxis, YAxis;
            switch (domain)
            {
                case CanonicalDomain.HalfPlane:
                    XAxis = new LinearAxis(AxisPosition.Bottom, -5, 5, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(XAxis);
                    YAxis = new LinearAxis(AxisPosition.Left, -0.5, 5, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 5, AbsoluteMinimum = -0.5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(YAxis);
                    break;
                case CanonicalDomain.Circular:
                    XAxis = new LinearAxis(AxisPosition.Bottom, -5, 5, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(XAxis);
                    YAxis = new LinearAxis(AxisPosition.Left, -5, 5, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(YAxis);
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DrawCurve(List<DataPoint> l)
        {
            lock (locker)
            {
                LineSeries ls = new LineSeries();
                ls.CanTrackerInterpolatePoints = true;
                ls.Smooth = true;
                ls.Color = OxyColors.Blue;
                ls.StrokeThickness = 2;
                foreach (DataPoint p in l) { ls.Points.Add(p); }
                PlotModel.Series.Add(ls);
            }
        }

        public void DrawCircle(double _X, double _Y, double _R, double _StrokeThickness = 2)
        {
            var an = new EllipseAnnotation()
            {
                X = _X,
                Y = _Y,
                Width = 2 * _R,
                Height = 2 * _R,
                Fill = OxyColors.Gray,
                StrokeThickness = _StrokeThickness,
            };
            PlotModel.Annotations.Add(an);
        }

        public void DrawBorder(Hydrodynamics_Sources.Potential w)
        {
            switch (w.f.ToString())
            {
                case "Porebrick":
                    double h = (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).h;
                    PolygonAnnotation pp = new PolygonAnnotation();
                    pp.Fill = OxyColors.Gray;
                    pp.Points.Add(new DataPoint(-6, 0));
                    pp.Points.Add(new DataPoint(0, 0));
                    pp.Points.Add(new DataPoint(0, h));
                    pp.Points.Add(new DataPoint(6, h));
                    pp.Points.Add(new DataPoint(6, -1));
                    pp.Points.Add(new DataPoint(-6, -1));
                    pp.StrokeThickness = 2;
                    pp.Stroke = OxyColors.Black;
                    PlotModel.Annotations.Add(pp);
                    break;
                case "IdentityTransform":
                    PolygonAnnotation pi = new PolygonAnnotation();
                    pi.Fill = OxyColors.Gray;
                    pi.Points.Add(new DataPoint(-6, 0));
                    pi.Points.Add(new DataPoint(6, 0));
                    pi.Points.Add(new DataPoint(6, -1));
                    pi.Points.Add(new DataPoint(-6, -1));
                    pi.StrokeThickness = 2;
                    pi.Stroke = OxyColors.Black;
                    PlotModel.Annotations.Add(pi);
                    break;
            }
        }

        public void Clear()
        {
            PlotModel.Series.Clear();
            PlotModel.Annotations.Clear();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
