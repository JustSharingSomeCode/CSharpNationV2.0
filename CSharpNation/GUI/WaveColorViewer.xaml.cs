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

        public event EventHandler SendLeftClick;
        public event EventHandler SendRightClick;

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

        public void UpdateColorViewer()
        {
            ColorViewer.Fill = new SolidColorBrush(Color.FromRgb((byte)wave.R, (byte)wave.G, (byte)wave.B));
        }

        private void SendLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            SendLeftClick?.Invoke(this, EventArgs.Empty);
        }

        private void SendRightBtn_Click(object sender, RoutedEventArgs e)
        {
            SendRightClick?.Invoke(this, EventArgs.Empty);
        }

        public void Select()
        {
            ControlBorder.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70));
        }

        public void Deselect()
        {
            ControlBorder.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
        }
    }
}
