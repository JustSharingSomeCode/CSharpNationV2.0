using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using CSharpNation.Analyzer;
using CSharpNation.Tools;

namespace CSharpNation.Visualizer
{
    class SpectrumVisualizer : GameWindow
    {
        public SpectrumVisualizer(int width, int height, string title, SpectrumAnalyzer _analyzer) : base(width, height, new GraphicsMode(new ColorFormat(8, 8, 8, 0), 24, 8, 4), title)
        {
            analyzer = _analyzer;

            VSync = VSyncMode.On;
        }

        private List<float> spectrum;
        private SpectrumAnalyzer analyzer;

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(new Color4(255, 0, 0, 255));

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, Width, 0.0f, Height, 0.0f, 1.0f);

            analyzer.multiplier = Height / 4;

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            spectrum = WaveTools.FixDiscontinuities(analyzer.GetSpectrum());

            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (spectrum == null)
            { return; }

            for (int i = 0; i < spectrum.Count; i++)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(255, 255, 255);

                GL.Vertex2(i * 10, 0);
                GL.Vertex2(i * 10, spectrum[i]);
                GL.Vertex2(i * 10 + 10, spectrum[i]);
                GL.Vertex2(i * 10 + 10, 0);

                GL.End();
            }

            Context.SwapBuffers();

            base.OnRenderFrame(e);
        }
    }
}
