#pragma warning disable

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
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

        double[,] intermediateMap;

        Thread LiebmannProcess;

        const double XMin = -5;

        const double XMax = 5;

        const double YMax = 5;

        const double YMin = -5;

        const double ArrowR = 8;

        const double ArrowCloserR = 4.242640687119286;

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

        public double RHeight { get; set; }

        public double RWidth { get; set; }

        public double AngleDegrees { get; set; }

        public double AngleRadians => AngleDegrees * Math.PI / 180.0;

        public double ArrowAngleRadians => AngleRadians + Math.PI;

        ArrowAnnotation arrow;

        public double Eps { get; set; }

        public HeatMapWindow()
        {
            InitializeComponent();
            AngleDegrees = 0;
            RHeight = 4;
            RWidth = 4;
            Eps = 0.001;
            arrow = new ArrowAnnotation();
            arrow.StartPoint = new DataPoint(ArrowR * Math.Cos(ArrowAngleRadians), ArrowR * Math.Sin(ArrowAngleRadians));
            arrow.EndPoint = new DataPoint(ArrowCloserR * Math.Cos(ArrowAngleRadians), ArrowCloserR * Math.Sin(ArrowAngleRadians));
            arrow.StrokeThickness = 4;
            arrow.Color = Colors.DarkRed;
            plot.Annotations.Add(arrow);
            LiebmannProcess = new Thread(GenerateHeatMap) { Priority = ThreadPriority.BelowNormal };
            LiebmannProcess.Start();
            rectangle.Points = new List<DataPoint>()
            {
                    new DataPoint(RWidth/2.0,RHeight/2.0),
                    new DataPoint(-RWidth/2.0,RHeight/2.0),
                    new DataPoint(-RWidth/2.0,-RHeight/2.0),
                    new DataPoint(RWidth/2.0,-RHeight/2.0)
            };
            SetGradientStopsForReload();
            DataContext = this;
            AngleSlider.ValueChanged += AngleSlider_ValueChanged;
            EpsSlider.ValueChanged += EpsSlider_ValueChanged;
            HeightSlider.ValueChanged += HWSlider_ValueChanged;
            WidthSlider.ValueChanged += HWSlider_ValueChanged;
            plot.Controller = new PlotController();
            plot.Controller.UnbindMouseDown(OxyMouseButton.Left);
        }

        private void AngleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LiebmannProcess.Abort();
            SetGradientStopsForReload();
            if (!plot.Annotations.Contains(arrow))
            {
                plot.Annotations.Add(arrow);
            }
            arrow.StartPoint = new DataPoint(ArrowR * Math.Cos(ArrowAngleRadians), ArrowR * Math.Sin(ArrowAngleRadians));
            arrow.EndPoint = new DataPoint(ArrowCloserR * Math.Cos(ArrowAngleRadians), ArrowCloserR * Math.Sin(ArrowAngleRadians));
            heatMapSeries.Data = new double[N, M];
            LiebmannProcess = new Thread(GenerateHeatMap) { Priority = ThreadPriority.BelowNormal };
            LiebmannProcess.Start();
        }

        private void EpsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LiebmannProcess.Abort();
            SetGradientStopsForReload();
            if (!plot.Annotations.Contains(arrow))
            {
                plot.Annotations.Add(arrow);
            }
            heatMapSeries.Data = new double[N, M];
            LiebmannProcess = new Thread(GenerateHeatMap) { Priority = ThreadPriority.BelowNormal };
            LiebmannProcess.Start();
        }

        private void HWSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LiebmannProcess.Abort();
            SetGradientStopsForReload();
            if (!plot.Annotations.Contains(arrow))
            {
                plot.Annotations.Add(arrow);
            }
            heatMapSeries.Data = new double[N, M];
            rectangle.Points = (new List<DataPoint>()
            {
                    new DataPoint(RWidth/2.0,RHeight/2.0),
                    new DataPoint(-RWidth/2.0,RHeight/2.0),
                    new DataPoint(-RWidth/2.0,-RHeight/2.0),
                    new DataPoint(RWidth/2.0,-RHeight/2.0)
            });
            LiebmannProcess = new Thread(GenerateHeatMap) { Priority = ThreadPriority.BelowNormal };
            LiebmannProcess.Start();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SetGradientStopsForBuild();
            while (LiebmannProcess.ThreadState == ThreadState.Running)
            {
            }
            plot.Annotations.Remove(arrow);
            heatMapSeries.Data = intermediateMap;
            Mouse.OverrideCursor = Cursors.Arrow;
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

        private void GenerateHeatMap()
        {
            double x;
            double y;
            Dispatcher.Invoke(() => { StartButtonImage.Source = Settings.ClockIcoSource; });
            intermediateMap = new double[N, M];
            double Vx = Math.Sin(AngleRadians);
            double Vy = Math.Cos(AngleRadians);
            int lowerI = iFromX(-RWidth / 2.0);
            int lowerJ = jFromY(-RHeight / 2.0);
            int higherI = iFromX(RWidth / 2.0);
            int higherJ = jFromY(RHeight / 2.0);
            x = XMin;
            for (int i = 0; i < N; i++, x += dx)
            {
                y = YMin;
                for (int j = 0; j < M; j++, y += dy)
                {
                    if (i >= lowerI && i <= higherI && j >= lowerJ && j <= higherJ)
                    {
                        continue;
                    }
                    intermediateMap[i, j] = Vy * y - Vx * x;
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
                        double temp = intermediateMap[i, j];
                        intermediateMap[i, j] = (intermediateMap[i + 1, j] + intermediateMap[i - 1, j] + intermediateMap[i, j + 1] + intermediateMap[i, j - 1]) / 4.0;
                        double dm = Math.Abs(temp - intermediateMap[i, j]);
                        if (dmax < dm)
                        {
                            dmax = dm;
                        }
                    }
                }
            } while (dmax > Eps);
            Dispatcher.Invoke(() => { StartButtonImage.Source = Settings.StartIcoSource; });
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
                case "StartButtonImage": StartButtonContainer.Margin = new Thickness(3,3,3,3); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSelectedSource; return;
            }
        }

        private void Ico_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": referContainer.Margin = new Thickness(7, 7, 7, 7); return;
                case "saveImage": saveContainer.Margin = new Thickness(9, 9, 9, 9); return;
                case "menuImage": menuContainer.Margin = new Thickness(13, 13, 13, 13); return;
                case "StartButtonImage": StartButtonContainer.Margin = new Thickness(7,7,7,7); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSource; return;
            }
        }

        private void Ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": WindowsReferences.RefW = new ReferenceWindow(this); WindowsReferences.RefW.Show(); return;
                case "saveImage": StartButton_Click(null, null); SaveWindow sw = new SaveWindow(new PlotWindowModel(plot,this)); sw.Show(); return;
                case "menuImage": WindowsReferences.MainW.Show(); Close(); return;
                case "StartButtonImage": StartButton_Click(null, null);return;
                case "exitImage": LiebmannProcess.Abort(); System.Diagnostics.Process.GetCurrentProcess().Kill(); return;
            }
        }

        private void StartButton_MouseEnter(object sender, MouseEventArgs e)
        {
            StartButtonContainer.Margin = new Thickness(3, 3, 3, 3);
        }

        private void StartButton_MouseLeave(object sender, MouseEventArgs e)
        {
            StartButtonContainer.Margin = new Thickness(7,7,7,7);
        }
    }
}
