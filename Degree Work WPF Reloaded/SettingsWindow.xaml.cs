using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using OxyPlot;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        //Hide close button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(System.IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(System.IntPtr hWnd, int nIndex, int dwNewLong);

        enum ColorTarget { Line, Vector, BorderFill, BorderStroke }
        enum ThicknessTarget { Line, Vector, BorderStroke }
        ColorTarget selectedColorTarget;
        ThicknessTarget selectedThicknessTarget;
        StreamLinesPlotGeomParams tmpGeom;
        StreamLinesPlotVisualParams tmpVisual;
        IStreamLinesPlotWindow w;
        byte R, G, B;

        public SettingsWindow(IStreamLinesPlotWindow _w)
        {
            tmpGeom = Settings.PlotGeomParams.Clone();
            tmpVisual = Settings.PlotVisualParams.Clone();
            //Hide close button
            Loaded += (sender, e) =>
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            };
            InitializeComponent();
            Deactivated += (sender, e) => { Close(); };
            FillRectangle();
            w = _w;
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
                case ThicknessTarget.Line: ThicknessSlider.Value = Settings.PlotVisualParams.LineStrokeThickness; break;
                case ThicknessTarget.Vector: ThicknessSlider.Value = Settings.PlotVisualParams.ArrowStokeThickness; break;
                case ThicknessTarget.BorderStroke: ThicknessSlider.Value = Settings.PlotVisualParams.BorderStrokeThickness; break;
            }
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

        private void FillRectangle()
        {
            ColorRect.Fill = new SolidColorBrush(Color.FromRgb(R,G,B));
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((sender as Slider).Name)
            {
                case "RSlider": R = (byte)RSlider.Value; break;
                case "GSlider": G = (byte)GSlider.Value; break;
                case "BSlider": B = (byte)BSlider.Value; break;
            }
            AssignColorValue();
            FillRectangle();
            w.OnPlotVisualParamsChanged();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (selectedThicknessTarget)
            {
                case ThicknessTarget.Line: Settings.PlotVisualParams.LineStrokeThickness = ThicknessSlider.Value; break;
                case ThicknessTarget.Vector: Settings.PlotVisualParams.ArrowStokeThickness = ThicknessSlider.Value; break;
                case ThicknessTarget.BorderStroke: Settings.PlotVisualParams.BorderStrokeThickness = ThicknessSlider.Value; break;
            }
            ThicknessOutputTextBlock.Text = ThicknessSlider.Value.ToString(Settings.Format);
            w.OnPlotVisualParamsChanged();
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

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "undoImage": undoImage.RenderTransform = new RotateTransform(-45); break;
                case "discardImage": discardContainer.Margin = new Thickness(4,4,4,4); break;
                case "okImage": okContainer.Margin = new Thickness(1, 1, 1, 1);  break;
            }
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "undoImage": undoImage.RenderTransform = new RotateTransform(0); break;
                case "discardImage": discardContainer.Margin = new Thickness(7,7,7,7); break;
                case "okImage": okContainer.Margin = new Thickness(4,4,4,4); break;
            }
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "undoImage":
                    Settings.PlotGeomParams = Settings.PlotGeomParamsConstant.Clone();
                    Settings.PlotVisualParams = Settings.PlotVisualParamsConstant.Clone();
                    SynchronizeSlidersValuesWithSettings();
                    break;
                case "discardImage":
                    Settings.PlotGeomParams = tmpGeom.Clone();
                    Settings.PlotVisualParams = tmpVisual.Clone();
                    SynchronizeSlidersValuesWithSettings();
                    break;
                case "okImage": Close(); return;
            }
        }

        private void SynchronizeSlidersValuesWithSettings()
        {
            targetList.SelectionChanged -= TargetList_SelectionChanged;
            targetThicknessList.SelectionChanged -= TargetThicknessList_SelectionChanged;
            targetList.SelectedIndex = 0;
            targetThicknessList.SelectedIndex = 0;

            RSlider.ValueChanged -= Slider_ValueChanged;
            GSlider.ValueChanged -= Slider_ValueChanged;
            BSlider.ValueChanged -= Slider_ValueChanged;
            RSlider.Value = R = Settings.PlotVisualParams.LineColor.R;
            GSlider.Value = G = Settings.PlotVisualParams.LineColor.G;
            BSlider.Value = B = Settings.PlotVisualParams.LineColor.B;
            FillRectangle();

            ThicknessSlider.ValueChanged -= ThicknessSlider_ValueChanged;
            ThicknessSlider.Value = Settings.PlotVisualParams.LineStrokeThickness;
            ThicknessOutputTextBlock.Text = ThicknessSlider.Value.ToString(Settings.Format);

            hVertSlider.ValueChanged -= PlotParamsSlider_ValueChanged;
            hHorSlider.ValueChanged -= PlotParamsSlider_ValueChanged;
            xMaxSlider.ValueChanged -= PlotParamsSlider_ValueChanged;
            xMinSlider.ValueChanged -= PlotParamsSlider_ValueChanged;
            yMaxSlider.ValueChanged -= PlotParamsSlider_ValueChanged;
            yMinSlider.ValueChanged -= PlotParamsSlider_ValueChanged;

            hVertSlider.Value = Settings.PlotGeomParams.hVertical;
            hHorSlider.Value = Settings.PlotGeomParams.MRKh;
            xMaxSlider.Value = Settings.PlotGeomParams.XMax;
            xMinSlider.Value = Settings.PlotGeomParams.XMin;
            yMaxSlider.Value = Settings.PlotGeomParams.YMax;

            hVertOutputTextBlock.Text = hVertSlider.Value.ToString(Settings.Format);
            hHorOutputTextBlock.Text = hHorSlider.Value.ToString(Settings.Format);
            xMaxOutputTextBlock.Text = xMaxSlider.Value.ToString(Settings.Format);
            xMinOutputTextBlock.Text = xMinSlider.Value.ToString(Settings.Format);
            yMaxOutputTextBlock.Text = yMaxSlider.Value.ToString(Settings.Format);
            yMinOutputTextBlock.Text = yMinSlider.Value.ToString(Settings.Format);

            targetList.SelectionChanged += TargetList_SelectionChanged;
            targetThicknessList.SelectionChanged += TargetThicknessList_SelectionChanged;
            RSlider.ValueChanged += Slider_ValueChanged;
            GSlider.ValueChanged += Slider_ValueChanged;
            BSlider.ValueChanged += Slider_ValueChanged;
            ThicknessSlider.ValueChanged += ThicknessSlider_ValueChanged;
            hVertSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            hHorSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            xMaxSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            xMinSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            yMaxSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            if (w is HalfPlane)
            {
                yMinSlider.Value = yMinSlider.Maximum = 0; yMinSlider.IsEnabled = false;
            }
            else
            {
                yMinSlider.Value = Settings.PlotGeomParams.YMin; yMinSlider.ValueChanged += PlotParamsSlider_ValueChanged;
            }
            w.OnPlotGeomParamsChanged();
            w.OnPlotVisualParamsChanged();

        }

    }

}
