using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Config;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CSharpNation.Visualizer
{
    class WaveController
    {
        public WaveController()
        {
            waves = WaveConfig.Waves.ToArray();
        }

        private Wave[] waves;

        public void UpdateWaves(ReplayBuffer replay, float x, float y, float radius)
        {
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].Update(replay.GetReplay(i), x, y, radius);
            }
        }

        public void DrawWaves(float x, float y)
        {
            Wave w;

            for (int i = waves.Length - 1; i >= 0; i--)
            {
                w = waves[i];

                if (w.Spectrum == null)
                {
                    continue;
                }

                for (int j = 0; j < w.CatmullRomPoints.Count - 1; j++)
                {
                    GL.Color3(Color.FromArgb(255, w.R, w.G, w.B));
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Vertex2(w.CatmullRomPoints[j]);
                    GL.Vertex2(w.CatmullRomPoints[j + 1]);
                    GL.Vertex2(x, y);

                    GL.End();
                }

                for (int j = 0; j < w.CatmullRomPoints.Count - 1; j++)
                {
                    GL.Color3(Color.FromArgb(255, w.R, w.G, w.B));
                    GL.Begin(PrimitiveType.Triangles);

                    GL.Vertex2(MirrorPosition(x, w.CatmullRomPoints[j]), w.CatmullRomPoints[j].Y);
                    GL.Vertex2(MirrorPosition(x, w.CatmullRomPoints[j + 1]), w.CatmullRomPoints[j + 1].Y);
                    GL.Vertex2(x, y);

                    GL.End();
                }
            }
        }

        private float MirrorPosition(float x, Vector2 vector)
        {
            return vector.X - ((vector.X - x) * 2);
        }

        private void DrawLines(Wave w)
        {
            for (int j = 0; j < w.Spectrum.Count; j++)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(Color.FromArgb(255, w.R, w.G, w.B));

                GL.Vertex2(j * 10, 0);
                GL.Vertex2(j * 10, w.Spectrum[j]);
                GL.Vertex2((j * 10) + 10, w.Spectrum[j]);
                GL.Vertex2((j * 10) + 10, 0);

                GL.End();
            }
        }
    }
}
