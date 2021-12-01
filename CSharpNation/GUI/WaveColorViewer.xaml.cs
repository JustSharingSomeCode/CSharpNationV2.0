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

using CSharpNation.Visualizer;

namespace CSharpNation.GUI
{
    /// <summary>
    /// Lógica de interacción para WaveColorViewer.xaml
    /// </summary>
    public partial class WaveColorViewer : UserControl
    {
        public WaveColorViewer()
        {
            InitializeComponent();
        }

        private Wave wave;
        public Wave Wave
        {
            get
            {
                return wave;
            }
            set
            {
                wave = value;
                UpdateColorViewer();
            }
        }

        private void UpdateColorViewer()
        {
            ColorViewer.Fill = new SolidColorBrush(Color.FromRgb((byte)wave.R, (byte)wave.G, (byte)wave.B));
        }
    }
}
