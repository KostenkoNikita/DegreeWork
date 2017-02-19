﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OxyPlot;


namespace Degree_Work
{
    static class Settings
    {
        public static OxyColor LineColor { get; set; }
        public static OxyColor ArrowColor { get; set; }
        public static OxyColor BorderFillColor { get; set; }
        public static OxyColor BorderStrokeColor { get; set; }
        public static double BorderStrokeThickness { get; set; }
        public static double LineStrokeThickness { get; set; }
        public static UInt16 Precision
        {
            get
            {
                return _precision;
            }
            set
            {
                _precision = value;
                Format = "0.";
                for (int i = 1; i <= _precision; i++)
                {
                    Format += "#";
                }
            }
        }
        public static string Format = "0.####"; 
        static UInt16 _precision = 4;
        static double precisionf
        {
            get
            {
                return Math.Pow(10, -_precision);
            }
        }
        internal static ImageSource exitIcoSource;
        internal static ImageSource exitIcoSelectedSource;

        static Settings()
        {
            exitIcoSource = new BitmapImage(new Uri(@"Resources/exitIco.bmp", UriKind.Relative));
            exitIcoSelectedSource = new BitmapImage(new Uri(@"Resources/exitSelected.bmp", UriKind.Relative));
        }

    }
}
