using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Input;
using OxyPlot;
using MathCore_2_0;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для HalfPlane.xaml
    /// </summary>
    public partial class HalfPlane : Window, IStreamLinesPlotWindow
    {
        complex CursorPosition, V;
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

        public HalfPlane()
        {
            viewModel = new PlotWindowModel(CanonicalDomain.HalfPlane);
            DataContext = viewModel;
            InitializeComponent();
            Settings.PlotGeomParams.hVertical = 0.5;
            Settings.PlotGeomParamsConstant.hVertical = 0.5;
            w = new Hydrodynamics_Sources.Potential(1, 0, 0, 0, new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform());
            s = new Hydrodynamics_Sources.StreamLinesBuilderHalfPlaneAndZone(w, viewModel, CanonicalDomain.HalfPlane);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное\nотображение");
            mapsList.Items.Add("Поребрик");
            mapsList.Items.Add("Полуплоскость с\nвыброшенным отрезком");
            mapsList.Items.Add("Номер 81");
            mapsList.SelectedIndex = 0;
            viewModel.PlotModel.MouseMove += PlotModel_MouseMove;
            viewModel.PlotModel.MouseDown += PlotModel_MouseDown;
            plot.Controller = new PlotController();
            plot.Controller.UnbindMouseDown(OxyMouseButton.Left);
        }

        private void PlotModel_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton.ToString()=="Left")
            {
                viewModel.RedrawArrow(CursorPosition, CursorPosition + 2 * V / V.abs, V, CanonicalDomain.HalfPlane);
                PlotRefresh();
            }
        }

        private void PlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            CursorPosition = viewModel.GetComplexCursorPositionOnPlot(e.Position);
            V = w.V_physical_plane(CursorPosition);
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
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Porebrick(1);
                    break;
                case 2:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.EjectedSegment(0, 1);
                    break;
                case 3:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Number81(1);
                    break;
            }
            Mouse.OverrideCursor = Cursors.Wait;
            ChangeParamsConfiguration();
            s.Rebuild();
            Mouse.OverrideCursor = null;
            PlotRefresh();
        }

        private void ChangeParamsConfiguration()
        {
            paramBox1.TextChanged -= paramBox1_TextChanged;
            paramBox2.TextChanged -= paramBox2_TextChanged;
            switch (mapsList.SelectedIndex)
            {
                case 0:
                    paramBox1.Text = String.Empty;
                    paramBox1.Visibility = Visibility.Hidden;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Hidden;
                    param1.Text = string.Empty;
                    param2.Visibility = Visibility.Hidden;
                    param2.Text = string.Empty;
                    break;
                case 1:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "h =";
                    param2.Visibility = Visibility.Hidden;
                    param2.Text = string.Empty;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    break;
                case 2:
                    paramBox1.Text = "0";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = "1";
                    paramBox2.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "X =";
                    param2.Visibility = Visibility.Visible;
                    param2.Text = "h =";
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    paramBox2.TextChanged += paramBox2_TextChanged;
                    break;
                case 3:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "h =";
                    param2.Visibility = Visibility.Hidden;
                    param2.Text = string.Empty;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    break;
            }
        }

        private void PlotRefresh()
        {
            plot.InvalidatePlot(true);
        }

        private void paramBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.Porebrick)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0) { (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).h = tmp; s.Rebuild(); PlotRefresh(); }
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
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.EjectedSegment)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    (w.f as Hydrodynamics_Sources.Conformal_Maps.EjectedSegment).X = tmp; s.Rebuild(); PlotRefresh(); 
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
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.Number81)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    (w.f as Hydrodynamics_Sources.Conformal_Maps.Number81).h = tmp; s.Rebuild(); PlotRefresh();
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
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.EjectedSegment)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(2));
                    if (tmp > 0) { (w.f as Hydrodynamics_Sources.Conformal_Maps.EjectedSegment).Y = tmp; s.Rebuild(); PlotRefresh(); }
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

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": referContainer.Margin = new Thickness(3,3,3,3); return;
                case "saveImage": saveContainer.Margin = new Thickness(4,4,4,4); return;
                case "menuImage": menuContainer.Margin = new Thickness(7,7,7,7); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSelectedSource; return;
            }
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "referImage": referContainer.Margin = new Thickness(7,7,7,7); return;
                case "saveImage": saveContainer.Margin = new Thickness(9,9,9,9); return;
                case "menuImage": menuContainer.Margin = new Thickness(13,13,13,13); return;
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
            if (CursorPosition.Re < -5 || CursorPosition.Re > 5 || CursorPosition.Im>5) { return true; }
            switch (w.f.ToString())
            {
                case "IdentityTransform":
                    return CursorPosition.Im < 0;
                case "Porebrick":
                    return (CursorPosition.Re <= 0 && CursorPosition.Im<0) || (CursorPosition.Re > 0 && CursorPosition.Im < (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).h);
                case "EjectedSegment":
                    return CursorPosition.Im < 0;
                case "Number81":
                    return CursorPosition.Im < 0 && CursorPosition.Re < 0;
                default: return true;
            }
        }

        public void OnPlotGeomParamsChanged()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            s?.ChangeParams(Settings.PlotGeomParams.XMin, Settings.PlotGeomParams.XMax, Settings.PlotGeomParams.YMax, Settings.PlotGeomParams.MRKh, Settings.PlotGeomParams.hVertical);
            Mouse.OverrideCursor = null;
            PlotRefresh();
        }

        public void OnPlotVisualParamsChanged()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            viewModel.ReassignVisualParams();
            Mouse.OverrideCursor = null;
            PlotRefresh();
        }
    }
}
