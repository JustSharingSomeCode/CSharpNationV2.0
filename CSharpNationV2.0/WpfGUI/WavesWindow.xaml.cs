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

        //private List<SpectrumWave> WaveList = new List<SpectrumWave>();  --planning change from array to list
        private List<LightWaveEditor> EditorList = new List<LightWaveEditor>();

        private double MinMargin = 10;
        private double ActualMargin;
        private int MaxEditors, xCount;
        private double MissingMargin;
        private double X, Y;

        private bool loaded = false;

        private void LoadWaves()
        {            
            Waves = ConfigurationManager.LoadWaves();            
        }        

        private void LoadEditors()
        {
            WavesViewerGrid.Children.Clear();
            EditorList.Clear();

            X = ActualMargin;
            Y = MinMargin;
            xCount = 0;

            for (int i = 0; i < Waves.Length; i++)
            {
                LightWaveEditor lwe = new LightWaveEditor(Waves[i])
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(X, Y, 0, 0)
                };
                
                EditorList.Add(lwe);
                WavesViewerGrid.Children.Add(EditorList.Last());

                X += 200 + ActualMargin;
                xCount++;

                if(xCount >= MaxEditors)
                {
                    X = ActualMargin;
                    Y += 300 + MinMargin;
                    xCount = 0;
                }
            }

            loaded = true;
        }

        
        private void FitEditorsToBounds()
        {            
            X = ActualMargin;
            Y = MinMargin;
            xCount = 0;

            for(int i = 0; i < EditorList.Count; i++)
            {
                EditorList[i].Margin = new Thickness(X, Y, 0, 0);

                X += 200 + ActualMargin;
                xCount++;

                if (xCount >= MaxEditors)
                {
                    X = ActualMargin;
                    Y += 300 + MinMargin;
                    xCount = 0;
                }
            }            
        }        

        private void AddEditor()
        {

        }

        private void SaveConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.SaveWaves(Waves);
        }

        private void ResetWavesBtn_Click(object sender, RoutedEventArgs e)
        {
            Waves = ConfigurationManager.GetDefaultWaves();
            
            LoadEditors();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                LoadEditors();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBounds();
            
            FitEditorsToBounds();
        }

        private void UpdateBounds()
        {            
            double w = (WavesViewer.ContentVisualWidth - MinMargin) / (200 + MinMargin);
            MaxEditors = (int)w;
            MissingMargin = (w - MaxEditors) * (200 + MinMargin);
            ActualMargin = MinMargin + MissingMargin / MaxEditors;
        }
    }
}
