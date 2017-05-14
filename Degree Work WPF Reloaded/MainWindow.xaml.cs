using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal const double textBlockExpansionCoefficient = 5;

        public MainWindow()
        {
            InitializeComponent();
            WindowsReferences.MainW = this;
            MenuItemImage.Source = Settings.MMFImageSource;
            MenuItemImageDescription.Text = Settings.EmptyDescription;
        }

        private void Icon_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "halfplaneImage": halfplaneContainer.Margin = new Thickness(5,0,5,0); MenuItemImage.Source = Settings.HalfPlaneImageSource; MenuItemImageDescription.Text = Settings.HafPlaneDescription;  return;
                case "zoneImage": zoneContainer.Margin = new Thickness(5, 0, 5, 0); MenuItemImage.Source = Settings.ZoneImageSource; MenuItemImageDescription.Text = Settings.ZoneDescription; return;
                case "circleImage": circleContainer.Margin = new Thickness(5, 0, 5, 0); MenuItemImage.Source = Settings.CircleImageSource; MenuItemImageDescription.Text = Settings.CircleDescription; return;
                case "heatMapImage": heatMapContainer.Margin = new Thickness(5, 0, 5, 0); MenuItemImage.Source = Settings.HeatMapImageSource; MenuItemImageDescription.Text = Settings.HeatMapDescription; return;
                case "referenceImage": referenceContainer.Margin = new Thickness(3, 3, 3, 3); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSelectedSource; return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }

        private void Icon_MouseLeave(object sender, MouseEventArgs e)
        {
            MenuItemImage.Source = Settings.MMFImageSource;
            MenuItemImageDescription.Text = Settings.EmptyDescription;
            switch ((sender as Image).Name)
            {
                case "halfplaneImage": halfplaneContainer.Margin = new Thickness(10,5,10,5); return;
                case "zoneImage": zoneContainer.Margin = new Thickness(10, 5, 10, 5); return;
                case "circleImage": circleContainer.Margin = new Thickness(10, 5, 10, 5); return;
                case "heatMapImage": heatMapContainer.Margin = new Thickness(10,5,10,5); return;
                case "referenceImage": referenceContainer.Margin = new Thickness(6,6,6,6); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSource; return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "halfplaneImage": WindowsReferences.HalfPlainW = new HalfPlane(); WindowsReferences.HalfPlainW.Show(); Hide(); return;
                case "zoneImage": WindowsReferences.ZoneW = new ZoneWindow(); WindowsReferences.ZoneW.Show(); Hide(); return;
                case "circleImage": WindowsReferences.CircleW = new CircleWindow(); WindowsReferences.CircleW.Show(); Hide(); return;
                case "heatMapImage": WindowsReferences.HMapW = new HeatMapWindow(); WindowsReferences.HMapW.Show(); Hide(); return;
                case "referenceImage": WindowsReferences.RefW = new ReferenceWindow(this); WindowsReferences.RefW.Show(); return;
                case "exitImage": Close(); return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }
    }
}
