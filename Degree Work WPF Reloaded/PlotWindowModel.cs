#pragma warning disable 612
//#define HELP_FOR_GROUP_LEADER

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
        private const double StagnationPointsRadius = 0.05;
        private PlotModel p;
        private ArrowAnnotation arrow;
        private TextAnnotation arrowText;
        public Annotation BorderBottom;
        PolygonAnnotation BorderPolyBottom => BorderBottom as PolygonAnnotation;
        EllipseAnnotation EllipseBorder => BorderBottom as EllipseAnnotation;
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
#if !HELP_FOR_GROUP_LEADER
                    XAxis = new LinearAxis(AxisPosition.Bottom, -5, 5, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(XAxis);
                    YAxis = new LinearAxis(AxisPosition.Left, -5, 5, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(YAxis);
                    break;
#else
                    XAxis = new LinearAxis(AxisPosition.Bottom, -3, 3, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(XAxis);
                    YAxis = new LinearAxis(AxisPosition.Left, -0.1, 1, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(YAxis);
                    break;
#endif
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
                if (BorderPolyBottom != null)
                {
                    BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                    BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                    BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                    if (BorderPolyTop != null)
                    {
                        BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                        BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                        BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                    }
                }
                else
                {
                    EllipseBorder.Fill = Settings.PlotVisualParams.BorderFillColor;
                    EllipseBorder.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                    EllipseBorder.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
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

        public void RedrawArrow(complex start, complex end, complex V, CanonicalDomain domain)
        {
            if (IsMouseClickedInPolygon)
            {
                DeleteArrow();
                IsMouseClickedInPolygon = false;
            }
            else if (complex.IsNaN(V))
            {
                DeleteArrow(); return;
            }
            else
            {
                if (!HasArrow()) { CreateArrow(); }
                arrow.StartPoint = start.ComplexToDataPoint();
                arrow.EndPoint = end.ComplexToDataPoint();
                arrowText.Text = $"X: {start.Re.ToString(Settings.Format)}; Y: {start.Im.ToString(Settings.Format)};".Replace(',', '.') +
                    $"\nVx: {V.Re.ToString(Settings.Format)}; Vy: {V.Im.ToString(Settings.Format)};".Replace(',', '.');
                arrowText.TextPosition = (start + (V.Im>=0? -1: 0.2)*(domain == CanonicalDomain.HalfPlane ? 0.6 : 1.2) * complex.i).ComplexToDataPoint();
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
                        case "EjectedSegment":
                            Hydrodynamics_Sources.Conformal_Maps.EjectedSegment tmp = s.W.f as Hydrodynamics_Sources.Conformal_Maps.EjectedSegment;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.X - PolygonLineHalfWidth, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.X - PolygonLineHalfWidth, tmp.Y));
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.X + PolygonLineHalfWidth, tmp.Y));
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.X + PolygonLineHalfWidth, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                        case "Number81":
                            Hydrodynamics_Sources.Conformal_Maps.Number81 n = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Number81;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(- PolygonLineHalfWidth, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(- PolygonLineHalfWidth, n.h));
                            BorderPolyBottom.Points.Add(new DataPoint(PolygonLineHalfWidth, n.h));
                            BorderPolyBottom.Points.Add(new DataPoint(PolygonLineHalfWidth, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                        case "Number79":
                            Hydrodynamics_Sources.Conformal_Maps.Number79 nn = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Number79;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, nn.h+1));
                            BorderPolyBottom.Points.Add(new DataPoint(-6,nn.h+6));
                            BorderPolyBottom.Points.Add(new DataPoint(6,nn.h+6));
                            BorderPolyBottom.Points.Add(new DataPoint(6,-1));
                            BorderPolyBottom.Points.Add(new DataPoint(0, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(0,1));
                            BorderPolyBottom.Points.Add(new DataPoint(6,1));
                            BorderPolyBottom.Points.Add(new DataPoint(6, nn.h+1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                        case "Number89":
                            Hydrodynamics_Sources.Conformal_Maps.Number89 ne = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Number89;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(-PolygonLineHalfWidth, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(-PolygonLineHalfWidth, ne.h1));
                            BorderPolyBottom.Points.Add(new DataPoint(PolygonLineHalfWidth, ne.h1));
                            BorderPolyBottom.Points.Add(new DataPoint(PolygonLineHalfWidth, ne.h2));
                            BorderPolyBottom.Points.Add(new DataPoint(6, ne.h2));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(-6, -1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                        case "Triangle":
                            Hydrodynamics_Sources.Conformal_Maps.Triangle nt = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Triangle;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(-nt.A/2.0, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(0, nt.h));
                            BorderPolyBottom.Points.Add(new DataPoint(nt.A / 2.0, 0));
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
                        case "Diffusor":
                            Hydrodynamics_Sources.Conformal_Maps.Diffusor db = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor;
                            if (db.angleDegrees == 90)
                            {
                                BorderBottom = new PolygonAnnotation();
                                BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyBottom.Points.Add(new DataPoint(-6,-db.h));
                                BorderPolyBottom.Points.Add(new DataPoint(0, -db.h));
                                BorderPolyBottom.Points.Add(new DataPoint(0, -5));
                                BorderPolyBottom.Points.Add(new DataPoint(-6, -5));
                                BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                BorderTop = new PolygonAnnotation();
                                BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyTop.Points.Add(new DataPoint(-6, db.h));
                                BorderPolyTop.Points.Add(new DataPoint(0, db.h));
                                BorderPolyTop.Points.Add(new DataPoint(0, 5));
                                BorderPolyTop.Points.Add(new DataPoint(-6, 5));
                                BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                PlotModel.Annotations.Add(BorderPolyBottom);
                                PlotModel.Annotations.Add(BorderPolyTop);
                            }
                            else
                            {
                                double angle = db.angleDegrees * Math.PI / 180;
                                BorderBottom = new PolygonAnnotation();
                                BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyBottom.Points.Add(new DataPoint(-6, -db.h));
                                BorderPolyBottom.Points.Add(new DataPoint(0, -db.h));
                                BorderPolyBottom.Points.Add(new DataPoint(20, -db.h - 20*Math.Tan(angle)));
                                BorderPolyBottom.Points.Add(new DataPoint(-6, -5));
                                BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                BorderTop = new PolygonAnnotation();
                                BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyTop.Points.Add(new DataPoint(-6, db.h));
                                BorderPolyTop.Points.Add(new DataPoint(0, db.h));
                                BorderPolyTop.Points.Add(new DataPoint(20, db.h + 20 * Math.Tan(angle)));
                                BorderPolyTop.Points.Add(new DataPoint(-6, 5));
                                BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                PlotModel.Annotations.Add(BorderPolyBottom);
                                PlotModel.Annotations.Add(BorderPolyTop);
                            }
                            break;
                    }
                    BorderBottom.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
                    BorderTop.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
                    break;
                case CanonicalDomain.Circular:
                    switch (s.W.f.ToString())
                    {
                        case "IdentityTransform":
                            BorderBottom = new EllipseAnnotation();
                            EllipseBorder.Fill = Settings.PlotVisualParams.BorderFillColor;
                            EllipseBorder.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            EllipseBorder.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            EllipseBorder.X = 0;
                            EllipseBorder.Y = 0;
                            EllipseBorder.Width = s.W.R * 2.0;
                            EllipseBorder.Height = s.W.R * 2.0;
                            PlotModel.Annotations.Add(EllipseBorder);
                            break;
                        case "Plate":
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(-s.W.R, PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(s.W.R, PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(s.W.R, -PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(-s.W.R, -PolygonLineHalfWidth));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                        case "JoukowskiAirfoil":
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            for (double theta = 0; theta < 2*Math.PI; theta += 0.001)
                            {
                                BorderPolyBottom.Points.Add(s.W.f.z(s.W.R * math.exp(complex.i * theta)).ComplexToDataPoint());
                            }
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                    }
                    BorderBottom.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
                    break;
            }
        }

        public void DrawStagnationPoints(complex r, complex l)
        {
            EllipseAnnotation rsp, lsp;
            rsp = new EllipseAnnotation() { X = r.Re, Y = r.Im, Width = 2*StagnationPointsRadius, Height = 2*StagnationPointsRadius, Stroke = OxyColors.Black, StrokeThickness=1, Fill = OxyColors.Red };
            lsp = new EllipseAnnotation() { X = l.Re, Y = l.Im, Width = 2 * StagnationPointsRadius, Height = 2 * StagnationPointsRadius, Stroke = OxyColors.Black, StrokeThickness = 1, Fill = OxyColors.Red };
            PlotModel.Annotations.Add(rsp);
            PlotModel.Annotations.Add(lsp);
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
