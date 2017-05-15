using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using OxyPlot;


namespace Degree_Work
{
    static class Settings
    {
        public const string HafPlaneDescription = "Обтекание полуплоскости и конформные\nотображения полуплоскости на различные области";
        public const string ZoneDescription = "Обтекание полосы -π..π и конформные\nотображения полосы -π..π на\nразличные области";
        public const string CircleDescription = "Обтекание единичной окружности и конформные\nотображения единичной окружности на\nразличные области";
        public const string HeatMapDescription = "Линии тока при обтекании прямоугольника,\nполученные при численном решении уравнения\nЛапласа для функции тока";
        public const string EmptyDescription = "Дипломная работа\nстудента группы МХ-13-1 ДНУ им. О. Гончара\nКостенко Никиты Викторовича";
        
        

        public static StreamLinesPlotGeomParams PlotGeomParams;
        public static StreamLinesPlotVisualParams PlotVisualParams;
        public static readonly StreamLinesPlotGeomParams PlotGeomParamsConstant;
        public static readonly StreamLinesPlotVisualParams PlotVisualParamsConstant;
        public static ushort Precision => (ushort)4;
        public static string Format => "0.####";
        public static double Eps => 0.0001;
        internal static ImageSource exitIcoSource;
        internal static ImageSource exitIcoSelectedSource;
        internal static ImageSource saveIcoSource;
        internal static ImageSource saveIcoSelectedSource;
        internal static ImageSource OKIcoSource;

        internal static ImageSource StartIcoSource;
        internal static ImageSource ClockIcoSource;

        internal static ImageSource HalfPlaneImageSource;
        internal static ImageSource ZoneImageSource;
        internal static ImageSource CircleImageSource;
        internal static ImageSource HeatMapImageSource;
        internal static ImageSource EmptyImageSource;

        internal static ImageSource MMFImageSource;




        static Settings()
        {
            exitIcoSource = new BitmapImage(new Uri(@"Resources/exitIco.bmp", UriKind.Relative));
            exitIcoSelectedSource = new BitmapImage(new Uri(@"Resources/exitSelected.bmp", UriKind.Relative));
            saveIcoSource = new BitmapImage(new Uri(@"Resources/saveIco3.png", UriKind.Relative));
            saveIcoSelectedSource = new BitmapImage(new Uri(@"Resources/saveIco3Selected.png", UriKind.Relative));
            OKIcoSource = new BitmapImage(new Uri(@"Resources/okayIcon.png", UriKind.Relative));

            StartIcoSource = new BitmapImage(new Uri(@"Resources/StartButtonIco.ico", UriKind.Relative));
            ClockIcoSource = new BitmapImage(new Uri(@"Resources/clockIco.gif", UriKind.Relative));

            HalfPlaneImageSource = new BitmapImage(new Uri(@"Resources/HalfPlaneImage.png", UriKind.Relative));
            ZoneImageSource = new BitmapImage(new Uri(@"Resources/ZoneImage.png", UriKind.Relative));
            CircleImageSource = new BitmapImage(new Uri(@"Resources/CircleImage.png", UriKind.Relative));
            HeatMapImageSource = new BitmapImage(new Uri(@"Resources/heatMapImage.png", UriKind.Relative));
            EmptyImageSource = new BitmapImage(new Uri(@"Resources/Empty.png", UriKind.Relative));

            MMFImageSource = new BitmapImage(new Uri(@"Resources/mmfImage.png", UriKind.Relative));

            PlotGeomParams = new StreamLinesPlotGeomParams() { hVertical = 0.5, MRKh = 0.3, XMax = 20, XMin = -20, YMax = 20, YMin = -20 };
            PlotVisualParams = new StreamLinesPlotVisualParams()
            {
                LineColor = OxyColors.Blue,
                ArrowColor = OxyColors.Black,
                BorderFillColor = OxyColors.Gray,
                BorderStrokeColor = OxyColors.Black,
                BorderStrokeThickness = 1,
                LineStrokeThickness = 3,
                ArrowStokeThickness = 3
            };
            PlotGeomParamsConstant = PlotGeomParams.Clone();
            PlotVisualParamsConstant = PlotVisualParams.Clone();
        }

    }
}
