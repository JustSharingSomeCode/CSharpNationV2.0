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
            replay = new ReplayBuffer(8);
        }

        public Wave(int r, int g, int b, int bars, int loops) : this()
        {
            R = r;
            G = g;
            B = b;
            AvgBars = bars;
            AvgLoops = loops;
        }

        private ReplayBuffer replay;
        public List<float> Spectrum { get; private set; }
        private List<float> promSpectrum;

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public int AvgBars { get; set; }
        public int AvgLoops { get; set; }

        public void Update(List<float> spectrum)
        {
            replay.Push(spectrum);

            Spectrum = replay.GetAverage();

            if(Spectrum == null)
            {
                return;
            }

            promSpectrum = WaveTools.LoopProm(Spectrum, AvgBars, AvgLoops);
            Spectrum = WaveTools.CombineWaves(Spectrum, promSpectrum);
        }
    }
}
