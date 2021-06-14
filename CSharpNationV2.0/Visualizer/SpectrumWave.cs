using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CSharpNationV2._0.Visualizer
{
    public class SpectrumWave
    {
        public List<float> SpectrumData { get; set; }

        public Color WaveColor { get; set; }

        public float Increment { get; set; }

        public int BarsInfluence { get; set; }

        public int PromLoops { get; set; }

        public float DegreesIncrement { get; set; }

        private float barWidth = 10;

        private float qualityIncrement = 0.2f;

        private Vector2 p1, p2, p3, p4;

        private double rads, PosX, PosY;

        private List<Vector2> CatmullRomPoints = new List<Vector2>();

        public void DrawWave(float x, float y, float circleRadius)
        {            
            for(int i = 0; i < CatmullRomPoints.Count - 1; i++)
            {
                GL.Color3(WaveColor);
                GL.Begin(PrimitiveType.Triangles);

                GL.Vertex2(CatmullRomPoints[i]);                
                GL.Vertex2(CatmullRomPoints[i + 1]);
                GL.Vertex2(x, y);

                GL.End();
            }

            for (int i = 0; i < CatmullRomPoints.Count - 1; i++)
            {
                GL.Color3(WaveColor);
                GL.Begin(PrimitiveType.Triangles);

                GL.Vertex2(MirrorPosition(x, CatmullRomPoints[i]), CatmullRomPoints[i].Y);
                GL.Vertex2(MirrorPosition(x, CatmullRomPoints[i + 1]), CatmullRomPoints[i + 1].Y);
                GL.Vertex2(x, y);

                GL.End();
            }            
        }

        private Vector2 GetPosition(float x, float y, int i, float circleRadius)
        {
            rads = Math.PI * (i * DegreesIncrement) / 180;

            PosX = x + (Math.Sin(rads) * (SpectrumData[i] + circleRadius));
            PosY = y + (Math.Cos(rads) * (SpectrumData[i] + circleRadius));

            return new Vector2((float)PosX, (float)PosY);
        }

        private float MirrorPosition(float x, Vector2 vector)
        {
            return vector.X - ((vector.X - x) * 2);
        }

        public void UpdatePoints(float x, float y, float circleRadius)
        {
            if (SpectrumData == null)
            {
                return;
            }

            CatmullRomPoints.Clear();

            for (int i = 0; i < SpectrumData.Count - 1; i++)
            {
                p1 = GetPosition(x, y, WaveTools.Clamp(0, SpectrumData.Count, i - 1), circleRadius);

                p2 = GetPosition(x, y, i, circleRadius);
                p3 = GetPosition(x, y, i + 1, circleRadius);

                p4 = GetPosition(x, y, WaveTools.Clamp(0, SpectrumData.Count - 1, i + 2), circleRadius);

                for (float j = 0f; j <= 1; j += qualityIncrement)
                {
                    CatmullRomPoints.Add(WaveTools.CatmullRom(j, p1, p2, p3, p4));
                }
            }
        }

        public void DrawLines(float y)
        {
            if(SpectrumData == null)
            {
                return;
            }

            for (int i = 0; i < SpectrumData.Count; i++)
            {
                GL.Color3(WaveColor);
                GL.Begin(PrimitiveType.LineLoop);

                GL.Vertex2(i * barWidth, y);
                GL.Vertex2(i * barWidth, SpectrumData[i] + y);
                GL.Vertex2((i + 1) * barWidth, SpectrumData[i] + y);
                GL.Vertex2((i + 1) * barWidth, y);

                GL.End();
            }
        }
    }
}
