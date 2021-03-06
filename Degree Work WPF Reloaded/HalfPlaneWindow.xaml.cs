﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Input;
using OxyPlot;
using Degree_Work.Mathematical_Sources.Complex;


namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для HalfPlane.xaml
    /// </summary>
    public partial class HalfPlane : Window, IStreamLinesPlotWindow
    {
        Complex CursorPosition, V;
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
            s = new Hydrodynamics_Sources.HalfPlaneAndZoneStreamLinesBuilder(w, viewModel, CanonicalDomain.HalfPlane);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное\nотображение");
            mapsList.Items.Add("Треугольник 1");
            mapsList.Items.Add("Полуплоскость с\nвыброшенным\nотрезком");
            mapsList.Items.Add("Треугольник 2");
            mapsList.Items.Add("Треугольник 3");
            mapsList.Items.Add("Четырёхугольник 1");
            mapsList.Items.Add("Полуплоскость с\nвыброшенным\nравнобедренным\nтреугольником");
            mapsList.Items.Add("Треугольник 4");
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
                if (!IsCursorInBorder())
                {
                    viewModel.RedrawArrow(CursorPosition, CursorPosition + 2 * V / V.Abs, V, CanonicalDomain.HalfPlane);
                    PlotRefresh();
                }
                else
                {
                    viewModel.DeleteArrow();
                    viewModel.IsMouseClickedInPolygon = false;
                    PlotRefresh();
                }
            }
        }

        private void PlotModel_MouseMove(object sender, OxyMouseEventArgs e)
        {
            CursorPosition = viewModel.GetComplexCursorPositionOnPlot(e.Position);
            V = w.V_physical_plane(CursorPosition);
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
            Mouse.OverrideCursor = Cursors.Wait;
            switch (mapsList.SelectedIndex)
            {
                case 0:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform();
                    s.Rebuild();
                    break;
                case 1:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Porebrick(1);
                    s.Rebuild();
                    break;
                case 2:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.EjectedSegment(0, 1);
                    s.Rebuild();
                    break;
                case 3:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Number81(1);
                    s.Rebuild();
                    break;
                case 4:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Number79(1);
                    Settings.PlotGeomParams.XMax = 65;
                    Settings.PlotGeomParams.XMin = -20;
                    Settings.PlotGeomParams.YMax = 70;
                    Settings.PlotGeomParams.MRKh = 0.1;
                    Settings.PlotGeomParams.hVertical = 0.3;
                    s.ChangeParams(Settings.PlotGeomParams.XMin, Settings.PlotGeomParams.XMax, Settings.PlotGeomParams.YMax, Settings.PlotGeomParams.MRKh, Settings.PlotGeomParams.hVertical);
                    break;
                case 5:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Number89(2,1);
                    s.Rebuild();
                    break;
                case 6:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Triangle(1, 1);
                    s.Rebuild();
                    break;
                case 7:
                    w.f = new Hydrodynamics_Sources.Conformal_Maps.Number85(1);
                    s.Rebuild();
                    break;
                default:
                    return;
            }
            ChangeParamsConfiguration();
            Mouse.OverrideCursor = Cursors.Arrow;
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
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = false;
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
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = false;
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
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = false;
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
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = false;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    break;
                case 4:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = String.Empty;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "h =";
                    param2.Visibility = Visibility.Hidden;
                    param2.Text = string.Empty;
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = false;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    break;
                case 5:
                    paramBox1.Text = "2";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = "1";
                    paramBox2.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "h1 =";
                    param2.Visibility = Visibility.Visible;
                    param2.Text = "h2 =";
                    paramBox1.IsReadOnly = true;
                    paramBox2.IsReadOnly = true;
                    break;
                case 6:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Text = "1";
                    paramBox2.Visibility = Visibility.Visible;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "h =";
                    param2.Visibility = Visibility.Visible;
                    param2.Text = "A =";
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = false;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    paramBox2.TextChanged += paramBox2_TextChanged;
                    break;
                case 7:
                    paramBox1.Text = "1";
                    paramBox1.Visibility = Visibility.Visible;
                    paramBox2.Visibility = Visibility.Hidden;
                    param1.Visibility = Visibility.Visible;
                    param1.Text = "h =";
                    param2.Visibility = Visibility.Hidden;
                    paramBox1.IsReadOnly = false;
                    paramBox2.IsReadOnly = true;
                    paramBox1.TextChanged += paramBox1_TextChanged;
                    break;
                default:
                    return;
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
                    if (tmp > 0) { (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).H = tmp; s.Rebuild(); PlotRefresh(); }
                    else { return; }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
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
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.Number81)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0)
                    {
                        (w.f as Hydrodynamics_Sources.Conformal_Maps.Number81).h = tmp;
                        s.Rebuild();
                        PlotRefresh();
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.Number79)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0)
                    {
                        (w.f as Hydrodynamics_Sources.Conformal_Maps.Number79).h = tmp;
                        s.Rebuild();
                        PlotRefresh();
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.Triangle)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    (w.f as Hydrodynamics_Sources.Conformal_Maps.Triangle).h = tmp; s.Rebuild(); PlotRefresh();
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.Number85)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(1));
                    if (tmp > 0)
                    {
                        (w.f as Hydrodynamics_Sources.Conformal_Maps.Number85).H = tmp;
                        s.Rebuild();
                        PlotRefresh();
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else
            {
                throw new InvalidOperationException("undefined conformal map");
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
                    if (tmp > 0)
                    {
                        (w.f as Hydrodynamics_Sources.Conformal_Maps.EjectedSegment).Y = tmp;
                        s.Rebuild();
                        PlotRefresh();
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else if (w.f is Hydrodynamics_Sources.Conformal_Maps.Triangle)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    double tmp = Convert.ToDouble(TemporaryString(2));
                    if (tmp > 0)
                    {
                        (w.f as Hydrodynamics_Sources.Conformal_Maps.Triangle).A = tmp;
                        s.Rebuild();
                        PlotRefresh();
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else
            {
                throw new InvalidOperationException("undefined conformal map");
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
            if (CursorPosition.Re < -5 || CursorPosition.Re > 5 || CursorPosition.Im>5) { return true; }
            switch (w.f.ToString())
            {
                case "IdentityTransform":
                    return CursorPosition.Im < 0;
                case "Porebrick":
                    return (CursorPosition.Re <= 0 && CursorPosition.Im<0) || (CursorPosition.Re > 0 && CursorPosition.Im < (w.f as Hydrodynamics_Sources.Conformal_Maps.Porebrick).H);
                case "EjectedSegment":
                    return CursorPosition.Im < 0;
                case "Number81":
                    return CursorPosition.Im < 0 && CursorPosition.Re < 0;
                case "Number79":
                    return (CursorPosition.Re<0 && CursorPosition.Im>((Hydrodynamics_Sources.Conformal_Maps.Number79)w.f).h+1) || (CursorPosition.Re > 0 && (CursorPosition.Im > ((Hydrodynamics_Sources.Conformal_Maps.Number79)w.f).h + 1 || CursorPosition.Im < 1));
                case "Number89":
                    return (CursorPosition.Re < 0 && CursorPosition.Im <= 0) || (CursorPosition.Re > 0 && CursorPosition.Im <= ((Hydrodynamics_Sources.Conformal_Maps.Number89)w.f).h2);
                case "Triangle":
                    return false;
                case "Number85":
                    return (CursorPosition.Im<0 && CursorPosition.Re > 0) || (CursorPosition.Im < (w.f as Hydrodynamics_Sources.Conformal_Maps.Number85).H && CursorPosition.Re < 0);
                default: return true;
            }
        }

        public void OnPlotGeomParamsChanged()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            s?.ChangeParams(Settings.PlotGeomParams.XMin, Settings.PlotGeomParams.XMax, Settings.PlotGeomParams.YMax, Settings.PlotGeomParams.MRKh, Settings.PlotGeomParams.hVertical);
            Mouse.OverrideCursor = Cursors.Arrow;
            PlotRefresh();
        }

        public void OnPlotVisualParamsChanged()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            viewModel.ReassignVisualParams();
            Mouse.OverrideCursor = Cursors.Arrow;
            PlotRefresh();
        }
    }
}
