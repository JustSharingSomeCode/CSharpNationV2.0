using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNationV2._0.Analyzer;
using CSharpNationV2._0.Textures;
using CSharpNationV2._0.Configuration;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CSharpNationV2._0.Visualizer
{
    public class SpectrumVisualizer : GameWindow
    {
        public SpectrumVisualizer(int width, int height, string title, SpectrumAnalyzer _analyzer, SpectrumWave[] waves) : base(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 0), 24, 8, 4), title)
        {
            VSync = VSyncMode.On;

            Width = width;
            Height = height;

            Title = title;

            _analyzer.multiplier = height / 4;            

            Analyzer = _analyzer;            

            Replay = new SpectrumReplay(9);

            Waves = waves;            
           
            for(int i = 0; i < Waves.Length; i++)
            {
                Waves[i].DegreesIncrement = 180f / (Analyzer._lines - 1);
            }

            Textures = new TextureManager();

            Textures.LoadBackgrounds(ConfigManager.GetBackgroundsFolder(), "*.jpg");
            Textures.LoadBackgrounds(ConfigManager.GetBackgroundsFolder(), "*.png");       
        }

        public TextureManager Textures;
        private SpectrumAnalyzer Analyzer;
        private SpectrumReplay Replay;

        private List<float> spectrumData = new List<float>();

        public SpectrumWave[] Waves;
        
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

            spectrumData = WaveTools.LoopProm(spectrumData, 1,2);                        

            for (int i = 0; i < Waves.Length; i++)
            {                
                if(Replay.IsNullSpectrum(i))
                {
                    continue;
                }

                Waves[i].SpectrumData = WaveTools.CombineWaves(WaveTools.LoopProm(Replay.GetSpectrumReplay(i), Waves[i].BarsInfluence, Waves[i].PromLoops), Replay.GetSpectrumReplay(i), Waves[i].Increment);
                Waves[i].UpdatePoints(Width / 2, Height / 2, Height / 4 + power);
            }

            Replay.PushSpectrum(spectrumData);            

            CalculateWavePower();

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);     
            
            if(Textures.LoadedBackgrounds != 0)
            {
                Textures.DrawTexture(Textures.GetActualBackground(), 0 - power, 0 - power, Width + power, Height + power, 255, 150, 150, 150);
            }
                        
            for (int i = Waves.Length - 1; i >= 0; i--)
            {
                Waves[i].DrawWave(Width / 2, Height / 2, Height / 4 + power);
                //Waves[i].DrawLines(0);
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
