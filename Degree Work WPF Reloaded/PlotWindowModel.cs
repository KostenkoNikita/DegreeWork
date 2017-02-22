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
    enum CanonicalDomain { HalfPlane, Circular }

    class PlotWindowModel : INotifyPropertyChanged
    {
        private PlotModel p;
        private ArrowAnnotation arrow;
        public Annotation Border;
        PolygonAnnotation BorderPoly => Border as PolygonAnnotation;
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
                for (int i = 0; i < p.Series.Count - 1; i++)
                {
                    ls = p.Series[i] as LineSeries;
                    if (ls != null)
                    {
                        ls.Color = Settings.PlotVisualParams.LineColor;
                        ls.StrokeThickness = Settings.PlotVisualParams.LineStrokeThickness;
                    }
                    else
                    {
                        continue;
                    }
                }
                BorderPoly.Fill = Settings.PlotVisualParams.BorderFillColor;
                BorderPoly.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                BorderPoly.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                if (arrow != null)
                {
                    arrow.StrokeThickness = Settings.PlotVisualParams.ArrowStokeThickness;
                    arrow.Color = Settings.PlotVisualParams.ArrowColor;
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
            PlotModel.Annotations.Add(arrow);
            
        }

        void DeleteArrow()
        {
            if (arrow != null)
            {
                PlotModel.Annotations.Remove(arrow);
            }
        }

        public void RedrawArrow(double x_start, double y_start, double x_end, double y_end)
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
            }
        }

        public void RedrawArrow(DataPoint start, DataPoint end)
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
            }
        }

        public void RedrawArrow(complex start, complex end)
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

        public void DrawBorder(Hydrodynamics_Sources.Potential w)
        {
            switch (w.f.ToString())
            {
                case "Porebrick":
                    double h = (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).h;
                    Border = new PolygonAnnotation();
                    BorderPoly.Fill = Settings.PlotVisualParams.BorderFillColor;
                    BorderPoly.Points.Add(new DataPoint(-6, 0));
                    BorderPoly.Points.Add(new DataPoint(0, 0));
                    BorderPoly.Points.Add(new DataPoint(0, h));
                    BorderPoly.Points.Add(new DataPoint(6, h));
                    BorderPoly.Points.Add(new DataPoint(6, -1));
                    BorderPoly.Points.Add(new DataPoint(-6, -1));
                    BorderPoly.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                    BorderPoly.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                    PlotModel.Annotations.Add(BorderPoly);
                    break;
                case "IdentityTransform":
                    Border = new PolygonAnnotation();
                    BorderPoly.Fill = Settings.PlotVisualParams.BorderFillColor;
                    BorderPoly.Points.Add(new DataPoint(-6, 0));
                    BorderPoly.Points.Add(new DataPoint(6, 0));
                    BorderPoly.Points.Add(new DataPoint(6, -1));
                    BorderPoly.Points.Add(new DataPoint(-6, -1));
                    BorderPoly.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                    BorderPoly.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                    PlotModel.Annotations.Add(BorderPoly);
                    break;
            }
            Border.MouseDown += (sender, e) => { IsMouseClickedInPolygon = true; };
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
