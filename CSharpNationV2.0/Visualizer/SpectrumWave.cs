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

        float barWidth = 10;

        public void DrawWave(float x, float y)
        {
            if(SpectrumData == null)
            {
                return;
            }

            for (int i = 0; i < SpectrumData.Count; i++)
            {
                GL.Color3(WaveColor);
                GL.Begin(PrimitiveType.Quads);

                GL.Vertex2(i * barWidth, y);
                GL.Vertex2(i * barWidth, SpectrumData[i] + y);
                GL.Vertex2((i + 1) * barWidth, SpectrumData[i] + y);                
                GL.Vertex2((i + 1) * barWidth, y);

                GL.End();
            }
        }
    }
}
