#pragma warning disable
//#define HELP_FOR_GROUP_LEADER

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using Degree_Work.Mathematical_Sources.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Functions.SpecialFunctions;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace Degree_Work
{
    /// <summary>
    /// Перечисление, что задаёт вид канонической области: обтекание полуплоскости, полосы или окружности
    /// </summary>
    enum CanonicalDomain { HalfPlane, Zone, Circular }

    class PlotWindowModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Константа, которая задаёт половину ширины линии при её рисовании для таких конформных отображений, как отображение полуплоскости на полуплоскость с выброшенным отрезком
        /// </summary>
        private const double PolygonLineHalfWidth = 0.02;

        /// <summary>
        /// Константа, которая задаёт радиус точки торможения при её рисовании
        /// </summary>
        private const double StagnationPointsRadius = 0.05;

        private PlotModel p;

        /// <summary>
        /// Вектор скорости, что появляется при нажатии на исследуемую область
        /// </summary>
        private ArrowAnnotation arrow;

        /// <summary>
        /// Текст, что появляется вместе с вектором скорости, несущий в себе информацию о месте, в котором произведено нажатие
        /// </summary>
        private TextAnnotation arrowText;

        /// <summary>
        /// Нижняя граница
        /// </summary>
        public Annotation BorderBottom;

        /// <summary>
        /// Безопасное приведение типов для нижней границы в случае полуплоскости или полосы
        /// </summary>
        PolygonAnnotation BorderPolyBottom => BorderBottom as PolygonAnnotation;

        /// <summary>
        /// Безопасное приведение типов для нижней границы в случае окружности
        /// </summary>
        EllipseAnnotation EllipseBorder => BorderBottom as EllipseAnnotation;

        /// <summary>
        /// Верхняя граница. будет присутствовать только в том случае, если
        /// вспомогательная плоскость имеет вид полосы. В остальных случаях
        /// роль границы выполняет BorderBottom
        /// </summary>
        public Annotation BorderTop;

        /// <summary>
        /// Безопасное приведение типов для верхней границы
        /// </summary>
        PolygonAnnotation BorderPolyTop => BorderTop as PolygonAnnotation;

        /// <summary>
        /// Нажата ли левая кнопка мыши на границе области
        /// </summary>
        internal bool IsMouseClickedInPolygon;

        object locker = new object();

        public PlotModel PlotModel
        {
            get { return p; }
            set { p = value; OnPropertyChanged("PlotModel"); }
        }

        /// <summary>
        /// Горизонтальная ось
        /// </summary>
        private Axis X_Axis
        {
            get { return PlotModel.Axes[0] as Axis; }
        }

        /// <summary>
        /// Вертикальная ось
        /// </summary>
        private Axis Y_Axis
        {
            get { return PlotModel.Axes[1] as Axis; }
        }

        /// <summary>
        /// Получение координат точки на экране в виде объекта DataPoint
        /// </summary>
        /// <param name="pos">Точка на экране</param>
        /// <returns></returns>
        public DataPoint GetDataPointCursorPositionOnPlot(ScreenPoint pos)
        {
            return OxyPlot.Axes.Axis.InverseTransform(pos, X_Axis, Y_Axis);
        }

        /// <summary>
        /// Получение координат точки на экране в виде комплексного числа 
        /// </summary>
        /// <param name="pos">Точка на экране</param>
        /// <returns></returns>
        public Complex GetComplexCursorPositionOnPlot(ScreenPoint pos)
        {
            return (Degree_Work.Mathematical_Sources.Complex.Complex)OxyPlot.Axes.Axis.InverseTransform(pos, X_Axis, Y_Axis);
        }

        /// <summary>
        /// Конструктор класса. Все графические пострения проходят через методы экземпляра этого класса
        /// ВНИМАНИЕ. ВЫЗЫВАЕТСЯ ТОЛЬКО ИЗ ОКНА ДЛЯ ПОСТРОЕНИЯ ЛИНИЙ ТОКА ПРИ ОБТЕКАНИИ ПРЯМОУГОЛЬНИКА
        /// </summary>
        internal PlotWindowModel(OxyPlot.Wpf.Plot plot, System.Windows.Window callerWindow)
        {
            if (callerWindow is HeatMapWindow)
            {
                PlotModel = plot.ActualModel;
            }
            else
            {
                throw new InvalidOperationException("ВЫЗЫВАЕТСЯ ТОЛЬКО ИЗ ОКНА ДЛЯ ПОСТРОЕНИЯ ЛИНИЙ ТОКА ПРИ ОБТЕКАНИИ ПРЯМОУГОЛЬНИКА");
            }
        }

        /// <summary>
        /// Конструктор класса. Все графические пострения проходят через методы экземпляра этого класса
        /// </summary>
        /// <param name="domain">Вид канонической области</param>
        internal PlotWindowModel(CanonicalDomain domain)
        {
            PlotModel = new PlotModel();
            SetUpModel(domain);
        }

        /// <summary>
        /// Настройка модели. Тут происходит настройка координатных осей.
        /// </summary>
        /// <param name="domain">Вид канонической области</param>
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
                    XAxis = new LinearAxis(AxisPosition.Bottom, -3, 5, "X") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "X", AbsoluteMaximum = 5, AbsoluteMinimum = -5, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(XAxis);
                    YAxis = new LinearAxis(AxisPosition.Left, -0.1, 1, "Y") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Y", AbsoluteMaximum = 1, AbsoluteMinimum = -0.1, Font = "Times New Roman", FontSize = 15 };
                    PlotModel.Axes.Add(YAxis);
                    break;
#endif
            }
        }

        /// <summary>
        /// Событие интерфейса INotifyPropertyChanged (в данной реализации не имеет подписчиков за ненадобностью)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Безопасная в отношении потоков отрисовка кривой
        /// </summary>
        /// <param name="l">Список точек DataPoint, для которых рисуется кривая</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void DrawCurve(List<DataPoint> l)
        {
            lock (locker)
            {
                LineSeries ls = new LineSeries();
                ls.Smooth = true;
                ls.Color = Settings.PlotVisualParams.LineColor;
#if !HELP_FOR_GROUP_LEADER
                ls.StrokeThickness = Settings.PlotVisualParams.LineStrokeThickness;
#elif HELP_FOR_GROUP_LEADER
                ls.StrokeThickness = 1;
#endif
                foreach (DataPoint p in l) { ls.Points.Add(p); }
                PlotModel.Series.Add(ls);
            }
        }

        /// <summary>
        /// Безопасная в отношении потоков отрисовка кривой
        /// </summary>
        /// <param name="l">Список точек DataPoint, для которых рисуется кривая</param>
        /// <param name="thickness">Толщина линии</param>
        /// <param name="color">Цвет линии</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void DrawCurve(List<DataPoint> l, double thickness, OxyColor color)
        {
            lock (locker)
            {
                LineSeries ls = new LineSeries();
                ls.Smooth = true;
                ls.Color = color;
                ls.StrokeThickness = thickness;
                foreach (DataPoint p in l) { ls.Points.Add(p); }
                PlotModel.Series.Add(ls);
            }
        }

        /// <summary>
        /// Сброс визуальных параметров графических элементов
        /// </summary>
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

        /// <summary>
        /// Создание вектора скорости и сопутствующего текста
        /// </summary>
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

        /// <summary>
        /// Удаление вектора скорости и сопутсвующего текста, если таковые присутствуют
        /// </summary>
        internal void DeleteArrow()
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

        /// <summary>
        /// Перемещение созданного вектора (и его создание, если таковой отсутствует)
        /// </summary>
        /// <param name="start">Начало вектора</param>
        /// <param name="end">Конец вектора</param>
        /// <param name="V">Значение скорости</param>
        /// <param name="domain">Вид канонической области</param>
        public void RedrawArrow(Complex start, Complex end, Complex V, CanonicalDomain domain)
        {
            if (IsMouseClickedInPolygon || Complex.IsNaN(V))
            {
                DeleteArrow();
                IsMouseClickedInPolygon = false;
            }
            else
            {
                if (!HasArrow()) { CreateArrow(); }
                arrow.StartPoint = start;
                arrow.EndPoint = end;
                arrowText.Text = $"X: {start.Re.ToString(Settings.Format)}; Y: {start.Im.ToString(Settings.Format)};".Replace(',', '.') +
                    $"\nVx: {V.Re.ToString(Settings.Format)}; Vy: {V.Im.ToString(Settings.Format)};".Replace(',', '.');
                arrowText.TextPosition = (start + (V.Im>=0? -1: 0.2)*(domain == CanonicalDomain.HalfPlane ? 0.6 : 1.2) * Complex.I);
            }
        }

        #region Костыль

        /// <summary>
        /// Перемещение созданного вектора (и его создание, если таковой отсутствует) для области окружности
        /// </summary>
        /// <param name="start">Начало вектора</param>
        /// <param name="end">Конец вектора</param>
        /// <param name="V">Значение скорости</param>
        /// <param name="domain">Вид канонической области</param>
        /// <param name="w">Комплексный потенциал</param>
        public void RedrawArrow(Complex start, Complex end, Complex V, CanonicalDomain domain, Hydrodynamics_Sources.Potential w)
        {
            if (IsMouseClickedInPolygon || Complex.IsNaN(V))
            {
                if (start.Abs<w.R || Complex.IsNaN(V))
                {
                    DeleteArrow();
                    IsMouseClickedInPolygon = false;
                }
                else
                {
                    if (!HasArrow()) { CreateArrow(); }
                    arrow.StartPoint = start;
                    arrow.EndPoint = end;
                    arrowText.Text = $"X: {start.Re.ToString(Settings.Format)}; Y: {start.Im.ToString(Settings.Format)};".Replace(',', '.') +
                        $"\nVx: {V.Re.ToString(Settings.Format)}; Vy: {V.Im.ToString(Settings.Format)};".Replace(',', '.');
                    arrowText.TextPosition = (start + (V.Im >= 0 ? -1 : 0.2) * 1.2 * Complex.I);
                }
            }
            else
            {
                if (!HasArrow()) { CreateArrow(); }
                arrow.StartPoint = start;
                arrow.EndPoint = end;
                arrowText.Text = $"X: {start.Re.ToString(Settings.Format)}; Y: {start.Im.ToString(Settings.Format)};".Replace(',', '.') +
                    $"\nVx: {V.Re.ToString(Settings.Format)}; Vy: {V.Im.ToString(Settings.Format)};".Replace(',', '.');
                arrowText.TextPosition = (start + (V.Im >= 0 ? -1 : 0.2) * 1.2 * Complex.I);
            }
        }

        #endregion

        /// <summary>
        /// Сохранение результирующего графика в формат JPG
        /// </summary>
        /// <param name="path">Путь к результирующему файлу</param>
        /// <param name="width">Ширина рисунка</param>
        /// <param name="height">Высота рисунка</param>
        public void SavePlotToJPG(string path, int width, int height)
        {
            var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
            var bitmap = pngExporter.ExportToBitmap(PlotModel);
            bitmap.SaveJPG100(path);
        }

        /// <summary>
        /// Сохранение результирующего графика в формат PNG
        /// </summary>
        /// <param name="path">Путь к результирующему файлу</param>
        /// <param name="width">Ширина рисунка</param>
        /// <param name="height">Высота рисунка</param>
        public void SavePlotToPNG(string path, int width, int height)
        {
            using (FileStream s = new FileStream(path, FileMode.Create))
            {
                var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
                pngExporter.Export(PlotModel, s);
            }
        }

        /// <summary>
        /// Сохранение результирующего графика в формат BMP
        /// </summary>
        /// <param name="path">Путь к результирующему файлу</param>
        /// <param name="width">Ширина рисунка</param>
        /// <param name="height">Высота рисунка</param>
        public void SavePlotToBMP(string path, int width, int height)
        {
            var pngExporter = new OxyPlot.Wpf.PngExporter { Width = width, Height = height, Background = OxyColors.White };
            var bitmap = pngExporter.ExportToBitmap(PlotModel);
            bitmap.GetBitmap().Save(path);
        }

        /// <summary>
        /// Имеется ли на графике вектор скорости
        /// </summary>
        /// <returns></returns>
        bool HasArrow()
        {
            foreach (Annotation a in PlotModel.Annotations)
            {
                if (a is ArrowAnnotation) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Метод, выполняющий рисование границы области
        /// </summary>
        /// <param name="s">Ссылка на объект, производный от абстрактного класса StreamLinesBuilder</param>
        public void DrawBorder(Hydrodynamics_Sources.StreamLinesBuilder s)
        {
            switch (s.Domain)
            {
                case CanonicalDomain.HalfPlane:
                    switch (s.W.f.ToString())
                    {
                        case "Porebrick":
                            double h = (s.W.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).H;
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
                        case "Number85":
                            Hydrodynamics_Sources.Conformal_Maps.Number85 nc = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Number85;
                            BorderBottom = new PolygonAnnotation();
                            BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyBottom.Points.Add(new DataPoint(6, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(-6+ PolygonLineHalfWidth, 0));
                            BorderPolyBottom.Points.Add(new DataPoint(-6+ PolygonLineHalfWidth, nc.H- PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(0, nc.H - PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(0, nc.H + PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(-6 - PolygonLineHalfWidth, nc.H+ PolygonLineHalfWidth));
                            BorderPolyBottom.Points.Add(new DataPoint(-6 - PolygonLineHalfWidth, -1));
                            BorderPolyBottom.Points.Add(new DataPoint(6, -1));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            break;
                    }
                    BorderBottom.MouseDown += (sender, e) =>
                    {
                        IsMouseClickedInPolygon = true;
                    };
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
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.l * Cos(tmp.Angle)-PolygonLineHalfWidth, tmp.l * Sin(tmp.Angle)));
                            BorderPolyBottom.Points.Add(new DataPoint(tmp.l * Cos(tmp.Angle)+ PolygonLineHalfWidth, tmp.l * Sin(tmp.Angle)));
                            BorderPolyBottom.Points.Add(new DataPoint(100 * tmp.l * Cos(tmp.Angle)+ PolygonLineHalfWidth, 100 * tmp.l * Sin(tmp.Angle)));
                            BorderPolyBottom.Points.Add(new DataPoint(100 * tmp.l * Cos(tmp.Angle)- PolygonLineHalfWidth, 100 * tmp.l * Sin(tmp.Angle)));
                            BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            BorderTop = new PolygonAnnotation();
                            BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                            BorderPolyTop.Points.Add(new DataPoint(tmp.l * Cos(tmp.Angle)- PolygonLineHalfWidth, -tmp.l * Sin(tmp.Angle)));
                            BorderPolyTop.Points.Add(new DataPoint(tmp.l * Cos(tmp.Angle)+ PolygonLineHalfWidth, -tmp.l * Sin(tmp.Angle)));
                            BorderPolyTop.Points.Add(new DataPoint(100 * tmp.l * Cos(tmp.Angle)+ PolygonLineHalfWidth, -100 * tmp.l * Sin(tmp.Angle)));
                            BorderPolyTop.Points.Add(new DataPoint(100 * tmp.l * Cos(tmp.Angle)- PolygonLineHalfWidth, -100 * tmp.l * Sin(tmp.Angle)));
                            BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                            BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                            PlotModel.Annotations.Add(BorderPolyBottom);
                            PlotModel.Annotations.Add(BorderPolyTop);
                            break;
                        case "Diffusor":
                            Hydrodynamics_Sources.Conformal_Maps.Diffusor db = s.W.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor;
                            if (db.AngleDegrees == 90)
                            {
                                BorderBottom = new PolygonAnnotation();
                                BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyBottom.Points.Add(new DataPoint(-6,-db.H));
                                BorderPolyBottom.Points.Add(new DataPoint(0, -db.H));
                                BorderPolyBottom.Points.Add(new DataPoint(0, -5));
                                BorderPolyBottom.Points.Add(new DataPoint(-6, -5));
                                BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                BorderTop = new PolygonAnnotation();
                                BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyTop.Points.Add(new DataPoint(-6, db.H));
                                BorderPolyTop.Points.Add(new DataPoint(0, db.H));
                                BorderPolyTop.Points.Add(new DataPoint(0, 5));
                                BorderPolyTop.Points.Add(new DataPoint(-6, 5));
                                BorderPolyTop.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyTop.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                PlotModel.Annotations.Add(BorderPolyBottom);
                                PlotModel.Annotations.Add(BorderPolyTop);
                            }
                            else
                            {
                                double angle = db.AngleDegrees * Math.PI / 180;
                                BorderBottom = new PolygonAnnotation();
                                BorderPolyBottom.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyBottom.Points.Add(new DataPoint(-6, -db.H));
                                BorderPolyBottom.Points.Add(new DataPoint(0, -db.H));
                                BorderPolyBottom.Points.Add(new DataPoint(20, -db.H - 20* Math.Tan(angle)));
                                BorderPolyBottom.Points.Add(new DataPoint(-6, -5));
                                BorderPolyBottom.StrokeThickness = Settings.PlotVisualParams.BorderStrokeThickness;
                                BorderPolyBottom.Stroke = Settings.PlotVisualParams.BorderStrokeColor;
                                BorderTop = new PolygonAnnotation();
                                BorderPolyTop.Fill = Settings.PlotVisualParams.BorderFillColor;
                                BorderPolyTop.Points.Add(new DataPoint(-6, db.H));
                                BorderPolyTop.Points.Add(new DataPoint(0, db.H));
                                BorderPolyTop.Points.Add(new DataPoint(20, db.H + 20 * Math.Tan(angle)));
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
                                BorderPolyBottom.Points.Add(s.W.f.z(s.W.R * Exp(Complex.I * theta)));
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

        /// <summary>
        /// Метод, выполняющий рисование точек торможения (если каноническая область является окружностью)
        /// </summary>
        /// <param name="r">Правая точка торможения</param>
        /// <param name="l">Левая точка торможения</param>
        public void DrawStagnationPoints(Complex r, Complex l)
        {
            EllipseAnnotation rsp, lsp;
            rsp = new EllipseAnnotation() { X = r.Re, Y = r.Im, Width = 2*StagnationPointsRadius, Height = 2*StagnationPointsRadius, Stroke = OxyColors.Black, StrokeThickness=1, Fill = OxyColors.Red };
            lsp = new EllipseAnnotation() { X = l.Re, Y = l.Im, Width = 2 * StagnationPointsRadius, Height = 2 * StagnationPointsRadius, Stroke = OxyColors.Black, StrokeThickness = 1, Fill = OxyColors.Red };
            PlotModel.Annotations.Add(rsp);
            PlotModel.Annotations.Add(lsp);
        }

        /// <summary>
        /// Очистка графической поверхности
        /// </summary>
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
