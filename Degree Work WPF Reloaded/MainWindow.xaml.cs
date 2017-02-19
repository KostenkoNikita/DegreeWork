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

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.Red;
            (sender as TextBlock).FontSize += textBlockExpansionCoefficient;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as TextBlock).Foreground = Brushes.Black;
            (sender as TextBlock).FontSize -= textBlockExpansionCoefficient;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as TextBlock).Name)
            {
                case "halfPlaneButton": WindowsReferences.HalfPlainW = new HalfPlane(); WindowsReferences.HalfPlainW.Show(); Hide(); return;
                case "circleButton": MessageBox.Show("Developer has not added this window yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
                case "referenceButton": MessageBox.Show("Developer has not added this window yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
                case "exitButton": (Process.GetCurrentProcess()).Kill(); return;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }
    }
}
