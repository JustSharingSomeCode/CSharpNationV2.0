using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

using CSharpNation.Tools;
using CSharpNation.Config;
using CSharpNation.Diagnostics;

using System.Diagnostics;

namespace CSharpNation.Visualizer
{
    public class Wave
    {
        public Wave()
        {
            replay = new ReplayBuffer(GlobalConfig.ReplayBufferSize);

            replayLog = GlobalPerformanceLog.AddPerformanceLog("Replay average");
            waveCalculationLog = GlobalPerformanceLog.AddPerformanceLog("Wave calculation");
            catmullCalculationLog = GlobalPerformanceLog.AddPerformanceLog("Catmull calculation");
        }

        public Wave(int r, int g, int b, int bars, int loops, float quality) : this()
        {
            R = r;
            G = g;
            B = b;
            AvgBars = bars;
            AvgLoops = loops;
            Quality = quality;
        }

        private PerformanceLog replayLog;
        private PerformanceLog waveCalculationLog;
        private PerformanceLog catmullCalculationLog;

        private ReplayBuffer replay;
        public List<float> Spectrum { get; private set; }
        private List<float> promSpectrum;

        public List<Vector2> CatmullRomPoints { get; private set; } = new List<Vector2>();
        public List<Vector2> GlowCatmullRomPoints { get; private set; } = new List<Vector2>();

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public int AvgBars { get; set; }
        public int AvgLoops { get; set; }

        private float quality;
        public float Quality
        {
            get
            {
                return quality;
            }
            set
            {
                if (value <= 0)
                {
                    quality = 0.1f;
                }
                else
                {
                    quality = value;
                }
            }
        }

        private double rads, PosX, PosY;
        private Vector2 p1, p2, p3, p4;

        public void Update(List<float> spectrum, float x, float y, float radius)
        {
            if (GlobalConfig.EnableReplayBuffer)
            {
                Stopwatch replayBufferSt = Stopwatch.StartNew();
                replay.Push(spectrum);

                Spectrum = replay.GetAverage();

                replayBufferSt.Stop();
                replayLog.AddValue(replayBufferSt.ElapsedTicks);
            }
            else
            {
                Spectrum = spectrum;
            }

            if(Spectrum == null)
            {
                return;
            }

            Stopwatch waveCalculationSt = Stopwatch.StartNew();

            if (GlobalConfig.UsePreviousWaveCalculation)
            {
                promSpectrum = WaveTools.PreviousLoopProm(Spectrum, AvgBars, AvgLoops);
                Spectrum = WaveTools.PreviousCombineWaves(promSpectrum, spectrum, 0.5f);
            }
            else
            {
                promSpectrum = WaveTools.LoopProm(Spectrum, AvgBars, AvgLoops);
                Spectrum = WaveTools.CombineWaves(Spectrum, promSpectrum);
            }

            waveCalculationSt.Stop();
            waveCalculationLog.AddValue(waveCalculationSt.ElapsedTicks);

            /*
            promSpectrum = WaveTools.LoopProm(Spectrum, AvgBars, AvgLoops);
            //Spectrum = WaveTools.CombineWaves(Spectrum, promSpectrum);
            Spectrum = WaveTools.CombineWaves2(promSpectrum, spectrum, 0.5f);
            //Spectrum = promSpectrum;
            */

            Stopwatch catmullCalculationSt = Stopwatch.StartNew();

            UpdatePoints(x, y, radius);

            catmullCalculationSt.Stop();
            catmullCalculationLog.AddValue(catmullCalculationSt.ElapsedTicks);

            if (GlobalConfig.EnableGlow)
            {
                UpdateGlowPoints(x, y, radius + GlobalConfig.GlowSize);
            }
        }

        private void UpdatePoints(float x, float y, float circleRadius)
        {
            if (Spectrum == null)
            {
                return;
            }

            CatmullRomPoints.Clear();

            for (int i = 0; i < Spectrum.Count - 1; i++)
            {
                p1 = GetPosition(x, y, WaveTools.Clamp(0, Spectrum.Count, i - 1), circleRadius);

                p2 = GetPosition(x, y, i, circleRadius);
                p3 = GetPosition(x, y, i + 1, circleRadius);

                p4 = GetPosition(x, y, WaveTools.Clamp(0, Spectrum.Count - 1, i + 2), circleRadius);

                for (float j = 0f; j <= 1; j += Quality)
                {
                    CatmullRomPoints.Add(CatmullRom(j, p1, p2, p3, p4));
                }
            }
        }

        private void UpdateGlowPoints(float x, float y, float circleRadius)
        {
            if (Spectrum == null)
            {
                return;
            }

            GlowCatmullRomPoints.Clear();

            for (int i = 0; i < Spectrum.Count - 1; i++)
            {
                p1 = GetPosition(x, y, WaveTools.Clamp(0, Spectrum.Count, i - 1), circleRadius);

                p2 = GetPosition(x, y, i, circleRadius);
                p3 = GetPosition(x, y, i + 1, circleRadius);

                p4 = GetPosition(x, y, WaveTools.Clamp(0, Spectrum.Count - 1, i + 2), circleRadius);

                for (float j = 0f; j <= 1; j += Quality)
                {
                    GlowCatmullRomPoints.Add(CatmullRom(j, p1, p2, p3, p4));
                }
            }
        }

        private Vector2 GetPosition(float x, float y, int i, float circleRadius)
        {
            rads = Math.PI * (i * GlobalConfig.DegreesIncrement) / 180;

            PosX = x + (Math.Sin(rads) * (Spectrum[i] + circleRadius));
            PosY = y + (Math.Cos(rads) * (Spectrum[i] + circleRadius));

            return new Vector2((float)PosX, (float)PosY);
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

        public override string ToString()
        {
            return R.ToString() + "|" + G.ToString() + "|" + B.ToString() + "|" + AvgBars.ToString() + "|" + AvgLoops.ToString() + "|" + Quality.ToString();
        }
    }
}
