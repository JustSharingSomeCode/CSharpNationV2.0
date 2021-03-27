using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNationV2._0.Visualizer
{
    public class SpectrumReplay
    {
        public SpectrumReplay(int n)
        {
            spectrumData = new List<float>[n];
        }

        private List<float>[] spectrumData;

        public void PushSpectrum(List<float> spectrum)
        {
            /*
            for(int i = spectrumData.Length - 1; i > 0; i--)
            {                
                spectrumData[i] = spectrumData[i - 1];
            }

            spectrumData[0] = spectrum;
            */

            for (int i = spectrumData.Length - 1; i > 0; i--)
            {
                if(spectrumData[i - 1] == null)
                {
                    continue;
                }

                spectrumData[i] = new List<float>(spectrumData[i - 1]);
            }

            spectrumData[0] = new List<float>(spectrum);
        }

        public List<float> GetSpectrumReplay(int index)
        {
            return spectrumData[index];
        }
    }
}
