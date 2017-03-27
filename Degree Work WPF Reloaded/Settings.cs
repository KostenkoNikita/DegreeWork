using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathCore_2_0;
using OxyPlot;


namespace Degree_Work
{
    static class Settings
    {
        public const string HafPlaneDescription = "Обтекание полуплоскости и конформные\nотображения полуплоскости на различные области";
        public const string ZoneDescription = "Обтекание полосы -π..π и конформные отображения\nполосы -π..π на различные области";
        public const string CircleDescription = "Обтекание единичной окружности и конформные отображения\nединичной окружности на различные области";

        public static StreamLinesPlotGeomParams PlotGeomParams;
        public static StreamLinesPlotVisualParams PlotVisualParams;
        public static readonly StreamLinesPlotGeomParams PlotGeomParamsConstant;
        public static readonly StreamLinesPlotVisualParams PlotVisualParamsConstant;
        public static ushort Precision { get { return precision.decimals; } set { precision.decimals = value; } }
        public static string Format => precision.format;
        internal static ImageSource exitIcoSource;
        internal static ImageSource exitIcoSelectedSource;
        internal static ImageSource saveIcoSource;
        internal static ImageSource saveIcoSelectedSource;
        internal static ImageSource OKIcoSource;

        internal static ImageSource HalfPlaneImageSource;
        internal static ImageSource ZoneImageSource;
        internal static ImageSource CircleImageSource;
        internal static ImageSource EmptyImageSource;




        static Settings()
        {
            exitIcoSource = new BitmapImage(new Uri(@"Resources/exitIco.bmp", UriKind.Relative));
            exitIcoSelectedSource = new BitmapImage(new Uri(@"Resources/exitSelected.bmp", UriKind.Relative));
            saveIcoSource = new BitmapImage(new Uri(@"Resources/saveIco3.png", UriKind.Relative));
            saveIcoSelectedSource = new BitmapImage(new Uri(@"Resources/saveIco3Selected.png", UriKind.Relative));
            OKIcoSource = new BitmapImage(new Uri(@"Resources/okayIcon.png", UriKind.Relative));

            HalfPlaneImageSource = new BitmapImage(new Uri(@"Resources/HalfPlaneImage.png", UriKind.Relative));
            ZoneImageSource = new BitmapImage(new Uri(@"Resources/ZoneImage.png", UriKind.Relative));
            CircleImageSource = new BitmapImage(new Uri(@"Resources/CircleImage.png", UriKind.Relative));
            EmptyImageSource = new BitmapImage(new Uri(@"Resources/Empty.png", UriKind.Relative));

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
