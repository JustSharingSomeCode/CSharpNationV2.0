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

using CSharpNationV2._0.Configuration;
using CSharpNationV2._0.Visualizer;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para WavesWindow.xaml
    /// </summary>
    public partial class WavesWindow : UserControl
    {
        public WavesWindow()
        {
            InitializeComponent();

            LoadWaves();
        }

        public SpectrumWave[] Waves { get; private set; }

        private void LoadWaves()
        {
            //SpectrumWave[] waves = ConfigManager.GetWaveConfig();
            Waves = ConfigurationManager.LoadWaves();

            double left = 10;

            for (int i = 0; i < Waves.Length; i++)
            {
                LightWaveEditor lwe = new LightWaveEditor(Waves[i])
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(left, 10, 0, 0)
                };

                WavesViewerGrid.Children.Add(lwe);

                left += lwe.MinWidth + 10;
            }
        }

        private void SaveConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.SaveWaves(Waves);
        }
    }
}
