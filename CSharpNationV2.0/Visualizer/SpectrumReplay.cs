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
            for(int i = spectrumData.Length - 1; i > 0; i--)
            {                
                spectrumData[i] = spectrumData[i - 1];
            }

            spectrumData[0] = spectrum;                      
        }

        public bool IsNullSpectrum(int index)
        {
            return spectrumData[index] == null;            
        }

        public List<float> GetSpectrumReplay(int index)
        {
            return spectrumData[index];
        }
    }
}
