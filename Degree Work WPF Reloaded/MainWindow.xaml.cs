using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Degree_Work_WPF_Reloaded
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double textBlockExpansionCoefficient = 5;

        public MainWindow()
        {
            InitializeComponent();
            exitButton.MouseDown += TextBlock_MouseDown;
        }

        private void exitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            exitButton.Foreground = Brushes.Red;
            exitButton.FontSize += textBlockExpansionCoefficient;
        }

        private void exitButton_MouseLeave(object sender, MouseEventArgs e)
        {
            exitButton.Foreground = Brushes.Black;
            exitButton.FontSize -= textBlockExpansionCoefficient;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as TextBlock).Name)
            {
                case "exitButton": (Process.GetCurrentProcess()).Kill(); break;
                default: throw new ArgumentException("undefined TextBlock name");
            }
        }
    }
}
