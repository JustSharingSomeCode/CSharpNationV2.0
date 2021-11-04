using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Tools;

namespace CSharpNation.Visualizer
{
    class Wave
    {
        public Wave()
        {
            replay = new ReplayBuffer(5);
        }

        public Wave(int r, int g, int b, int bars) : this()
        {
            R = r;
            G = g;
            B = b;
            AvgBars = bars;
        }

        private ReplayBuffer replay;
        public List<float> Spectrum { get; private set; }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public int AvgBars { get; set; }

        public void Update(List<float> spectrum)
        {
            replay.Push(spectrum);

            Spectrum = replay.GetAverage();

            if(Spectrum == null)
            {
                return;
            }

            Spectrum = WaveTools.PromSpectrum(Spectrum, AvgBars);
        }
    }
}
