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
using CSharpNationV2._0.Textures;
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
            //Analyzer = new SpectrumAnalyzer();

            ConfigManager.LoadConfigFile();

            BackgroundsTxt.Text = ConfigManager.BackgroundsFolder;

            Waves = ConfigManager.GetWaveConfig();

            UpdateWaveEditor();

            textureManager = new TextureManager();

            //ReloadTextures();

            //Console.WriteLine(ConfigManager.GetBackgroundConfig()[0]);
        }

        private SpectrumAnalyzer Analyzer;
        private Thread VisualizerThread;
        private SpectrumVisualizer Visualizer;

        private SpectrumWave[] Waves;

        private TextureManager textureManager;

        private float[] LogoOffset = new float[4];

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
            using (Visualizer = new SpectrumVisualizer(1280, 720, "CSharpNation_V2.0", Analyzer, Waves, textureManager, LogoOffset))
            {
                Visualizer.Run();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            DevicesCb.ItemsSource = Analyzer.GetDevices();

            if (DevicesCb.HasItems)
            {
                DevicesCb.SelectedIndex = 0;
            }
            */
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            if (VisualizerThread.IsAlive)
            {
                Visualizer.Close();
            }

            Analyzer.Free();

    */
            //ConfigManager.SaveConfig(BackgroundsTxt.Text, Waves, textureManager.LoadedTextures);
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

            for (int i = 0; i < Waves.Length; i++)
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

        private void PreviousBackgroundBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Visualizer != null)
            {
                Visualizer.Textures.PreviousBackground();
            }
        }

        private void NextBackgroundBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Visualizer != null)
            {
                Visualizer.Textures.NextBackground();
            }
        }

        private void BackgroundsTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ConfigManager.BackgroundsFolder = BackgroundsTxt.Text;
            }
        }

        private void TopOffsetTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    LogoOffset[1] = float.Parse(TopOffsetTxt.Text);
                }
                catch
                {
                    TopOffsetTxt.Text = LogoOffset[1].ToString();
                }
            }
        }

        private void BottomOffsetTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    LogoOffset[3] = float.Parse(BottomOffsetTxt.Text);
                }
                catch
                {
                    BottomOffsetTxt.Text = LogoOffset[3].ToString();
                }
            }
        }

        private void LeftOffsetTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    LogoOffset[0] = float.Parse(LeftOffsetTxt.Text);
                }
                catch
                {
                    LeftOffsetTxt.Text = LogoOffset[0].ToString();
                }
            }
        }

        private void RightOffsetTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    LogoOffset[2] = float.Parse(RightOffsetTxt.Text);
                }
                catch
                {
                    RightOffsetTxt.Text = LogoOffset[2].ToString();
                }
            }
        }

        private void ReloadTextures()
        {
            textureManager.Clean();

            textureManager.LoadTextureData(ConfigManager.BackgroundsFolder, "*.jpg");
            textureManager.LoadTextureData(ConfigManager.BackgroundsFolder, "*.png");
        }

        private void ReloadTexturesBtn_Click(object sender, RoutedEventArgs e)
        {
            ReloadTextures();
        }

        private void TestWBtn_Click(object sender, RoutedEventArgs e)
        {
            CSharpNationManager cm = new CSharpNationManager();
            cm.Show();
        }
    }
}
