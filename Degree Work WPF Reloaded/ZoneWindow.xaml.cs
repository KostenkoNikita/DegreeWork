using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Input;
using OxyPlot;
using Degree_Work.Mathematical_Sources.Complex;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для ZoneWindow.xaml
    /// </summary>
    public partial class ZoneWindow : Window, IStreamLinesPlotWindow
    {
        Complex CursorPosition, V;
        private double oldDiffusorAngle;
        private PlotWindowModel viewModel;
        Hydrodynamics_Sources.Potential w;
        Hydrodynamics_Sources.StreamLinesBuilder s;
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
            s = new Hydrodynamics_Sources.HalfPlaneAndZoneStreamLinesBuilder(w, viewModel, CanonicalDomain.Zone);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное\nотображение");
            mapsList.Items.Add("Плоскость с двумя\nотброшенными лучами");
            mapsList.Items.Add("Диффузор");
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
                viewModel.RedrawArrow(CursorPosition, CursorPosition + 2 * V / V.Abs, V, CanonicalDomain.Zone);
                PlotRefresh();
            }
        }

        private void PlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            CursorPosition = viewModel.GetComplexCursorPositionOnPlot(e.Position);
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.Diffusor && CursorPosition.Im < 0)
            {
                V = w.V_physical_plane(CursorPosition.Conjugate);
                V = V.Conjugate;
            }
            else
            {
                V = w.V_physical_plane(CursorPosition);
            }
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.EjectedRays && CursorPosition.Re < 0 && (Math.Abs(CursorPosition.Im) < CursorPosition.Re * Math.Tan((w.f as Hydrodynamics_Sources.Conformal_Maps.EjectedRays).Angle)))
            {
                V = -V;
            }
            if (Complex.IsNaN(V) || IsCursorInBorder())
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
                case 2:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Diffusor(1, 90f);
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
                    angleSlider.Minimum = 90;
                    angleSlider.Maximum = 180;
                    angleSlider.Value = 90;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    paramBox2.TextChanged += paramBox2_TextChanged;
                    angleSlider.ValueChanged += angleSlider_ValueChanged;
                    break;
                case 2:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = "90";
                    paramBox2.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param2.Visibility = Visibility.Visible;
                    angleSlider.Visibility = Visibility.Visible;
                    angleSlider.Minimum = 0;
                    angleSlider.Maximum = 105;
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
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.Diffusor)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0) { (w.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor).h = tmp; s.Rebuild(); PlotRefresh(); }
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
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.Diffusor)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    float tmp = Convert.ToSingle(TemporaryString(2));
                    if (tmp >= 0 && tmp <= 90)
                    {
                        Hydrodynamics_Sources.Conformal_Maps.Diffusor d = w.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor;
                        d.angleDegrees = tmp;
                        s.Rebuild();
                        PlotRefresh();
                    }
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
                case "Diffusor":
                    if (ValidateSliderValueForDiffusor()) { paramBox2.Text = angleSlider.Value.ToString(Settings.Format); }
                    angleSlider.ValueChanged += angleSlider_ValueChanged;
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
                    case "referImage": WindowsReferences.RefW = new ReferenceWindow(this); WindowsReferences.RefW.Show(); return;
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
                case "Diffusor":
                    return (CursorPosition.Re <0 && Math.Abs(CursorPosition.Im)>=(w.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor).h) 
                        || (CursorPosition.Re >0 && Math.Abs(CursorPosition.Im) >= (w.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor).h+ CursorPosition.Re* Math.Tan((w.f as Hydrodynamics_Sources.Conformal_Maps.Diffusor).angleDegrees*Math.PI/180.0));
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

        //true -- присвоено новое значение
        private bool ValidateSliderValueForDiffusor()
        {
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.Diffusor)
            {
                angleSlider.ValueChanged -= angleSlider_ValueChanged;
                double tmp = angleSlider.Value;
                double[] tmp_array = new double[8]
                    {
                        Math.Abs(tmp-15),
                        Math.Abs(tmp-18),
                        Math.Abs(tmp-22.5),
                        Math.Abs(tmp-30),
                        Math.Abs(tmp-36),
                        Math.Abs(tmp-45),
                        Math.Abs(tmp-60),
                        Math.Abs(tmp-90)
                    };
                int tmp_min = 0;
                for (int i = 1; i < 8; i++)
                {
                    tmp_min = tmp_array[i] < tmp_array[tmp_min] ? i : tmp_min;
                }
                double CloserValue = 0 ;
                switch (tmp_min)
                {
                    case 0:
                        CloserValue = 15;
                        break;
                    case 1:
                        CloserValue = 18;
                        break;
                    case 2:
                        CloserValue = 22.5;
                        break;
                    case 3:
                        CloserValue = 30;
                        break;
                    case 4:
                        CloserValue = 36;
                        break;
                    case 5:
                        CloserValue = 45;
                        break;
                    case 6:
                        CloserValue = 60;
                        break;
                    case 7:
                        CloserValue = 90;
                        break;
                }
                if (CloserValue == oldDiffusorAngle) { angleSlider.Value = oldDiffusorAngle; return false; }
                else
                {
                    angleSlider.Value = oldDiffusorAngle = CloserValue;
                    return true;
                }
            }
            else
            {
                throw new InvalidOperationException("conformal function isn't diffusor map");
            }
        }
    }
}
