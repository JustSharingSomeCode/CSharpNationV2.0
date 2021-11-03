using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Config;

namespace CSharpNation.Visualizer
{
    class ReplayBuffer
    {
        public ReplayBuffer()
        {
            replay = new List<float>[GlobalConfig.WaveCount];
        }

        public ReplayBuffer(int length)
        {
            replay = new List<float>[length];
        }

        private List<float>[] replay;

        public void Push(List<float> spectrum)
        {
            for (int i = replay.Length - 1; i > 0; i--)
            {
                replay[i] = replay[i - 1];
            }

            replay[0] = spectrum;
        }

        public List<float> GetReplay(int index)
        {
            if (index >= 0 && index < replay.Length)
            {
                return replay[index];
            }

            return null;
        }

        public List<float> GetAverage()
        {
            for(int i = 0; i < replay.Length; i++)
            {
                if(replay[i] == null)
                {
                    return null;
                }
            }

            float[] avg = new float[GlobalConfig.Lines];

            for (int i = 0; i < GlobalConfig.Lines; i++)
            {
                for (int j = 0; j < replay.Length; j++)
                {
                    avg[i] += replay[j][i];
                }

                avg[i] /= replay.Length;
            }

            return avg.ToList();
        }
    }
}
