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
    /// Lógica de interacción para WaveEditor.xaml
    /// </summary>
    public partial class WaveEditor : UserControl
    {
        public WaveEditor()
        {
            InitializeComponent();            
        }

        public SpectrumWave Wave;

        public int R
        {
            get
            {
                return (int)RedSlider.Value;
            }
            set
            {
                RedSlider.Value = value;
            }
        }

        public int G
        {
            get
            {
                return (int)GreenSlider.Value;
            }
            set
            {
                GreenSlider.Value = value;
            }
        }

        public int B
        {
            get
            {
                return (int)BlueSlider.Value;
            }
            set
            {
                BlueSlider.Value = value;
            }
        }        

        public void UpdateColorPicker()
        {
            SelectedColor.Fill = new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
            if(Wave != null)
            {
                Wave.WaveColor = System.Drawing.Color.FromArgb(R, G, B);
            }            
        }

        private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateColorPicker();
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            IncrementTxt.Text = Wave.Increment.ToString();
            BarsInfluenceTxt.Text = Wave.BarsInfluence.ToString();
            PromLoopsTxt.Text = Wave.PromLoops.ToString();
        }

        private void IncrementTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                try
                {
                    float val = float.Parse(IncrementTxt.Text);

                    if(val >= 0 && val <= 1)
                    {
                        Wave.Increment = val;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    IncrementTxt.Text = Wave.Increment.ToString();
                }
            }
        }

        private void BarsInfluenceTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    int val = int.Parse(BarsInfluenceTxt.Text);
                    Wave.BarsInfluence = val;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    BarsInfluenceTxt.Text = Wave.BarsInfluence.ToString();
                }
            }
        }

        private void PromLoopsTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    int val = int.Parse(PromLoopsTxt.Text);                
                    Wave.PromLoops = val;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    PromLoopsTxt.Text = Wave.PromLoops.ToString();
                }
            }
        }
    }
}
