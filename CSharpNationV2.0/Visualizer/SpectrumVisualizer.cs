using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNationV2._0.Analyzer;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CSharpNationV2._0.Visualizer
{
    public class SpectrumVisualizer : GameWindow
    {
        public SpectrumVisualizer(int width, int height, string title, SpectrumAnalyzer _analyzer) : base(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 0), 24, 8, 4), title)
        {
            VSync = VSyncMode.On;

            Width = width;
            Height = height;

            Title = title;

            _analyzer.multiplier = height / 4;            

            Analyzer = _analyzer;


            //DegreesIncrement = 180.0f / _analyzer._lines;

            Replay = new SpectrumReplay(9);
            Waves = new SpectrumWave[9];

            Waves[0] = new SpectrumWave
            {
                WaveColor = Color.White
            };

            Waves[1] = new SpectrumWave
            {
                WaveColor = Color.Yellow
            };

            Waves[2] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(255, 150, 0)
            };

            Waves[3] = new SpectrumWave
            {
                WaveColor = Color.Red
            };

            Waves[4] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(255, 100, 255)
            };

            Waves[5] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(50, 50, 155)
            };

            Waves[6] = new SpectrumWave
            {
                WaveColor = Color.Blue
            };

            Waves[7] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(50, 205, 255)
            };

            Waves[8] = new SpectrumWave
            {
                WaveColor = Color.FromArgb(0, 255, 0)
            };
           
            for(int i = 0; i < Waves.Length; i++)
            {
                Waves[i].DegreesIncrement = 180f / (Analyzer._lines - 1);
            }
        }

        private SpectrumAnalyzer Analyzer;
        private SpectrumReplay Replay;

        private List<float> spectrumData = new List<float>();

        private SpectrumWave[] Waves;

        //private float DegreesIncrement = 0;
        private float power = 0;        

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(new Color4(100, 100, 100, 255));

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, Width, 0.0f, Height, 0.0f, 1.0f);

            Analyzer.multiplier = Height / 4;            

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            spectrumData = FixDiscontinuities(Analyzer.GetSpectrum());

            for (int i = 0; i < Waves.Length; i++)
            {
                Waves[i].SpectrumData = WaveTools.PromSpectrum(Replay.GetSpectrumReplay(i), i + 1);
                Waves[i].UpdatePoints(Width / 2, Height / 2, Height / 4 + power);
            }

            Replay.PushSpectrum(spectrumData);            

            CalculateWavePower();

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);           
                        
            for (int i = Waves.Length - 1; i >= 0; i--)
            {
                Waves[i].DrawWave(Width / 2, Height / 2, Height / 4 + power);
            }

            DrawCircle(Width / 2, Height / 2, (Height / 4) + power, Color.White);
            DrawCircle(Width / 2, Height / 2, (Height / 4.2) + power, Color.Black);

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }

        private void DrawCircle(double X, double Y, double Radius, Color C)
        {
            GL.Color3(C);
            GL.Begin(PrimitiveType.Polygon);

            double rads, PosX, PosY;

            for (int i = 0; i <= 360; i += 2)
            {
                rads = Math.PI * i / 180;
                PosX = X + (Math.Sin(rads) * Radius);
                PosY = Y + (Math.Cos(rads) * Radius);

                GL.Vertex2(PosX, PosY);
            }

            GL.End();
        }

        private void CalculateWavePower()
        {
            for (int i = 0; i < spectrumData.Count; i++)
            {
                power += spectrumData[i];
            }

            power /= spectrumData.Count;
        }     
        
        private List<float> FixDiscontinuities(List<float> spectrum)
        {
            float[] fixedSpectrum = new float[spectrum.Count];

            fixedSpectrum[0] = spectrum[0];

            for(int i = 1; i < spectrum.Count - 1; i++)
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
    }
}
