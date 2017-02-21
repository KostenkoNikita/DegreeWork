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
    public partial class HalfPlane : Window
    {
        complex CursorPosition, V;
        private PlotWindowModel viewModel;
        Hydrodynamics_Sources.Potential w;
        Hydrodynamics_Sources.StreamLinesBuilderHalfPlane s;
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
            w = new Hydrodynamics_Sources.Potential(1, 0, 0, 0, new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform());
            s = new Hydrodynamics_Sources.StreamLinesBuilderHalfPlane(w, viewModel, -20, 20, 20, 0.3, 0.5);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное\nотображение");
            mapsList.Items.Add("Поребрик");
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
                viewModel.RedrawArrow(CursorPosition, CursorPosition + 2 * V / V.abs);
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
            }
            ChangeParamsConfiguration();
            s.Rebuild();
            PlotRefresh();
        }

        private void ChangeParamsConfiguration()
        {
            switch (mapsList.SelectedIndex)
            {
                case 0:
                    paramBox1.TextChanged -= paramBox1_TextChanged;
                    paramBox2.TextChanged -= paramBox1_TextChanged;
                    paramBox1.Text = String.Empty;
                    paramBox1.Visibility = Visibility.Hidden;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Hidden;
                    param2.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Visible;
                    param2.Visibility = Visibility.Hidden;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    paramBox2.TextChanged += paramBox1_TextChanged;
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
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0) { (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).h = tmp; s.Rebuild(); PlotRefresh(); }
                    else { throw new FormatException(); }
                }
                catch
                {
                    return;
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
                return;
            }
            else
            {
                switch ((sender as Image).Name)
                {
                    case "referImage": return;
                    case "saveImage": SaveWindow sw = new SaveWindow(viewModel); sw.Show(); return;
                    case "menuImage": WindowsReferences.MainW.Show(); Close(); return;
                    case "exitImage": Process.GetCurrentProcess().Kill(); return;
                }
            }
        }

        private void paramBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            //code will be added with new conformal maps
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
                default: return true;
            }
        }
    }
}
