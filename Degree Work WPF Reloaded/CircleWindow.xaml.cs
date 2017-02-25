//#define HELP_FOR_GROUP_LEADER

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
    /// Логика взаимодействия для CircleWindow.xaml
    /// </summary>
    public partial class CircleWindow : Window, IStreamLinesPlotWindow
    {
        complex CursorPosition, V;
        private PlotWindowModel viewModel;
        Hydrodynamics_Sources.Potential w;
#if HELP_FOR_GROUP_LEADER
        Hydrodynamics_Sources.PotentialHelp wHelp;
#endif
        Hydrodynamics_Sources.StreamLinesBuilder s;

        private string TemporaryString(int num)
        {
            switch (num)
            {
                case 1: return paramBox1.Text.Trim(' ').Replace('.', ',');
                default: return null;
            }
        }

        public CircleWindow()
        {
            viewModel = new PlotWindowModel(CanonicalDomain.Circular);
            DataContext = viewModel;
            InitializeComponent();
            Settings.PlotGeomParams.hVertical = 0.5;
            Settings.PlotGeomParamsConstant.hVertical = 0.5;
            w = new Hydrodynamics_Sources.Potential(1, 0, 1, 0, new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform());
            s = new Hydrodynamics_Sources.CircleStreamLinesBuilder(w, viewModel);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное отображение");
            mapsList.Items.Add("Обтекание пластины");
#if HELP_FOR_GROUP_LEADER
            mapsList.Items.Add("Help");
#endif
            mapsList.SelectedIndex = 0;
            viewModel.PlotModel.MouseMove += PlotModel_MouseMove;
            viewModel.PlotModel.MouseDown += PlotModel_MouseDown;
            plot.Controller = new PlotController();
            plot.Controller.UnbindMouseDown(OxyMouseButton.Left);
        }

        private void PlotModel_MouseDown(object sender, OxyMouseDownEventArgs e)
        {
#if !HELP_FOR_GROUP_LEADER
            if (e.ChangedButton.ToString() == "Left")
            {
                viewModel.RedrawArrow(CursorPosition, CursorPosition + 2 * V / V.abs, V, CanonicalDomain.Circular);
                PlotRefresh();
            }
#endif
        }

        private void PlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
#if !HELP_FOR_GROUP_LEADER
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
#endif
        }

        private void MapsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (mapsList.SelectedIndex)
            {
                case 0: w.f = new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform(); break;
                case 1: w.f = new Hydrodynamics_Sources.Conformal_Maps.Plate(); break;
#if HELP_FOR_GROUP_LEADER
                case 2: Settings.PlotGeomParams.MRKh = 0.01; Settings.PlotGeomParams.hVertical = 0.05; wHelp = new Hydrodynamics_Sources.PotentialHelp(1, 1, 1); s = new Hydrodynamics_Sources.StreamLinesBuilderForGroupLeader(wHelp,viewModel); break;
#endif
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
            angleSlider.ValueChanged -= angleSlider_ValueChanged;
            switch (mapsList.SelectedIndex)
            {
                case 0:
                    paramBox1.Text = w.AlphaDegrees.ToString();
                    paramBox1.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "α =";
                    angleSlider.Visibility = Visibility.Visible;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    angleSlider.ValueChanged += angleSlider_ValueChanged;
                    break;
                case 1:
                    paramBox1.Text = w.AlphaDegrees.ToString();
                    paramBox1.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "α =";
                    angleSlider.Visibility = Visibility.Visible;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    angleSlider.ValueChanged += angleSlider_ValueChanged;
                    break;
#if HELP_FOR_GROUP_LEADER
                case 2:
                    paramBox1.Text = string.Empty;
                    paramBox1.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Hidden;
                    param1.Text = string.Empty ;
                    angleSlider.Visibility = Visibility.Hidden;
                    paramBox1.TextChanged -= paramBox1_TextChanged;
                    angleSlider.ValueChanged -= angleSlider_ValueChanged;
                    break;
#endif

            }
        }

        private void PlotRefresh()
        {
            plot.InvalidatePlot(true);
        }

        private void angleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch (w.f.ToString())
            {
                case "IdentityTransform":
                    paramBox1.Text = angleSlider.Value.ToString(Settings.Format);
                    break;
                case "Plate":
                    paramBox1.Text = angleSlider.Value.ToString(Settings.Format);
                    break;
            }
        }

        private void paramBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (w.f is Hydrodynamics_Sources.Conformal_Maps.IdentityTransform || w.f is Hydrodynamics_Sources.Conformal_Maps.Plate)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp >=-90 && tmp<=90) { w.AlphaDegrees = tmp; s.Rebuild(); PlotRefresh(); }
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
                    return CursorPosition.abs < w.R;
                case "Plate":
                    return false;
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
