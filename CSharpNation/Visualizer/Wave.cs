using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNation.Visualizer
{
    class Wave
    {
        public Wave()
        {
            replay = new ReplayBuffer(5);
        }

        public Wave(int r, int g, int b) : this()
        {
            R = r;
            G = g;
            B = b;
        }

        private ReplayBuffer replay;
        public List<float> Spectrum { get; private set; }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public void Update(List<float> spectrum)
        {
            replay.Push(spectrum);

            Spectrum = replay.GetAverage();            
        }
    }
}
