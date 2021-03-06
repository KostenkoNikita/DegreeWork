﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        readonly ProcessStartInfo projectInfo = new ProcessStartInfo()
        {
            FileName = @"D:\Programming\GITHUB\DegreeWork\Degree Work WPF Reloaded\bin\Release\Degree Work.exe", UseShellExecute = false
        };

        int handlers = 0;

        readonly List<ImageSource> imageList = new List<ImageSource>()
        {
            new BitmapImage(new Uri(@"Slides\0.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\1.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\2.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\3.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\4.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\5.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\6.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\7.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\8.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\9.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\10.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\11.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\12.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\13.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\14.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\15.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\16.bmp",UriKind.Relative)),
            new BitmapImage(new Uri(@"Slides\help.bmp",UriKind.Relative))
        };

        readonly Dictionary<string, Action> ExecutableCommands;

        string WrittenCommand
        {
            get
            {
                return Console.Text.Replace(" ", string.Empty).Replace("\n", string.Empty).Replace("/", string.Empty).ToLowerInvariant();
            }
        }

        int CurrentSlider;

        bool WasAppStartedFromSlides = false;

        bool WasHelpCommandLast = false;

        public MainWindow()
        {
            InitializeComponent();
            Sliders.Source = imageList[0];
            CurrentSlider = 0;
            ExecutableCommands = new Dictionary<string, Action>();
            ExecutableCommands["exit"] = new Action(() => { Console.Text = string.Empty; Close(); Application.Current.Shutdown(0); });
            ExecutableCommands["app"] = new Action(() => { Console.Text = string.Empty; AppStart(); Focus(); });
            ExecutableCommands["null"] = new Action(() => { Console.Text = string.Empty; Focus(); });
            ExecutableCommands[""] = new Action(() => { Console.Text = string.Empty; Focus(); });
            ExecutableCommands["slide0"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[0]; CurrentSlider = 0; Focus(); });
            ExecutableCommands["slide1"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[1]; CurrentSlider = 1; Focus(); });
            ExecutableCommands["slide2"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[2]; CurrentSlider = 2; Focus(); });
            ExecutableCommands["slide3"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[3]; CurrentSlider = 3; Focus(); });
            ExecutableCommands["slide4"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[4]; CurrentSlider = 4; Focus(); });
            ExecutableCommands["slide6"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[6]; CurrentSlider = 6; Focus(); });
            ExecutableCommands["slide7"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[7]; CurrentSlider = 7; Focus(); });
            ExecutableCommands["slide8"] = new Action(() => { Console.Text = string.Empty; Sliders.Source = imageList[8]; CurrentSlider = 8; Focus(); });
            ExecutableCommands["euler"] = new Action(()=> { Sliders.Source = imageList[9]; WasHelpCommandLast = true; });
            ExecutableCommands["conteq"] = new Action(() => { Sliders.Source = imageList[10]; WasHelpCommandLast = true; });
            ExecutableCommands["bern"] = new Action(() => { Sliders.Source = imageList[11]; WasHelpCommandLast = true; });
            ExecutableCommands["dir"] = new Action(() => { Sliders.Source = imageList[12]; WasHelpCommandLast = true; });
            ExecutableCommands["lieb"] = new Action(() => { Sliders.Source = imageList[13]; WasHelpCommandLast = true; });
            ExecutableCommands["shw"] = new Action(() => { Sliders.Source = imageList[14]; WasHelpCommandLast = true; });
            ExecutableCommands["gen"] = new Action(() => { Sliders.Source = imageList[15]; WasHelpCommandLast = true; });
            ExecutableCommands["my"] = new Action(() => { Sliders.Source = imageList[16]; WasHelpCommandLast = true; });
            ExecutableCommands["help"] = new Action(() => { Sliders.Source = imageList[17]; WasHelpCommandLast = true; });
            ExecutableCommands["ret"] = new Action(() => { return; });
            KeyDown += MainWindow_KeyDown; handlers++;
            Console.Focusable = false;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    {
                        if (CurrentSlider == 8) { return; }
                        else if (CurrentSlider == 4)
                        {
                            Sliders.Source = imageList[6];
                            CurrentSlider = 6;
                            if (!WasAppStartedFromSlides)
                            {
                                AppStart("SpecialPassConfirmed");
                            }
                        }
                        else
                        {
                            Sliders.Source = imageList[++CurrentSlider];
                        }
                        break;
                    }
                case Key.Left:
                    {
                        if (CurrentSlider == 0) { return; }
                        else if (CurrentSlider == 6)
                        {
                            Sliders.Source = imageList[4];
                            CurrentSlider = 4;
                        }
                        else
                        {
                            Sliders.Source = imageList[--CurrentSlider];
                        }
                        break;
                    }
                case Key.Divide:
                    {
                        Console.Focusable = true;
                        Console.CaretBrush = Console.Foreground;
                        Console.Focus();
                        Console.TextChanged += Console_TextChanged;
                        Console.KeyDown += Console_KeyDown;
                        if (handlers == 1)
                        {
                            KeyDown -= MainWindow_KeyDown; handlers--;
                        }
                        break;
                    }
                default:
                    return;
            }
        }

        private void Console_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (WasHelpCommandLast)
                {
                    if (WrittenCommand == "ret" || WrittenCommand == "exit")
                    {
                        ////////На случай выхода////////
                        ExecuteCommand();
                        ////////////////////////////////
                        Console.TextChanged -= Console_TextChanged;
                        Console.KeyDown -= Console_KeyDown;
                        Console.Focusable = false;
                        Console.CaretBrush = Console.Background;
                        if (handlers == 0)
                        {
                            KeyDown += MainWindow_KeyDown; handlers++;
                        }
                        WasHelpCommandLast = false;
                        Console.Text = string.Empty;
                        Sliders.Source = imageList[CurrentSlider];
                        Focus();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (WrittenCommand == "ret")
                    {
                        return;
                    }
                    Console.TextChanged -= Console_TextChanged;
                    Console.KeyDown -= Console_KeyDown;
                    Console.Focusable = false;
                    Console.CaretBrush = Console.Background;
                    if (handlers == 0)
                    {
                        KeyDown += MainWindow_KeyDown; handlers++;
                    }
                    ExecuteCommand();
                }
            }
        }

        private void Console_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.Foreground = ExecutableCommands.ContainsKey(WrittenCommand) ? Brushes.Black : Brushes.DarkRed;
        }

        void ExecuteCommand()
        {
            if (ExecutableCommands.ContainsKey(WrittenCommand))
            {
                ExecutableCommands[WrittenCommand].Invoke();
                if (WasHelpCommandLast)
                {
                    Console.Text = "/";
                    Console.CaretIndex = 1;
                    Console.Focusable = true;
                    Console.CaretBrush = Console.Foreground;
                    Console.Focus();
                    Console.TextChanged += Console_TextChanged;
                    Console.KeyDown += Console_KeyDown;
                    if (handlers == 1)
                    {
                        KeyDown -= MainWindow_KeyDown; handlers--;
                    }
                }
            }
            else
            {
                MessageBox.Show($"Undefined command: {WrittenCommand}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Text = "/";
                Console.CaretIndex = 1;
                Console.Focusable = true;
                Console.CaretBrush = Console.Foreground;
                Console.Focus();
                Console.TextChanged += Console_TextChanged;
                Console.KeyDown += Console_KeyDown;
                if (handlers == 1)
                {
                    KeyDown -= MainWindow_KeyDown; handlers--;
                }
            }
        }

        void AppStart([CallerMemberName] string from = null)
        {
            if (from == "ExecuteCommand" || from == ".ctor")
            {
                Process p = Process.Start(projectInfo);
                p.WaitForExit();
            }
            else if (from == "SpecialPassConfirmed")
            {
                Process p = Process.Start(projectInfo);
                p.WaitForExit();
                WasAppStartedFromSlides = true;
            }
            else
            {
                throw new MethodAccessException();
            }
        }
    }
}
