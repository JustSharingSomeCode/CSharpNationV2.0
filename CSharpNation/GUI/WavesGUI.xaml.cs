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

        private void WavesControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

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

                viewers.Add(wcv);
                _ = WavesViewerContent.Children.Add(wcv);
                x += 100 + 10;
            }
        }
    }
}
