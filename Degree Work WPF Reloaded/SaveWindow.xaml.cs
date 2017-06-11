using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;


namespace Degree_Work
{
    partial class SaveWindow : Window
    {
        enum PicFormat { PNG, BMP, JPG };
        PlotWindowModel viewModel;
        int width;
        int height => (int)(width / WidthDividedOnHeight);
        PicFormat format;
        double WidthDividedOnHeight;
        string path;

        internal SaveWindow(PlotWindowModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            Deactivated += OnWindowDeactivated;
            WidthDividedOnHeight = (vm.PlotModel.Width) / (vm.PlotModel.Height);
            widthSlider.Minimum = widthSlider.Value = vm.PlotModel.Width;
            heightSlider.Minimum = heightSlider.Value = vm.PlotModel.Height;           
            formatList.SelectionChanged += FormatList_SelectionChanged;
            formatList.SelectedIndex = 0;
            path = pathTextBox.Text = AppDomain.CurrentDomain.BaseDirectory + @"\Saved Plots\";
            pathTextBox.TextChanged += (sender, e) => { path = pathTextBox.Text; };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            width = (int)widthSlider.Value; WidthOutput(); HeightOutput();
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
            heightSlider.Value = height;
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
                if (!path.EndsWith(@"\")) { path += @"\"; }
                if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
                Deactivated -= OnWindowDeactivated;
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    viewModel.GetType().InvokeMember($"SavePlotTo{format.ToString()}", System.Reflection.BindingFlags.InvokeMethod, null, viewModel, new object[] { path + $"Plot[{DateTime.Now.ToString().Replace(':', '.').Replace(" ", " Time=")}].{format.ToString().ToLower()}", width, height });
                    Mouse.OverrideCursor = Cursors.Arrow;
                    doneImage.Source = Settings.OKIcoSource;
                    doneImage.MouseEnter -= ico_MouseEnter;
                    doneImage.MouseLeave -= ico_MouseLeave;
                }
                catch
                {
                    MessageBox.Show("Возможно, Вы некорректно указали путь.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
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
