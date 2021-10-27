using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows;

using CSharpNation.Visualizer;

namespace CSharpNation
{
    public class CSharpNationController
    {
        private Thread vth;
        private SpectrumVisualizer visualizer;

        public CSharpNationController()
        {
            vth = new Thread(VisualizerThread);
            //visualizer = new SpectrumVisualizer(1024, 576, "CSharpNation");
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
            using (visualizer = new SpectrumVisualizer(1024, 576, "CSharpNation"))
            {
                visualizer.Run();
            }
        }
    }
}
