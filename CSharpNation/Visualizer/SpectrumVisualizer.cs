﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using CSharpNation.Analyzer;
using CSharpNation.Tools;
using CSharpNation.Textures;
using CSharpNation.Config;
using CSharpNation.Particles;
using System.Drawing;

namespace CSharpNation.Visualizer
{
    class SpectrumVisualizer : GameWindow
    {
        public SpectrumVisualizer(int width, int height, string title, SpectrumAnalyzer _analyzer) : base(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 0), 24, 8, 8), title)
        {
            analyzer = _analyzer;

            VSync = VSyncMode.On;

            replay = new ReplayBuffer();
            waveController = new WaveController();
            textureController = new TextureController();
            particlesController = new ParticleController(width, height);
            random = new Random();
        }

        private List<float> spectrum;
        private SpectrumAnalyzer analyzer;
        private ReplayBuffer replay;
        private WaveController waveController;
        private TextureController textureController;
        private ParticleController particlesController;        

        private Random random;

        private float power;
        private int bgSeconds = 0;
        private int frameCount = 0;
        private float radius;

        private float Rx, Ry;
        private int Dx = -1, Dy = 1;
        private int ShakeCount = 0;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(new Color4(50, 50, 50, 255));

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, Width, 0.0f, Height, 0.0f, 1.0f);

            analyzer.multiplier = Height / 4;

            textureController.ResizeTextures(Width, Height);

            particlesController.UpdateBounds(Width, Height);

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            spectrum = WaveTools.FixDiscontinuities(analyzer.GetSpectrum());

            if (GlobalConfig.UsePreviousWaveCalculation)
            {
                spectrum = WaveTools.PreviousLoopProm(spectrum, 1, 2);
            }
            else
            {
                spectrum = WaveTools.LoopProm(spectrum, 1, 2);
            }

            replay.Push(spectrum);

            CalculateWavePower();

            if(GlobalConfig.EnableShaking)
            {
                switch (ShakeCount)
                {
                    case 0:
                        Dx = -1;
                        Dy = 1;
                        break;
                    case 1:
                        Dx = 1;
                        Dy = -1;
                        break;
                    case 2:
                        Dx = -1;
                        Dy = -1;
                        break;
                    case 3:
                        Dx = 1;
                        Dy = 1;
                        ShakeCount = 0;
                        break;
                }

                float pw = WaveTools.Clamp(0.0f, float.MaxValue, power - GlobalConfig.ShakeThreshold);

                Rx = (float)random.NextDouble() * (pw * GlobalConfig.ShakeMultiplier) * Dx;
                Ry = (float)random.NextDouble() * (pw * GlobalConfig.ShakeMultiplier) * Dy;

                ShakeCount++;
            }
            else
            {
                Rx = 0;
                Ry = 0;
            }

            waveController.UpdateWaves(replay, Width / 2 + Rx, Height / 2 + Ry, Height / 4 + power);

            if (GlobalConfig.AutoBackgroundChange)
            {
                frameCount++;

                if (frameCount >= GlobalConfig.Fps)
                {
                    frameCount = 0;
                    bgSeconds++;
                }

                if (bgSeconds >= GlobalConfig.BackgroundTime)
                {
                    bgSeconds = 0;
                    textureController.NextBackground();
                }
            }

            particlesController.UpdateParticles(power);

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (spectrum == null)
            { return; }

            int dim = GlobalConfig.BackgroundDim;

            textureController.DrawBackground(0, 0, Width, Height, GlobalConfig.BackgroundMovement ? power : 0, 255, dim, dim, dim);

            particlesController.DrawParticles();
            
            waveController.DrawWaves(Width / 2 + Rx, Height / 2 + Ry, radius);

            //DrawCircle(Width / 2, Height / 2, (Height / 4) + power, Color.White);
            //DrawCircle(Width / 2, Height / 2, (Height / 4.2) + power, Color.Black);

            textureController.DrawLogo(Width / 2 - radius + Rx, Height / 2 - radius + Ry, Width / 2 + radius + Rx, Height / 2 + radius + Ry);
            
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
            for (int i = 0; i < spectrum.Count; i++)
            {
                power += spectrum[i];
            }

            power /= spectrum.Count;
            radius = Height / 4 + power;
        }

        public void NextBackground()
        {
            textureController.NextBackground();
        }

        public void PreviousBackground()
        {
            textureController.PreviousBackground();
        }

        public void SearchBackground(string filename)
        {
            textureController.SearchBackground(filename);
        }
    }
}
