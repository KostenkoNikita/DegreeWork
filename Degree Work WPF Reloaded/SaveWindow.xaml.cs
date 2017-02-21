using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Degree_Work
{
    partial class SaveWindow : Window
    {
        enum PicFormat { PNG, BMP, JPG };
        PlotWindowModel viewModel;
        int width, height;
        PicFormat format;
        string path;

        internal SaveWindow(PlotWindowModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            Deactivated += OnWindowDeactivated;
            widthSlider.Minimum = widthSlider.Value = vm.PlotModel.Width;
            heightSlider.Minimum = heightSlider.Value = vm.PlotModel.Height;
            formatList.SelectionChanged += FormatList_SelectionChanged;
            formatList.SelectedIndex = 0;
            path = pathTextBox.Text = Directory.GetCurrentDirectory() + @"\Saved Plots\";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            pathTextBox.TextChanged += (sender, e) => { path = pathTextBox.Text; };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((sender as Slider).Name)
            {
                case "widthSlider": width = (int)widthSlider.Value; WidthOutput(); return;
                case "heightSlider": height = (int)heightSlider.Value; HeightOutput(); return;
            }
        }

        private void FormatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            format = (PicFormat)(formatList.SelectedIndex);
            //false focus for "unfocus" ComboBox
            widthSlider.Focus();
        }

        private void WidthOutput()
        {
            widthOutputTextBlock.Text = $"{width} px.";
        }

        private void HeightOutput()
        {
            heightOutputTextBlock.Text = $"{height} px.";
        }

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            doneImage.Source = Settings.saveIcoSelectedSource;
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            doneImage.Source = Settings.saveIcoSource;
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (doneImage.Source == Settings.OKIcoSource) { Close(); }
            else
            {
                string name;
                Deactivated -= OnWindowDeactivated;
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    switch (format)
                    {
                        case PicFormat.BMP:
                            name = $"Plot[{DateTime.Now.ToString().Replace(':', '.').Replace(" ", " Time=")}].bmp";
                            viewModel.SavePlotToBMP(path + name, width, height);
                            break;
                        case PicFormat.PNG:
                            name = $"Plot[{DateTime.Now.ToString().Replace(':', '.').Replace(" "," Time=")}].png";
                            viewModel.SavePlotToPNG(path + name, width, height);
                            break;
                        case PicFormat.JPG:
                            name = $"Plot[{DateTime.Now.ToString().Replace(':', '.').Replace(" ", " Time=")}].jpg";
                            viewModel.SavePlotToJPG(path + name, width, height);
                            break;
                    }
                    Mouse.OverrideCursor = null;
                    doneImage.Source = Settings.OKIcoSource;
                    doneImage.MouseEnter -= ico_MouseEnter;
                    doneImage.MouseLeave -= ico_MouseLeave;
                }
                catch
                {
                    MessageBox.Show("Возможно, Вы некорректно указали путь.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Mouse.OverrideCursor = null;
                }
                finally
                {
                    Deactivated += OnWindowDeactivated;
                    viewModel.PlotModel.InvalidatePlot(true);
                }
            }
        }

        private void OnWindowDeactivated(object sender, EventArgs e)
        {
            Close();
        }
    }
}
