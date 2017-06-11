using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Shapes;

namespace Presentation
{
    public partial class MainWindow : Window
    {
        static readonly List<ImageSource> imageList = new List<ImageSource>()
        {
            new BitmapImage(new Uri(@"Slides\0.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\1.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\2.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\3.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\4.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\5.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\6.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\7.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\8.bmp",UriKind.Relative))
        };

        public MainWindow()
        {
            InitializeComponent();
            this.MouseDown += MainWindow_MouseDown;
            string tmp = AppDomain.CurrentDomain.BaseDirectory;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Sliders.Source = imageList[1];
        }
    }
}
