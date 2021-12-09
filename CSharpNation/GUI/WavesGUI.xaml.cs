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
using System.Windows.Navigation;
using System.Windows.Shapes;

using CSharpNation.Config;
using CSharpNation.Visualizer;

namespace CSharpNation.GUI
{
    /// <summary>
    /// Lógica de interacción para WavesGUI.xaml
    /// </summary>
    public partial class WavesGUI : UserControl
    {
        public WavesGUI()
        {
            InitializeComponent();

            waves = WaveConfig.Waves;
            viewers = new List<WaveColorViewer>();

            DrawWaves();
        }

        private List<Wave> waves;
        private List<WaveColorViewer> viewers;

        private Wave selectedWave;
        private WaveColorViewer colorViewer;

        private void DrawWaves()
        {
            double x = 0;

            for (int i = 0; i < waves.Count; i++)
            {
                WaveColorViewer wcv = new WaveColorViewer()
                {
                    Wave = waves[i],
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(x, 0, 0, 0)
                };

                wcv.MouseLeftButtonUp += Wcv_MouseLeftButtonUp;
                wcv.SendLeftClick += Wcv_SendLeftClick;
                wcv.SendRightClick += Wcv_SendRightClick;

                viewers.Add(wcv);
                _ = WavesViewerContent.Children.Add(wcv);
                x += 100 + 10;
            }
        }

        private void Wcv_SendRightClick(object sender, EventArgs e)
        {
            WaveColorViewer wcv = (WaveColorViewer)sender;
            SwapWaves(wcv.Wave, 1);
        }

        private void Wcv_SendLeftClick(object sender, EventArgs e)
        {
            WaveColorViewer wcv = (WaveColorViewer)sender;
            SwapWaves(wcv.Wave, -1);
        }


        private void SwapWaves(Wave wave, int dir)
        {
            int waveIndex = -1;

            for (int i = 0; i < waves.Count; i++)
            {
                if (wave == waves[i])
                {
                    waveIndex = i;
                }
            }

            if (waveIndex == -1 || (waveIndex == 0 && dir == -1) || (waveIndex == waves.Count - 1 && dir == 1))
            {
                return;
            }

            Wave waveTemp = waves[waveIndex + dir];

            waves[waveIndex + dir] = wave;
            viewers[waveIndex + dir].Wave = wave;
            waves[waveIndex] = waveTemp;
            viewers[waveIndex].Wave = waveTemp;
        }

        private void Wcv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            colorViewer = (WaveColorViewer)sender;
            selectedWave = colorViewer.Wave;
            RedAdder.Value = selectedWave.R;
            GreenAdder.Value = selectedWave.G;
            BlueAdder.Value = selectedWave.B;
        }

        private void RedAdder_OnValueChanged(object sender, EventArgs e)
        {
            selectedWave.R = (int)RedAdder.Value;
            colorViewer.UpdateColorViewer();
        }

        private void GreenAdder_OnValueChanged(object sender, EventArgs e)
        {
            selectedWave.G = (int)GreenAdder.Value;
            colorViewer.UpdateColorViewer();
        }

        private void BlueAdder_OnValueChanged(object sender, EventArgs e)
        {
            selectedWave.B = (int)BlueAdder.Value;
            colorViewer.UpdateColorViewer();
        }

        private void SaveConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.SaveConfig();
        }
    }
}
