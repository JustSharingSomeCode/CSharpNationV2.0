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
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Drawing;
using CSharpNationV2._0.Analyzer;
using CSharpNationV2._0.Visualizer;
using System.Threading;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            VisualizerThread = new Thread(VisualizerProcess);
            Analyzer = new SpectrumAnalyzer();

            Waves = new SpectrumWave[9];

            Waves[0] = new SpectrumWave
            {
                WaveColor = Color.White,
                Increment = 0,
                BarsInfluence = 0,
                PromLoops = 0
            };

            Waves[1] = new SpectrumWave
            {
                WaveColor = Color.Yellow,
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 1
            };

            Waves[2] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(255, 150, 0),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 2
            };

            Waves[3] = new SpectrumWave
            {
                WaveColor = Color.Red,
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 3
            };

            Waves[4] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(255, 100, 255),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 4
            };

            Waves[5] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(50, 50, 155),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 5
            };

            Waves[6] = new SpectrumWave
            {
                WaveColor = Color.Blue,
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 6
            };

            Waves[7] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(50, 205, 255),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 7
            };

            Waves[8] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(0, 255, 0),
                Increment = 0.5f,
                BarsInfluence = 2,
                PromLoops = 8
            };

            UpdateWaveEditor();
        }

        private SpectrumAnalyzer Analyzer;
        private Thread VisualizerThread;
        private SpectrumVisualizer Visualizer;

        private SpectrumWave[] Waves;

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {            
            if (VisualizerThread.IsAlive)
            {
                MessageBoxResult result = MessageBox.Show("The visualizer is actually running, ¿Do you want to start a new one?", "Visualizer actually running", MessageBoxButton.YesNo);

                if(result == MessageBoxResult.Yes)
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
            using (Visualizer = new SpectrumVisualizer(1280, 720, "CSharpNation_V2.0", Analyzer, Waves))
            {
                Visualizer.Run();
            }            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DevicesCb.ItemsSource = Analyzer.GetDevices();

            if(DevicesCb.HasItems)
            {
                DevicesCb.SelectedIndex = 0;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {                
            if(VisualizerThread.IsAlive)
            {
                Visualizer.Close();
            }

            Analyzer.Free();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            Analyzer.PauseCapture();
        }

        private void ResumeBtn_Click(object sender, RoutedEventArgs e)
        {
            Analyzer.ResumeCapture();
        }

        private void DevicesCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            Analyzer.ChangeDevice(DevicesCb.SelectedIndex);
        }   
        
        private void UpdateWaveEditor()
        {
            EditorList.Items.Clear();

            for(int i = 0; i < Waves.Length; i++)
            {
                WaveEditor editor = new WaveEditor()
                {
                    Width = 500,
                    Height = 115,

                    R = Waves[i].WaveColor.R,
                    G = Waves[i].WaveColor.G,
                    B = Waves[i].WaveColor.B,

                    //Increment = Waves[i].Increment,
                    //BarsInfluence = Waves[i].BarsInfluence,
                    //PromLoops = Waves[i].PromLoops,

                    Wave = Waves[i]
                };

                //editor.UpdateColorPicker();

                EditorList.Items.Add(editor);
            }
        }
    }
}
