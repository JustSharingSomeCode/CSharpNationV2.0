using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNationV2._0.Visualizer
{
    public class WaveTools
    {
        public static List<float> PromSpectrum(List<float> spectrum, int influence)
        {
            if(spectrum == null)
            {
                return null;
            }

            if(influence <= 0)
            {
                return spectrum;
            }

            float[] promediatedSpectrum = new float[spectrum.Count];

            float actualValue = 0;

            for(int i = 0; i < spectrum.Count; i++)
            {
                actualValue = spectrum[i];

                for(int j = i - influence; j < i; j++)
                {
                    actualValue += spectrum[Clamp(0, spectrum.Count - 1, j)];
                }

                for (int j = i + 1; j <= i + influence; j++)
                {
                    actualValue += spectrum[Clamp(0, spectrum.Count - 1, j)];
                }

                actualValue /= influence + 1;

                promediatedSpectrum[i] = actualValue;
            }            

            return promediatedSpectrum.ToList();
        }        

        public static int Clamp(int min, int max, int value)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static Vector2 CatmullRom(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            Vector2 a = 2f * p1;
            Vector2 b = p2 - p0;
            Vector2 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector2 d = -p0 + 3f * p1 - 3f * p2 + p3;

            Vector2 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

            return pos;
        }
    }
}
