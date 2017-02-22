using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        enum ColorTarget { Line, Vector, BorderFill, BorderStroke }
        enum ThicknessTarget { Line, Vector, BorderStroke }
        ColorTarget selectedColorTarget;
        ThicknessTarget selectedThicknessTarget;
        IStreamLinesPlotWindow w;
        delegate void VisualParamsChangedAsyncDelegate();
        VisualParamsChangedAsyncDelegate d;
        IAsyncResult res;

        byte R, G, B;
        double thickness;

        public SettingsWindow(IStreamLinesPlotWindow _w)
        {
            InitializeComponent();
            w = _w;
            d = new VisualParamsChangedAsyncDelegate(w.OnPlotVisualParamsChanged);
            targetList.SelectionChanged += TargetList_SelectionChanged;
            targetList.SelectedIndex = 0;
            ThicknessSlider.ValueChanged += ThicknessSlider_ValueChanged;
            targetThicknessList.SelectionChanged += TargetThicknessList_SelectionChanged;
            targetThicknessList.SelectedIndex = 0;

            hVertSlider.Value = Settings.PlotGeomParams.hVertical; hVertSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            hHorSlider.Value = Settings.PlotGeomParams.MRKh; hHorSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            xMaxSlider.Value = Settings.PlotGeomParams.XMax; xMaxSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            xMinSlider.Value = Settings.PlotGeomParams.XMin; xMinSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            yMaxSlider.Value = Settings.PlotGeomParams.YMax; yMaxSlider.ValueChanged += PlotParamsSlider_ValueChanged;

            if (w is HalfPlane)
            {
                yMinSlider.Value = yMinSlider.Maximum = 0; yMinSlider.IsEnabled = false;
            }
            else
            {
                yMinSlider.Value = Settings.PlotGeomParams.YMin; yMinSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            }

            hVertOutputTextBlock.Text = hVertSlider.Value.ToString(Settings.Format);
            hHorOutputTextBlock.Text = hHorSlider.Value.ToString(Settings.Format);
            xMaxOutputTextBlock.Text = xMaxSlider.Value.ToString(Settings.Format);
            xMinOutputTextBlock.Text = xMinSlider.Value.ToString(Settings.Format);
            yMaxOutputTextBlock.Text = yMaxSlider.Value.ToString(Settings.Format);
            yMinOutputTextBlock.Text = yMinSlider.Value.ToString(Settings.Format);            
        }

        private void TargetThicknessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedThicknessTarget = (ThicknessTarget)targetThicknessList.SelectedIndex;
            switch (selectedThicknessTarget)
            {
                case ThicknessTarget.Line: thickness = Settings.PlotVisualParams.LineStrokeThickness; break;
                case ThicknessTarget.Vector: thickness = Settings.PlotVisualParams.ArrowStokeThickness; break;
                case ThicknessTarget.BorderStroke: thickness = Settings.PlotVisualParams.BorderStrokeThickness; break;
            }
            SetThicknessSliderValue();
            RSlider.Focus();
        }

        private void TargetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColorTarget = (ColorTarget)targetList.SelectedIndex;
            switch (selectedColorTarget)
            {
                case ColorTarget.Line: R = Settings.PlotVisualParams.LineColor.R; G = Settings.PlotVisualParams.LineColor.G; B = Settings.PlotVisualParams.LineColor.B; break;
                case ColorTarget.Vector: R = Settings.PlotVisualParams.ArrowColor.R; G = Settings.PlotVisualParams.ArrowColor.G; B = Settings.PlotVisualParams.ArrowColor.B; break;
                case ColorTarget.BorderFill: R = Settings.PlotVisualParams.BorderFillColor.R; G = Settings.PlotVisualParams.BorderFillColor.G; B = Settings.PlotVisualParams.BorderFillColor.B; break;
                case ColorTarget.BorderStroke: R = Settings.PlotVisualParams.BorderStrokeColor.R; G = Settings.PlotVisualParams.BorderStrokeColor.G; B = Settings.PlotVisualParams.BorderStrokeColor.B; break;
            }
            SetColorSliderValue();
            RSlider.Focus();
        }

        private void SetColorSliderValue()
        {
            RSlider.Value = R;
            GSlider.Value = G;
            BSlider.Value = B;
        }

        private void SetThicknessSliderValue()
        {
            ThicknessSlider.Value = thickness;
        }

        private void FillRectangle()
        {
            ColorRect.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
        }

        private void AssignColorValue()
        {
            switch (selectedColorTarget)
            {
                case ColorTarget.Line: Settings.PlotVisualParams.LineColor = OxyColor.FromRgb(R,G,B); break;
                case ColorTarget.Vector: Settings.PlotVisualParams.ArrowColor = OxyColor.FromRgb(R, G, B); break;
                case ColorTarget.BorderFill: Settings.PlotVisualParams.BorderFillColor = OxyColor.FromRgb(R, G, B); break;
                case ColorTarget.BorderStroke: Settings.PlotVisualParams.BorderStrokeColor = OxyColor.FromRgb(R, G, B); break;
            }
        }

        private void AssignThicknessValue()
        {
            switch (selectedThicknessTarget)
            {
                case ThicknessTarget.Line: Settings.PlotVisualParams.LineStrokeThickness = thickness; break;
                case ThicknessTarget.Vector: Settings.PlotVisualParams.ArrowStokeThickness  = thickness; break;
                case ThicknessTarget.BorderStroke: Settings.PlotVisualParams.BorderStrokeThickness = thickness; break;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((sender as Slider).Name)
            {
                case "RSlider": R = (byte)RSlider.Value; break;
                case "GSlider": G = (byte)GSlider.Value; break;
                case "BSlider": B = (byte)BSlider.Value; break;
            }
            FillRectangle();
            AssignColorValue();
            res = d.BeginInvoke(null, null);
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            thickness = ThicknessSlider.Value;
            ThicknessOutputTextBlock.Text = thickness.ToString(Settings.Format);
            AssignThicknessValue();
            res = d.BeginInvoke(null, null);
        }

        private void PlotParamsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.PlotGeomParams.hVertical = hVertSlider.Value; hVertOutputTextBlock.Text = hVertSlider.Value.ToString(Settings.Format);
            Settings.PlotGeomParams.MRKh = hHorSlider.Value; hHorOutputTextBlock.Text = hHorSlider.Value.ToString(Settings.Format);
            Settings.PlotGeomParams.XMax = xMaxSlider.Value; xMaxOutputTextBlock.Text = xMaxSlider.Value.ToString(Settings.Format);
            Settings.PlotGeomParams.XMin = xMinSlider.Value; xMinOutputTextBlock.Text = xMinSlider.Value.ToString(Settings.Format);
            Settings.PlotGeomParams.YMax = yMaxSlider.Value; yMaxOutputTextBlock.Text = yMaxSlider.Value.ToString(Settings.Format);
            Settings.PlotGeomParams.YMin = yMinSlider.Value; yMinOutputTextBlock.Text = yMinSlider.Value.ToString(Settings.Format);
            w.OnPlotGeomParamsChanged();
        }







    }
}
