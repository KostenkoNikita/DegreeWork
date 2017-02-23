#pragma warning disable 612

using System;
using System.IO;

using System.Collections.Generic;
using System.ComponentModel;
using MathCore_2_0;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace Degree_Work
{
    enum CanonicalDomain { HalfPlane, Zone, Circular }

    class PlotWindowModel : INotifyPropertyChanged
    {
        private const double PolygonLineHalfWidth = 0.02;
        private PlotModel p;
        private ArrowAnnotation arrow;
        private TextAnnotation arrowText;
        public Annotation BorderBottom;
        PolygonAnnotation BorderPolyBottom => BorderBottom as PolygonAnnotation;
        //BorderTop будет присутствовать только в том случае, если
        //вспомогательная плоскость имеет вид полосы
        public Annotation BorderTop;
        PolygonAnnotation BorderPolyTop => BorderTop as PolygonAnnotation;
        bool IsMouseClickedInPolygon;
        object locker = new object();

        public PlotModel PlotModel
        {
            get { return p; }
            set { p = value; OnPropertyChanged("PlotModel"); }
        }

        private Axis X_Axis
        {
            get { return PlotModel.Axes[0] as Axis; }
        }
        private Axis Y_Axis
        {
            get { return PlotModel.Axes[1] as Axis; }
        }
        public DataPoint GetDataPointCursorPositionOnPlot(ScreenPoint pos)
        {
            return OxyPlot.Axes.Axis.InverseTransform(pos, X_Axis, Y_Axis);
        }
        public complex GetComplexCursorPositionOnPlot(ScreenPoint pos)
        {
            return OxyPlot.Axes.Axis.InverseTransform(pos, X_Axis, Y_Axis).DataPointToComplex();
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
                case CanonicalDomain.Zone:
                    XAxis = new LinearAxis(AxisPosition.Bottom, -5, 5, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(XAxis);
                    YAxis = new LinearAxis(AxisPosition.Left, -5, 5, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 5, AbsoluteMinimum = -0.5, Font = "Times New Roman", FontSize = 15 };
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
                ls.Smooth = true;
                ls.Color = Settings.PlotVisualParams.LineColor;
                ls.StrokeThickness = Settings.PlotVisualParams.LineStrokeThickness;
                foreach (DataPoint p in l) { ls.Points.Add(p); }
                PlotModel.Series.Add(ls);
            }

        }

        public void ReassignVisualParams()
        {
            lock (locker)
            {
                LineSeries ls;
                for (int i = 0; i < p.Series.Count; i++)
                {
                    ls = p.Series[i] as LineSeries;
                    if (ls != null)
                    {
                        ls.StrokeThickness = Settings.PlotVisualParams.LineStrokeThickness;
                        ls.Color = Settings.PlotVisualParams.LineColor;
                    }
                    else
                    {
                        continue;
                    }
                }
                BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                if (BorderPolyTop != null)
                {
                    BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                    BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                    BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                }
                if (arrow != null)
                {
                    arrow.Color = Settings.PlotVisualParams.ArrowColor;
                    arrow.StrokeThickness = Settings.PlotVisualParams.ArrowStokeThickness;
                }
                else { return; }
            }
        }

        void CreateArrow()
        {
            arrow = new ArrowAnnotation()
            {
                Color = Settings.PlotVisualParams.ArrowColor,
                StrokeThickness = Settings.PlotVisualParams.ArrowStokeThickness,
                StartPoint = new DataPoint(0,0),
                EndPoint = new DataPoint(0,0),
            };
            arrowText = new TextAnnotation()
            {
                FontSize = 25,
                Background = OxyColors.White,
                Font = "Times New Roman",
                StrokeThickness = 2,
                Stroke = OxyColors.Black,
            };
            PlotModel.Annotations.Add(arrow);
            PlotModel.Annotations.Add(arrowText);
        }

        void DeleteArrow()
        {
            if (arrow != null)
            {
                PlotModel.Annotations.Remove(arrow);
            }
            if (arrowText != null)
            {
                PlotModel.Annotations.Remove(arrowText);
            }
        }

        public void RedrawArrow(double x_start, double y_start, double x_end, double y_end, CanonicalDomain domain)
        {
            if (IsMouseClickedInPolygon)
            {
                DeleteArrow();
                IsMouseClickedInPolygon = false;
            }
            else
            {
                if (!HasArrow()) { CreateArrow(); }
                arrow.StartPoint = new DataPoint(x_start, y_start);
                arrow.EndPoint = new DataPoint(x_end, y_end);
                arrowText.Text = "Vector";
                arrowText.TextPosition = new DataPoint(x_start, y_start - (domain==CanonicalDomain.HalfPlane? 0.6 : 1.2));
            }
        }

        public void RedrawArrow(DataPoint start, DataPoint end, CanonicalDomain domain)
        {
            if (IsMouseClickedInPolygon)
            {
                DeleteArrow();
                IsMouseClickedInPolygon = false;
            }
            else
            {
                if (!HasArrow()) { CreateArrow(); }
                arrow.StartPoint = start;
                arrow.EndPoint = end;
                arrowText.Text = "Vector";
                arrowText.TextPosition = new DataPoint(start.X, start.Y- (domain == CanonicalDomain.HalfPlane ? 0.6 : 1.2));
            }
        }

        public void RedrawArrow(complex start, complex end, complex V, CanonicalDomain domain)
        {
            if (IsMouseClickedInPolygon)
            {
                DeleteArrow();
                IsMouseClickedInPolygon = false;
            }
            else
            {
                if (!HasArrow()) { CreateArrow(); }
                arrow.StartPoint = start.ComplexToDataPoint();
                arrow.EndPoint = end.ComplexToDataPoint();
                arrowText.Text = $"X: {start.Re.ToString(Settings.Format)}; Y: {start.Im.ToString(Settings.Format)};".Replace(',', '.') +
                    $"\nVx: {V.Re.ToString(Settings.Format)}; Vy: {V.Im.ToString(Settings.Format)};".Replace(',','.');
                arrowText.TextPosition = (start- (domain == CanonicalDomain.HalfPlane ? 0.6 : 1.2) * complex.i).ComplexToDataPoint();
            }
        }

        public void SavePlotToJPG(string path, int width, int height)
        {
            var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
            var bitmap = pngExporter.ExportToBitmap(PlotModel);
            bitmap.SaveJPG100(path);
        }

        public void SavePlotToPNG(string path, int width, int height)
        {
            using (FileStream s = new FileStream(path, FileMode.Create))
            {
                var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
                pngExporter.Export(PlotModel, s);
            }
        }

        public void SavePlotToBMP(string path, int width, int height)
        {
            var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
            var bitmap = pngExporter.ExportToBitmap(PlotModel);
            bitmap.GetBitmap().Save(path);
        }

        bool HasArrow()
        {
            foreach (Annotation a in PlotModel.Annotations)
            {
                if (a is ArrowAnnotation) { return true; }
            }
            return false;
        }

        public void DrawBorder(Hydrodynamics_Sources.StreamLinesBuilder s)
        {
            switch (s.Domain)
            {
                case CanonicalDomain.HalfPlane:
                    switch (s.W.f.ToString())
                    {
                        case "Porebrick":
                            double h = (s.W.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).h;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(0, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(0, h));
                            BorderPolyBottom.Points.Add(new DataPoint(6, h));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                        case "IdentityTransform":
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                    }
                    BorderBottom.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
                    break;
                case CanonicalDomain.Zone:
                    switch (s.W.f.ToString())
                    {
                        case "IdentityTransform":
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -Math.PI));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -Math.PI));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -6));
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -6));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            BorderTop = new PolygonAnnotation();
                            BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyTop.Points.Add(new DataPoint(-6, Math.PI));
                            BorderPolyTop.Points.Add(new DataPoint(6, Math.PI));
                            BorderPolyTop.Points.Add(new DataPoint(6, 6));
                            BorderPolyTop.Points.Add(new DataPoint(-6, 6));
                            BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            PlotModel.Annotations.Add(BorderPolyTop);
                            break;
                        case "EjectedRays":
                            Hydrodynamics_Sources.Conformal_Maps.EjectedRays tmp = s.W.f as Hydrodynamics_Sources.Conformal_Maps.EjectedRays;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.l * Math.Cos(tmp.Angle)-PolygonLineHalfWidth, tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.l * Math.Cos(tmp.Angle)+ PolygonLineHalfWidth, tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyBottom.Points.Add(new DataPoint(100 * tmp.l * Math.Cos(tmp.Angle)+ PolygonLineHalfWidth, 100 * tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyBottom.Points.Add(new DataPoint(100 * tmp.l * Math.Cos(tmp.Angle)- PolygonLineHalfWidth, 100 * tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            BorderTop = new PolygonAnnotation();
                            BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyTop.Points.Add(new DataPoint(tmp.l * Math.Cos(tmp.Angle)- PolygonLineHalfWidth, -tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyTop.Points.Add(new DataPoint(tmp.l * Math.Cos(tmp.Angle)+ PolygonLineHalfWidth, -tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyTop.Points.Add(new DataPoint(100 * tmp.l * Math.Cos(tmp.Angle)+ PolygonLineHalfWidth, -100 * tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyTop.Points.Add(new DataPoint(100 * tmp.l * Math.Cos(tmp.Angle)- PolygonLineHalfWidth, -100 * tmp.l * Math.Sin(tmp.Angle)));
                            BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            PlotModel.Annotations.Add(BorderPolyTop);
                            break;
                    }
                    BorderBottom.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
                    BorderTop.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
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
