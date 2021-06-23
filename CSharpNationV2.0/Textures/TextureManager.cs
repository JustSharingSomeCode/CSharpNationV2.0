using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace CSharpNationV2._0.Textures
{
    public class TextureManager
    {        
        public int LoadedBackgrounds
        {
            get
            {
                return loadedTextures.Count;
            }
        }

        private int actualBackground = 0;
        private List<int> loadedTextures = new List<int>();

        public void LoadBackgrounds(string path, string extension)
        {
            if(!Directory.Exists(path))
            {
                return;
            }

            string[] backgroundPaths = Directory.GetFiles(path, extension);

            int texture;

            for(int i = 0; i < backgroundPaths.Length; i++)
            {
                texture = LoadTexture(backgroundPaths[i]);

                if(texture != -1)
                {
                    loadedTextures.Add(texture);
                }
            }            
        }

        public int LoadTexture(string path)
        {
            if(!File.Exists(path))
            {
                return -1;
            }

            try
            {
                Bitmap bitmap = new Bitmap(path);

                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                GL.GenTextures(1, out int tex);
                GL.BindTexture(TextureTarget.Texture2D, tex);

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                return tex;
            }
            catch
            {
                return -1;
            }
        }

        public int GetActualBackground()
        {
            return loadedTextures[actualBackground];
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

        public void NextBackground()
        {
            actualBackground++;

            if(actualBackground >= LoadedBackgrounds)
            {
                actualBackground = 0;
            }
        }

        public void PreviousBackground()
        {
            actualBackground--;

            if (actualBackground <= 0)
            {
                actualBackground = LoadedBackgrounds - 1;
            }
        }
    }
}
