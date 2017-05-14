#pragma warning disable

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Wpf;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для HeatMapWindow.xaml
    /// </summary>
    public partial class HeatMapWindow : Window
    {
        public double[,] Data { get; private set; }

        public double AngleDegrees { get; set; }

        public string AngleToString { get { return AngleDegrees.ToString(System.Globalization.CultureInfo.InvariantCulture); } private set { } }

        const double XMin = -5;

        const double XMax = 5;

        const double YMax = 5;

        const double YMin = -5;

        /// <summary>
        /// Количество узлов по координате Х
        /// </summary>
        const int N = 301;

        /// <summary>
        /// Количество узлов по координате У
        /// </summary>
        const int M = 301;

        const double dx = (XMax - XMin) / (N - 1);

        const double dy = (YMax - YMin) / (M - 1);

        DataPoint[] rectPoints;

        public double Height { get; set; }

        public double Width { get; set; }

        public HeatMapWindow()
        {
            InitializeComponent();
            Height = 4;
            Width = 4;
            SetGradientStopsForReload();
            FindRectangleDataPoints();
            DrawReactangle();
            AngleSlider.ValueChanged += AngleSlider_ValueChanged;
            DataContext = this;
        }

        private void AngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetGradientStopsForReload();
            heatMapSeries.Data = new double[N, M];
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SetGradientStopsForBuild();
            heatMapSeries.Data = GenerateHeatMap(AngleDegrees*Math.PI/180.0, 0.001);
        }

        private void SetGradientStopsForBuild()
        {
            linearColorAxis.GradientStops.Clear();
            linearColorAxis.GradientStops.Add(new GradientStop(Colors.Navy, 0.5));
            linearColorAxis.GradientStops.Add(new GradientStop(Colors.LightCyan, 0.0));
            linearColorAxis.GradientStops.Add(new GradientStop(Colors.LightCyan, 1.0));
        }

        private void SetGradientStopsForReload()
        {
            linearColorAxis.GradientStops.Clear();
            linearColorAxis.GradientStops.Add(new GradientStop(Colors.White, 0.5));
            linearColorAxis.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            linearColorAxis.GradientStops.Add(new GradientStop(Colors.White, 1.0));
        }

        private void FindRectangleDataPoints()
        {
            rectPoints = new DataPoint[4]
                {
                    new DataPoint(Width/2.0,Height/2.0),
                    new DataPoint(-Width/2.0,Height/2.0),
                    new DataPoint(-Width/2.0,-Height/2.0),
                    new DataPoint(Width/2.0,-Height/2.0)
                };
        }

        private void DrawReactangle()
        {
            if (rectPoints == null) { FindRectangleDataPoints(); }
            plot.Annotations.Clear();
            PolygonAnnotation pa = new PolygonAnnotation();
            pa.Points.Add(rectPoints[0]);
            pa.Points.Add(rectPoints[1]);
            pa.Points.Add(rectPoints[2]);
            pa.Points.Add(rectPoints[3]);
            pa.Fill = Colors.Gray;
            pa.StrokeThickness = 1;
            pa.Stroke = Colors.Black;
            plot.Annotations.Add(pa);
        }

        private double[,] GenerateHeatMap(double angleRadians, double eps)
        {
            double x;
            double y;
            double[,] res = new double[N, M];
            double Vx = Math.Sin(angleRadians);
            double Vy = Math.Cos(angleRadians);
            int lowerI = iFromX(-Width/2.0);
            int lowerJ = jFromY(-Height/2.0);
            int higherI = iFromX(Width / 2.0);
            int higherJ = jFromY(Height / 2.0);
            x = XMin;
            for (int i = 0; i < N; i++, x += dx)
            {
                y = YMin;
                for (int j = 0; j < M; j++, y += dy)
                {
                    if (i >= lowerI && i <= higherI && j >= lowerJ && j <= higherJ)
                    {
                        res[i, j] = 0;
                        continue;
                    }
                    res[i, j] = Vy * y - Vx * x;
                }
            }
            double dmax = 0;
            do
            {
                dmax = 0;
                for (int i = 1; i < N - 1; i++)
                {
                    for (int j = 1; j < M - 1; j++)
                    {
                        if (i >= lowerI && i <= higherI && j >= lowerJ && j <= higherJ)
                        {
                            continue;
                        }
                        double temp = res[i, j];
                        res[i, j] = (res[i + 1, j] + res[i - 1, j] + res[i, j + 1] + res[i, j - 1]) / 4.0;
                        double dm = Math.Abs(temp - res[i, j]);
                        if (dmax < dm)
                        {
                            dmax = dm;
                        }
                    }
                }
            } while (dmax > eps);
            return res;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int iFromX(double x)
        {
            return Convert.ToInt32((x - XMin) / dx);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private int jFromY(double y)
        {
            return Convert.ToInt32((y - YMin) / dy);
        }

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": referContainer.Margin = new Thickness(3, 3, 3, 3); return;
                case "saveImage": saveContainer.Margin = new Thickness(4, 4, 4, 4); return;
                case "menuImage": menuContainer.Margin = new Thickness(7, 7, 7, 7); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSelectedSource; return;
            }
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": referContainer.Margin = new Thickness(7, 7, 7, 7); return;
                case "saveImage": saveContainer.Margin = new Thickness(9, 9, 9, 9); return;
                case "menuImage": menuContainer.Margin = new Thickness(13, 13, 13, 13); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSource; return;
            }
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": WindowsReferences.RefW = new ReferenceWindow(this); WindowsReferences.RefW.Show(); return;
                case "saveImage": /*SaveWindow sw = new SaveWindow(viewModel); sw.Show();*/ return;
                case "menuImage": WindowsReferences.MainW.Show(); Close(); return;
                case "exitImage": System.Diagnostics.Process.GetCurrentProcess().Kill(); return;
            }
        }
    }
}
