using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Config;

using OpenTK.Graphics.OpenGL;

namespace CSharpNation.Visualizer
{
    class WaveController
    {
        public WaveController()
        {
            waves = new Wave[GlobalConfig.WaveCount];

            waves[0] = new Wave(255, 255, 255);
            waves[1] = new Wave(200, 200, 200);
            waves[2] = new Wave(150, 150, 150);
            waves[3] = new Wave(100, 100, 100);
            waves[4] = new Wave(50, 50, 50);
        }

        private Wave[] waves;

        public void UpdateWaves(ReplayBuffer replay)
        {
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].Update(replay.GetReplay(i));
            }
        }

        public void DrawWaves()
        {
            Wave w;

            for (int i = waves.Length - 1; i >= 0; i--)
            {
                w = waves[i];

                if (w.Spectrum == null)
                {
                    continue;
                }

                for(int j = 0; j < w.Spectrum.Count; j++)
                {
                    GL.Begin(PrimitiveType.Quads);
                    GL.Color3(Color.FromArgb(255,w.R, w.G, w.B));                    

                    GL.Vertex2(j * 10, 0);
                    GL.Vertex2(j * 10, w.Spectrum[j]);
                    GL.Vertex2((j * 10) + 10, w.Spectrum[j]);
                    GL.Vertex2((j * 10) + 10, 0);

                    GL.End();
                }
            }
        }
    }
}
