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
        }

        private void Icon_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "halfplaneImage": halfplaneContainer.Margin = new Thickness(0, 5, 0, 5); return;
                case "zoneImage": zoneContainer.Margin = new Thickness(0, 5, 0, 5); return;
                case "circleImage": circleContainer.Margin = new Thickness(0, 5, 0, 5); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSelectedSource; return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }

        private void Icon_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "halfplaneImage": halfplaneContainer.Margin = new Thickness(1, 10, 1, 10); return;
                case "zoneImage": zoneContainer.Margin = new Thickness(1, 10, 1, 10); return;
                case "circleImage": circleContainer.Margin = new Thickness(1, 10, 1, 10); return;
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
                case "circleImage": MessageBox.Show("Developer has not added this window yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
                case "exitImage": (Process.GetCurrentProcess()).Kill(); return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }
    }
}
