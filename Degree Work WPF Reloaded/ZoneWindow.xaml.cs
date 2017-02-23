﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Input;
using OxyPlot;
using MathCore_2_0;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для ZoneWindow.xaml
    /// </summary>
    public partial class ZoneWindow : Window, IStreamLinesPlotWindow
    {
        complex CursorPosition, V;
        private PlotWindowModel viewModel;
        Hydrodynamics_Sources.Potential w;
        Hydrodynamics_Sources.StreamLinesBuilderHalfPlaneAndZone s;
        private string TemporaryString(int num)
        {
            switch (num)
            {
                case 1: return paramBox1.Text.Trim(' ').Replace('.', ',');
                case 2: return paramBox2.Text.Trim(' ').Replace('.', ',');
                default: return null;
            }
        }
        public ZoneWindow()
        {
            viewModel = new PlotWindowModel(CanonicalDomain.Zone);
            DataContext = viewModel;
            InitializeComponent();
            Settings.PlotGeomParams.hVertical = 2 * Math.PI / 16.0;
            Settings.PlotGeomParamsConstant.hVertical = Settings.PlotGeomParams.hVertical;
            w = new Hydrodynamics_Sources.Potential(1, 0, 0, 0, new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform());
            s = new Hydrodynamics_Sources.StreamLinesBuilderHalfPlaneAndZone(w, viewModel, CanonicalDomain.Zone);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное\nотображение");
            mapsList.Items.Add("Плоскость с двумя\nотброшенными лучами");
            mapsList.SelectedIndex = 0;
            viewModel.PlotModel.MouseMove += PlotModel_MouseMove;
            viewModel.PlotModel.MouseDown += PlotModel_MouseDown;
            plot.Controller = new PlotController();
            plot.Controller.UnbindMouseDown(OxyMouseButton.Left);
        }

        private void PlotModel_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton.ToString() == "Left")
            {
                viewModel.RedrawArrow(CursorPosition, CursorPosition + 2 * V / V.abs, V, CanonicalDomain.Zone);
                PlotRefresh();
            }
        }

        private void PlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            CursorPosition = viewModel.GetComplexCursorPositionOnPlot(e.Position);
            V = w.V_physical_plane(CursorPosition);
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.EjectedRays && CursorPosition.Re < 0)
            {
                V = -V;
            }
            if (complex.IsNaN(V) || IsCursorInBorder())
            {
                ClearTextBoxes();
                return;
            }
            else
            {
                xTextBox.Text = CursorPosition.Re.ToString(Settings.Format);
                yTextBox.Text = CursorPosition.Im.ToString(Settings.Format);
                VxTextBox.Text = V.Re.ToString(Settings.Format);
                VyTextBox.Text = V.Im.ToString(Settings.Format);
            }
        }

        private void MapsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (mapsList.SelectedIndex)
            {
                case 0: w.f = new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform(); break;
                case 1:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.EjectedRays(1, 0.5);
                    break;
            }
            ChangeParamsConfiguration();
            s.Rebuild();
            PlotRefresh();
        }

        private void ChangeParamsConfiguration()
        {
            paramBox1.TextChanged -= paramBox1_TextChanged;
            paramBox2.TextChanged -= paramBox2_TextChanged;
            angleSlider.ValueChanged -= angleSlider_ValueChanged;
            switch (mapsList.SelectedIndex)
            {
                case 0:
                    paramBox1.Text = String.Empty;
                    paramBox1.Visibility = Visibility.Hidden;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Hidden;
                    param2.Visibility = Visibility.Hidden;
                    angleSlider.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = "90";
                    paramBox2.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param2.Visibility = Visibility.Visible;
                    angleSlider.Visibility = Visibility.Visible;
                    angleSlider.Minimum = 0;
                    angleSlider.Maximum = 180;
                    angleSlider.Value = 90;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    paramBox2.TextChanged += paramBox2_TextChanged;
                    angleSlider.ValueChanged += angleSlider_ValueChanged;
                    break;
            }
        }

        private void PlotRefresh()
        {
            plot.InvalidatePlot(true);
        }

        private void paramBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.EjectedRays)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0 && tmp<180) { (w.f as Hydrodynamics_Sources.Conformal_Maps.EjectedRays).l = tmp; s.Rebuild(); PlotRefresh(); }
                    else { throw new FormatException(); }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void paramBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.EjectedRays)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(2));
                    if (tmp >= 0 && tmp <= 180) { (w.f as Hydrodynamics_Sources.Conformal_Maps.EjectedRays).a = tmp / 180.0; s.Rebuild(); PlotRefresh(); }
                    else { throw new FormatException(); }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void angleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (w.f.ToString())
            {
                case "EjectedRays":
                    paramBox2.Text = angleSlider.Value.ToString(Settings.Format);
                    break;
            }
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
            if (sender is Viewbox)
            {
                //in case "cogwheel"
                SettingsWindow settingswin = new SettingsWindow(this); settingswin.Show(); return;
            }
            else
            {
                switch ((sender as Image).Name)
                {
                    case "referImage": MessageBox.Show("Developer has not added this window yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
                    case "saveImage": SaveWindow sw = new SaveWindow(viewModel); sw.Show(); return;
                    case "menuImage": WindowsReferences.MainW.Show(); Close(); return;
                    case "exitImage": Process.GetCurrentProcess().Kill(); return;
                }
            }
        }

        private void ClearTextBoxes()
        {
            xTextBox.Text = string.Empty;
            yTextBox.Text = string.Empty;
            VxTextBox.Text = string.Empty;
            VyTextBox.Text = string.Empty;
        }

        private bool IsCursorInBorder()
        {
            if (CursorPosition.Re < -5 || CursorPosition.Re > 5 || CursorPosition.Im > 5) { return true; }
            switch (w.f.ToString())
            {
                case "IdentityTransform":
                    return CursorPosition.Im < -Math.PI || CursorPosition.Im > Math.PI;
                case "EjectedRays":
                    return false;
                default: return true;
            }
        }

        public void OnPlotGeomParamsChanged()
        {
            s?.ChangeParams(Settings.PlotGeomParams.XMin, Settings.PlotGeomParams.XMax, Settings.PlotGeomParams.YMax, Settings.PlotGeomParams.MRKh, Settings.PlotGeomParams.hVertical);
            PlotRefresh();
        }

        public void OnPlotVisualParamsChanged()
        {
            viewModel.ReassignVisualParams();
            PlotRefresh();
        }
    }
}
