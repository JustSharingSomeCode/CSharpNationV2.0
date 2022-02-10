using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CSharpNation.Tools
{
    class WaveTools
    {
        public static List<float> FixDiscontinuities(List<float> spectrum)
        {
            float[] fixedSpectrum = new float[spectrum.Count];

            fixedSpectrum[0] = spectrum[0];

            for (int i = 1; i < spectrum.Count - 1; i++)
            {
                if (spectrum[i] < fixedSpectrum[i - 1] && spectrum[i] < spectrum[i + 1])
                {
                    fixedSpectrum[i] = (fixedSpectrum[i - 1] + spectrum[i + 1]) / 2;
                }
                else
                {
                    fixedSpectrum[i] = spectrum[i];
                }
            }

            fixedSpectrum[fixedSpectrum.Length - 1] = spectrum[spectrum.Count - 1];

            return fixedSpectrum.ToList();
        }

        public static List<float> PromSpectrum(List<float> spectrum, int bars)
        {
            if (bars <= 0)
            {
                return spectrum;
            }

            float[] prom = new float[spectrum.Count];

            int count;

            for (int i = 0; i < spectrum.Count; i++)
            {
                count = 1;
                prom[i] = spectrum[i];

                for (int j = i + 1; j < i + bars; j++)
                {
                    if (j >= spectrum.Count)
                    {
                        break;
                    }

                    prom[i] += spectrum[j];
                    count++;
                }

                for (int j = i - 1; j > i - bars; j--)
                {
                    if (j < 0)
                    {
                        break;
                    }

                    prom[i] += spectrum[j];
                    count++;
                }

                prom[i] /= count;
            }

            return prom.ToList();
        }

        public static List<float> LoopProm(List<float> spectrum, int bars, int loops)
        {
            for(int i = 0; i < loops; i++)
            {
                spectrum = PromSpectrum(spectrum, bars);
            }

            return spectrum;
        }

        public static List<float> Normalize(List<float> spectrum)
        {
            float max = spectrum.Max();

            if (max == 0)
            {
                max = 1;
            }

            for (int i = 0; i < spectrum.Count; i++)
            {
                spectrum[i] /= max;
            }

            return spectrum;
        }

        public static List<float> CombineWaves(List<float> spectrum, List<float> promSpectrum)
        {
            List<float> norm = Normalize(promSpectrum);
            float max = spectrum.Max();

            if (max == 0)
            {
                max = 1;
            }

            for (int i = 0; i < norm.Count; i++)
            {
                norm[i] *= max;
            }

            return norm;
        }

        public static int Clamp(int min, int max, int value)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static float Clamp(float min, float max, float value)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static double VectorLenght(Vector2 a, Vector2 b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
    }
}
