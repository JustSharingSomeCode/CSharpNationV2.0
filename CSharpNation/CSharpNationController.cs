using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows;

using CSharpNation.Visualizer;
using CSharpNation.Analyzer;
using CSharpNation.Config;

namespace CSharpNation
{
    public class CSharpNationController
    {
        private Thread vth;
        private SpectrumVisualizer visualizer;
        private SpectrumAnalyzer analyzer;

        public CSharpNationController()
        {
            vth = new Thread(VisualizerThread);
            analyzer = new SpectrumAnalyzer();
        }

        public void StartVisualizer()
        {
            if (vth.IsAlive)
            {
                MessageBoxResult result = MessageBox.Show("The visualizer is actually running, ¿Do you want to start a new one?", "Visualizer actually running", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    visualizer.Close();

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
            vth = new Thread(VisualizerThread);

            vth.Start();
        }

        private void VisualizerThread()
        {
            using (visualizer = new SpectrumVisualizer(1280, 720, "CSharpNation", analyzer))
            {
                visualizer.Run(GlobalConfig.Fps);
            }

            GlobalConfig.SaveConfig();
        }

        public void PauseCapture()
        {
            analyzer.PauseCapture();
        }

        public void ResumeCapture()
        {
            analyzer.ResumeCapture();
        }

        public void Cleanup()
        {
            if(vth.IsAlive)
            {
                visualizer.Close();
            }
            
            analyzer.Free();
        }

        public void NextBackground()
        {
            visualizer.NextBackground();
        }

        public void PreviousBackground()
        {
            visualizer.PreviousBackground();
        }
    }
}
