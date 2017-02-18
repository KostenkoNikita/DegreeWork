using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для HalfPlane.xaml
    /// </summary>
    public partial class HalfPlane : Window
    {
        private PlotWindowModel viewModel;
        Hydrodynamics_Sources.Potential w;
        Hydrodynamics_Sources.StreamLinesBuilderHalfPlane s;

        public HalfPlane()
        {
            viewModel = new PlotWindowModel(CanonicalDomain.HalfPlane);
            DataContext = viewModel;
            InitializeComponent();
            w = new Hydrodynamics_Sources.Potential(1, 0, 0, 0, new Hydrodynamics_Sources.Conformal_Maps.IdentityTransform());
            s = new Hydrodynamics_Sources.StreamLinesBuilderHalfPlane(w, viewModel, -20, 20, 20, 0.3, 0.25);
            mapsList.SelectionChanged += MapsList_SelectionChanged;
            mapsList.Items.Add("Тождественное\nотображение");
            mapsList.Items.Add("Поребрик");
            mapsList.SelectedIndex = 0;

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
            s.Rebuild();
            PlotRefresh();
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.Red;
            (sender as TextBlock).FontSize += MainWindow.textBlockExpansionCoefficient;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.Black;
            (sender as TextBlock).FontSize -= MainWindow.textBlockExpansionCoefficient;
        }

        private void refer_MouseEnter(object sender, MouseEventArgs e)
        {
            referImage.Source = new BitmapImage(new Uri(@"Resources/referIcoChecked.png", UriKind.Relative));
        }

        private void refer_MouseLeave(object sender, MouseEventArgs e)
        {
            referImage.Source = new BitmapImage(new Uri(@"Resources/referIco.png", UriKind.Relative));
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as TextBlock).Name)
            {
                case "menuButton": WindowsReferences.MainW.Show(); Close(); return;
                case "exitButton": (Process.GetCurrentProcess()).Kill(); return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }

        void PlotRefresh()
        {
            plot.InvalidatePlot(true);
        }

    }
}
