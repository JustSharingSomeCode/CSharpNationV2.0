using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

using CSharpNation.Tools;

namespace CSharpNation.Textures
{
    class Texture
    {
        public enum Display
        {
            NotFound,
            Fullscreen,
            Halfscreen,
            MirroredLeftHalf,
            MirroredRightHalf
        }

        public string Path { get; set; }
        public Display DisplayMode { get; set; }

        private Bitmap textureBitmap;
        public int TextureData { get; private set; } = -1;
        public int OriginalWidth { get; private set; }
        public int OriginalHeight { get; private set; }

        public float WidthRatio { get; private set; }
        public float HeightRatio { get; private set; }

        public float ActualWidth { get; private set; }
        public float ActualHeight { get; private set; }

        public float FillY { get; private set; }
        public float FillX { get; private set; }

        public string FileName
        {
            get
            {
                return Path.Split((char)92).Last();
            }
        }

        private Bitmap GetBitmap(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                return new Bitmap(path);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error loading image: {0}, Error: {1}", FileName, ex.Message);
                ErrorLog.AddError(new Error(Error.Type.NonCriticalError, "Error loading bitmap on file: " + FileName + ", Details: " + ex.Message));
                return null;
            }
        }

        private int LoadTextureData(Bitmap bitmap, Display displayMode = Display.Fullscreen)
        {
            try
            {
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                GL.GenTextures(1, out int tex);
                GL.BindTexture(TextureTarget.Texture2D, tex);

                BitmapData data = null;

                switch (displayMode)
                {
                    case Display.Halfscreen:
                    case Display.Fullscreen:
                        data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        break;
                    case Display.MirroredLeftHalf:
                        data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width / 2, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        break;
                    case Display.MirroredRightHalf:
                        data = bitmap.LockBits(new Rectangle(bitmap.Width / 2, 0, bitmap.Width / 2, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        break;
                }

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);

                bitmap.Dispose();

                return tex;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error loading image: {0}, Error: {1}", FileName, ex.Message);
                ErrorLog.AddError(new Error(Error.Type.NonCriticalError, "Error processing file: " + FileName + ", Details: " + ex.Message));
                return -1;
            }
        }

        public void LoadTexture()
        {
            textureBitmap = GetBitmap(Path);

            if (textureBitmap == null)
            {
                return;
            }

            OriginalWidth = textureBitmap.Width;
            OriginalHeight = textureBitmap.Height;

            if (DisplayMode == Display.NotFound)
            {
                if (OriginalHeight > OriginalWidth)
                {
                    DisplayMode = Display.Halfscreen;
                }
                else
                {
                    DisplayMode = Display.Fullscreen;
                }
            }
            else
            {
                if (DisplayMode != Display.Fullscreen && DisplayMode != Display.Halfscreen)
                {
                    OriginalWidth /= 2;
                }
            }

            WidthRatio = (float)OriginalWidth / OriginalHeight;
            HeightRatio = (float)OriginalHeight / OriginalWidth;

            TextureData = LoadTextureData(textureBitmap, DisplayMode);

            textureBitmap = null;
        }

        public void UpdateScale(float screenWidth, float screenHeight)
        {
            if (DisplayMode != Display.Fullscreen)
            {
                screenWidth /= 2.0f;
            }

            float missingWidth = screenWidth - OriginalWidth;
            float missingHeight = screenHeight - OriginalHeight;

            if (missingWidth > missingHeight)
            {
                ActualWidth = screenWidth;
                ActualHeight = screenWidth * HeightRatio;
            }
            else
            {
                ActualHeight = screenHeight;
                ActualWidth = screenHeight * WidthRatio;
            }

            if (ActualHeight < screenHeight)
            {
                float scale = screenHeight / ActualHeight;
                ActualHeight *= scale;
                ActualWidth *= scale;
            }

            if (ActualWidth < screenWidth)
            {
                float scale = screenWidth / ActualWidth;
                ActualHeight *= scale;
                ActualWidth *= scale;
            }

            FillY = (ActualHeight - screenHeight) / 2.0f;
            FillX = (ActualWidth - screenWidth) / 2.0f;
        }

        public override string ToString()
        {
            return FileName + "|" + DisplayMode.ToString();
        }
    }
}
