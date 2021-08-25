using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNationV2._0.Configuration;

namespace CSharpNationV2._0.Visualizer
{
    public class WaveTools
    {
        public static List<float> PromSpectrum(List<float> spectrum, int influence)
        {
            /*
            if(spectrum == null)
            {
                return null;
            }
            */

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
        
        public static List<float> LoopProm(List<float> spectrum, int influence, int loops)
        {
            for(int i = 0; i < loops; i++)
            {
                spectrum = PromSpectrum(spectrum, influence);
            }

            return spectrum;
        }
        
        public static List<float> Normalize(List<float> spectrum)
        {
            /*
            if(spectrum == null)
            {
                return null;
            }
            */

            float max = spectrum.Max();            

            float[] normalizedList = new float[spectrum.Count];

            for(int i = 0; i < spectrum.Count; i++)
            {
                normalizedList[i] = spectrum[i] / max;
            }

            return normalizedList.ToList();
        }        
        
        public static List<float> CombineWaves(List<float> spectrum, List<float> raw, float increase)
        {
            /*
            if (spectrum == null || spectrum2 == null)
            {
                return null;
            }
            */

            for (int i = 0; i < spectrum.Count; i++)
            {
                spectrum[i] = spectrum[i] * increase + raw[i] * (1.0f - increase);
            }
            
            List<float> normalizedList = Normalize(spectrum);

            float max = raw.Max();

            for(int i = 0; i < spectrum.Count; i++)
            {
                normalizedList[i] = normalizedList[i] * max;
            }

            return normalizedList;
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

        public static Vector2 IncreaseVector(float multiplier, Vector2 vectorToIncrease)
        {
            Vector2 vectorToOrigin = new Vector2((vectorToIncrease.X - ConfigurationManager.VisualizerWidth / 2) * multiplier,
                (vectorToIncrease.Y - ConfigurationManager.VisualizerHeight / 2) * multiplier);

            Vector2 increasedVector = new Vector2(vectorToIncrease.X + vectorToOrigin.X, vectorToIncrease.Y + vectorToOrigin.Y);

            return increasedVector;
        }
    }
}
