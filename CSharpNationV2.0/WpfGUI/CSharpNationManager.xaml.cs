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

using System.Windows.Media.Animation;
using CSharpNationV2._0.Analyzer;
using System.Threading;
using CSharpNationV2._0.Visualizer;
using CSharpNationV2._0.Configuration;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para CSharpNationManager.xaml
    /// </summary>
    public partial class CSharpNationManager : Window
    {
        public CSharpNationManager()
        {
            InitializeComponent();

            windowManager = new WindowManager();

            errorWindow = new ErrorWindow();

            ChangeWindows(windowManager.GetWaveWindow());

            VisualizerThread = new Thread(VisualizerProcess);
            Analyzer = new SpectrumAnalyzer();

            DevicesCb.ItemsSource = Analyzer.GetDevices();

            if (DevicesCb.HasItems)
            {
                DevicesCb.SelectedIndex = 0;
            }
        }

        private WindowManager windowManager;
        private ErrorWindow errorWindow;

        private SpectrumAnalyzer Analyzer;
        private Thread VisualizerThread;
        private SpectrumVisualizer Visualizer;

        private bool IsMenuExpanded = false;

        private void TitleGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.WindowState = WindowState.Normal;
                DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation()
            {
                From = MenuGrid.ActualWidth,
                To = IsMenuExpanded ? 50 : 150,
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new CubicEase()
            };

            MenuGrid.BeginAnimation(WidthProperty, anim);

            IsMenuExpanded = !IsMenuExpanded;
        }

        private void MenuGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentGrid.Margin = new Thickness(MenuGrid.Width, 0, 0, 0);
        }

        private void WavesBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindows(windowManager.GetWaveWindow());
        }

        private void TexturesBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindows(windowManager.GetTexturesWindow());
        }

        private void ChangeWindows(FrameworkElement window)
        {
            ContentGrid.Children.Clear();

            ContentGrid.Children.Add(window);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (VisualizerThread.IsAlive)
            {
                MessageBoxResult result = MessageBox.Show("The visualizer is actually running, ¿Do you want to start a new one?", "Visualizer actually running", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    Visualizer.Close();

                    StartVisualizerThread();
                }
            }
            else
            {
                StartVisualizerThread();
            }
        }

        private void StartVisualizerThread()
        {
            VisualizerThread = new Thread(VisualizerProcess);

            VisualizerThread.Start();
        }

        private void VisualizerProcess()
        {
            using (Visualizer = new SpectrumVisualizer(1280, 720, "CSharpNation_V2.0", Analyzer, windowManager.GetWaves(), windowManager.GetTextureManager(), new float[4]))
            {
                Visualizer.Run();
            }
        }

        private void DevicesCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Analyzer.ChangeDevice(DevicesCb.SelectedIndex);
        }

        private void Manager_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (VisualizerThread.IsAlive)
            {
                Visualizer.Close();
            }

            Analyzer.Free();

            ConfigurationManager.SaveConfig();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            Analyzer.PauseCapture();
        }

        private void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            Analyzer.ResumeCapture();
        }

        private void ErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWindows(errorWindow);
        }
    }
}
