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
using CSharpNationV2._0.Configuration;
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

            ConfigManager.LoadConfigFile();

            BackgroundsTxt.Text = ConfigManager.GetBackgroundsFolder();

            Waves = ConfigManager.GetWaveConfig();            

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

            ConfigManager.SaveConfig(BackgroundsTxt.Text, Waves);
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

                    Wave = Waves[i]
                };                

                EditorList.Items.Add(editor);
            }
        }
    }
}
