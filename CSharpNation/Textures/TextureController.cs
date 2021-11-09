using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpNation.Config;
using OpenTK.Graphics.OpenGL;

namespace CSharpNation.Textures
{
    class TextureController
    {
        public TextureController()
        {
            textures = new List<Texture>(TexturesConfig.Textures);

            LoadTextures();
        }

        private List<Texture> textures = new List<Texture>();

        private int actualBackground = 0;

        private void LoadTextures()
        {
            for(int i = 0; i < textures.Count; i++)
            {
                textures[i].LoadTexture();

                if(textures[i].TextureData == -1)
                {
                    textures.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ResizeTextures(float width, float height)
        {
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i].UpdateScale(width, height);
            }
        }

        public void DrawTexture(int texture, float x, float y, float xMax, float yMax, int a, int r, int g, int b)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Color4(Color.FromArgb(a, r, g, b));
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 1);
            GL.Vertex2(x, y);

            GL.TexCoord2(0, 0);
            GL.Vertex2(x, yMax);

            GL.TexCoord2(1, 0);
            GL.Vertex2(xMax, yMax);

            GL.TexCoord2(1, 1);
            GL.Vertex2(xMax, y);

            GL.End();
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawBackground(float x, float y, float xMax, float yMax, float power, int a, int r, int g, int b)
        {
            Texture td = textures[actualBackground];

            float wp = power * td.WidthRatio;
            float hp = wp * td.HeightRatio;

            if(xMax % 2 != 0)
            {
                xMax++;
            }

            switch (td.DisplayMode)
            {
                case Texture.Display.Fullscreen:
                    DrawTexture(td.TextureData, x - td.FillX - wp, y - td.FillY - hp, xMax + td.FillX + wp, yMax + td.FillY + hp, a, r, g, b);
                    break;

                case Texture.Display.Halfscreen:
                case Texture.Display.MirroredLeftHalf:
                case Texture.Display.MirroredRightHalf:

                    //left side
                    DrawTexture(td.TextureData, x - (td.FillX * 2.0f) - (wp * 2.0f), y - td.FillY - hp, xMax / 2.0f, yMax + td.FillY + hp, a, r, g, b);

                    //right side
                    DrawTexture(td.TextureData, xMax + (td.FillX * 2.0f) + (wp * 2.0f), 0.0f - td.FillY - hp, xMax / 2.0f, yMax + td.FillY + hp, a, r, g, b);
                    break;
            }
        }

        public void NextBackground()
        {
            if (textures.Count <= 0)
            {
                return;
            }

            actualBackground++;

            if (actualBackground >= textures.Count)
            {
                actualBackground = 0;
            }
        }

        public void PreviousBackground()
        {
            if (textures.Count <= 0)
            {
                return;
            }

            actualBackground--;

            if (actualBackground < 0)
            {
                actualBackground = textures.Count - 1;
            }
        }
    }
}
