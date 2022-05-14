using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Tools;
using CSharpNation.Config;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CSharpNation.Visualizer
{
    class WaveController
    {
        public WaveController()
        {
            WaveConfig.UpdateReplayBuffer();
            waves = WaveConfig.Waves.ToArray();

            for(int i = 0; i < waves.Length; i++)
            {
                if (waves[i].Quality == 0)
                {
                    waves[i].Quality = 0.2f;
                    ErrorLog.AddError(new Error(Error.Type.NonCriticalError, "Detected wave with quality set to 0, changed to 0.2 as this can produce an infinite loop"));
                }
            }
        }

        private Wave[] waves;

        public void UpdateWaves(ReplayBuffer replay, float x, float y, float radius)
        {
            for (int i = 0; i < waves.Length; i++)
            {
                waves[i].Update(replay.GetReplay(i), x, y, radius);
            }
        }

        public void DrawWaves(float x, float y, float radius)
        {
            Wave w;

            for (int i = waves.Length - 1; i >= 0; i--)
            {
                w = waves[i];

                if (w.Spectrum == null)
                {
                    continue;
                }

                //DrawLines(w);

                if (GlobalConfig.EnableGlow)
                {
                    for (int j = 0; j < w.GlowCatmullRomPoints.Count - 1; j++)
                    {
                        //get opacity based on lenght
                        int alpha = (int)(WaveTools.Clamp(0.0f, 1.0f, (float)(WaveTools.VectorLenght(new Vector2(x, y), w.GlowCatmullRomPoints[j]) - (radius + GlobalConfig.GlowSize)) / GlobalConfig.GlowMaxAlphaAtSize) * GlobalConfig.GlowMaxAlpha);

                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                        GL.Begin(PrimitiveType.Triangles);

                        GL.Color4(Color.FromArgb(0, w.R, w.G, w.B));
                        GL.Vertex2(w.GlowCatmullRomPoints[j]);
                        GL.Vertex2(w.GlowCatmullRomPoints[j + 1]);
                        GL.Color4(Color.FromArgb(alpha, w.R, w.G, w.B));
                        GL.Vertex2(w.CatmullRomPoints[j]);

                        GL.End();

                        GL.Begin(PrimitiveType.Triangles);

                        GL.Color4(Color.FromArgb(0, w.R, w.G, w.B));
                        GL.Vertex2(w.GlowCatmullRomPoints[j + 1]);
                        GL.Color4(Color.FromArgb(alpha, w.R, w.G, w.B));
                        GL.Vertex2(w.CatmullRomPoints[j + 1]);
                        GL.Vertex2(w.CatmullRomPoints[j]);

                        GL.End();
                        GL.Disable(EnableCap.Blend);

                        //left side

                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                        GL.Begin(PrimitiveType.Triangles);

                        GL.Color4(Color.FromArgb(0, w.R, w.G, w.B));
                        GL.Vertex2(MirrorPosition(x, w.GlowCatmullRomPoints[j]), w.GlowCatmullRomPoints[j].Y);
                        GL.Vertex2(MirrorPosition(x, w.GlowCatmullRomPoints[j + 1]), w.GlowCatmullRomPoints[j + 1].Y);
                        GL.Color4(Color.FromArgb(alpha, w.R, w.G, w.B));
                        GL.Vertex2(MirrorPosition(x, w.CatmullRomPoints[j]), w.CatmullRomPoints[j].Y);

                        GL.End();

                        GL.Begin(PrimitiveType.Triangles);

                        GL.Color4(Color.FromArgb(0, w.R, w.G, w.B));
                        GL.Vertex2(MirrorPosition(x, w.GlowCatmullRomPoints[j + 1]), w.GlowCatmullRomPoints[j + 1].Y);
                        GL.Color4(Color.FromArgb(alpha, w.R, w.G, w.B));
                        GL.Vertex2(MirrorPosition(x, w.CatmullRomPoints[j + 1]), w.CatmullRomPoints[j + 1].Y);
                        GL.Vertex2(MirrorPosition(x, w.CatmullRomPoints[j]), w.CatmullRomPoints[j].Y);

                        GL.End();
                        GL.Disable(EnableCap.Blend);
                    }
                }                
            }

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
