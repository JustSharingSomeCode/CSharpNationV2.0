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

using CSharpNationV2._0.Visualizer;

namespace CSharpNationV2._0.WpfGUI
{
    /// <summary>
    /// Lógica de interacción para LightWaveEditor.xaml
    /// </summary>
    public partial class LightWaveEditor : UserControl
    {
        public LightWaveEditor()
        {
            InitializeComponent();
        }

        public LightWaveEditor(SpectrumWave wave) : this()
        {
            Wave = wave;
            
            R = wave.WaveColor.R;
            G = wave.WaveColor.G;
            B = wave.WaveColor.B;

            IncrementTxt.Text = wave.Increment.ToString();
            BarsInfluenceTxt.Text = wave.BarsInfluence.ToString();
            PromLoopsTxt.Text = wave.PromLoops.ToString();

            Handled = false;

            UpdateColorPicker();
        }

        private bool Handled = true;

        public SpectrumWave Wave { get; private set; }

        public int R
        {
            get
            {
                return (int)R_Slider.Value;
            }
            set
            {
                R_Slider.Value = value;
            }
        }

        public int G
        {
            get
            {
                return (int)G_Slider.Value;
            }
            set
            {
                G_Slider.Value = value;
            }
        }

        public int B
        {
            get
            {
                return (int)B_Slider.Value;
            }
            set
            {
                B_Slider.Value = value;
            }
        }

        private void RGB_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!Handled)
            {
                UpdateColorPicker();
            }
        }

        public void UpdateColorPicker()
        {
            SelectedColor.Fill = new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
            
            if (Wave != null)
            {
                Wave.WaveColor = System.Drawing.Color.FromArgb(R, G, B);
            }            
        }
    }
}
