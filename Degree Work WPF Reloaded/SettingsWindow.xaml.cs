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
using System.Windows.Shapes;
using OxyPlot;

namespace Degree_Work
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        enum ColorTarget { Line, Vector, BorderFill, BorderStroke }
        enum ThicknessTarget { Line, Vector, BorderStroke }
        ColorTarget selectedColorTarget;
        ThicknessTarget selectedThicknessTarget;
        byte R, G, B;
        double thickness;

        public SettingsWindow()
        {
            InitializeComponent();
            targetList.SelectionChanged += TargetList_SelectionChanged;
            targetList.SelectedIndex = 0;
            ThicknessSlider.ValueChanged += ThicknessSlider_ValueChanged;
            targetThicknessList.SelectionChanged += TargetThicknessList_SelectionChanged;
            targetThicknessList.SelectedIndex = 0;
        }

        private void TargetThicknessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedThicknessTarget = (ThicknessTarget)targetThicknessList.SelectedIndex;
            switch (selectedThicknessTarget)
            {
                case ThicknessTarget.Line: thickness = Settings.LineStrokeThickness; break;
                case ThicknessTarget.Vector: thickness = Settings.ArrowStokeThickness; break;
                case ThicknessTarget.BorderStroke: thickness = Settings.BorderStrokeThickness; break;
            }
            SetThicknessSliderValue();
            RSlider.Focus();
        }

        private void TargetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedColorTarget = (ColorTarget)targetList.SelectedIndex;
            switch (selectedColorTarget)
            {
                case ColorTarget.Line: R = Settings.LineColor.R; G = Settings.LineColor.G; B = Settings.LineColor.B; break;
                case ColorTarget.Vector: R = Settings.ArrowColor.R; G = Settings.ArrowColor.G; B = Settings.ArrowColor.B; break;
                case ColorTarget.BorderFill: R = Settings.BorderFillColor.R; G = Settings.BorderFillColor.G; B = Settings.BorderFillColor.B; break;
                case ColorTarget.BorderStroke: R = Settings.BorderStrokeColor.R; G = Settings.BorderStrokeColor.G; B = Settings.BorderStrokeColor.B; break;
            }
            SetColorSliderValue();
            RSlider.Focus();
        }

        private void SetColorSliderValue()
        {
            RSlider.Value = R;
            GSlider.Value = G;
            BSlider.Value = B;
        }

        private void SetThicknessSliderValue()
        {
            ThicknessSlider.Value = thickness;
        }

        private void FillRectangle()
        {
            ColorRect.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
        }

        private void AssignColorValue()
        {
            switch (selectedColorTarget)
            {
                case ColorTarget.Line: Settings.LineColor = OxyColor.FromRgb(R,G,B); break;
                case ColorTarget.Vector: Settings.ArrowColor = OxyColor.FromRgb(R, G, B); break;
                case ColorTarget.BorderFill: Settings.BorderFillColor = OxyColor.FromRgb(R, G, B); break;
                case ColorTarget.BorderStroke: Settings.BorderStrokeColor = OxyColor.FromRgb(R, G, B); break;
            }
        }

        private void AssignThicknessValue()
        {
            switch (selectedThicknessTarget)
            {
                case ThicknessTarget.Line: Settings.LineStrokeThickness = thickness; break;
                case ThicknessTarget.Vector: Settings.ArrowStokeThickness  = thickness; break;
                case ThicknessTarget.BorderStroke: Settings.BorderStrokeThickness = thickness; break;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((sender as Slider).Name)
            {
                case "RSlider": R = (byte)RSlider.Value; break;
                case "GSlider": G = (byte)GSlider.Value; break;
                case "BSlider": B = (byte)BSlider.Value; break;
            }
            FillRectangle();
            AssignColorValue();
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            thickness = ThicknessSlider.Value;
            ThicknessOutputTextBlock.Text = thickness.ToString(Settings.Format);
            AssignThicknessValue();
        }












        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

    }
}
